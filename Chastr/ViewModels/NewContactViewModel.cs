using Chastr.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chastr.ViewModels
{
    public class NewContactViewModel : BaseViewModel<Contact>
    {
        private string pubKey;

        public NewContactViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(pubKey);
        }

        public string PubKey
        {
            get => pubKey;
            set => SetProperty(ref pubKey, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Contact newContact = new Contact()
            {
                Id = Guid.NewGuid().ToString(),
                PubKey = PubKey
            };

            await DataStore.AddItemAsync(newContact);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
