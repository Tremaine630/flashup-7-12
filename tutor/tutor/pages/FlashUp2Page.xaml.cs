using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tutor.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashUp2Page : ContentPage
    {
        public FlashUp2Page(int pm)
        {
            InitializeComponent();

            Label lbl = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "",
                FontSize = 30
            };
            flashUp2Stack.Children.Add(lbl);

            Button btnHome = new Button
            {
                Text = "Continue to Home",
                HeightRequest = 200,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };
            flashUp2Stack.Children.Add(btnHome);
            btnHome.Clicked += async (sender, EventArgs) =>
            {
                await Navigation.PushAsync(new FlashUpPage());
            };

            //This code will show on screen IF the previous screen was TIMED
            if (pm == 0)
            {
                lbl.Text = "TIME'S UP!";
            }

            //This code will show on screen IF the previous screen was RANDOMIZED
            if (pm == 1)
            {
                lbl.Text = "OUT OF CARDS!";
            }
        }
    }
}