   M S B A M L   `   `   `  ���� 
  Yapper    Yapper.Client.ChatWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $��
chatWindow��  � Title$  Yapper��$	��409��$	��529��$��0,1,0,0q����	#04000000.��� }�. $��Stretch=����	#FFF0F0F0 J� 	AllowDrop .  � ColumnDefinitions �� $��8 ��� $	��61* ��� $
��256* ��� $	��34* ��� $	��67* ��� $��65 ��� $��8 � � RowDefinitions �� $��8 ��� $
��248* ��� $��57 ��� $��27 ��� $$��24                             �.+��� -   $��
messageBox��$��Stretch=�$��Stretch=�$	��Top=�$��1��$��5��$��1�� �� 
IsReadOnly .  �� IsUndoEnabled .  " �� VerticalScrollBarVisibility$
 Auto=� �� TextWrapping$
 Wrap=��� -   $��gridSplitter1��$��1��$��1��$��2��$��Stretch=�$��Bottom=�$��Stretch=�$��Bottom=� �� ResizeBehavior$ BasedOnAlignment=�$��5�����	#FFCCCCCC$��0,1,0,0q��� -   $��sendBox��$��1��$��2��$��Stretch=�$��Stretch=�$��0,3,3,0q�$��16��$��4��$
 Auto=�$	 �� HorizontalScrollBarVisibility$	 Hidden=�$
 Wrap=�{�.  �� -   $��sendBtn��$
��Send��$��5��$��2��$��Stretch=�$��Stretch=�$��16��$��3,3,0,0q�{�.  �� -   $��
statusBar1��$��7��$��4��$��0,1,0,0q�$��Stretch=�$��Bottom=�$��Bottom=�$��Stretch=����	#FF888888.@��� $
��Left=�.���� -   $��	statusBlk��Ready�� $��Right=�.���� -   $��pingBlk��140�� $��
Server IP:��$��3��$��27��$��Right=�$��Bottom=�$��1��$��61��$��Right=��� -	   $��ipBox��$��2��$��3��$��Stretch=�$��Stretch=�$��Stretch=�$��90��$	��0,2q�$��	127.0.0.1���� $��Port:��$��27��$��Right=�$��Bottom=�$��34��$��3��$��3��$��Right=��� -
   $��portBox��$��4��$��Stretch=�$��Stretch=�$��3��$��0,2,3,2q� . $��Stretch=�$��30��$��Stretch=�$
��2323���� -   $��
connectBtn��$��Connect��$��5��$��3��$��Stretch=�$��3,5,0,2q�$��16��$��Center=�