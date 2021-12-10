using NBitcoin;
using NBitcoin.DataEncoders;
using NBitcoin.Secp256k1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Chastr.Utils;
using Xamarin.Essentials;

namespace Chastr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitPage : ContentPage
    {
        private const int KEY_SIZE = 32;
        public string privateKey { get; set; }
        public string publicKey { get; set; }

        public InitPage()
        {
            InitializeComponent();
        }

        private async void btnGenPrivateKey_Clicked(object sender, EventArgs e)
        {
            var eckey = Context.Instance.CreateECPrivKey(Generate256BitsOfRandomEntropy());
            var ecpubkey = eckey.CreatePubKey();
            await SecureStorage.SetAsync(Constants.PRIVATE_KEY, ByteArrayToString(eckey.sec.ToBytes()));
            await SecureStorage.SetAsync(Constants.PUBLIC_KEY, ByteArrayToString(ecpubkey.ToBytes()));

            await Navigation.PopModalAsync();
        }

        public static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[KEY_SIZE];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
    }
}