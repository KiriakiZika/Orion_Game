using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma
{
    public class ScenePack
    {
        public int Scenepack_id { get; set; } //scenepack identifier
        public string Background_image { get; set; } //default background for scenes in this pack
        public List<Scene> Scenes { get; set; } = new List<Scene>(); //list of scenes in this pack
    }
}
