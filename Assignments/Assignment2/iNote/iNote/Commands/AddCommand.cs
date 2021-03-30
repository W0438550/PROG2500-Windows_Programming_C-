using iNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace iNote.Commands
{
    public class AddCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private NoteFileViewModel NFViewModel;
        private TextBox MyTextBox;

        public AddCommand(NoteFileViewModel nfViewModel, TextBox textBox)
        {
            NFViewModel = nfViewModel;
            MyTextBox = textBox;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        // Add a note file 
        public void Execute(object parameter)
        {
            NFViewModel.SelectedFile = new Models.NoteFile("", "");

            // make selectable/unselectable of command buttons
            MyTextBox.IsReadOnly = false;
            NFViewModel.canDelete = false;
            NFViewModel.canSave = true;
            NFViewModel.canEdit = false;
            NFViewModel.isNew = true;

            NFViewModel.ChangesMade();
            NFViewModel.PerformFiltering();
            NFViewModel.CreateAListOfFiles();

        }

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
