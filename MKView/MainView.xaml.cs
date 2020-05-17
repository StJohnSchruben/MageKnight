using MKViewModel;
using System;
using System.Windows.Controls;
using System.Windows;
namespace MKView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            this.mm.Visibility = Visibility.Visible;
        }

        private void mainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.mm.Visibility = Visibility.Visible;
        }

        private void CollapsViews()
        {
            this.mm.Visibility = Visibility.Hidden;
            this.ab.Visibility = Visibility.Hidden;
            this.bg.Visibility = Visibility.Hidden;
            this.mkg.Visibility = Visibility.Hidden;
        }

        private void ArmyBuilder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.ab.Visibility = Visibility.Visible;
        }

        private void BattleGround_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.bg.Visibility = Visibility.Visible;
        }

        private void Database_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CollapsViews();
            this.mkg.Visibility = Visibility.Visible;
        }

        private void UnderConstruction_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
