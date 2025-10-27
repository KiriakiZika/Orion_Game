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
        public readonly SolidColorBrush Colour1_champagne = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE5D9");
        public readonly SolidColorBrush Colour2_pink = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCAD4");
        public readonly SolidColorBrush Colour3_cherry = (SolidColorBrush)new BrushConverter().ConvertFrom("#F4ACB7");
        public readonly SolidColorBrush Colour4_mountain = (SolidColorBrush)new BrushConverter().ConvertFrom("#9D8189");
        public readonly SolidColorBrush Colour5_platinum = (SolidColorBrush)new BrushConverter().ConvertFrom("#D8E2DC");
        public readonly SolidColorBrush ColourX_error = (SolidColorBrush)new BrushConverter().ConvertFrom("#6F170B");
    }
}
