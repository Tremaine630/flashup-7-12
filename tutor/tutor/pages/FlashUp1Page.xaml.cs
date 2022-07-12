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
    public partial class FlashUp1Page : ContentPage
    {
        public FlashUp1Page(int pm, int pi, int z)
        {
            InitializeComponent();
            Random r = new Random();
            //int cLength = 10;

            //This counts how many lines are populated in the text file and stores it as an int (counti)
            int counti = 0;
            string pathBack = @"/storage/emulated/0/Android/data/com.companyname.tutor/files/SaveCardBack" + z + ".txt";
            string pathFront = @"/storage/emulated/0/Android/data/com.companyname.tutor/files/SaveCardFront" + z + ".txt";
            if (File.Exists(pathFront))
            {
                foreach (string line in File.ReadLines(pathFront, Encoding.UTF8))
                {
                    if (line != "")
                    {
                        counti++;
                    }
                }
            }
            Console.WriteLine(counti);

            string[] CardBack = new string[counti];
            string[] CardFront = new string[counti];
            int[] doneCards = new int[counti];
            int nc = 0;
            bool newCard = true;

            int piSI0 = 5;
            int piSI1 = 10;
            int piSI2 = 15;
            int dur0 = 10; //seconds
            int dur1 = 300; //seconds
            int dur2 = 600; //seconds

            //TESTING PURPOSES
            //Console.WriteLine("fu1-pm: " + pm); 
            //Console.WriteLine("fu1-pi: " + pi); 

            //Create a Progress Bar
            ProgressBar pb = new ProgressBar();
            pb.WidthRequest = 200;
            pb.HeightRequest = 30;
            pb.IsVisible = false;



            //initialize variables to send through to a function
            uint pbduration = 0;    //Progress Bar Duration
            int tduration = 0;      //Timer Duration (constant)
            int tcount = 0;         //Timer Count (Counts every interval until selected is reached)
            int lblcountdown = 0;   //countdown for the Label Text
            string lcount = "";     //Label Countdown Text for backwards Timer Duration Countdown
            int counter = 1;
            int piselect = 0;

            //Creates a Timer/Label for the progress bar
            Label lblTimer = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = lcount
            };
            flashUp1Stack.Children.Add(lblTimer);

            Label lblCounter = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = counter.ToString()
            };
            flashUp1Stack.Children.Add(lblCounter);

            //Switch case for 'pm' & 'pi' selection
            switch (pm)
            {
                case 0:
                    //code to input into FlashUp1Page (TIMED)
                    //For Timed
                    lblTimer.IsVisible = true;
                    pb.IsVisible = true;

                    //For Randomized
                    lblCounter.IsVisible = false;

                    break;
                case 1:
                    //code to input into FlashUp1Page (RANDMIZED)
                    //For Timed
                    lblTimer.IsVisible = false;
                    pb.IsVisible = false;

                    //For Randomized
                    lblCounter.IsVisible = true;


                    break;
            }

            switch (pi)
            {
                case 0:
                    if (pm == 0)
                    {
                        //If mode = Timed
                        //code to input into progress ring
                        pbduration = (uint)dur0 * 1000; ; //Duration = 3 Minutes (in milliseconds)
                        tduration = dur0;
                        lblcountdown = tduration;
                        break;
                    }
                    if (pm == 1)
                    {
                        //If mode = Randomized
                        //code to input into progress ring
                        piselect = piSI0;
                        lblCounter.Text = "1/" + piselect;
                        break;
                    }
                    break;
                case 1:
                    if (pm == 0)
                    {
                        //If mode = Timed
                        //code to input into progress ring
                        pbduration = (uint)dur1 * 1000; //Duration = 5 Minutes (in milliseconds)
                        tduration = dur1; //Duration (in Seconds)
                        lblcountdown = tduration;
                        break;
                    }
                    if (pm == 1)
                    {
                        //If mode = Randomized
                        //code to input into progress ring
                        piselect = piSI1;
                        lblCounter.Text = "1/" + piselect;
                        break;
                    }
                    break;
                case 2:
                    if (pm == 0)
                    {
                        //If mode = Timed
                        //code to input into progress ring
                        pbduration = (uint)dur2 * 1000; ; //Duration = 10 Minutes (in milliseconds)
                        tduration = dur2; //Duration (in Seconds)
                        lblcountdown = tduration;
                        break;
                    }
                    if (pm == 1)
                    {
                        //If mode = Randomized
                        //code to input into progress ring
                        piselect = piSI2;
                        lblCounter.Text = "1/" + piselect;
                        break;
                    }
                    break;
            }


            Button[] btnCard = new Button[counti];
            for (int i = 0; i < btnCard.Length; i++)
            {
                btnCard[i] = new Button
                {
                    Text = "",
                    HeightRequest = 150,
                    HorizontalOptions = LayoutOptions.Fill,
                    IsVisible = false
                };
                int p = i;
                //This line calls the BtnSubClick function which is what happens when any Subject button is clicked.
                btnCard[i].Clicked += (sender, EventArgs) => { BtnCardClick(sender, EventArgs, p, r); };
            }

            //gets the cards fronts and backs
            if (File.Exists(pathBack))
            {
                int c = 0;
                foreach (string line in File.ReadLines(pathBack, Encoding.UTF8))
                {
                    if (line != "")
                    {
                        CardBack[c] = line;
                        c++;
                    }                }
            }
            if (File.Exists(pathFront))
            {
                int c = 0;
                foreach (string line in File.ReadLines(pathFront, Encoding.UTF8))
                {
                    if (line != "")
                    {
                        CardFront[c] = line;
                        c++;
                    }
                }
                for (int i = 0; i < btnCard.Length; i++)
                {
                    if (CardFront[i] != "")
                    {

                        btnCard[i].Text = CardFront[i];
                    }
                }
            }

            async void BtnCardClick(object sender, System.EventArgs e, int p, Random rr)
            {
                //Current card
                await DisplayAlert("Answer", CardBack[p], "ok");
                doneCards[p] = p + 1;
                nc++;
                btnCard[p].IsVisible = false;
                flashUp1Stack.Children.Remove(btnCard[p]);
                if (nc == counti)
                {
                    Array.Clear(doneCards, 0, doneCards.Length);
                    nc = 0;
                }
                int k = rr.Next(counti);
                newCard = Cardcheck(k);

                if (newCard == false || p == k)
                {
                    while (newCard == false || p == k)
                    {
                        k = rr.Next(counti);
                        newCard = Cardcheck(k);
                    }
                }

                btnCard[k].IsVisible = true;

                flashUp1Stack.Children.Add(btnCard[k]);

                //If mode is set to RANDOMIZED
                if (pm == 1)
                {
                    //Increase the count by 1 since we already start on 1;
                    counter++;
                    //Sets the label text to "x/piselect"
                    lblCounter.Text = counter.ToString() + "/" + piselect.ToString();
                    //Runs a check - If the counter is equal to the piselect + 1
                    if (counter == piselect + 1)
                    {
                        //Empty the Label text
                        lblCounter.Text = "";
                        //reset the counter
                        counter = 0;
                        //open new page
                        await Navigation.PushAsync(new FlashUp2Page(pm));
                    }
                }

            }
            bool Cardcheck(int k)
            {
                bool ret;
                for (int i = 0; i < btnCard.Length; i++)
                {
                    if (doneCards[i] == k + 1)
                    {
                        ret = false;
                        return ret;
                    }
                }
                ret = true;

                return ret;
            }

            //Progressbar Settings
            if (pm == 0)
            {
                pb0Settings(pbduration, tduration, lblcountdown, counti); //The function called initiates the first card, timer, and the progress bar at the same time
            }
            if (pm == 1)
            {
                pb1Settings(counti); //This function called just initiates the first card.
            }

            async void pb0Settings(uint duration, int durationtimer, int lblcd, int cL)
            {
                //Timer setup
                Timer t = new Timer(TimerCallback, null, 0, 1000); //Calls function every 1000ms or 1sec

                void TimerCallback(object o)
                {
                    if (tcount >= tduration) //it counts every second until the selected index interval is reached
                    {

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //Open a new page and pass in the PM variable to set what shows on the next page
                            pb.IsVisible = false;
                            lblTimer.Text = "TIME'S UP!";
                            //open new page
                            await Navigation.PushAsync(new FlashUp2Page(pm));
                        });


                        //When the progress bar is filled, this timer should also be completed at the same time, so the code here would
                        //clear the screen and/or open a new page that just has a big "Time's Up!"

                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //This sets a countdown in seconds for the Label Text
                            lblTimer.Text = lblcd.ToString();
                            lblcd--;
                        });
                    }
                    //This increases the count increment until the selected Timed Interval is reached.
                    tcount++;
                }

                //PB setup
                flashUp1Stack.Children.Add(pb);

                //Adds a randome first card to the deck
                Random randNum = new Random();
                int h = randNum.Next(0, cL - 1);
                btnCard[h].IsVisible = true;
                flashUp1Stack.Children.Add(btnCard[h]);

                //initiates the Progress bar
                await pb.ProgressTo(1, duration, Easing.Linear);
                //When it is finished, dispose of the timer so it doesn't continue in the background
                t.Dispose();


            }

            void pb1Settings(int cL)
            {
                //Adds a randome first card to the deck
                Random randNum = new Random();
                int h = randNum.Next(0, cL - 1);
                btnCard[h].IsVisible = true;
                flashUp1Stack.Children.Add(btnCard[h]);
            }
        }
    }
}