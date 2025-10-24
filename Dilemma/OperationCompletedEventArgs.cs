using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma
{
    public class OperationCompletedEventArgs : EventArgs
    {
        public bool IsSuccessful { get; }
        public string Message { get; }

        public OperationCompletedEventArgs(bool isSuccessful, string message = "")
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }

}
