   M S B A M L   `   `   `  ����A  =MKView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MKViewMKView  5      +clr-namespace:MKView.ViewsMKView.Views  5      B >MKModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null2&clr-namespace:MKModel;assembly=MKModelMKModel 5      &     MKView.Views.MageKnightGenerator   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      %viewclr-namespace:MKView     5      ,localclr-namespace:MKView.Views     5      4m&clr-namespace:MKModel;assembly=MKModel     5        � 	Resources  5       �� ImageSource   
background �� DataType  MKModel.IClick   MKView.Views.Click��%�   &	        (�� 9     5      *, +)�� 5   
   $  ../MageKnightGenerator.jpg��6   6>   �� 5   
   "  6   .R�5        5      5      .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��5* �6   �� 5      �� 5      $��4* �6   �� 5      $��5* �6   �� 5      $��5* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dg��5      $��Right=�6�   $	��935��6�   $��	0,5,135,0q�6  $��5��6  $	��Top=�6*  $	��790��6B  $��2��6N  ��6L   �� *MageKnights+ �� SelectedItem 6p   �� *SelectedMageKnight+ �� UpdateSourceTrigger$ PropertyChanged=� �� NotifyOnSourceUpdated . 5      �� 5    
   
����ح�$��1��6e   $
��Left=�6u   $	��935��6�   $��
880,10,0,0q�6�   $	��Top=�6�   $	��340��6�   $��5��6�   $��2��6�   ��6   �� * SelectedMageKnight.Dial.Clicks+��5!      �� 5"      .R�5#      �� 5$      ��5&      �� 5'      .R�5(      (� $	��340��6   $��85��6*   .��5)        5*      5+      5-      �� 5.   
   $��2��6   $
��Left=�6R   $	��175��6m   $��
270,35,0,0q�6z   $	��Top=�6�   $	��160��6�   ��6!   �� *SelectedMageKnight.ModelImage+6�   5/      50      