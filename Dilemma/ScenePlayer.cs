using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma
{
    public static class ScenePlayer
    {
        //DESERIALIZER
        public static void PlayScene(ScenePack pack, int sceneIndex)
        {
            if (sceneIndex >= pack.Scenes.Count)
            {
                Console.WriteLine("➡️ End of pack. Move to next ScenePack.");
                return;
            }

            var scene = pack.Scenes[sceneIndex];
            string background = scene.Background_image ?? pack.Background_image;

            Console.WriteLine($"🎬 Scene {scene.Scene_id} | Background: {background}");

            // Display characters, dialogue, etc.
            /*foreach (var c in scene.Characters)
                foreach (var kvp in c)
                    Console.WriteLine($"🧍 {kvp.Key} → {kvp.Value}");*/

            if (scene.Choices == null || scene.Choices.Count == 0)
            {
                Console.WriteLine("➡️ No choices — moving to next scene...");
                PlayScene(pack, sceneIndex + 1);
            }
            else
            {
                Console.WriteLine("🪧 Choices:");
                foreach (var choice in scene.Choices)
                    Console.WriteLine($"  [{choice.Choice_id}] {choice.Text}");

                // Example: automatically pick the first one for now
                var chosen = scene.Choices[0];
                Console.WriteLine($"➡️ Outcome: {chosen.Outcome}");
            }
        }
    }
}
