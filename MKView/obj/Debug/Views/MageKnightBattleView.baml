   M S B A M L   `   `   `  ����A  =MKView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MKView.ViewsMKView.Views  5      '    !MKView.Views.MageKnightBattleView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:MKView.Views     5      -   5          � 	Resources  5
      #   MKView.Views.BoolToVisibility   BoolToVisibility*   $MKView.Views.InverseBoolToVisibility  InverseBoolToVisibility&    MKView.Views.TotalWidthConverter  TotalWidthConverter+   %MKView.Views.ActionCountToVisibility1  ActionCountToVisibility1+   %MKView.Views.ActionCountToVisibility2  ActionCountToVisibility2��%�   &	        &	      &	 &     &	 9     &	 L       5   
   6!     5   
   6(     5   
   6$     5   
   6)     5   
   6)   5      .��5      � .+�5   
      MKView.Views.ArcControl  -   5          Name$ 
arcControl��5      ��6/   �� *	ViewModel+ �� 	Converter# [ ��6{   �� *Model+# [    MKView.Views.Dial  5   
   -   5          Name$
 dial��5       � DataContext 6#   �� *Dial+A� 5   
   -   5         A�Name$ CenterOfDial��5      
���  ��$��5��63   $��5��6>   y�6H   �� *	IsInRange+# [  A� 5   
   -   5        $ Counter1��5      
��� � �$��10��60   $��10��6<   $��58,30,37,65q�6�   y�6G   �� *ActionCount+# [ 6�   A� 5   
   -   5        $ Counter2��5      
��� � �$��10��60   $��10��6<   $��37,30,58,65q�6�   y�6G   �� *ActionCount+# [ 6�   5      5      