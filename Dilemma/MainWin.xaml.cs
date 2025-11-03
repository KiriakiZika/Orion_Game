using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Dilemma
{
    public partial class MainWin : Window, IMainWin
    {
        //UI Components
        private Grid mainGrid;
        private IPalette p = new Palette();
        public ScenePlayer sp;

        //Game Components
        List<(int,bool)> choices = new List<(int,bool)>();

        public MainWin(string background_image = "imgs/error.jpg", string[] charFiles = null, string[] buttonContents = null, string text=null)
        {
            //Event Listeners for window
            this.SizeChanged += Window_SizeChanged;
            this.Closing += OnWindowClosing;
            // Subscribe to PreviewKeyDown for global capture
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            //GUI Setup
            try
            {
                InitializeComponent();
                // --- Window properties ---
                this.Title = "ORION";
                this.WindowState = WindowState.Maximized;
                //
                SetGUI(background_image,charFiles,buttonContents,text);
            }
            catch (Exception ex)
            {
                ErrorHandler error = new ErrorHandler(false,ex.Message);
            }

            //Game start
            try 
            {
                sp = new ScenePlayer(this);
                StartGame();
            }
            catch (Exception ex)
            {
                ErrorHandler error = new ErrorHandler(false, ex.Message);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateVisual();
            this.UpdateLayout();
        }
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Whenever you press Enter, it triggers the Continue button
            if (e.Key == Key.Enter)
            {
                ContinueButtonClicked(sender, e);
            }
        }
        private Grid TryToLoadImage(string imageName)
        {
            //will carry the results
            Grid container = new Grid();

            // Create an Image control
            System.Windows.Controls.Image bgImage = new System.Windows.Controls.Image()
            {
                Stretch = Stretch.UniformToFill,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Try loading main image
            try
            {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageName);
                BitmapImage bitmap = new BitmapImage(new Uri(path, UriKind.Absolute));
                bgImage.Source = bitmap;

                container.Children.Add(bgImage);
            }
            // Load error/fallback image (it is in resources)
            catch (Exception)
            {
                TextBox errorPanel = new TextBox()
                {
                    //text controls
                    Text = "Image couldn't load.",
                    FontFamily = new FontFamily("Reem Kufi"),
                    FontSize = 20,
                    TextAlignment = TextAlignment.Center,
                    Padding = new Thickness(15),
                    Foreground = p.GetColour("Error"),
                    Background = Brushes.Transparent,
                    IsReadOnly = true
                };

                container.Children.Add(errorPanel);
            }
            return container;
        }


        //////////////////////////////////////////////////////
        //UI COMPONENTS
        public void SetGUI(string background_image, string[] charFiles = null, string[] buttonContents = null, string text = null) 
        {
            //Clear previous motherGrid content (if any)
            Grid motherGrid = new Grid();
            motherGrid.ColumnDefinitions.Clear();
            motherGrid.RowDefinitions.Clear();
            motherGrid.Children.Clear();

            //BACKGROUND IMAGE
            Grid background = TryToLoadImage(background_image);
            motherGrid.Children.Add(background);
            

            //Clear previous mainGrid content (if any) + new instance
            mainGrid = new Grid() { Margin = new Thickness(0) };
            mainGrid.ColumnDefinitions.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.Children.Clear();
            motherGrid.Children.Add(mainGrid);

            //CHARACTERS
            FillMainGrid(charFiles, buttonContents);
            //Second row
            SetDialogueBubble(text, buttonContents==null);

            this.Content = motherGrid;
        }

        //IMAGE AND CHOICE PANEL
        private void FillMainGrid(string[] charFiles = null, string[] buttonContents = null)
        {
            Grid char_choices_layer = new Grid() { Margin = new Thickness(0) };
            char_choices_layer.ColumnDefinitions.Clear();
            char_choices_layer.RowDefinitions.Clear();
            char_choices_layer.Children.Clear();

            // Define two columns: left for image, right for choices
            char_choices_layer.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(2, GridUnitType.Star)
            });
            char_choices_layer.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });

            //Create character image (LEFT)
            Grid characterImage = SetCharacters(charFiles);

            // Create choice panel (RIGHT)
            Grid choiceGrid = SetChoices(buttonContents);

            // Add panels to the grid
            Grid.SetColumn(characterImage, 0);
            Grid.SetColumn(choiceGrid, 1);
            char_choices_layer.Children.Add(characterImage);
            char_choices_layer.Children.Add(choiceGrid);

            // Add layer to maingrid
            mainGrid.Children.Add(char_choices_layer);

            // --- Assign content ---
            this.Content = mainGrid;
        }
        //CHARACTERS
        private Grid SetCharacters(string[] charFiles = null)
        {
            Grid container = new Grid();
            container.ColumnDefinitions.Clear();
            container.RowDefinitions.Clear();
            container.Children.Clear();

            //Temp array of images
            if (charFiles == null || charFiles.Length == 0)
            {
                charFiles = new string[] { "imgs/undefined.png", "imgs/undefined.png" };
            }

            for (int i = 0; i < charFiles.Length; i++)
            {
                // Add a new column
                container.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

                // Load and add image=character to the column
                Grid character = TryToLoadImage(charFiles[i]);
                Grid.SetColumn(character, i);
                container.Children.Add(character);
            }

            return container;
        }
        //CHOICE BUTTONS
        private Grid SetChoices(string[] buttonContents = null)
        {
            //Clear previous content
            choices.Clear();

            //Add new content
            for (int i = 0; buttonContents != null && i < buttonContents.Length; i++)
            {
                choices.Add((i, false));
            }

            // Create container Grid
            Grid container = new Grid()
            {
                Margin = new Thickness(10)
            };
            container.ColumnDefinitions.Clear();
            container.RowDefinitions.Clear();
            container.Children.Clear();

            if (buttonContents == null || buttonContents.Length == 0)
            {
                return container;
            }

            // Split container vertically into 2 rows
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2.5, GridUnitType.Star) }); // top spacing
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // buttons

            // Create a nested Grid to hold buttons
            Grid buttonGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Bottom, // inside its row
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            //temp array to hold buttons
            ChoiceButton[] buttons = new ChoiceButton[buttonContents.Length];
            //Create rows and buttons
            for (int i = 0; i < buttonContents.Length; i++)
            {
                buttonGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                ChoiceButton cb = new ChoiceButton(this)
                {
                    Tag = i+1,
                    Content = $"{buttonContents[i]}",
                    FontFamily = new FontFamily("Reem Kufi"),
                    FontSize = 20,
                    Padding = new Thickness(10), //inside
                    Margin = new Thickness(10), //outside
                    Background = p.GetColour("Mountain"),
                    Foreground = p.GetColour("Champagne"),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                Grid.SetRow(cb, i);
                buttonGrid.Children.Add(cb);
                buttons[i] = cb;
            }
            // Place buttonGrid in the second row (middle row)
            Grid.SetRow(buttonGrid, 0);
            container.Children.Add(buttonGrid);

            return container;
        }
        //DIALOGUE BOX
        private void SetDialogueBubble(string text, bool noChoices)
        {
            Grid dialogue_layer = new Grid();
            dialogue_layer.ColumnDefinitions.Clear();
            dialogue_layer.RowDefinitions.Clear();
            dialogue_layer.Children.Clear();

            // Create textbox for dialogue, description and thoughts
            TextBox dialogueBox = new TextBox()
            {
                //text controls
                Text = "Text couldn't load.",
                FontFamily = new FontFamily("Reem Kufi"),
                FontSize = 30,
                Padding = new Thickness(15), //how indented the text is inside dialogueBox
                Foreground = p.GetColour("DarkBrown"),
                //textbox controls
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = SystemParameters.PrimaryScreenHeight / 4,
                IsHitTestVisible = false, //isEnabled adds opacity
                BorderBrush = p.GetColour("DarkBrown"),
                BorderThickness = new Thickness(10),
                Background = p.GetColour("Seashell"),
                IsReadOnly = true,
                Margin = new Thickness(10) //how indented the dialogueBox is inside parent (row 2)
            };
            if (text != null)
            {
                dialogueBox.Text = text;
            }
            dialogue_layer.Children.Add(dialogueBox);

            // Continue Button, only if no choices
            if (noChoices)
            {
                Button continueButton = new Button
                {
                    Content = ">>>",
                    FontFamily = new FontFamily("Reem Kufi"),
                    FontSize = 40,
                    Foreground = p.GetColour("DarkBrown"),
                    Background = Brushes.Transparent,
                    //extra border
                    BorderBrush = p.GetColour("DarkBrown"),
                    BorderThickness = new Thickness(2),
                    Padding = new Thickness(5),
                    Margin = new Thickness(35),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom
                };
                continueButton.Click += ContinueButtonClicked;
                dialogue_layer.Children.Add(continueButton);
            }

            // Add dialogue_layer to maingrid
            mainGrid.Children.Add(dialogue_layer);

            // --- Assign content ---
            this.Content = mainGrid;
        }
        private void ContinueButtonClicked(object sender, RoutedEventArgs e)
        {
            if (choices.Count > 0) { MessageBox.Show("Please make a choice!"); return; }
            sp.AllowContinue(0);

            // Move focus to the window
            this.Focus();  // set logical focus to the window
            FocusManager.SetFocusedElement(this, this);
        }


        //////////////////////////////////////////////////////
        //GAME COMPONENTS
        public void StartGame()
        {
            //Create scenepacks
            MakeScenePack();

            //Load scenepacks
            sp.Play();
        }
        private void MakeScenePack()
        {
            //default procedure

            int scenepack_id = 1;
            string background_image = "classroom.png";
            List<string> characters = new List<string> { "g1_neutral.png", "g1_neutral.png" };

            //-----------1-----------
            //Create Choices (if any)
            Choice c1 = new Choice
            {
                Choice_id = 1,
                Text = "Risk it all for character development.",
                Outcome = 2
            };
            Choice c2 = new Choice
            {
                Choice_id = 2,
                Text = "Stay safe and let natural selection work.",
                //Pretend you didn’t see anything and walk away slowly.
                //Nope. That sounds like a “him” problem.
                Outcome = 3
            };

            //-----------2-----------
            //Create Scenes before the choices
            
            //list for all scenes
            List<Scene> scenes = new List<Scene>();

            //for ease, use a list for text
            List<string> dialogues = new List<string>
            {
                "Annie: Well, that was weird.",
                "Mads: Eh, a weird guy munching on a sweater. I've seen worse.",
                "Annie: You're right. Let's go to class.",
                "Mads: Wait. See that? That's the weird guy from the park.",
                "Annie: What is Jessica doing with him?",
                "Mads: She seems to be bothering him. We should go help him."
            };

            //Create all scenes automatically and add to the list
            int i;
            for (i = 0; i < dialogues.Count; i++)
            {
                Scene scene = new Scene
                {
                    Scene_id = i + 1,
                    Dialogue = dialogues[i]
                };
                if (i == 4 || i == 5)
                {
                    scene.Background_image = "hallway.png";
                }
                scenes.Add(scene);
            }

            //Create Scene with choices, FINAL SCENE
            Scene sceneLast = new Scene
            {
                Scene_id = i+1,
                Background_image = "hallway.png",
                Characters = new List<string> { "g1_confused.png", "g1_neutral.png" },
                Dialogue = "Annie: Are you sure?",
                Choices = new List<Choice> { c1, c2 }
            };
            scenes.Add(sceneLast);


            //-----------3----------
            ScenePackBuilder.CreateCostumScenePack(scenepack_id, background_image, characters, scenes);
        }
    }
}
