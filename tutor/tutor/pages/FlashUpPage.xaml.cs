using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tutor.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashUpPage : ContentPage
    {

        public FlashUpPage()
        {
            InitializeComponent();

            
            string[] subname = new string[10];


            //Creates a stacklayout.
            StackLayout stackModeInt = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            //Adds this stack to the mainStack, allowing it to take up only the first stack space in the mainStack.
            mainFlashStack.Children.Add(stackModeInt);


            //———————————————————————————————————————————— MODE PICKER ————————————————————————————————————————————//
            //Creates the "Select Mode" picker
            Picker pickMode = new Picker
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Select Mode"
            };
            pickMode.Items.Add("Timed");
            pickMode.Items.Add("Randomized");

            //———————————————————————————————————————————— INTERVAL PICKER ————————————————————————————————————————————//
            //Creates the "Select Interval" picker.
            Picker pickInterval = new Picker
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                Title = "Interval",
                IsEnabled = false
            };

            int pm = 0;
            int pi = 0;
            pickMode.SelectedIndexChanged += (sender, args) =>
            {
                pickInterval.IsEnabled = true;
                if (pickMode.SelectedIndex == 0)
                {
                    pickInterval.Items.Remove("5 Cards");
                    pickInterval.Items.Remove("10 Cards");
                    pickInterval.Items.Remove("15 Cards");
                    //Time intervals
                    pickInterval.Items.Add("3 Minutes");
                    pickInterval.Items.Add("5 Minutes");
                    pickInterval.Items.Add("10 Minutes");
                    //Set variable to pass through to next page
                    pm = pickMode.SelectedIndex;
                }
                else if (pickMode.SelectedIndex == 1)
                {
                    pickInterval.Items.Remove("3 Minutes");
                    pickInterval.Items.Remove("5 Minutes");
                    pickInterval.Items.Remove("10 Minutes");
                    //Randomized Card intervals
                    pickInterval.Items.Add("5 Cards");
                    pickInterval.Items.Add("10 Cards");
                    pickInterval.Items.Add("15 Cards");
                    //Set variable to pass through to next page
                    pm = pickMode.SelectedIndex;
                }
            };
            pickInterval.SelectedIndexChanged += (sender, e) =>
            {
                pi = pickInterval.SelectedIndex;
            };

            //———————————————————————————————————————————— STACK1 LAYOUT ————————————————————————————————————————————//
            //Adds these buttons to the first stack.
            stackModeInt.Children.Add(pickMode);
            stackModeInt.Children.Add(pickInterval);

            //———————————————————————————————————————————— BUTTON ARRAY ————————————————————————————————————————————//
            //Creates a Button Array and initializes them.
            Button[] btnSub = new Button[10];
            for (int i = 0; i < btnSub.Length; i++)
            {
                btnSub[i] = new Button
                {
                    Text = "",
                    HeightRequest = 150,
                    HorizontalOptions = LayoutOptions.Fill,
                    IsVisible = false
                };
                int z = i;
                //This line calls the BtnSubClick function which is what happens when any Subject button is clicked.
                btnSub[i].Clicked += (sender, EventArgs) => { BtnSubClick(sender, EventArgs, z); }; ;
            }
            string pathSubject = @"/storage/emulated/0/Android/data/com.companyname.tutor/files/SaveSubjects.txt";
            if (File.Exists(pathSubject))
            {
                int c = 0;
                foreach (string line in File.ReadLines(pathSubject, Encoding.UTF8))
                {
                    subname[c] = line;
                    c++;
                }
                for (int i = 0; i < btnSub.Length; i++)
                {
                    if (subname[i] != "")
                    {
                        btnSub[i].IsVisible = true;
                        btnSub[i].Text = subname[i];
                        mainFlashStack.Children.Add(btnSub[i]);
                    }
                }
            }
            //———————————————————————————————————————————— SUBJECT CLICK ————————————————————————————————————————————//
            //When any Subject button is clicked, it calls this function
            async void BtnSubClick(object sender, EventArgs e, int z)
            {
                //This Function will take the user to the Flashcard Page
                if (pickMode.SelectedIndex != -1 && pickInterval.SelectedIndex != -1)
                {
                    await Navigation.PushAsync(new FlashUp1Page(pm, pi, z));
                }
                else
                {
                    await DisplayAlert("Mode or Interval Not Selected!", "Please make sure both field settings are selected.", "Ok");
                }
                //Also add a way to select which button in the array was selected [i]
                //This will help in filtering which flashcards are pulled from the storage.
            }
        }
    }
}