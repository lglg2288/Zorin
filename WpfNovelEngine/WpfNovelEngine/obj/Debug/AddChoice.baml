   M S B A M L   `   `   `  ����I  EWpfNovelEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfNovelEngineWpfNovelEngine  5   	       WpfNovelEngine.AddChoice   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WpfNovelEngine     5   	     � Title$  	AddChoice��5   	    � 
ResizeMode$ NoResize=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	   $	��100��5   	   $	��450��6    � WindowStyle$
 None=�5   	    � 	Resources 5         ComboBoxToggleButton �� 
TargetType � ColumnDefinitions ��Name �� CornerRadius	 F�Name  ComboBoxTextBox
 �� 
TargetType �� Property �� Value �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation �� Triggers  P� Property! P� Value" �� 
TargetName# P� 
SourceName$ �� Triggers% �� Padding��%�  &	        &	 +    '�� �    7   7  '�� .    �� 5   
   " d�67   .R�5      �  5      �� 5      �� 5      $��20 �6'   .+�5      �� $ Border��6   $��2��5      $ 0��5      ���	#FF3F3F3F���	#FF97A0A5$��2q�5      �� 5      $��0��6   $ 0��5      $��1q�5      ���	#FF3F3F3F���	#FF97A0A5$��	1,1,2.5,1q�5       F� 5!      $	 Arrow��6   $��1��5"      ���#00fbff$
��Left=�5$      $��Center=�5%      $��	1.3,0,0,0q�5&      =����        ����  -1�    ��- �     -1����     ���� ��- 	5'      5)      5*      �� 5,   
   " ��62   .R�5-      �� $ PART_ContentHost��6   |�.  #��z��6A   5.      �� 50   
   "
 ��6*   .�51      ��  �@J�SnapsToDevicePixels . 64   �� 52       �@�OverridesDefaultStyle . 66   �� 53       � ��$
 Auto=�6K   �� 54       � ��$
 Auto=�6I   �� 55       � �� . 6>   �� 56       � ��$	 120��6)   �� 57       � ��$ 17��6*   �� 58       � ��
 �����6+   �� 59       � ��$ 15(�6*   �� 5:       � �� 5;      �� 5<      " ��6&   .R�5=      � .+�5>      d� $��ToggleButton��5?   !   8��  $��2��5A   !   |�.  $ Press=�5D   !    5C   !   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5E      �� 5F      $��ContentSite��6/    .  #��z 6\   #��z 5G   !   #��z��5H   !   $��3,3,23,3q�5I   !   $��Center=�5J   !   $
��Left=�5K   !   �� 5L      $ PART_EditableTextBox��6&   8�� $
��Left=�5O   !   $��Center=�5P   !   $��3,3,23,3q�5Q   !   |�. ���	#FF3F3F3F
��� � �$y�Hidden=�5U   !   # z 5V   !   ��5M   !   Z� 5V   !   � 5W      $��Popup��5X   !   $��Bottom=�5Y   !   #��z 5Z   !    . |�.  $��Slide=�5]   !   .��5_   "   � $��DropDown��6'    . #��z 5a   #   #��z 5b   #   .+�5c   &   �� $ DropDownBorder��5d   %   ���	#FF3F3F3F$��1q�5g   %   ���#8888885h   %   �� 5i   &   $��4,6,4,6q�63    . .��5j   *   ��  . $ 	Contained=�6H   5k   '   5l   #   5m      5n       5o      P� 5p        �@��HasItems! .  . �5q   "   �� $" DropDownBorder��6)    � ��$ 95��6Z   5r      P� 5s        � {�! .  . �5t   "   ��  � �� �#8888886?   5u      P� 5v        �@��
IsGrouping! . . �5w   "   ��  � �� .  6R   5x      P� 5y      $# Popup��6&     �@�AllowsTransparency! . . �5z   "   �� $" DropDownBorder��6)    �@��CornerRadius$ 0��6]   �� 5{   "   $" DropDownBorder��6)    � ��$ 0,2,0,0q�6W   5|      P� 5}        �@��
IsEditable! . . �5~   "   ��  � �� .  6>   �� 5   "   $" PART_EditableTextBox��6)    � y�$ Visible=�6a   �� 5�   "   $" ContentSite��6)    � y�$ Hidden=�6X   5�      5�      5�      $ 5�      5�      �� 5�   
   "
 ��6.   .�5�      ��  �@J�SnapsToDevicePixels . 64   �� 5�       � �� �#00fbff6+   �� 5�       �@�OverridesDefaultStyle . 66   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� $��Border��6!   $% 2q�5�   #    . ._�5�      �� 5�       5�      P� 5�        �@��IsHighlighted! . . �5�   "   �� $" Border��6)    � �� �	#FF4F4F4F6S   5�      P� 5�        � {�! .  . �5�   "   ��  � �� �#8888886?   5�      5�      5�      5�      5�      .��5�      � $��0,0,0,0q�6   ���#202020.+�5�   
   �� ���#032e00$��17(�6%   $��Italic$�63   $��0,-122,-100,-17q�6F   $����ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
D ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
DD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
 ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
D ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
DD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
 ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
D ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
DD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
 ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD ADD 
��5�      z�5�      �� 5�      & �� Angle$& 15Q�6"   5�      �� 5�   
   $��Сhoice leads to:��6   
�������$	��Top=�6>   $
��Left=�6V   ���#00fbff$��17(�6�   $��1��6�   �� 5�   
   $��
Storyline:��6   
����������#00fbff$��26��6N   $��1��6Z   �� 5�   
   $��Page:��6   
����������#00fbff$��	277,0,0,0q�6I   $��26��6\   $��1��6h   �� 5�   
   -   5        $��comboBoxStoryline��5�      $��64,36,173,37q�6,   $��1��6B   6O   �� 5�   
   -   5        $��comboBoxPage��5�      $��320,36,10,39q�6'   $��1��6=   6J   �� 5�   
   -   5        ' ��Name$' btnOk��5�      $��OK��6    $��20��6-   $��80��69   ���#505050
����������#00fbff$��275,73,95,7q�6�   6�   �� 5�   
   -   5        $' 	btnCancel��5�      $��Cancel��6$   $��20��65   $��80��6A   $��360,73,10,7q�6L   ���#505050
����������#00fbff6�   5�      5�      