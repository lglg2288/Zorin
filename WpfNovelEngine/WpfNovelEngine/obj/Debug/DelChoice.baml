   M S B A M L   `   `   `  ����I  EWpfNovelEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfNovelEngineWpfNovelEngine  5   	       WpfNovelEngine.DelChoice   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WpfNovelEngine     5   	     � Title$  	DelChoice��5   	    � WindowStyle$
 None=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	   $	��100��5   	   $	��400��6    � 	Resources 5         ComboBoxToggleButton �� 
TargetType � ColumnDefinitions ��Name �� CornerRadius F�Name  ComboBoxTextBox	 �� 
TargetType
 �� Property �� Value �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation �� Triggers P� Property  P� Value! �� 
TargetName" P� 
SourceName# �� Triggers$ �� Padding��%�  &	        &	 +    '�� �    7   7  '�� .    �� 5   
   " d�67   .R�5      �  5      �� 5      �� 5      $��20 �6'   .+�5      �� $ Border��6   $��2��5      $ 0��5      ���	#FF3F3F3F���	#FF97A0A5$��2q�5      �� 5      $��0��6   $ 0��5      $��1q�5      ���	#FF3F3F3F���	#FF97A0A5$��	1,1,2.5,1q�5      F� 5       $ Arrow��6   $��1��5!      ���#00fbff$
��Left=�5#      $��Center=�5$      $��	1.3,0,0,0q�5%      =����        ����  -1�    ��- �     -1����     ���� ��- 	5&      5(      5)      �� 5+   
   " ��62   .R�5,      �� $ PART_ContentHost��6   |�.  #��z��6A   5-      �� 5/   
   "	 ��6*   .�50      �� 
 �@J�SnapsToDevicePixels . 64   �� 51      
 �@�OverridesDefaultStyle . 66   �� 52      
 � ��$
 Auto=�6K   �� 53      
 � ��$
 Auto=�6I   �� 54      
 � �� . 6>   �� 55      
 � ��$	 120��6)   �� 56      
 � ��$ 17��6*   �� 57      
 � ��
 �����6+   �� 58      
 � ��$ 15(�6*   �� 59      
 � �� 5:      �� 5;      " ��6&   .R�5<      � .+�5=      d� $��ToggleButton��5>   !   8��  $��2��5@   !   |�.  $ Press=�5C   !    5B   !   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5D      �� 5E      $��ContentSite��6/    .  #��z 6\   #��z 5F   !   #��z��5G   !   $��3,3,23,3q�5H   !   $��Center=�5I   !   $
��Left=�5J   !   �� 5K      $ PART_EditableTextBox��6&   8�� $
��Left=�5N   !   $��Center=�5O   !   $��3,3,23,3q�5P   !   |�. ���	#FF3F3F3F
��� � �$y�Hidden=�5T   !   # z 5U   !   ��5L   !   Z� 5U   !   � 5V      $��Popup��5W   !   $��Bottom=�5X   !   #��z 5Y   !    . |�.  $��Slide=�5\   !   .��5^   "   � $��DropDown��6'    . #��z 5`   #   #��z 5a   #   .+�5b   &   �� $ DropDownBorder��5c   %   ���	#FF3F3F3F$��1q�5f   %   ���#8888885g   %   �� 5h   &   $��4,6,4,6q�63    . .��5i   *   ��  . $ 	Contained=�6H   5j   '   5k   #   5l      5m       5n      P� 5o       �@��HasItems  .  . �5p   "   �� $! DropDownBorder��6)   
 � ��$ 95��6Z   5q      P� 5r       � {�  .  . �5s   "   �� 
 � �� �#8888886?   5t      P� 5u       �@��
IsGrouping  . . �5v   "   �� 
 � �� .  6R   5w      P� 5x      $" Popup��6&    �@�AllowsTransparency  . . �5y   "   �� $! DropDownBorder��6)   
 �@��CornerRadius$ 0��6]   �� 5z   "   $! DropDownBorder��6)   
 � ��$ 0,2,0,0q�6W   5{      P� 5|       �@��
IsEditable  . . �5}   "   �� 
 � �� .  6>   �� 5~   "   $! PART_EditableTextBox��6)   
 � y�$ Visible=�6a   �� 5   "   $! ContentSite��6)   
 � y�$ Hidden=�6X   5�      5�      5�      # 5�      5�      �� 5�   
   "	 ��6.   .�5�      �� 
 �@J�SnapsToDevicePixels . 64   �� 5�      
 � �� �#00fbff6+   �� 5�      
 �@�OverridesDefaultStyle . 66   �� 5�      
 � �� 5�      �� 5�      " ��6&   .R�5�      �� $��Border��6!   $$ 2q�5�   #    . ._�5�      �� 5�       5�      P� 5�       �@��IsHighlighted  . . �5�   "   �� $! Border��6)   
 � �� �	#FF4F4F4F6S   5�      P� 5�       � {�  .  . �5�   "   �� 
 � �� �#8888886?   5�      5�      5�      5�      5�      .��5�      � ���#202020.+�5�   
   �� ���#520000$��17(�6%   $��Italic$�63   $��0,-110,-100,0q�6F   $����DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
TE DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
ETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
ELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
TE DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
ETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
ELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
TE DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
ETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
ELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE DELETE 
��5�      z�5�      �� 5�      % �� Angle$% 15Q�6"   5�      �� 5�   
   $��Choice:��6   
�������$��Center=�65   $
��Left=�6P   ���#00fbff6k   �� 5�   
   -   5        & ��Name$& comboBoxChoice��5�      $��29��6+   ���#505050
����������#00fbff$��	49,0,10,0q�6u   �� 5�   
   -   5        ' ��Name$' btnOk��5�      $��OK��6    $��20��6-   $��80��69   $��Bottom=�6D   ���#505050
����������#00fbff$��	120,0,0,2q�6�   6�   �� 5�   
   -   5        $' 	btnCancel��5�      $��Cancel��6$   $��20��65   $��80��6A   $��Bottom=�6L   $��Right=�6g   $��0,0,10,2q�6�   ���#505050
����������#00fbff6�   5�      5�      