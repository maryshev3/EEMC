   M S B A M L   `   `   `  ����?  ;EEMC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:EEMC.Views
EEMC.Views  5   	   1clr-namespace:EEMC.ViewModelsEEMC.ViewModels  5   	       EEMC.Views.MainWindow   5   	   Z VDevExpress.Mvvm.UI, Version=22.1.0.0, Culture=neutral, PublicKeyToken=382ad28f6ae31f30W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:EEMC.Views     5   	   4
viewmodelsclr-namespace:EEMC.ViewModels     5   	   -   5          �Name$  MNWindow��5   	    � Title$ 
MainWindow��5
   	   $	��540��5   	   $	��960��5   	   $	��600��5   	   $	��340��5   	    � DataContext 5   	   �� *MainWindowVM+   ViewModelLocator �� Source# [   � 	Resources 5       �� 
TargetType  RoundButton �� 	Resources �� Property �� Value��%�   &	       �� 5   
   " ��6    5      ���� 5      '��       " ��6   .�5      ��  �@��CornerRadius$ 10��65   5      5      5      5      .��5      � 	 � ColumnDefinitions	 5   
   �� 5      $	��200 �6   �� 5      $��* �6   
 � RowDefinitions
 5"   
   �� 5#      $��* �6   .+�5&   
   � .+�5'      �� $
��Auto=�6   .��5(      �� -   5        $��CoursesList��5)      $��2��5+      $��0q�5,      $��Stretch=�5-      $��Stretch=�5.      ���#EE5648��5*      ��  �� Path$ Courses.Courses� �� Mode$ OneWay=���50      �� 51      .R�52      �� #���  53   !   ���#FFFFFF���#EE5648$
��auto��59   !   $
��auto��5:   !   $��Stretch=�5;   !   $��10q�5<   !   $��Center=�5=   !   $��5q�5>   !   
���  ����54   !   �� $
 Name���55   !   ��  �� ElementName$ CoursesList��$ DataContext.bMenu_Click���56   !   �� 5?   !   5A      ��5C      �� 5D      " ��6     �� Triggers 5E      P� 5F   "    P� Property �@��HasItems P� Value$ false��6>   . �5G   &   ��  � �� 5H   *   �� 5I   .   .R�5J   2   � ���#EE5648.+�5L   6   �� 
�������
�������$��1q�5O   9    �� CornerRadius$ 10, 0, 10, 10��5P   9   $	��100��5Q   9   $��Center=�5R   9   $��10q�5S   9   ._�5T   :   � 
 5U   >   �� 5V   B   $��3* �6P   �� 5W   B   $��4* �6P   .+�5Y   >   ��  �� TextAlignment$ Center=�5Z   E   ���#EE5648$��0��5\   E   $��5q�5]   E   42Пока-что отсутствуют курсы.�� 5^   b   Добавить?5`   ?   �� 5a   >   
�������$��0q�6X   $��1��6l   $��10q�6y   $��30��6�   .��5b   B   �� $��/Resources/AddImage.png��6H   5c   ?   5d   ;   5e   7   5f   3   5g   /   5i   '   5j   #   5l      5n      5o      5p       � 5r   
   $��1��5t        � NavigationUIVisibility$ Hidden=�5u      ���#F5E7E3��5s      �� *CurrentPage+5v      5w      5x      