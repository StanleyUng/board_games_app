   M S B A M L   `   `   `  ����d  `Cecs475.BoardGames.Chess.View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=68e71c13048d452aM+clr-namespace:Cecs475.BoardGames.Chess.ViewCecs475.BoardGames.Chess.View  5   	   4    .Cecs475.BoardGames.Chess.View.PawnPromoteBlack   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:Cecs475.BoardGames.Chess.View     5   	     � WindowStyle$
  None=�5   	   
�������
���   �$��5q�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency . $	��150��5   	   $	��450��6    � 	Resources 5         
BlackQueen  	BlackRook  BlackBishop  BlackKnight��%�   &	        &	 2     &	 c     &	 �     �� 5   
   $��Resources/BlackQueen.png��6#   �� 5   
   $��Resources/BlackRook.png��6"   �� 5   
   $��Resources/BlackBishop.png��6$   �� 5   
   $��Resources/BlackKnight.png��6$   5      .��5      � .+�5   
   �� Promote pawn to:6'   �� 5   
   -   5         ��Name$ BlackQueenPromote��5      #��[  6,   
����  �$	��100��6d   $	��100��6q   $
��Left=�5      $��	25,25,0,0q�6,   $	��Top=�6?   6W   �� 5   
   -   5        $ BlackRookPromote��5      #��[ 6+   
���  ��$	��100��6a   $	��100��6n   $
��Left=�5      $��
125,25,0,0q�6,   $	��Top=�6@   6X   �� 5   
   -   5        $ BlackBishopPromote��5      #��[ 6-   
��� ���$	��100��6h   $	��100��6u   $
��Left=�5      $��
225,25,0,0q�6,   $	��Top=�6@   6X   �� 5   
   -   5        $ BlackKnightPromote��5      #��[ 6-   
��� � �$	��100��6g   $	��100��6t   $
��Left=�5       $��
325,25,0,0q�6,   $	��Top=�6@   6X   5!      5"      