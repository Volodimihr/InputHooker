   M S B A M L   `   `   `  ����U  QExam_InputHooker_Karvatyuk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Exam_InputHooker_KarvatyukExam_InputHooker_Karvatyuk  5   	   +    %Exam_InputHooker_Karvatyuk.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:Exam_InputHooker_Karvatyuk     5   	   -   5          � Title$  InputHooker��5   	    � Icon$ eye.ico��6   $	��450��5	   	   $	��800��6    � 	Resources 5
      7   1Exam_InputHooker_Karvatyuk.StringToArrayConverter   strToArrConverter��%   &	          5   
   6'   5      .��5      � $��5q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��auto �6   .+�5   
   �� $��0��6   .��5      �� -   5         ��Name$ 	tbxInputs��5       �� TextWrapping$
 Wrap=�6)    �� 
IsReadOnly . ��6O   �� *	InputsStr+ �� UpdateSourceTrigger$ PropertyChanged=�6�   5      � 5   
   $��1��6   
������� � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��auto �6#   �� 5      $
��auto �6#   �� 5      $
��auto �6#   �� 5      $
��auto �6#   .+�5      �� -   5        $ 	tbxSearch��5   &   $��0��6   $��5q�69   $��16(�6D   $��Center=�6R   ��5      �� *KeyWordsArray+	 �� Mode$	 OneWayToSource=�
 �� 	Converter#
 [  $ PropertyChanged=�5   P   �� 5       -   5         ��Name$ 	btnSearch��5    %   $��1��6   $��16(�68   $��Search��6F   $��5q�6W   $��5q�6b   6n   �� 5!      -   5        $��2��5!      $��16(�6%   $��Save to file��63   $��5q�6J   $��5q�6U   6a   �� 5"      -   5        $��3��5"      $��16(�6%   $
��Hide��63   $��5q�6B   $��5q�6M   6Y   �� 5#      -   5        $��4��5#      $��16(�6%   $��Close��63   $��5q�6C   $��5q�6N   6Z   5$      5%      5&      