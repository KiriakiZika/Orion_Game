using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private Grid mainGrid = new Grid(); // --- Main container ---
        private Palette p = new Palette();
        public MainWin()
        {
            //Event Listeners for window
            this.SizeChanged += Window_SizeChanged;
            this.Closing += OnWindowClosing;
            // Subscribe to PreviewKeyDown for global capture
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            //Main program
            try
            {
                InitializeComponent();
                // --- Window properties ---
                this.Title = "ORION";
                this.WindowState = WindowState.Maximized;

                Grid motherGrid = new Grid();
                Grid background = TryToLoadImage("background.jpg");
                motherGrid.Children.Add(background);
                motherGrid.Children.Add(mainGrid);

                // Define two rows: top for Label, rest for content
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) }); // label row
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // content row

                // First row
                FillRow1();
                //Second row
                FillRow2();

                this.Content = motherGrid;
            }
            catch (Exception ex)
            {
                ErrorHandler error = new ErrorHandler(false,ex.Message);
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
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"imgs/{imageName}");
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
                    Foreground = p.ColourX_error,
                    Background = Brushes.Transparent,
                    IsReadOnly = true
                };

                container.Children.Add(errorPanel);
            }
            return container;
        }

        //IMAGE AND CHOICE PANEL
        public void FillRow1()
        {
            Grid row1_container = new Grid();
            //Grid img = TryToLoadImage("background.jpg");
            //row1_container.Children.Add(img);

            Grid row1 = new Grid() { Margin = new Thickness(0) };
            // Define two columns: left for image, right for choices
            row1.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(2, GridUnitType.Star)
            });
            row1.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            row1_container.Children.Add(row1);

            //Create character image (LEFT)
            Grid characterImage = SetColumn1();

            // Create choice panel (RIGHT) - can pass button contents as string array
            Grid choiceGrid = SetColumn2();

            // Add panels to the grid
            Grid.SetColumn(characterImage, 0);
            Grid.SetColumn(choiceGrid, 1);
            row1.Children.Add(characterImage);
            row1.Children.Add(choiceGrid);

            // Add row1 grid to maingrid
            Grid.SetRow(row1_container, 0);
            mainGrid.Children.Add(row1_container);

            // --- Assign content ---
            this.Content = mainGrid;
        }
        //IMAGE(S)
        private Grid SetColumn1()
        {
            //Contain image or else it doesn't work smh
            Grid container = new Grid();
            container.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            container.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });

            Grid img1 = TryToLoadImage("choso.png");
            Grid img2 = TryToLoadImage("lawliet.png");

            Grid.SetColumn(img1, 0);
            Grid.SetColumn(img2, 1);
            container.Children.Add(img1);
            container.Children.Add(img2);

            return container;
        }
        //CHOICE BUTTONS
        private Grid SetColumn2(string[] buttonContents = null)
        {
            // Create container Grid
            Grid container = new Grid() { Margin = new Thickness(10) };
            // Add a single row that fills the Grid
            container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            if (buttonContents == null || buttonContents.Length == 0)
            {
                buttonContents = new string[] { "cry","drink coffee","jump off a bridge" };
            }

            // Create a nested Grid to hold buttons vertically
            Grid buttonGrid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Center, // <-- centers the buttons
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            //temp array to hold buttons
            ChoiceButton[] buttons = new ChoiceButton[buttonContents.Length];
            //Create rows and buttons
            for (int i = 0; i < buttonContents.Length; i++)
            {
                buttonGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                ChoiceButton cb = new ChoiceButton
                {
                    Content = $"{buttonContents[i]}",
                    FontFamily = new FontFamily("Reem Kufi"),
                    FontSize = 20,
                    Padding = new Thickness(10), //inside
                    Margin = new Thickness(10), //outside
                    Background = p.Colour4_mountain,
                    Foreground = p.Colour1_champagne,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetRow(cb, i);
                buttonGrid.Children.Add(cb);
                buttons[i] = cb;
            }
            container.Children.Add(buttonGrid);

            // Force layout update to get ActualWidth
            buttonGrid.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            buttonGrid.Arrange(new Rect(buttonGrid.DesiredSize));

            // Find the widest button
            double maxWidth = buttons.Max(b => b.ActualWidth);
            // Set all buttons to the same width
            foreach (var b in buttons)
            {
                b.Width = maxWidth;
            }

            return container;
        }

        //DIALOGUE BOX
        public void FillRow2()
        {
            Grid row2 = new Grid();

            // Create textbox for dialogue, description and thoughts
            TextBox dialogueBox = new TextBox()
            {
                //text controls
                Text = "Sample text here.",
                FontFamily = new FontFamily("Reem Kufi"),
                FontSize = 20,
                Padding = new Thickness(15), //how indented the text is inside dialogueBox
                Foreground = p.Colour6_darkbrown,
                //textbox controls
                IsEnabled = false,
                BorderBrush = p.Colour6_darkbrown,
                BorderThickness = new Thickness(10),
                Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255)), // 128 = 50% opacity
                IsReadOnly = true,
                Margin = new Thickness(10) //how indented the dialogueBox is inside parent (row 2)
            };
            row2.Children.Add(dialogueBox);

            // Continue Button
            Button continueButton = new Button
            {
                Content = ">>>",
                FontFamily = new FontFamily("Reem Kufi"),
                FontSize = 20,
                Foreground = p.Colour1_champagne,
                Background = Brushes.Transparent,
                //extra border
                BorderBrush = p.Colour6_darkbrown,
                BorderThickness = new Thickness(2),
                Padding = new Thickness(5),
                Margin = new Thickness(35),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            continueButton.Click += ContinueButtonClicked;
            row2.Children.Add(continueButton);

            // Add row2 to maingrid
            Grid.SetRow(row2, 1);
            mainGrid.Children.Add(row2);

            // --- Assign content ---
            this.Content = mainGrid;
        }
        private void ContinueButtonClicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Next Dialogue Bubble");
        }
    }
}
