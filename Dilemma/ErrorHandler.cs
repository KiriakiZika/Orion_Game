using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dilemma
{
    public class ErrorHandler : EventArgs
    {
        public bool IsSuccessful { get; }
        public string Message { get; }

        public ErrorHandler(bool isSuccessful, string message = "")
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public void ShowError()
        {
            MessageBox.Show($"Error: {Message}");
        }
    }

}
