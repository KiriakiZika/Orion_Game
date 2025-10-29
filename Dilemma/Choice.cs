using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma
{
    public class Choice
    {
        public int Choice_id { get; set; } //choice identifier
        public string Text { get; set; } //displayed choice text
        public string Outcome { get; set; } //JSON filename
    }
}
