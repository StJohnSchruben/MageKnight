   M S B A M L   `   `   `  ����A  =MKView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MKView.ViewsMKView.Views  5      clr-namespace:MKViewMKView  5      F BMKViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:MKViewModel;assembly=MKViewModelMKViewModel 5
          MKView.Views.LoginView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:MKView.Views     5      %viewclr-namespace:MKView     5      =vm.clr-namespace:MKViewModel;assembly=MKViewModel     5
        � DataContext  5	      �� *LoginViewModel+   MKView.SharedResources   ViewModelLocator �� Source# Z  � 	Resources 5       �� ImageSource   
Background  TextBox.Static.Border �� Color  TextBox.MouseOver.Border  TextBox.Focus.Border  TextBoxStyle1 �� 
TargetType �� Property �� Value-  'System.Windows.Input.KeyboardNavigation	 �� 
TargetType
 ��Name J� SnapsToDevicePixels �� CornerRadius ��Name �� Triggers P� Property P� Value �� 
TargetName �� Triggers ]� 
Conditions �� Property �� Value��%�  &	        &	 *     &	 M     &	 p     &	 �     7  �� 5   
   $ dungeon.jpg��6   6/   �� 5   
   $ 	#FFABAdB3��68   �� 5   
   $ 	#FF7EB4EA��6;   �� 5   
   $ 	#FF569DE5��67   �� 5   
   " ��6&   .�5      ��  � ��# � ��6+   �� 5       � ��8   6,   �� 5       � ��# � ��6+   �� 5       � ��$ 1q�60   �� 5       �@ TabNavigation$
 None=�6A   �� 5       � ��$
 Left=�6;   �� 5       �@�FocusVisualStyle 61   Z� �� 5       �@J�	AllowDrop . 6*   �� 5       �@��PanningMode$ VerticalFirst=�69   �� 5       �@��IsFlicksEnabled .  67   �� 5       � �� 5      �� 5      "	 ��6&   .R�5      �� $
 border��6!   #��z��61   #��z��6]   #��z��6�    . $ 9��6�   ._�5       �� $ PART_ContentHost��6+   |�.  $��Hidden=�6W   $��Hidden=�6~   5!       5"      P� 5#       � {� .  . �5$   "   �� $ border��6<    �@J�Opacity$
 0.56Q�6P   5%      P� 5&       �@J�IsMouseOver . . �5'   "   �� $ border��6@    � ��
 � ���6T   5(      P� 5)       �@J�IsKeyboardFocused . . �5*   "   �� $ border��6@    � ��
 � ���6T   5+      5-      5/       50      ]� 51       52      �� 53      + �@��#IsInactiveSelectionHighlightEnabled . 6S   �� 54       �@��IsSelectionActive .  6A   ./�56      ��  �@��SelectionBrush# � /�67   57      59      5:      .��5;      � #��[  6    � RowDefinitions 5<   
   �� 5=      $
��103* �6   �� 5>      $��8* �6   �� 5?      $	��10* �6   �� 5@      $��7* �6   �� 5A      $
��112* �6    � ColumnDefinitions 5C   
   �� 5D      $
��138* �6   �� 5E      $	��17* �6   �� 5F      $	��27* �6   �� 5G      $
��138* �6   .+�5I   
   �� $��	UserName:��6   
�������$	��Top=�6:   $��68��6R   $��1��6]   $��26��6m   $��1��6y   �� 5J   
   $��	Password:��6   
�������$	��Top=�6:   $��68��6R   $��1��6]   $��26��6m   $��2��6y   �� 5K   
   $��23��6    �� TextWrapping$
 Wrap=�6   $	��120��6L   $��2��6X   $��1��6h   #���  6u   
�������
���  ��$��2q�6�   ��62   �� *
UserName+6�   �� 5L   
   $��23��6   $
 Wrap=�6   $	��120��6L   $��2��6X   $��2��6h   #���  6u   
�������
���  ��$��2q�6�   ��62   �� *
Password+6�   �� 5M   
   $��Sign In��6   $��75��6>   $��2��6I   $��20��6Y   $��3��6e   
���   �
�������
�������$��1q�6�   ��6#   �� *SignIn+6�   �� 5N   
   $��Sign Up��6   $��75��6>   $��1��6I   $��20��6Y   $��3��6e   
���   �
�������
�������$��1q�6�   ��6#   �� *SignUp+6�   5O      5P      