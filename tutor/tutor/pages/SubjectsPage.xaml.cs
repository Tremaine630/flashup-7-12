using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tutor.services.File;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tutor.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectsPage : ContentPage
    {
        public SubjectsPage()
        {
            InitializeComponent();
            string strKB = "";
            string[] subname = new string[10];
            //———————————————————————————————————————————— STACK SLOT1 ————————————————————————————————————————————//
            //Creates a stacklayout.
            StackLayout stackAddRem = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            //Adds this stack to the mainStack, allowing it to take uz only the first stack space in the mainStack.
            mainStack.Children.Add(stackAddRem);

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

            //———————————————————————————————————————————— ADD SUBJECT BUTTON ————————————————————————————————————————————//
            //Creates the "Add Subject" button
            Button btnAddSub = new Button
            {
                Text = "Add Subject",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //This is what happens when the "Add Subject" button is clicked:
            btnAddSub.Clicked += async (sender, e) =>
            {
                strKB = ""; //Empties the string if it is already populated.
                //This loop checks through each button in the array and checks its properties to see if it is inside the main Stack Layout.
                for (int i = 0; i < btnSub.Length; i++)
                {
                    if (btnSub[i].IsVisible == true)
                    {
                        continue;
                    }
                    if (btnSub[i].IsVisible == false)
                    {
                        btnSub[i].IsVisible = true;
                        //Displays text prompt asking user to enter a Subject Name. Will not accept an empty string.
                        while (strKB == "")
                        {
                            strKB = await DisplayPromptAsync("Add A Subject", "Subject Name:", placeholder: "Subject", keyboard: Keyboard.Chat, maxLength: 30);
                            for (int y = 0; y < btnSub.Length; y++)
                            {
                                if (strKB == btnSub[y].Text)
                                {
                                    await DisplayAlert("Add Card", "Card already exist", "Ok");
                                    strKB = "";
                                    break;
                                }
                            }
                        }
                        btnSub[i].Text = strKB;             //Sets the Button text to the strKB.
                        mainStack.Children.Add(btnSub[i]);  //Puts the button in the stack to display on screen.
                        saveSubFile();                      //Saves the file
                        break;                              //Breaks the loop
                    }
                    if (btnSub[i].Text == "")
                    {
                        btnSub[i].IsVisible = false;
                        mainStack.Children.Remove(btnSub[i]);
                        saveSubFile();
                    }
                }
            };

            //———————————————————————————————————————————— EDIT SUBJECT BUTTON ————————————————————————————————————————————//
            //Creates the "Edit Subject" button.
            Button btnEditSub = new Button
            {
                Text = "Edit",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill
            };
            //This is what happens when the "Edit Subject" button is clicked:
            btnEditSub.Clicked += async (sender, e) =>
            {
                string l = await DisplayPromptAsync("Edit", "Which subject do you want to edit?:", placeholder: "Subject", keyboard: Keyboard.Chat, maxLength: 150);
                for (int i = 0; i < btnSub.Length; i++)
                {
                    if (l == btnSub[i].Text)
                    {
                        strKB = await DisplayPromptAsync("Edit", "Subject:", placeholder: "Subject", keyboard: Keyboard.Chat, maxLength: 150);
                        btnSub[i].Text = strKB;
                        saveSubFile();
                    }
                }
            };

            //———————————————————————————————————————————— REMOVE SUBJECT BUTTON ————————————————————————————————————————————//
            //Creates the "Remove Subject" button.
            Button btnRemoveSub = new Button
            {
                Text = "Remove",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill
            };
            //This is what happens when the "Remove" button is clicked:
            btnRemoveSub.Clicked += async (sender, e) =>
            {
                //Asks the user which card they want to remove
                string l = await DisplayPromptAsync("Remove", "Which Subject do you want to remove?:", placeholder: "Subject", keyboard: Keyboard.Chat, maxLength: 150);
                for (int i = 0; i < btnSub.Length; i++)
                {
                    if (l == btnSub[i].Text && btnSub[i].IsVisible == true)
                    {
                        btnSub[i].IsVisible = false;
                        btnSub[i].Text = "";

                        mainStack.Children.Remove(btnSub[i]);

                        string pathFront = @"/storage/emulated/0/Android/data/com.companyname.tutor/files/SaveCardFront" + i + ".txt";
                        string pathBack = @"/storage/emulated/0/Android/data/com.companyname.tutor/files/SaveCardBack" + i + ".txt";
                        //Remove flashcardFront associated with Subject
                        if (File.Exists(pathFront))
                        {
                            File.Delete(pathFront);
                        }

                        //Remove flashcardBack associated with Subject
                        if (File.Exists(pathBack))
                        {
                            File.Delete(pathBack);
                        }

                        saveSubFile();
                        await DisplayAlert("Remove", "Subject was removed", "ok");
                        break;
                    }
                    else if (i == btnSub.Length)
                    {
                        await DisplayAlert("Remove", "Subject was not found", "ok");
                    }
                }
            };

            //———————————————————————————————————————————— SAVE SUBJECT BUTTON ————————————————————————————————————————————//
            /*Button btnSave = new Button()
            {
                Text = "Save",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill
            };
            btnSave.Clicked += (sender, e) =>
            {
                saveSubFile();
            };*/

            //———————————————————————————————————————————— STACK1 LAYOUT ————————————————————————————————————————————//
            //Adds these buttons to the first stack.
            stackAddRem.Children.Add(btnAddSub);
            stackAddRem.Children.Add(btnEditSub);
            stackAddRem.Children.Add(btnRemoveSub);
            //stackAddRem.Children.Add(btnSave);

            //———————————————————————————————————————————— RECALL FROM FILE ————————————————————————————————————————————//
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
                        mainStack.Children.Add(btnSub[i]);
                    }
                }
            }

            //———————————————————————————————————————————— SUBJECT CLICK ————————————————————————————————————————————//
            //When any Subject button is clicked, it calls this function
            async void BtnSubClick(object sender, System.EventArgs e,int z)
            {
                string subN = "";
                subN = btnSub[z].Text;
                await Navigation.PushAsync(new Sub1Page(z, subN));
            }

            void saveSubFile()
            {
                for (int i = 0; i < btnSub.Length; i++)
                {
                    subname[i] = btnSub[i].Text;
                }
                string filename1 = "SaveSubjects.txt";
                DependencyService.Get<IFileService>().CreateFile(subname, filename1);

                //DisplayAlert("Saved", "Subjects Were Saved", "OK");
            }
        }
    }
}