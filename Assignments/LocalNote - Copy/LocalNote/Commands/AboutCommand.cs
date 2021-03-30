using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace LocalNote.Commands
{
    public class AboutCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            // initialize a new text for message dialog
            string message = "UWP application - Local Note.";

            // initialize a new MessageDialog instance
            MessageDialog messageDialog = new MessageDialog(message, "Local Note");

            // display the message dialog
            await messageDialog.ShowAsync();
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
