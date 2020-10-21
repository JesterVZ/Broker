using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TestAnimatedGraph.Model
{
    interface IErrors
    {
        public void ShowMessage(string message, MessageBoxImage messageBoxImage);
    }
}
