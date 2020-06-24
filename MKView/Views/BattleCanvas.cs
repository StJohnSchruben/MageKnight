using MKModel;
using MKViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MKView.Views
{
    public class BattleCanvas : Canvas
    {
        protected bool isDragging;
        private Point clickPosition;

        public BattleCanvas()
        {
            this.MouseMove += BattleCanvas_MouseMove;
        }

        bool initialize = false;

        // this is a hack to initialize the propertychanged handler. The content is null on load thats why 
        private void BattleCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.initialize)
            {
                return;
            }

            this.initialize = true;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this); i++)
            {
                ContentPresenter child = VisualTreeHelper.GetChild(this, i) as ContentPresenter;
                MageKnightBattleView mage = FindFirstGenericChild.FindFirstChild<MageKnightBattleView>(child);
                mage.dial.MouseLeftButtonDown += Dial_MouseLeftButtonDown;
                IMageKnightBattleViewModel battleViewData = mage.DataContext as IMageKnightBattleViewModel;
                battleViewData.PropertyChanged += BattleViewData_PropertyChanged;
            }
        }

        private void BattleViewData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMageKnightBattleViewModel.ActionMode))
            {
                IMageKnightBattleViewModel data = sender as IMageKnightBattleViewModel;

                if (data.ActionMode == ActionMode.Attack || data.ActionMode == ActionMode.MoveFormation)
                {
                    this.TargetingMage = data;
                }
                else if (this.TargetingMage != null && this.TargetingMage.ActionMode == ActionMode.Pass)
                {
                    this.TargetingMage = null;
                }
            }
        }

        IMageKnightBattleViewModel TargetingMage;

        private static Point _offset;
        private void Dial_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var draggableControl = sender as UserControl;
            var mageKnightBattleView = draggableControl.FindAncestor<MageKnightBattleView>();

            IMageKnightBattleViewModel battleViewData = mageKnightBattleView.DataContext as IMageKnightBattleViewModel;
            if (this.TargetingMage != null && this.TargetingMage.Index != battleViewData.Index)
            {
                if (this.TargetingMage.ActionMode == ActionMode.Attack)
                {
                    this.TargetingMage.AddTarget(battleViewData);
                }
                else if (this.TargetingMage.ActionMode == ActionMode.MoveFormation)
                {
                    if (!this.TargetingMage.MoveFormationParticipants.Contains(battleViewData) && TargetingMage.FrendlyBaseContact.Contains(battleViewData) && TargetingMage.Faction == battleViewData.Faction)
                    {
                        this.TargetingMage.MoveFormationParticipants.Add(battleViewData);
                        battleViewData.Move.Execute(null);
                    }
                }
            }
        }
    }
}

