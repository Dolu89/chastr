using Chastr.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chastr.ViewModels
{
    [QueryProperty(nameof(ContactId), nameof(ContactId))]
    public class ContactDetailViewModel : BaseViewModel<Contact>
    {
        private string contactId;
        private string pubKey;
        public string Id { get; set; }

        public string PubKey
        {
            get => pubKey;
            set => SetProperty(ref pubKey, value);
        }

        public string ContactId
        {
            get
            {
                return contactId;
            }
            set
            {
                contactId = value;
                LoadContactId(value);
            }
        }

        public async void LoadContactId(string contactId)
        {
            try
            {
                var Contact = await DataStore.GetItemAsync(contactId);
                Id = Contact.Id;
                PubKey = Contact.PubKey;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Contact");
            }
        }
    }
}
