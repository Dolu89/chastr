﻿using Chastr.ViewModels.Contacts;
using Xamarin.Forms;

namespace Chastr.Views.Contacts
{
    public partial class ContactDetailPage : ContentPage
    {
        public ContactDetailPage()
        {
            InitializeComponent();
            BindingContext = new ContactDetailViewModel();
        }
    }
}