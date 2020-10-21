using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TestAnimatedGraph.Model
{
    class Errors: IErrors
    {
        public void ShowMessage(string message, MessageBoxImage messageBoxImage)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, messageBoxImage);
        }
    }
}
