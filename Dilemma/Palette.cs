using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Dilemma
{
    internal class Palette
    {
        //https://coolors.co/ffe5d9-ffcad4-f4acb7-9d8189-d8e2dc
        public SolidColorBrush Colour1_champagne { get; set; }
        public SolidColorBrush Colour2_pink { get; set; }
        public SolidColorBrush Colour3_cherry { get; set; }
        public SolidColorBrush Colour4_mountain { get; set; }
        public SolidColorBrush Colour5_platinum { get; set; }

        public Palette()
        {
            Colour5_platinum = (SolidColorBrush)new BrushConverter().ConvertFrom("#D8E2DC");
            Colour1_champagne = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE5D9");
            Colour2_pink = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCAD4");
            Colour3_cherry = (SolidColorBrush)new BrushConverter().ConvertFrom("#F4ACB7");
            Colour4_mountain = (SolidColorBrush)new BrushConverter().ConvertFrom("#9D8189");
        }
    }
}
