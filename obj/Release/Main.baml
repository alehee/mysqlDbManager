   M S B A M L   `   `   `  ���� I  EmysqlDbManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:mysqlDbManagermysqlDbManager      mysqlDbManager.Main   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:mysqlDbManager     -     � 
ResizeMode$  NoResize=� � Title$ Database `` management��$	��450��$	��800��.��� .+��� -    ��Name$ B_Logout��$��Logout��$��70��$��20��$��704,0,0,390q�
���  ���� -   $ 	B_Refresh��$��Refresh��$��70��$��20��$��550,0,0,390q��� -    ��Name$ 	CB_Tables��$	��140��$��20��$��329,0,0,390q��� -    ��Name$ TB_Query��$	��350��$��20��$��0,0,424,390q��� -   $ B_Query��$
��Send��$��50��$��20��$��
0,0,15,390q��� -   $ B_MagicQuery��$��	Edit data��$��55��$��20��$��100,0,0,390q��� $
��Auto=�$
��Auto=�$��0,0,0,60q�$	��300��$	��772��.��� -    �Name$ G_Table��$
��Left=�$��5q�$	��Top=� � ColumnDefinitions �� $	��100 ��� $	��100 ��� $	��100 � � RowDefinitions �� $��30 ��� $��50 ��� $��50 �.+��� $��0��$��0��$
��Bold"�.��
Header 1�� $��0��$��1��$
��Bold"�.��
Header 2�� $��0��$��2��$
��Bold"�.��
Header 3�� $��1��$��0��.��
Button 4�� $��1��$��1��.��
Button 5�� $��1��$��2��.��
Button 6�� $��2��$��0��.��
Button 7�� $��2��$��1��.��
Button 8�� $��2��$��2��.��
Button 9�� -	    ��Name$ L_ChangeCellAlert��$/��)To apply changes press enter on Text Box!��$
��Bold"�$��Right=�$��Bottom=�$��10,60q�	 J� Opacity$	 0Q��� -
   
 ��Name$
 TB_Log��$
��Left=�$��
10,335,0,0q� �� Padding$ 2q�$
��Wrap=�$��Console output��$	��Top=�$��70��$	��400�����	#FFDEDEDE�� -   $��Export .xlsx��$��70��$��20��$��300,20q�$��Right=�$��Bottom=��� -   $��Export .json��$��70��$��20��$��300,50q�$��Right=�$��Bottom=��� $��Cheese Logo.png��$��Right=�$��Bottom=�$��70,20q�$��50���� -   $ 	L_Version��$��v. 0.0.0��$��Right=�$��Bottom=�$��20,20q�