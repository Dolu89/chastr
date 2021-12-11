using Chastr.Models;
using Chastr.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chastr.Views
{
    public partial class NewContactPage : ContentPage
    {
        public Contact Contact { get; set; }

        public NewContactPage()
        {
            InitializeComponent();
            BindingContext = new NewContactViewModel();
        }
    }
}