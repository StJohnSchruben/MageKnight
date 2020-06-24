using MKService;
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
    /// Interaction logic for BattleGround.xaml
    /// </summary>
    public partial class BattleGround : UserControl
    {
        public BattleGround()
        {
            InitializeComponent();
            this.Loaded += BattleGround_Loaded;
        }

        private void BattleGround_Loaded(object sender, RoutedEventArgs e)
        {
            IGameViewModel game = this.DataContext as IGameViewModel;
            if (game.User1.Id != ServiceTypeProvider.Instance.LoggedInUserId)
            {
                this.Angle.CenterX = this.ActualHeight / 2;
                this.Angle.CenterY = this.ActualHeight / 2;
                this.Angle.Angle = 180;
            }
        }
    }
}
