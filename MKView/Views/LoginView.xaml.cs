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

namespace MKView.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            this.Loaded += LoginView_Loaded;
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            ILoginViewModel data = this.DataContext as ILoginViewModel;
            data.PropertyChanged += Data_PropertyChanged;
        }

        private void Data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ILoginViewModel.IsLoggedIn))
            {
                ILoginViewModel data = this.DataContext as ILoginViewModel;
                data.PropertyChanged += Data_PropertyChanged;
                
                this.Visibility = data.IsLoggedIn ?  Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}
