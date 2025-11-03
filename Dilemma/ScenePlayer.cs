using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Dilemma
{
    public class ScenePlayer
    {
        MainWin mw;
        ScenePack currentPack = null;
        Scene currentScene = null;

        // Custom EventArgs to pass selected choice ID
        public class ContinueEventArgs : EventArgs
        {
            public int SelectedChoiceId { get; private set; }

            public ContinueEventArgs(int selectedChoiceId)
            {
                SelectedChoiceId = selectedChoiceId;
            }
        }
        
        // The main window will subscribe to this event.
        public event EventHandler<ContinueEventArgs> CanContinueChanged;

        //if all scenes skipped, kill all awaits
        private CancellationTokenSource sceneCts;

        public ScenePlayer(MainWin mainWin)
        {
            mw = mainWin;
        }

        public void Play()
        {
            int starterPack_index = 1;
            PlayPack(starterPack_index);
        }

        //DESERIALIZER
        private async void PlayPack(int sp_id)
        {
            string fileName = "packs/" + "pack" + sp_id + ".json";
            
            if (!File.Exists(fileName))
            {
                MessageBox.Show("Scenepack file not found: " + fileName);
                Ending();
                return;
            }

            // Read JSON synchronously
            string json = File.ReadAllText(fileName);
            // Deserialize JSON
            ScenePack pack = JsonSerializer.Deserialize<ScenePack>(json);
            currentPack = pack;

            if (pack == null)
            {
                new ErrorHandler(false, "Failed to deserealize");
            }
            if (pack.Scenepack_id != sp_id)
            {
                new ErrorHandler(false,"Scenepack id isn't the same as requested");
            }

            // Start playing from the first scene
            int starterScene_index = 1;
            // Create a new CancellationTokenSource for this scene sequence
            sceneCts = new CancellationTokenSource();
            await PlayScene(pack, starterScene_index, sceneCts.Token);
        }
        private async Task PlayScene(ScenePack pack, int scene_id, CancellationToken token)
        {
            Scene scene = pack.Scenes[scene_id-1];
            if (scene == null)
            {
                new ErrorHandler(false, "Scene not found in pack");
                return;
            }
            currentScene = scene;
            MessageBox.Show("Playing Scene " + scene.Scene_id);

            //Use scene-specific background and characters if they exist, otherwise use pack defaults
            string background = scene.Background_image ?? pack.Background_image;
            List<string> characters = scene.Characters ?? pack.Characters;

            //Get dialogue text and choices
            string dialogue = scene.Dialogue;
            string[] choices = scene.Choices?.Select(c => c.Text).ToArray();

            UpdateSceneGUI(background, characters, dialogue, choices);


            // Wait for the player to make a choice, and get the ID
            try
            {
                int selectedChoiceId = await WaitUntilCanContinueAsync(token);
                //No choices made
                if (selectedChoiceId == 0)
                {
                    //Continue to next scene in pack
                    if (scene_id < pack.Scenes.Count)
                    {
                        await PlayScene(pack, scene_id + 1, token);
                    }
                    //Last scene in pack, next pack
                    else
                    {
                        PlayPack(pack.Scenepack_id + 1);
                    }
                    return;
                }
                else// if (scene.Choices != null)
                {
                    //find id of choice made, go to outcome scenepack
                    Choice selectedChoice = scene.Choices.FirstOrDefault(c => c.Choice_id == selectedChoiceId);
                    PlayPack(selectedChoice.Outcome);
                }
            }
            catch (TaskCanceledException)
            {
                // Scene was skipped, just exit immediately
                return;
            }
        }


        //MainWin calls this method when player makes a choice
        public void AllowContinue(int selectedChoice_id)
        {
            if (CanContinueChanged != null)
                CanContinueChanged(this, new ContinueEventArgs(selectedChoice_id));
        }

        //Waits until MainWin signals that player made a choice
        private Task<int> WaitUntilCanContinueAsync(CancellationToken token)
        {
            var tcs = new TaskCompletionSource<int>();

            EventHandler<ContinueEventArgs> handler = null;
            handler = (sender, e) =>
            {
                tcs.TrySetResult(e.SelectedChoiceId);
                CanContinueChanged -= handler;
            };

            CanContinueChanged += handler;

            // If the token is cancelled, cancel the task
            token.Register(() =>
            {
                tcs.TrySetCanceled();
                CanContinueChanged -= handler;
            });

            return tcs.Task;
        }



        //Other functionality
        public async Task SkipToLastScene()
        {
            // Cancel all currently waiting scenes
            sceneCts?.Cancel();

            // Create a new token for the last scene
            sceneCts = new CancellationTokenSource();

            // Start last scene with new token
            await PlayScene(currentPack, currentPack.Scenes.Count, sceneCts.Token);
        }


        //Updates the GUI with the current scene's assets and text
        private void UpdateSceneGUI(string background, List<string> characters, string dialogue, string[] choices = null)
        {
            //Directories
            string imgsDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "imgs");

            //Load static assets like background and characters
            string backgroundPath = System.IO.Path.Combine(imgsDir, background);
            string[] charFiles = new string[characters.Count];
            
            for (int i = 0 ; i < charFiles.Length; i++)
            {
                string charPath = System.IO.Path.Combine(imgsDir, characters[i]);
                charFiles[i] = charPath;
            }
            mw.SetGUI(backgroundPath, charFiles, choices, dialogue);
        }

        private void Ending()
        {
            //MessageBox.Show("No more scenepacks available. The End.");
        }
    }
}
