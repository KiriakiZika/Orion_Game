using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Dilemma
{
    public class ChoiceButton : Button
    {
        public ChoiceButton()
        {
            //Event handlers
            Click += CbClick;
            MouseEnter += CbHover;
        }

        public void CbClick(object sender, EventArgs e)
        {
            MessageBox.Show("Button clicked!");
        }
        public void CbHover(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                //
            }
        }
    }
}
