using LocalNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace LocalNote.Commands
{
    public class EditCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private NoteFileViewModel NFViewModel;
        private TextBox myTextBox;

        public EditCommand(NoteFileViewModel nfViewModel, TextBox textBox)
        {
            NFViewModel = nfViewModel;
            myTextBox = textBox;
        }

        public bool CanExecute(object parameter)
        {
            return NFViewModel.canEdit;
        }

        public void Execute(object parameter)
        {
            // make button selectable and unselectable
            myTextBox.IsReadOnly = false;
            NFViewModel.canEdit = false;
            NFViewModel.isNew = false;
            NFViewModel.canSave = true;

            NFViewModel.ChangesMade();
        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
