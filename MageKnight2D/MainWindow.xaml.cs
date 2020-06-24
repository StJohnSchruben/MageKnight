using MKViewModel;
using System;
using System.Collections.Generic;
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

namespace MageKnight2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IArmyBuilder abData;
        ILoginViewModel lvData;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ILoginViewModel lvData = this.lv.DataContext as ILoginViewModel;
            lvData.PropertyChanged += LvData_PropertyChanged;
        }

        private void LvData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ILoginViewModel.IsLoggedIn))
            {
                if (lvData == null)
                {
                    lvData = this.lv.DataContext as ILoginViewModel;
                }

                if (lvData.IsLoggedIn)
                {
                    var ab = new MKView.Views.ArmyBuilder();
                    this.cc.Content = ab;
                    abData = ab.DataContext as IArmyBuilder;
                    abData.PropertyChanged += AbData_PropertyChanged;
                }
                else
                {
                    var lv = new MKView.Views.LoginView();
                    this.cc.Content = lv;
                }
            }
        }

        private void AbData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IArmyBuilder.IsAppliedToBoard))
            {
                if (abData.IsAppliedToBoard)
                {
                    this.cc.Content = new MKView.Views.Dashboard();
                }
                else
                {
                    var ab = new MKView.Views.ArmyBuilder();
                    this.cc.Content = ab;
                }
            }
        }
    }
}
