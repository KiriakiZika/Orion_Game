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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            //Main program
            try
            {
                InitializeComponent();
                // --- Window properties ---
                this.Title = "ORION";
                this.WindowState = WindowState.Maximized;
                this.Background = p.Colour1_champagne;

                // Define two rows: top for Label, rest for content
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) }); // label row
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // content row

                // First row
                FillRow1();
                //Second row
                FillRow2();
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
            Application.Current.Shutdown();
        }

        public void FillRow1()
        {
            Grid row1 = new Grid();
            row1.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(2, GridUnitType.Star)
            });
            row1.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });

            //Create left panel
            var leftPanel = new StackPanel
            {
                Background = p.Colour3_cherry,
                Margin = new Thickness(10)
            };

            // Create right panel
            var rightPanel = new StackPanel
            {
                Background = p.Colour4_mountain,
                Margin = new Thickness(10)
            };

            // Add panels to the grid
            Grid.SetColumn(leftPanel, 0);
            Grid.SetColumn(rightPanel, 1);
            row1.Children.Add(leftPanel);
            row1.Children.Add(rightPanel);

            // Add row1 grid to maingrid
            Grid.SetRow(row1, 0);
            mainGrid.Children.Add(row1);

            // --- Assign content ---
            this.Content = mainGrid;
        }
        public void FillRow2()
        {
            // Create textbox for dialogue, description and thoughts
            TextBox dialogueBox = new TextBox()
            {
                //text controls
                Text = "Sample text here.",
                FontFamily = new FontFamily("Reem Kufi"),
                FontSize = 20,
                Foreground = p.Colour4_mountain,
                Padding = new Thickness(15), //how indented the text is inside dialogueBox
                //textbox controls
                Background = Brushes.Transparent,
                IsReadOnly = true,
                Margin = new Thickness(10) //how indented the dialogueBox is inside parent (row 2)
            };

            // Add textbox to maingrid
            Grid.SetRow(dialogueBox, 1);
            mainGrid.Children.Add(dialogueBox);

            // --- Assign content ---
            this.Content = mainGrid;
        }
    }
}
