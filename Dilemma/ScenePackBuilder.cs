using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dilemma
{
    public static class ScenePackBuilder
    {
        public static ScenePack CreateSampleScenePack()
        {
            return new ScenePack
            {
                Scenepack_id = 1,
                Background_image = "background.jpg",
                Scenes = new List<Scene>
                {
                    // Scene 1: has choices, own background
                    new Scene
                    {
                        Scene_id = 101,
                        Background_image = "child.jpg",
                        Characters = new List<Dictionary<string, string>>
                        {
                            //new() { { "Character1_image", "hero.png" } },
                            //new() { { "Character2_image", "guide.png" } }
                        },
                        Choices = new List<Choice>
                        {
                            //new() { Choice_id = 1, Text = "Follow the light", Outcome = "light_path.json" },
                            //new() { Choice_id = 2, Text = "Stay on the trail", Outcome = "trail_path.json" }
                        }
                    },

                    // Scene 2: no choices -> next scene in pack
                    new Scene
                    {
                        Scene_id = 102,
                        Characters = new List<Dictionary<string, string>>
                        {
                            //new() { { "Character1_image", "villager.png" } }
                        }
                    }
                }
            };
        }

        public static void SaveScenePack(ScenePack scenePack, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(scenePack, options);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"✅ ScenePack {scenePack.Scenepack_id} saved to {filePath}");
        }
    }
}
