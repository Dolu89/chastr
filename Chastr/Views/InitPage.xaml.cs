using NBitcoin;
using NBitcoin.Secp256k1;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Chastr.Utils;
using Xamarin.Essentials;
using Chastr.Websocket;

namespace Chastr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitPage : ContentPage
    {
        private const int KEY_SIZE = 32;

        public InitPage()
        {
            InitializeComponent();
        }

        private async void btnGenPrivateKey_Clicked(object sender, EventArgs e)
        {
            var privKey = Context.Instance.CreateECPrivKey(GenerateRandomScalar());
            var pubKey = privKey.CreatePubKey().ToXOnlyPubKey();
            await SecureStorage.SetAsync(Constants.PRIVATE_KEY, ByteArrayToString(privKey.sec.ToBytes()));
            await SecureStorage.SetAsync(Constants.PUBLIC_KEY, ByteArrayToString(pubKey.ToBytes()));

            RelaysPool.Startup();
            await Navigation.PopModalAsync();
        }

        public static Scalar GenerateRandomScalar()
        {
            Scalar scalar = Scalar.Zero;
            Span<byte> output = stackalloc byte[KEY_SIZE];
            do
            {
                RandomUtils.GetBytes(output);
                scalar = new Scalar(output, out int overflow);
                if (overflow != 0 || scalar.IsZero)
                {
                    continue;
                }
                break;
            } while (true);
            return scalar;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
    }
}