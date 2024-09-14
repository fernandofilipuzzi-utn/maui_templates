using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TakeDialer
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            if (PhoneDialer.IsSupported == true)
            {
                PhoneDialer.Open("343154807427");

            }
            else
            { 
            }
        }
    }

}
