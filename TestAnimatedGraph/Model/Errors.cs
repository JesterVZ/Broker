using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TestAnimatedGraph.Model
{
    class Errors: IErrors
    {
        public void InsufficientFunds(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
