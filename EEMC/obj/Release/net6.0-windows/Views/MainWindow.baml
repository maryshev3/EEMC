   M S B A M L   `   `   `  ���� ?  ;EEMC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:EEMC.Views
EEMC.Views  1clr-namespace:EEMC.ViewModelsEEMC.ViewModels      EEMC.Views.MainWindow   Z VDevExpress.Mvvm.UI, Version=22.1.0.0, Culture=neutral, PublicKeyToken=382ad28f6ae31f30W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:EEMC.Views     4
viewmodelsclr-namespace:EEMC.ViewModels     -     �Name$  MNWindow�� � Title$ 
MainWindow��$	��540��$	��960�� � DataContext �� *MainWindowVM+   ViewModelLocator �� Source# [   � 	Resources  �� 
TargetType  RoundButton �� 	Resources �� Property �� Value��%W   &	       �� " �� ���� '��       " ��.���  �@��CornerRadius$ 10��.��� 	 � ColumnDefinitions	 �� $	��200 ��� $��* �
 � RowDefinitions
 �� $��* �.+�� .+��� $
��Auto=�.���� -   $��CoursesList��$��2��$��0q�$��Stretch=�$��Stretch=����#EE5648����  �� Path$ Courses.Courses� �� Mode$ OneWay=����� .R��� #���  ���#FFFFFF���#EE5648$
��auto��$
��auto��$��Stretch=�$��10q�$��Center=�$��5q�
���  ������ $
 Name�����  �� ElementName$ CoursesList��$ DataContext.bMenu_Click�����  � $��1�����#F5E7E3���� *CurrentPage+