   M S B A M L   `   `   `  ����E  AZoomAndPan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:ZoomAndPan;assembly=ZoomAndPan
ZoomAndPan  5      A =MKView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MKView.ViewsMKView.Views 5      clr-namespace:MKViewMKView 5      F BMKViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:MKViewModel;assembly=MKViewModelMKViewModel 5	      B >MKModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null2&clr-namespace:MKModel;assembly=MKModelMKModel 5
         MKView.Views.ZoomControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      C
ZoomAndPan,clr-namespace:ZoomAndPan;assembly=ZoomAndPan     5      ,viewsclr-namespace:MKView.Views     5      %viewclr-namespace:MKView     5      =vm.clr-namespace:MKViewModel;assembly=MKViewModel     5	      4m&clr-namespace:MKModel;assembly=MKModel     5
        � DataContext  5      �� *GameViewModel+  MKView.SharedResources   ViewModelLocator �� Source# Z  � 	Resources 5         Commands.ZoomOut  Commands.ZoomIn  Commands.JumpBackToPrevZoom  Commands.Fill  Commands.OneHundredPercent#  MKView.Views.BoolToVisibility  BoolToVisibility&   MKView.Views.TotalWidthConverter  TotalWidthConverter �� DataType+  %MKViewModel.MageKnightBattleViewModel'  !MKView.Views.MageKnightBattleView��%  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     (�� �     5      *, +)�� 5   
   6   �� 5   
   6   �� 5   
   6   �� 5   
   6   �� 5   
   6     5   
   6!     5   
   6$   �� 5   
   "  6   .R�5        5      5       J� InputBindings 5      �� 5   
   
 �� Key$ Minus��5       �� Command# [  5      �� 5!   
   $
 Plus��5"      # [ 5#      �� 5%   
   $ 	Backspace��5&      # [ 5'       J� CommandBindings 5,      )  #System.Windows.Input.CommandBinding  5.   
   -   5        	   Command#	 [  5/      50        52   
   -   5        #	 [ 53      54        56   
   -   5        #	 [ 57      59        5;   
   -   5        #	 [ 5<      5=        5?   
   -   5        #	 [ 5@      5A      .��5E      � .+�5F   
   ]� .+�5G      � $��5,5,5,5q�5H      $��Bottom=�5I      
 � ColumnDefinitions
 5K      �� 5L      $��* �6'   �� 5N      $��40 �6'   �� 5P      $��5 �6'   �� 5R      $��40 �6'   �� 5T      $��5 �6'   �� 5V      $��25 �6'   �� 5X      $��15 �6'   �� 5Z      $��5 �6'   �� 5\      $��20 �6'   �� 5^      $��5 �6'   �� 5`      $	��120 �6'   �� 5b      $��5 �6'   �� 5d      $��20 �6'   �� 5f      $��10 �6'   �� 5h      $
��Auto �6'   .+�5n      �� $��1��5o      #��[ 5p      .��5q      Fill5s      �� 5u      $��3��5v      #��[ 5w      .��5x      100%5z      �� 5~      $��6��5      $��Center=�5�      $��%��5�      �� 5�      $��8��5�      #��[  5�      .��5�      -5�      �� 5�      $��12��5�      ��5�      �� *LoginViewModel.LogOut+# Z .��5�      LogOut5�      �� 5�      $��14��5�      5�      �� 5�      -   5         ��Name$ scroller��5�      ��. $��Visible=�5�      $��Visible=�5�      .��5�      "   ZoomAndPan.ZoomAndPanControl  -   5          Name$ zoomAndPanControl��5�      
�������   ContentViewportWidth 5�      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" � �� Path$ ActualWidth�   ContentViewportHeight 5�      ��  �� *FindAncestor+" �$ ActualWidth�.��5�      � -   5         �Name$ zoomGrid��5�      ��5�      ��  �� *FindAncestor+" �$ ActualWidth���5�      ��  �� *FindAncestor+" �$ ActualWidth�.+�5�      (� -	   5         (�Name$ content��5�   "   .��5�        MKView.Views.BattleGround    62   �� 5�      �� 5�      -
   5         ��Name$ dragZoomCanvas��5�      $y�	Collapsed=�5�      .+�5�      �� -   5         ��Name$ dragZoomBorder��5�      
���   �$��1q�5�      
������� �� CornerRadius$ 1��5�       J� Opacity$ 0Q�5�      5�      5�      5�      5�      5�      5�      5�      