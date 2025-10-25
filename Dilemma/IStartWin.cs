using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Dilemma
{
    internal interface IStartWin
    {
        event EventHandler<ErrorHandler> OperationCompleted;
        void Show();
        void Close();
    }
}