using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Dilemma
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //THIS IS WHERE THE APP BEGINS
            var startWindow = new StartWin();
            var mainWindow = new MainWin();

            //WE START WITH STARTWIN, THEN GO TO MAINWIN VIA ORCHESTRATOR
            var orchestrator = new AppOrchestrator(startWindow, mainWindow);
            orchestrator.Run();
        }
    }
}
