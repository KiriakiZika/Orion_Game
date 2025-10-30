using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dilemma
{
    internal interface IPalette
    {
        Dictionary<string, SolidColorBrush> Colours { get; }
        SolidColorBrush GetColour(string colourName);
    }
}
