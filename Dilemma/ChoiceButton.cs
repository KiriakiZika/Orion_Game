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
        MainWin mw;
        public ChoiceButton(MainWin mw)
        {
            this.mw = mw;
            //Event handlers
            Click += CbClick;
            MouseEnter += CbHover;
        }

        public void CbClick(object sender, EventArgs e)
        {
            //Check sender type
            var btn = sender as ChoiceButton;
            if (btn == null)
            {
                return;
            }

            //send id of selected choice
            mw.sp.AllowContinue((int)btn.Tag);

            // Move focus to the window
            mw.Focus();  // set logical focus to the window
            FocusManager.SetFocusedElement(mw, mw);
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
