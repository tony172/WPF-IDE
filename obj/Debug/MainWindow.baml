   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	   > :ide, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ideide 5   	      ide.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:ide     5   	   O KScintillaNET.WPF, Version=1.0.0.16559, Culture=neutral, PublicKeyToken=null)sn http://scintillanet.codeplex.com  5   	   -   5          � Title$  
MainWindow��5
   	   $	��350��6   $	��500��6)    � WindowStartupLocation$ CenterScreen=�65    � WindowState$ 	Maximized=�6�   .��5      �  � ColumnDefinitions 5   
   �� 5      $��50��6   $	��400��6-   $	��130 �6<   �� 5      $��3 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $	��10* �6   �� 5      $��3 �6   �� 5      $��50��6   $	��400��6+   $��3* �6;   .+�5   
   o� $��0��6   $��3��6   $��0��63   $��Stretch=�6@   $��30��6^   $	��Top=�6j   .@�5      m� $
��File��6   $��30��6%   $��40��61   .@�5      m� -   5        $��New Project��5      $
��Left=�61   $��25��6L   $	��160��6X    m� Icon 5      �� 5      $��Resources\new.bmp��6    5!      m� 5"      -   5        $��
btnNewFile��5"      $��New File��6-   {�.  $
��Left=�6Q   $��25��6l   $	��160��6x   6�   m� 5#      -   5        $��Open Project��5#      $
��Left=�61   $��25��6L   $	��160��6X    5$      �� 5%      $��Resources\open.bmp��6    5'      m� 5)      -   5        $��btnSave��5)      {�.  $
��Save��6<   $
��Left=�6J   $��25��6e   $	��160��6q    5*      �� 5+      $��Resources\save.bmp��6    5-      m� 5/      -   5        $��Refresh��5/      $
��Left=�6,   $��25��6G   $	��160��6S   6_   m� 50      -   5        $��Settings��50      $
��Left=�6-   $��25��6H   $	��160��6T   6`   m� 52      -   5        $
��Exit��52      $
��Left=�6)   $��25��6D   $	��160��6P   54      58      m� 5:      -	   5         � ToolTip$ Compile��5:      $��26��6*   $��30��66   $��
100,0, 0,0q�6Z    5;      �� 5<      $��Resources\build.bmp��6   5>      m� 5?      -
   5        $ Compile and Run��5?      $��25��61   $��30��6=   $��10,0,0,0q�6a    5@      �� 5A      $��Resources\run.png��6   5C      5E      �� 5F   
   $��0��6   $��3��6%   $��3��69   $��Stretch=�6F   $��Stretch=�6d   .@�5G      �� $��Compiler��6   .��5H      � ���	#FFE5E5E5.+�5I      �� -   5        $��richTextBox1��5I   "    �� 
IsReadOnly . $��Stretch=�6H   $��Stretch=�6g   . �5J      /� .T�5K      J� .*�6(   .*�5L   "   �� $� ��6&   5M      5N      5O      5Q      5R      �� 5S      $��Errors��6   .��5T      � ���	#FFE5E5E5.+�5U      �� -   5        $��errorListView��5U      $��Stretch=�64   $��Stretch=�6R    �� View 5V      �� 5W      .P�5X   "   �� 5Y      5[      5]      5^      �� 5_      $��Performance��6   $
��Left=�6+   $��20��6F   $	��Top=�6R   $��80��6j   .��5`      � ���	#FFE5E5E5	 � ContextMenu	 5a      �� 5b      .@�5c      m� -   5        $��Clear��5c   '   66   5d       5f      �� 5g      $��1* �6+   �� 5h      $��1* �6+   .+�5j      #   LiveCharts.Wpf.CartesianChart  -   5        $��ramChart��5j   )   $��0��6W   &    LiveCharts.Wpf.Charts.Base.Chart
   Series
 69   �� *	ramValues+6W     5k      -   5        $��cpuChart��5k   )   $��1��6X   
 6:   �� *	cpuValues+6X   5l      5m      5n      #  ScintillaNET.WPF.ScintillaWPF  5o   
   -   5        $��editor��5o   ,   {�.  $��2��6:   $��1��6J   $��Stretch=�6W   $��Stretch=�6u   6�   �� 5p   
   $��1��6   $��1��6'   $��Stretch=�64   $��Stretch=�6S   �� 5q   
   $��2��6   $��3��6$   $��Stretch=�68   $��Stretch=�6W   R� 5r   
   -   5        $��	treeView1��5r   *   
�������$��0��6;   $��1��6K   $��Stretch=�6X   $��Stretch=�6v   6�   5w      5y      