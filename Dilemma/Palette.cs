using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Xml.Linq;

namespace Dilemma
{
    internal class Palette : IPalette
    {
        public Dictionary<string,SolidColorBrush> Colours { get; }
        public Palette() 
        {
            //https://coolors.co/ffe5d9-ffcad4-f4acb7-9d8189-d8e2dc
            SolidColorBrush Colour1_champagne = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE5D9");
            SolidColorBrush Colour2_pink = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCAD4");
            SolidColorBrush Colour3_cherry = (SolidColorBrush)new BrushConverter().ConvertFrom("#F4ACB7");
            SolidColorBrush Colour4_mountain = (SolidColorBrush)new BrushConverter().ConvertFrom("#9D8189");
            SolidColorBrush Colour5_platinum = (SolidColorBrush)new BrushConverter().ConvertFrom("#D8E2DC");
            SolidColorBrush Colour6_darkbrown = (SolidColorBrush)new BrushConverter().ConvertFrom("#120d0a");
            SolidColorBrush Colour7_seashell = (SolidColorBrush)new BrushConverter().ConvertFrom("#FDF5ED");
            SolidColorBrush ColourX_error = (SolidColorBrush)new BrushConverter().ConvertFrom("#6F170B");

            Colours = new Dictionary<string, SolidColorBrush>
            {
                { "Champagne", Colour1_champagne },
                { "Pink", Colour2_pink },
                { "Cherry", Colour3_cherry },
                { "Mountain", Colour4_mountain },
                { "Platinum", Colour5_platinum },
                { "DarkBrown", Colour6_darkbrown },
                { "Seashell", Colour7_seashell },
                { "Error", ColourX_error}
            };
        }

        public SolidColorBrush GetColour(string colourName) 
        {
            //gets colour by name, else gives error colour
            return Colours.TryGetValue(colourName, out var brush) ? brush : Colours["Error"];
        }

    }
}
