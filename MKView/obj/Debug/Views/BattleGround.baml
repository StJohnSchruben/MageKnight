   M S B A M L   `   `   `  ����A  =MKView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MKView.ViewsMKView.Views  5      F BMKViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:MKViewModel;assembly=MKViewModelMKViewModel 5          MKView.Views.BattleGround   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:MKView.Views     5      =vm.clr-namespace:MKViewModel;assembly=MKViewModel     5      
���iii�  � 	Resources  5         
MageKnight'   !MKView.Views.MageKnightBattleView��%2   &	        �� 5   
   .R�5        5      5      .��5      � $
��4000��6   $
��4000��6   .+�5   
   �� -   5         ��Name$ ic��5      #��[  6$   $��1��6�   ��6P   �� *ActiveMageKnights+��6|   �� *ActualHeight+��6�   �� *ActualWidth+��5      �� 5      .R�5         MKView.Views.BattleCanvas    Name$ renderCanvas��6)    K� IsItemsHost . 
��� � ���5   !   ��  �� Path$ ActualWidth� �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType$
 GridL���5   !   �� *ActualHeight+��6A   �� *ActualWidth+5      ��5      �� 5       �� 
TargetType" ��6   .�5      �� $   MKView.Views.DraggableExtender	 �� Property	 �@ CanDrag
 �� Value
 . 6H   �� 5       	 �@��Left
 64   �� $ XCord� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� NotifyOnSourceUpdated .  �� NotifyOnTargetUpdated . �� 5!      	 �@��Top
 63   �� $ YCord�$ TwoWay=�$ PropertyChanged=� .  . 5"      5$      � 5%   
   $
��3600��6   $
��3600��6   .+�5&      �� 
��� ���$��5q�6*   �� 5'      $	��200��6   
��� ���$��5q�67   $
��1800��6K   $	��Top=�6X   �� 5(      $	��200��6   
��� ���$��5q�67   $
��1800��6K   $��Bottom=�6X   5)      z�5*   
   �� 5+      -   5        	Angle��5+      5-      5.      