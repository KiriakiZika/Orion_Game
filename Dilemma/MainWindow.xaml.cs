using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dilemma
{
    public partial class MainWin : Window, IMainWin
    {
        public MainWin()
        {
            InitializeComponent();

            //define start image:
            string filename = "image.jps";
            AddBackground(filename);
        }

        public void AddBackground(string filename)
        {

        }
    }
}
