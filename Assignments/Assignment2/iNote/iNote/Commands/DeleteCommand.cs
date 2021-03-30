using iNote.Models;
using iNote.Repositories;
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
    public class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private NoteFileViewModel NFViewModel;

        iNoteRepo iNoteRepo = new iNoteRepo();

        public DeleteCommand(NoteFileViewModel nfViewModel)
        {
            NFViewModel = nfViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return NFViewModel.canDelete;
        }

        public void Execute(object parameter)
        {
            // delete the selected file from the windows storage
            iNoteRepo.DeleteFile(NFViewModel.FileName);

            // set the selected file empty and make unsectable the Edit and Save button
            NFViewModel.SelectedFile = new NoteFile("", "");
            NFViewModel.canEdit = false;
            NFViewModel.canSave = false;

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
