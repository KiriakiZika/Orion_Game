using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dilemma
{
    public class Scene
    {
        public int Scene_id { get; set; } //scene identifier

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] //can be null
        public string Background_image { get; set; } //override scenepack background, use this

        public List<Dictionary<string, string>> Characters { get; set; } //character images

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] //can be null - if there are no choices, go to next scene in pack
        public List<Choice> Choices { get; set; } //if there are choices, wait for player input, then go to outcome
    }
}
