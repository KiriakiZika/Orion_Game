using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dilemma
{
    internal class AppOrchestrator
    {
        public static bool IsFinished { get; private set; }
        private readonly IStartWin _startWin;
        private readonly IMainWin _mainWin;
        
        public AppOrchestrator(IStartWin startWin, IMainWin mainWin)
        {
            _startWin = startWin;
            _mainWin = mainWin;

            _startWin.OperationCompleted += OnClassACompleted;
        }
        public void Run()
        {
            IsFinished = false;
            _startWin.Show();
        }
        private void OnClassACompleted(object sender, OperationCompletedEventArgs e)
        {
            if (!e.IsSuccessful)
                MessageBox.Show($"Start Window failed. Error: {e.Message}");

            IsFinished = true;
            TransitionToMainWin();
        }
        private void TransitionToMainWin()
        {
            _startWin.Close();
            _mainWin.Show();
        }
    }
}
