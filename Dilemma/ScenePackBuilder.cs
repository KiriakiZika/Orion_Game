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
            ScenePack scenePack = new ScenePack
            {
                Scenepack_id = 1,
                Background_image = "background.jpg",
                Scenes = new List<Scene>()
            };

            //Add Scene 1: has choices, own background
            Scene scene1 = new Scene
            {
                Scene_id = 101,
                Background_image = "child.jpg",
                Characters = new List<string>(),
                Dialogue = "You see two figures ahead. Who do you follow?",
                Choices = new List<Choice>()
            };
            scenePack.Scenes.Add(scene1);

            //Create characters for Scene 1
            scene1.Characters.Add("choso.png");
            scene1.Characters.Add("lawliet.png");

            //Create choices for Scene 1
            scene1.Choices.Add(new Choice
            {
                Choice_id = 1,
                Text = "Follow choso",
                Outcome = "pack2.json"
            });
            scene1.Choices.Add(new Choice
            {
                Choice_id = 2,
                Text = "Follow lawliet",
                Outcome = "pack3.json"
            });

            //Add Scene 2: no choices -> next scene in pack
            Scene scene2 = new Scene
            {
                Scene_id = 102,
                //background_image = null, //uses pack background
                Characters = new List<string>(),
                Dialogue = "You chose to follow both characters and ended up lost in the woods.",
            };
            scenePack.Scenes.Add(scene2);

            //Create characters for Scene 2
            scene2.Characters.Add("lawliet.png");
            scene2.Characters.Add("choso.png");

            return scenePack;
        }

        //SERIALIZE
        public static void SaveScenePack(ScenePack scenePack, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(scenePack, options);
            File.WriteAllText("packs/" + filePath, json);
        }
    }
}
