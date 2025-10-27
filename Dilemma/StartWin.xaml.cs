using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dilemma
{
    public partial class StartWin : Window, IStartWin
    {
        private Grid mainGrid = new Grid(); // --- Main container ---
        private Palette p = new Palette();

        public event EventHandler<ErrorHandler> OperationCompleted;

        public StartWin()
        {
            //Event Listeners for window
            this.SizeChanged += Window_SizeChanged;
            this.Closing += OnWindowClosing;

            //Main program
            try
            {
                InitializeComponent();
                // --- Window properties ---
                this.Title = "ORION";
                this.WindowState = WindowState.Maximized;
                this.Background = p.Colour1_champagne;

                // Define two rows: top for Label, rest for content
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.3, GridUnitType.Star) }); // label row
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // content row

                //First row : Label
                FillRow1();
                //Second row : Menu
                FillRow2();
            }
            catch (Exception ex)
            {
                OnOperationCompleted(new ErrorHandler(false, ex.Message));
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateVisual();
            this.UpdateLayout();
        }
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            //force-close
            if (!AppOrchestrator.IsFinished)
            {
                Application.Current.Shutdown();
            }
            //normal close - open main window
            else
            {
                this.Hide();
            }
        }

        public void FillRow1()
        {
            // --- Label ---
            var label = new System.Windows.Controls.Label
            {
                Content = "Welcome to Orion",
                FontSize = 24,
                FontFamily = new FontFamily("Reem Kufi"),
                Foreground = p.Colour4_mountain,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(label, 0);
            mainGrid.Children.Add(label);

            // --- Assign content ---
            this.Content = mainGrid;
        }
        public void FillRow2()
        {
            // --- Content area (second row) ---
            var menuGrid = new Grid();
            Grid.SetRow(menuGrid, 1);

            // --- Panel for vertical orientation = buttons below each other
            var buttonPanel = new StackPanel
            {
                Background = p.Colour5_platinum,
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0)
            };

            // -- Create buttons using helper class
            CreateButton(buttonPanel, 1, "START");
            CreateButton(buttonPanel, 2, "CONTINUE");
            CreateButton(buttonPanel, 3, "ACHIEVEMENTS");
            CreateButton(buttonPanel, 4, "SETTINGS");
            CreateButton(buttonPanel, 5, "HELP");

            // -- Add buttons to parent
            menuGrid.Children.Add(buttonPanel);

            // -- Add menugrid to mainGrid
            mainGrid.Children.Add(menuGrid);

            // --- Assign content ---
            this.Content = mainGrid;
        }

        public void CreateButton(StackPanel parent, int handler, String text)
        {
            // Set up the main Grid (this) like your UserControl wrapper
            Button btn = new Button() {
                Content = text,
                Tag = handler,
                Width = 250,
                Height = 80,
                Margin = new Thickness(10) //how away from other buttons
            };

            // Add TextBlock
            /*lbl = new TextBlock
            {
                Text = text,
                FontFamily = new FontFamily("Reem Kufi"),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };*/

            // Events
            btn.MouseEnter += (s, e) => CbHover(s, e);
            btn.Click += (s, e) => CbClick(s, e);

            // Add to main panel
            parent.Children.Add(btn);
        }

        //EVENT-LISTENERS FOR MENU BUTTONS
        public void CbHover(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                //Manager_Audio.Play("audio/soundeffects/buttonhover.mp3", Manager_Audio.SoundCategory.UI);
            }
        }
        public void CbClick(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                switch (btn.Tag)
                {
                    case 1:
                        StartGame();
                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                }
            }
        }

        public void StartGame() 
        {
            OnOperationCompleted(new ErrorHandler(true, ""));
        }

        //EVENT-LISTENER FOR OPERATION COMPLETED
        protected virtual void OnOperationCompleted(ErrorHandler e)
        {
            OperationCompleted?.Invoke(this, e);
        }
    }
}
