using Chastr.Models;
using Chastr.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Chastr.ViewModels.Messages
{
    [QueryProperty(nameof(ContactPublicKey), nameof(ContactPublicKey))]
    public class MessagesViewModel : BaseViewModel<Message>
    {
        private Message _selectedItem;
        private string contactPublicKey;

        public ObservableCollection<Message> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Message> ItemTapped { get; }

        public MessagesViewModel()
        {
            Title = "Messages";
            Items = new ObservableCollection<Message>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            AddItemCommand = new Command(OnAddItem);
        }

        public string ContactPublicKey
        {
            get
            {
                return contactPublicKey;
            }
            set
            {
                contactPublicKey = value;
                LoadMessagesByContactId(value);
            }
        }

        public async void LoadMessagesByContactId(string contactId)
        {
            try
            {
                var messages = await DataStore.GetItemsAsync();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Contact");
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetQueryableItemsAsync().Where(i =>
                    // From contact
                    i.PublicKey == ContactPublicKey
                    // or sent the contact
                    //i.Tags.Exists(t => 
                    //    t.Data.Contains(ContactId))
                    )
                .ToListAsync();

                var privateKey = await SecureStorage.GetAsync(Constants.PRIVATE_KEY);

                foreach (var item in items)
                {
                    var encrypted = item.Content.Split("?iv=");
                    var encryptedText = encrypted[0];
                    var iv = encrypted[1];
                    var txt = AES.Decrypt(encryptedText, iv, privateKey.ToLower(), item.PublicKey);
                    item.Content = txt;
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Message SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        private async void OnAddItem(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewMessagePage));
        }
    }
}