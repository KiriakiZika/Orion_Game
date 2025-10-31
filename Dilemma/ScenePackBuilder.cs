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
        //CREATOR-------------------------------------
        public static void CreateCostumScenePack(int scenepack_id, string background_image, List<string> characters, List<Scene> scenes)
        {
            ScenePack scenePack = new ScenePack
            {
                Scenepack_id = scenepack_id,
                Background_image = background_image,
                Characters = characters,
                Scenes = scenes
            };

            SaveScenePack(scenePack, $"pack{scenePack.Scenepack_id}.json");
        }
        

        //SERIALIZE------------------------------------
        private static void SaveScenePack(ScenePack scenePack, string filePath)
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
