using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Dilemma
{
    public class ScenePlayer
    {
        MainWin mw;

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


        public ScenePlayer(MainWin mainWin)
        {
            mw = mainWin;
        }

        public void Play()
        {
            PlayPack(1);
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

            if (pack == null)
            {
                new ErrorHandler(false, "Failed to deserealize");
            }
            if (pack.Scenepack_id != sp_id)
            {
                new ErrorHandler(false,"Scenepack id isn't the same as requested");
            }

            // Start playing from the first scene
            for (int i = 1; i <= pack.Scenes.Count; i++)
            {
                await PlayScene(pack, i);
            }
        }
        private async Task PlayScene(ScenePack pack, int scene_id)
        {
            Scene scene = pack.Scenes[scene_id-1];
            if (scene == null)
            {
                new ErrorHandler(false, "Scene not found in pack");
                return;
            }

            //Use scene-specific background and characters if they exist, otherwise use pack defaults
            string background = scene.Background_image ?? pack.Background_image;
            List<string> characters = scene.Characters ?? pack.Characters;
            
            //Get dialogue text and choices
            string dialogue = scene.Dialogue;
            string[] choices = scene.Choices?.Select(c => c.Text).ToArray();

            UpdateSceneGUI(background, characters, dialogue, choices);


            // Wait for the player to make a choice, and get the ID
            int selectedChoiceId = await WaitUntilCanContinueAsync();

            //No choices made
            if (selectedChoiceId == 0) 
            {
                //Last scene in pack, go to next pack
                if (scene_id >= pack.Scenes.Count)
                {
                    PlayPack(pack.Scenepack_id + 1);
                }
                return;
            }
            else 
            {
                //find id of choice made, go to outcome scenepack
                Choice selectedChoice = scene.Choices.FirstOrDefault(c => c.Choice_id == selectedChoiceId);
                PlayPack(selectedChoice.Outcome);
            }
        }


        //MainWin calls this method when player makes a choice
        public void AllowContinue(int selectedChoice_id)
        {
            if (CanContinueChanged != null)
                CanContinueChanged(this, new ContinueEventArgs(selectedChoice_id));
        }

        //Waits until MainWin signals that player made a choice
        private Task<int> WaitUntilCanContinueAsync()
        {
            var tcs = new TaskCompletionSource<int>();

            EventHandler<ContinueEventArgs> handler = null;
            handler = delegate (object sender, ContinueEventArgs e)
            {
                tcs.TrySetResult(e.SelectedChoiceId); // pass the selected ID
                CanContinueChanged -= handler;
            };

            CanContinueChanged += handler;

            return tcs.Task;
        }


        private void UpdateSceneGUI(string background, List<string> characters, string dialogue, string[] choices = null)
        {
            //Directories
            string imgsDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "imgs");

            //Load static assets like background and characters
            string backgroundPath = System.IO.Path.Combine(imgsDir, background);
            string[] charFiles = new string[characters.Count];
            foreach (var ch in characters)
            {
                string charPath = System.IO.Path.Combine(imgsDir, ch);
                charFiles[characters.IndexOf(ch)] = charPath;
            }
            mw.SetGUI(backgroundPath, charFiles, choices, dialogue);
        }

        private void Ending()
        {
            //MessageBox.Show("No more scenepacks available. The End.");
        }
    }
}
