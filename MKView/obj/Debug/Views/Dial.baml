   M S B A M L   `   `   `  ����A  =MKView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MKView.ViewsMKView.Views  5          MKView.Views.Dial   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:MKView.Views     5      $	��100��5      $	��100��6     � 	Resources  5	      #   MKView.Views.BoolToVisibility   BoolToVisibility��%   &	          5
   
   6!   5      .��5      � -   5         �Name$
 grid��5       � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��.8* �6   �� 5      $	��.8* �6   �� 5      $
��1.2* �6    � RowDefinitions 5   
   �� 5      $	��75* �6   �� 5      $	��35* �6   �� 5      $	��33* �6   �� 5      $	��37* �6   �� 5      $	��20* �6   .+�5   
   A� -   5         A�Name$ 
EdgeOfDial��5      
���   �
����  �$��.5��6A   $��5��6V   $��4��6g   �� 5   
   
�������$��Center=�6>   $��4��6\   $��Center=�6p   ��6   �� *Name+6p      MKView.Views.Stat  5   
   $��1��6   $��1��6"    � DataContext 62   �� *Click.Speed+  5   
   $��2��6   $��1��6"    62   �� *Click.Attack+  5    
   $��3��6   $��1��6"    62   �� *Click.Defense+  5"   
   $��3��6   $��2��6"    62   �� *Click.Damage+5#      5$      