using System;
using System.Collections.Generic;
using System.Linq;

namespace Cecs475.BoardGames.Chess {
	public class ChessBoard : IGameBoard {
		/// <summary>
		/// The number of rows and columns on the chess board.
		/// </summary>
		public const int BOARD_SIZE = 8;

		// Reminder: there are 3 different types of rooks
       private sbyte[,] mBoard = new sbyte[8, 8] {
				{-2, -4, -5, -6, -7, -5, -4, -3 },
				{-1, -1, -1, -1, -1, -1, -1, -1 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{1, 1, 1, 1, 1, 1, 1, 1 },
				{2, 4, 5, 6, 7, 5, 4, 3 }
		  }; 

      /* Board Tests
      private sbyte[,] mBoard = new sbyte[8, 8] {
				{-2, -4, -5, -6, -7, -5, -4, -3 },
				{-1, -1, -1, -1, -1, -1, -1, -1 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{1, 1, 1, 1, 1, 1, 1, 1 },
				{2, 4, 5, 6, 7, 5, 4, 3 }
		  }; 
       
      // Blank
      private sbyte[,] mBoard = new sbyte[8, 8] {
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 },
				{0, 0, 0, 0, 0, 0, 0, 0 }
		  };
      */
             
      // The player, represented by 1 or -1
      private int mPlayer = 1;

      // Pawn promotion flag
      private bool pawnPromotionTime = false;
      
		/// <summary>
		/// Constructs a new chess board with the default starting arrangement.
		/// </summary>
		public ChessBoard() {
			MoveHistory = new List<IGameMove>();
         Value = 0;
		}

		/// <summary>
		/// Constructs a new chess board by only placing pieces as specified.
		/// </summary>
		/// <param name="startingPositions">a sequence of tuple pairs, where each pair specifies the starting
		/// position of a particular piece to place on the board</param>
		public ChessBoard(IEnumerable<Tuple<BoardPosition, ChessPiecePosition>> startingPositions)
			 : this()
		{ // NOTE THAT THIS CONSTRUCTOR CALLS YOUR DEFAULT CONSTRUCTOR FIRST

			foreach (int i in Enumerable.Range(0, 8)) { // another way of doing for i = 0 to < 8
				foreach (int j in Enumerable.Range(0, 8)) {
					mBoard[i, j] = 0;
				}
			}
			foreach (var pos in startingPositions) {
				SetPosition(pos.Item1, pos.Item2);
			}
		}

		/// <summary>
		/// A difference in piece values for the pieces still controlled by white vs. black, where
		/// a pawn is value 1, a knight and bishop are value 3, a rook is value 5, and a queen is value 9.
		/// </summary>
		public int Value { get; private set; }

		public int CurrentPlayer { get { return mPlayer == 1 ? 1 : 2; } }

		public IList<IGameMove> MoveHistory { get; private set; }

		/// <summary>
		/// Returns the piece and player at the given position on the board.
		/// </summary>
		public ChessPiecePosition GetPieceAtPosition(BoardPosition position) {
			var boardVal = mBoard[position.Row, position.Col];
			return new ChessPiecePosition((ChessPieceType)Math.Abs(mBoard[position.Row, position.Col]),
				boardVal > 0 ? 1 : boardVal < 0 ? 2 : 0);
		}

		public void ApplyMove(IGameMove move) {
         ChessMove m = move as ChessMove;
         m.Piece = GetPieceAtPosition(m.StartPosition);
         ChessPiecePosition empty = new ChessPiecePosition(ChessPieceType.Empty, 0);
         ChessPiecePosition movingPiece = GetPieceAtPosition(m.StartPosition);
         
         // Normal Moves
         if (m.MoveType == ChessMoveType.Normal) {
            // Check if the ending position empty
            if(PositionIsEmpty(m.EndPosition)) {
               // Set the starting postion to empty (0)
               SetPosition(m.StartPosition, empty);
               // The piece is now moved
               SetPosition(m.EndPosition, movingPiece);
            }
            // Check if the ending position has an enemy
            else if (PositionIsEnemy(m.EndPosition, CurrentPlayer)) {
               // Set the starting postion to empty (0)
               SetPosition(m.StartPosition, empty);
               // The enemy piece is now captured, record it for this move
               m.Captured = GetPieceAtPosition(m.EndPosition);
               // Update the value of the board after capture
               Value += GetPieceValue(m.Captured.PieceType) * mPlayer;
               // The piece is now moved
               SetPosition(m.EndPosition, movingPiece);
            }
            
            // Check for Pawn promotion, this happens after a move is applied
            if (GetPieceAtPosition(m.EndPosition).PieceType == ChessPieceType.Pawn 
               && (m.EndPosition.Row == 0 || m.EndPosition.Row == 7)) {
               MoveHistory.Add(m);
               pawnPromotionTime = true;
               return;
            }
            
         } else if (m.MoveType == ChessMoveType.EnPassant) {
            // The starting point is now empty
            SetPosition(m.StartPosition, empty);
            // The ending point now has the piece we want to move
            SetPosition(m.EndPosition, movingPiece);
            // Set the captured piece before removing it
            m.Captured = GetPieceAtPosition(new BoardPosition(m.EndPosition.Row + mPlayer, m.EndPosition.Col));
            // remove captured piece and set to empty.
            SetPosition(new BoardPosition(m.EndPosition.Row + mPlayer, m.EndPosition.Col), empty);
            Value += GetPieceValue(m.Captured.PieceType) * mPlayer;

         } else if (m.MoveType == ChessMoveType.CastleKingSide) {
            // Get the Rook that we are going to move
            BoardPosition rookPos = new BoardPosition(m.StartPosition.Row, m.StartPosition.Col + 3);
            ChessPiecePosition movingRook = GetPieceAtPosition(rookPos);
            // Set the end position to the movingPiece (King)
            SetPosition(m.EndPosition, movingPiece);
            // Empty the postion it started at
            SetPosition(m.StartPosition, empty);
            // Set the Rook at this position after the castle
            SetPosition(new BoardPosition(m.StartPosition.Row, m.StartPosition.Col + 1), movingRook);
            // Empty the rook spot
            SetPosition(rookPos, empty);
            
         } else if (m.MoveType == ChessMoveType.CastleQueenSide) {

            BoardPosition rookPos = new BoardPosition(m.StartPosition.Row, m.StartPosition.Col - 4);
            ChessPiecePosition movingRook = GetPieceAtPosition(rookPos);
            SetPosition(m.EndPosition, movingPiece);
            SetPosition(m.StartPosition, empty);
            SetPosition(new BoardPosition(m.StartPosition.Row, m.StartPosition.Col - 1), movingRook);
            SetPosition(rookPos, empty);

         } else if (m.MoveType == ChessMoveType.PawnPromote) {

            //ChessPiecePosition newPiece = new ChessPiecePosition((ChessPieceType)m.EndPosition.Col, CurrentPlayer);
            m.Piece = new ChessPiecePosition((ChessPieceType)m.EndPosition.Col, CurrentPlayer);
            Value -= GetPieceValue(GetPieceAtPosition(m.StartPosition).PieceType) * mPlayer;
            SetPosition(m.StartPosition, m.Piece);
            Value += GetPieceValue(GetPieceAtPosition(m.StartPosition).PieceType) * mPlayer;
            pawnPromotionTime = false;

         }
         
         mPlayer = -mPlayer;
         MoveHistory.Add(m);
		}

		public IEnumerable<IGameMove> GetPossibleMoves() {
			// TODO: implement this method by returning a list of all possible moves.
         List<ChessMove> moves = new List<ChessMove>();
			List<ChessMove> possMoves = new List<ChessMove>();
         
         // Check if a pawn is up for promotion
         if (pawnPromotionTime) {
            // Using a member variable is a hassle
            ChessMove recentMove = MoveHistory[MoveHistory.Count - 1] as ChessMove;
            possMoves.AddRange(GetPossiblePromotionMoves(recentMove.EndPosition) as IEnumerable<ChessMove>);
            return possMoves;
         }
         
         for(int i = 0; i < BOARD_SIZE; i++) {
            for(int j = 0; j < BOARD_SIZE; j++) {
               BoardPosition pos = new BoardPosition(i, j);
					ChessPiecePosition piece = GetPieceAtPosition(pos);
               if( piece.Player == CurrentPlayer) { 
                  if (piece.PieceType == ChessPieceType.Pawn) {
                     possMoves.AddRange(GetPawnPossibleMoves(pos) as IEnumerable<ChessMove>);
					   } else if (piece.PieceType == ChessPieceType.Knight) {
                     possMoves.AddRange(GetKnightPossibleMoves(pos) as IEnumerable<ChessMove>);
					   } else if (piece.PieceType == ChessPieceType.RookKing 
						   || piece.PieceType == ChessPieceType.RookQueen 
						   || piece.PieceType == ChessPieceType.RookPawn) {
                     possMoves.AddRange(GetRookPossibleMoves(pos) as IEnumerable<ChessMove>);
					   } else if (piece.PieceType == ChessPieceType.Bishop) {
                     possMoves.AddRange(GetBishopPossibleMoves(pos) as IEnumerable<ChessMove>);
					   } else if (piece.PieceType == ChessPieceType.Queen) {
                     possMoves.AddRange(GetBishopPossibleMoves(pos) as IEnumerable<ChessMove>);
                     possMoves.AddRange(GetRookPossibleMoves(pos) as IEnumerable<ChessMove>);
					   } else if (piece.PieceType == ChessPieceType.King) {
                     possMoves.AddRange(GetKingPossibleMoves(pos) as IEnumerable<ChessMove>);
                     possMoves.AddRange(GetCastlingPossibleMoves(pos) as IEnumerable<ChessMove>);
					   } else {
						   continue;
					   }
               }
            }
         }

         // Cheating the system to keep track of value LOL
         // Keep track of the value before filtering messes it all up
         //int tempVal = Value;
         // Keep track of player
         int temp = CurrentPlayer;
         // Filter out the moves that leave the king in check
         for (int i = 0; i < possMoves.Count; i++) {
            // Apply the move, ApplyMove will switch the players unless it is a pawn promotion move.
            ApplyMove(possMoves[i]);
            // Find the king after the move is applied, with respect to the "ACTUAL" current player
            var kingPos = GetPositionsOfPiece(ChessPieceType.King, temp);
            // The king does not exist, don't even bother
            if (kingPos.Count() == 0) {
               UndoLastMove();
               break;
            }
            // Does the king even exist
            if (kingPos.Count() != 0) {
               var king = kingPos.First();
               List<BoardPosition> threatened = new List<BoardPosition>();
               // Account for a normal move going into a pawn promotion
               // This means the player will not change, the 4 promotion moves won't actually be applied though
               if (pawnPromotionTime) {
                  threatened.AddRange(GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1));
               } else {
                  threatened.AddRange(GetThreatenedPositions(CurrentPlayer));
               }
               if (threatened.Contains(king)) {
                  // Drops the index by 1 or we end up skipping a move
                  possMoves.RemoveAt(i);
                  i = i - 1;
               }
            }
            // The undo will change pawn promotion time as needed
            UndoLastMove();
         }
         // Set the value to what it technically should be LOL
         // Value = tempVal;
         
         return possMoves;
		}

      private IEnumerable<IGameMove> GetPawnPossibleMoves(BoardPosition pos) {
         List<BoardPosition> test = new List<BoardPosition>();
         List<ChessMove> moves = new List<ChessMove>();
         List<ChessMove> possMoves = new List<ChessMove>();
         var i = CurrentPlayer == 1 ? -1 : CurrentPlayer == 2 ? 1 : 0;

         // Two moves in the diagonal directions
         BoardPosition d1 = new BoardPosition(pos.Row + i, pos.Col - i);
         BoardPosition d2 = new BoardPosition(pos.Row + i, pos.Col + i);

         // Can only move diagonally if it is in bounds and has an enemy to kill
         if (PositionInBounds(d1) && PositionIsEnemy(d1, CurrentPlayer)) {
            test.Add(d1);
         }
         if (PositionInBounds(d2) && PositionIsEnemy(d2, CurrentPlayer)) {
            test.Add(d2);
         }

         // One space in front of the pawn
         BoardPosition s1 = new BoardPosition(pos.Row + i, pos.Col);
         if (PositionInBounds(s1) && PositionIsEmpty(s1)) {
            test.Add(s1);
         }

         // Two spaces in front of the pawn
         BoardPosition s2 = new BoardPosition(pos.Row + (i * 2), pos.Col);

         // Check if nothing is in the way and which player goes which way
         if (CurrentPlayer == 1) {
            if (pos.Row == 6 && 
               PositionIsEmpty(s1) &&
               PositionIsEmpty(s2)) {
               test.Add(s2);
            }
         }
         else {
            if (pos.Row == 1 && 
               PositionIsEmpty(s1) &&
               PositionIsEmpty(s2)) {
               test.Add(s2);
            }
         }

         foreach(BoardPosition m in test) {
            moves.Add(new ChessMove(pos, m));
         }
         
         // is En Passant possible? 
         // Is the history empty?
         if (MoveHistory.Count != 0) {
            // Check the most recent move
            ChessMove recentMove = MoveHistory[MoveHistory.Count - 1] as ChessMove;
            // Check that the most recent move was made by a Pawn, and was not my us
            if (recentMove.Piece.PieceType == ChessPieceType.Pawn && recentMove.Piece.Player != CurrentPlayer) {
               // Check if the start pos - end pos = 2, meaning the pawn did a double jump
               int moved = Math.Abs(recentMove.StartPosition.Row - recentMove.EndPosition.Row);
               if (moved == 2) {
                  if (pos.Row == recentMove.EndPosition.Row) {
                     // Check if there is a pawn next to us
                     if (pos.Col + 1 == recentMove.EndPosition.Col) {
                        moves.Add(new ChessMove(pos, new BoardPosition(pos.Row - mPlayer, pos.Col + 1), ChessMoveType.EnPassant));
                     } else if (pos.Col - 1 == recentMove.EndPosition.Col) {
                        moves.Add(new ChessMove(pos, new BoardPosition(pos.Row - mPlayer, pos.Col - 1), ChessMoveType.EnPassant));
                     }
                  }
               }
            }
         }

         return moves;
      }

      private IEnumerable<IGameMove> GetKnightPossibleMoves(BoardPosition pos) {
         List<ChessMove> moves = new List<ChessMove>();
         List<ChessMove> possMoves = new List<ChessMove>();
         var threatened = GetKnightThreatenedPositions(CurrentPlayer, pos);

         foreach(BoardPosition p in threatened) {
            if (GetPlayerAtPosition(p) != CurrentPlayer) {
               moves.Add(new ChessMove(pos, p));
            }
         }
         return moves;
      }

      private IEnumerable<IGameMove> GetBishopPossibleMoves(BoardPosition pos) {
         List<ChessMove> moves = new List<ChessMove>();
         List<ChessMove> possMoves = new List<ChessMove>();
         var threatened = GetBishopThreatenedPositions(CurrentPlayer, pos);

         foreach(BoardPosition p in threatened) {
            if (GetPlayerAtPosition(p) != CurrentPlayer) {
               moves.Add(new ChessMove(pos, p));
            }
         }
         
         return moves;
      }

      private IEnumerable<IGameMove> GetRookPossibleMoves(BoardPosition pos) {
         List<ChessMove> moves = new List<ChessMove>();
         List<ChessMove> possMoves = new List<ChessMove>();
         var threatened = GetRookThreatenedPositions(CurrentPlayer, pos);

         foreach(BoardPosition p in threatened) {
            if (GetPlayerAtPosition(p) != CurrentPlayer) {
               moves.Add(new ChessMove(pos, p));
            }
         }
         
         return moves;
      }

      private IEnumerable<IGameMove> GetKingPossibleMoves(BoardPosition pos) {
         List<ChessMove> possMoves = new List<ChessMove>();
         var threatenedbyMe = GetKingThreatenedPositions(CurrentPlayer, pos);
         var threatenedbyEnemy = GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);
         
         foreach(BoardPosition p in threatenedbyMe) {
            if ((GetPlayerAtPosition(p) != CurrentPlayer) && !threatenedbyEnemy.Contains(p)) {
               possMoves.Add(new ChessMove(pos, p));
            }
         }
         
         return possMoves;
      }

      private IEnumerable<IGameMove> GetCastlingPossibleMoves(BoardPosition pos) {
         List<ChessMove> possMoves = new List<ChessMove>();
         var t = GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);
         var king = GetPositionsOfPiece(ChessPieceType.King, CurrentPlayer);

         if (t.Contains(pos)) {
            return possMoves;
         }
         
         // Depending on the player, which row is it?
         int row = CurrentPlayer == 1 ? 7 : 0;
         // The exact position of where the king should be for castling
         var kingPos = GetPieceAtPosition(new BoardPosition(row, 4));
         // The exact position of where the RookKing should be for castling
         var rookKingPos = GetPieceAtPosition(new BoardPosition(row, 7));
         // The exact position of where the RookQueen should be for castling
         var rookQueenPos = GetPieceAtPosition(new BoardPosition(row, 0));
         // The threatened positions

         var threats = GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);

         bool kingInHistory = false;
         bool rookKingInHistory = false;
         bool rookQueenInHistory = false;

         bool kingInPlace = false;
         bool rookKingInPlace = false;
         bool rookQueenInPlace = false;

         // Is there a move in the history that involves the King, RookKing, or RookQueen?
         if (MoveHistory.Count != 0) { 
            foreach(ChessMove m in MoveHistory) {
               if (m.Piece.PieceType == ChessPieceType.King && m.Piece.Player == CurrentPlayer) {
                  kingInHistory = true;
                  break;
               }
               if (m.Piece.PieceType == ChessPieceType.RookKing && m.Piece.Player == CurrentPlayer) {
                  rookKingInHistory = true;
                  break;
               }
               if (m.Piece.PieceType == ChessPieceType.RookQueen && m.Piece.Player == CurrentPlayer) {
                  rookQueenInHistory = true;
                  break;
               }
            }
         }

         // Are the King, RookKing, and RookQueen in the exact spots they need to be in to perform a castle?
         if (kingPos.PieceType == ChessPieceType.King && kingPos.Player == CurrentPlayer) {
            kingInPlace = true;
         }
         if (rookKingPos.PieceType == ChessPieceType.RookKing && rookKingPos.Player == CurrentPlayer) {
            rookKingInPlace = true;
         }
         if (rookQueenPos.PieceType == ChessPieceType.RookQueen && rookQueenPos.Player == CurrentPlayer) {
            rookQueenInPlace = true;
         }
         
         // If a king move does not exist in the history and the king is in the spot needed to castle
         if (!kingInHistory && kingInPlace) {
            if (!rookKingInHistory && rookKingInPlace) {
               List<BoardPosition> path = new List<BoardPosition>();
               bool clearPath = true;
               
               // Empty Check, is every spot on this path empty?   
               path.Add(new BoardPosition(pos.Row, pos.Col + 1));
               path.Add(new BoardPosition(pos.Row, pos.Col + 2));

               BoardPosition a = new BoardPosition(pos.Row, pos.Col + 1);
               BoardPosition b = new BoardPosition(pos.Row, pos.Col + 2);

               if (!PositionIsEmpty(a)) {
                  clearPath = false;
               }
               if (!PositionIsEmpty(b)) {
                  clearPath = false;
               }

               if (clearPath) {
                  //if (GetPieceAtPosition(new BoardPosition(row, 7)).PieceType.Equals(ChessPieceType.RookKing)
                  if (GetPieceAtPosition(new BoardPosition(row, 7)).PieceType.Equals(ChessPieceType.RookKing)
                          && !threats.Contains(a)  && !threats.Contains(b)
                           ) {
                     possMoves.Add(new ChessMove(pos, new BoardPosition(row, 6), ChessMoveType.CastleKingSide));
                  }
               }
            }
            if (!rookQueenInHistory && rookQueenInPlace) {
               List<BoardPosition> path = new List<BoardPosition>();
               bool clearPath = true;
               
               // Empty Check, is every spot on this path empty?   
               path.Add(new BoardPosition(pos.Row, pos.Col - 1));
               path.Add(new BoardPosition(pos.Row, pos.Col - 2));
               path.Add(new BoardPosition(pos.Row, pos.Col - 3));

               BoardPosition a = new BoardPosition(pos.Row, pos.Col - 1);
               BoardPosition b = new BoardPosition(pos.Row, pos.Col - 2);
               BoardPosition c = new BoardPosition(pos.Row, pos.Col - 3);

               if (!PositionIsEmpty(a)) {
                  clearPath = false;
               }
               if (!PositionIsEmpty(b)) {
                  clearPath = false;
               }
               if (!PositionIsEmpty(c)) {
                  clearPath = false;
               }
              

               if (clearPath) {
                  //if (GetPieceAtPosition(new BoardPosition(row, 0)).PieceType.Equals(ChessPieceType.RookQueen)
                  if (GetPieceAtPosition(new BoardPosition(row, 0)).PieceType.Equals(ChessPieceType.RookQueen)
                          && !threats.Contains(a)  && !threats.Contains(b)
                           ) {
                     possMoves.Add(new ChessMove(pos, new BoardPosition(row, 2), ChessMoveType.CastleQueenSide));
                  }
               }

            }
         }
         return possMoves;
      }

      private IEnumerable<IGameMove> GetPossiblePromotionMoves(BoardPosition pos) {
         List<ChessMove> possMoves = new List<ChessMove>();

         possMoves.Add(new ChessMove(pos, new BoardPosition(-1, (int)ChessPieceType.RookPawn), ChessMoveType.PawnPromote));
         possMoves.Add(new ChessMove(pos, new BoardPosition(-1, (int)ChessPieceType.Knight), ChessMoveType.PawnPromote));
         possMoves.Add(new ChessMove(pos, new BoardPosition(-1, (int)ChessPieceType.Bishop), ChessMoveType.PawnPromote));
         possMoves.Add(new ChessMove(pos, new BoardPosition(-1, (int)ChessPieceType.Queen), ChessMoveType.PawnPromote));

         return possMoves;
      }

		/// <summary>
		/// Gets a sequence of all positions on the board that are threatened by the given player. A king
		/// may not move to a square threatened by the opponent.
		/// </summary>
		public IEnumerable<BoardPosition> GetThreatenedPositions(int byPlayer) {
			List<BoardPosition> threatenedPos= new List<BoardPosition>();

			for (int i = 0; i < BOARD_SIZE; i++) {
				for (int j = 0; j < BOARD_SIZE; j++) {
					BoardPosition pos = new BoardPosition(i, j);
					ChessPiecePosition piece = GetPieceAtPosition(pos);
               if (piece.Player == byPlayer) { 
					   if (piece.PieceType == ChessPieceType.Pawn) {
						   threatenedPos.AddRange(GetPawnThreatenedPositions(byPlayer, pos));
					   } else if (piece.PieceType == ChessPieceType.Knight) {
                     threatenedPos.AddRange(GetKnightThreatenedPositions(byPlayer, pos));
					   } else if (piece.PieceType == ChessPieceType.RookKing 
						   || piece.PieceType == ChessPieceType.RookQueen 
						   || piece.PieceType == ChessPieceType.RookPawn) {
                     threatenedPos.AddRange(GetRookThreatenedPositions(byPlayer, pos));
					   } else if (piece.PieceType == ChessPieceType.Bishop) {
                     threatenedPos.AddRange(GetBishopThreatenedPositions(byPlayer, pos));
					   } else if (piece.PieceType == ChessPieceType.Queen) {
                     threatenedPos.AddRange(GetRookThreatenedPositions(byPlayer, pos));
						   threatenedPos.AddRange(GetBishopThreatenedPositions(byPlayer, pos));
					   } else if (piece.PieceType == ChessPieceType.King) {
                     threatenedPos.AddRange(GetKingThreatenedPositions(byPlayer, pos));
					   } else {
						   continue;
					   }
               }
				}
			}

         return threatenedPos;
		}

      private IEnumerable<BoardPosition> GetPawnThreatenedPositions(int byPlayer, BoardPosition pos) {
         // A Pawn threatens in the 2 spaces diagonally in front of it.
			List<BoardPosition> moves = new List<BoardPosition>();

         int i = (byPlayer == 1 ? -1 : 1);
         BoardPosition a = new BoardPosition(pos.Row + i, pos.Col - 1);
         BoardPosition b = new BoardPosition(pos.Row + i, pos.Col + 1);

         // Check if the moves are in bounds
         if (PositionInBounds(a)) {
            moves.Add(a);
         }
         if (PositionInBounds(b)) {
            moves.Add(b);
         }

			return moves;
		}

      private IEnumerable<BoardPosition> GetKnightThreatenedPositions(int byPlayer, BoardPosition pos) {
			// A Knight threatens 8 spaces around it in an L shape, no regards to who is in the way
			List<BoardPosition> moves = new List<BoardPosition>();
         List<BoardPosition> checkMoves = new List<BoardPosition>();

         checkMoves.Add(new BoardPosition(pos.Row - 2, pos.Col - 1));
         checkMoves.Add(new BoardPosition(pos.Row - 2, pos.Col + 1));
         checkMoves.Add(new BoardPosition(pos.Row - 1, pos.Col - 2));
         checkMoves.Add(new BoardPosition(pos.Row - 1, pos.Col + 2));
         checkMoves.Add(new BoardPosition(pos.Row + 1, pos.Col - 2));
         checkMoves.Add(new BoardPosition(pos.Row + 1, pos.Col + 2));
         checkMoves.Add(new BoardPosition(pos.Row + 2, pos.Col - 1));
         checkMoves.Add(new BoardPosition(pos.Row + 2, pos.Col + 1));

         // Check that the moves are in bounds
         foreach (BoardPosition m in checkMoves) {
            if (PositionInBounds(m)) {
               moves.Add(m);
            }
         }

         return moves;
		}

      private IEnumerable<BoardPosition> GetBishopThreatenedPositions(int byPlayer, BoardPosition pos) {
         // A Bishop threatens in the 4 diagonal directions up to an enemy/ally piece in the way
			List<BoardPosition> moves = new List<BoardPosition>();
			int nRow = pos.Row;
			int nCol = pos.Col;

			for (int i = 0; i < BOARD_SIZE; i++) { // Up, Left (-1 , -1)
				nRow = nRow - 1;
				nCol = nCol - 1;
				BoardPosition newPos = new BoardPosition(nRow, nCol);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

			nRow = pos.Row;
			nCol = pos.Col;

			for (int i = 0; i < BOARD_SIZE; i++) { // Up, right (-1, +1)
				nRow = nRow - 1;
				nCol = nCol + 1;
				BoardPosition newPos = new BoardPosition(nRow, nCol);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

			nRow = pos.Row;
			nCol = pos.Col;

			for (int i = 0; i < BOARD_SIZE; i++) { // Down, Left (+1, -1)
				nRow = nRow + 1;
				nCol = nCol - 1;
				BoardPosition newPos = new BoardPosition(nRow, nCol);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

			nRow = pos.Row;
			nCol = pos.Col;
         
			for (int i = 0; i < BOARD_SIZE; i++) { // Down, right (+1, +1)
				nRow = nRow + 1;
				nCol = nCol + 1;
				BoardPosition newPos = new BoardPosition(nRow, nCol);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

			return moves;
		}

      private IEnumerable<BoardPosition> GetRookThreatenedPositions(int byPlayer, BoardPosition pos) {
			// A Rook threatens in the 4 directions vertical and horizontal up to an enemy/ally piece in the way
			List<BoardPosition> moves = new List<BoardPosition>();
			int nRow = pos.Row;
			int nCol = pos.Col;
			
			// A Rook can move in 4 possible directions
			for (int i = 0; i < BOARD_SIZE; i++) { // Up
				nRow = nRow - 1;
				BoardPosition newPos = new BoardPosition(nRow, pos.Col);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

			nRow = pos.Row;
         nCol = pos.Col;

			for (int i = 0; i < BOARD_SIZE; i++) { // Down
				nRow = nRow + 1;
				BoardPosition newPos = new BoardPosition(nRow, pos.Col);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

         nRow = pos.Row;
         nCol = pos.Col;

			for (int i = 0; i < BOARD_SIZE; i++) { // Right
            nCol = nCol - 1;
				BoardPosition newPos = new BoardPosition(pos.Row, nCol);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

			nRow = pos.Row;
         nCol = pos.Col;

			for (int i = 0; i < BOARD_SIZE; i++) { // Left
				nCol = nCol + 1;
				BoardPosition newPos = new BoardPosition(pos.Row, nCol);
				if (PositionInBounds(newPos)) {
               if (PositionIsEmpty(newPos)) {
                  moves.Add(newPos);
               }
               else if (PositionIsEnemy(newPos, byPlayer) || !PositionIsEnemy(newPos, byPlayer)) {
                  moves.Add(newPos);
                  break; // Stop traveling in this direction because we found an enemy or ally
               }
				}
            // If the spot we check is not in boounds, we can break immediately in this direction
            else if (!PositionInBounds(newPos)) {
               break;
            }
			}

			return moves;
		}

      private IEnumerable<BoardPosition> GetKingThreatenedPositions(int byPlayer, BoardPosition pos) {
			// A king threatens in one space in all directions
			List<BoardPosition> moves = new List<BoardPosition>();
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if (i == 0 && j == 0) {
							continue;
					}
               BoardPosition p = pos.Translate(i, j);
               if (PositionInBounds(p)) {
                  moves.Add(p);
               }
				}
			}

         return moves;
		}
      
		public void UndoLastMove() {
         ChessMove m = MoveHistory[MoveHistory.Count - 1] as ChessMove;
         int otherPlayer = CurrentPlayer == 1 ? 2 : 1;
         ChessPiecePosition empty = new ChessPiecePosition(ChessPieceType.Empty, 0);
         ChessPiecePosition pieceMovingBack = new ChessPiecePosition(m.Piece.PieceType, otherPlayer);
         // It is currently the other players turn we are undoing, so this is our peice that was captured.
         //ChessPiecePosition captured = new ChessPiecePosition(m.Captured.PieceType, CurrentPlayer);
         
         if (MoveHistory.Count > 0) { 
            if (m.MoveType == ChessMoveType.Normal) {
               if (pawnPromotionTime) {
                  ChessPiecePosition pieceMovingBackFromPromotion = new ChessPiecePosition(m.Piece.PieceType, CurrentPlayer);
                  SetPosition(m.StartPosition, pieceMovingBackFromPromotion);
               
                  SetPosition(m.EndPosition, m.Captured);
                  Value -= GetPieceValue(m.Captured.PieceType) * mPlayer;

                  MoveHistory.RemoveAt(MoveHistory.Count - 1);
                  pawnPromotionTime = false;
                  return;
               }

               SetPosition(m.StartPosition, pieceMovingBack);

               if (!m.Captured.Equals(null)) {
                  SetPosition(m.EndPosition, m.Captured);
                  Value += GetPieceValue(m.Captured.PieceType) * mPlayer;
               } else {
                  SetPosition(m.EndPosition, empty);
               }

            } else if (m.MoveType == ChessMoveType.EnPassant) {
               SetPosition(m.StartPosition, pieceMovingBack);
               SetPosition(m.EndPosition, empty);
               SetPosition(new BoardPosition(m.EndPosition.Row - mPlayer, m.EndPosition.Col), m.Captured);
               Value += GetPieceValue(m.Captured.PieceType) * mPlayer;

            } else if (m.MoveType == ChessMoveType.CastleKingSide) {
               BoardPosition rookPos = new BoardPosition(m.StartPosition.Row, m.StartPosition.Col + 1);
               ChessPiecePosition movingRook = GetPieceAtPosition(rookPos);
               SetPosition(m.StartPosition, pieceMovingBack);
               SetPosition(m.EndPosition, empty);
               SetPosition(new BoardPosition(m.StartPosition.Row, m.StartPosition.Col + 3), movingRook);
               SetPosition(rookPos, empty);

            } else if (m.MoveType == ChessMoveType.CastleQueenSide) {
               BoardPosition rookPos = new BoardPosition(m.StartPosition.Row, m.StartPosition.Col - 1);
               ChessPiecePosition movingRook = GetPieceAtPosition(rookPos);
               SetPosition(m.StartPosition, pieceMovingBack);
               SetPosition(m.EndPosition, empty);
               SetPosition(new BoardPosition(m.StartPosition.Row, m.StartPosition.Col - 4), movingRook);
               SetPosition(rookPos, empty);

            } else if (m.MoveType == ChessMoveType.PawnPromote) {
               ChessPiecePosition demoted = new ChessPiecePosition(ChessPieceType.Pawn, otherPlayer);
               Value -= GetPieceValue(GetPieceAtPosition(m.StartPosition).PieceType) * -mPlayer;
               SetPosition(m.StartPosition, new ChessPiecePosition(ChessPieceType.Pawn, otherPlayer));
               Value += GetPieceValue(ChessPieceType.Pawn) * -mPlayer;
               pawnPromotionTime = true;
            }
         } else {
            // Whoever wrote this test WTF...
            throw new Exception();
         }
         
         // Reset the remaining game state.
         mPlayer = -mPlayer;
			MoveHistory.RemoveAt(MoveHistory.Count - 1);
		}

		/// <summary>
		/// Returns true if the given position on the board is empty.
		/// </summary>
		/// <remarks>returns false if the position is not in bounds</remarks>
		public bool PositionIsEmpty(BoardPosition pos) {
			return GetPieceAtPosition(pos).PieceType == ChessPieceType.Empty && PositionInBounds(pos);
		}

		/// <summary>
		/// Returns true if the given position contains a piece that is the enemy of the given player.
		/// </summary>
		/// <remarks>returns false if the position is not in bounds</remarks>
		public bool PositionIsEnemy(BoardPosition pos, int player) {
         var boardVal =  mBoard[pos.Row, pos.Col];
         var playerAtPos = (boardVal > 0) ? 1 :(boardVal < 0) ? 2 : 0;
         return (player != playerAtPos) && (playerAtPos != 0);
		}

		/// <summary>
		/// Returns true if the given position is in the bounds of the board.
		/// </summary>
		public static bool PositionInBounds(BoardPosition pos) {
			return pos.Row >= 0 && pos.Row < BOARD_SIZE && pos.Col >= 0 && pos.Col < BOARD_SIZE;
		}

		/// <summary>
		/// Returns which player has a piece at the given board position, or 0 if it is empty.
		/// </summary>
		public int GetPlayerAtPosition(BoardPosition pos) {
			return GetPieceAtPosition(pos).Player;
		}

		/// <summary>
		/// Gets the value weight for a piece of the given type.
		/// </summary>
		/*
	  * VALUES:
	  * Pawn: 1
	  * Knight: 3
	  * Bishop: 3
	  * Rook: 5
	  * Queen: 9
	  * King: infinity (maximum integer value)
	  */
		public int GetPieceValue(ChessPieceType pieceType) {
         switch(pieceType) {
            case ChessPieceType.Empty: return 0;
            case ChessPieceType.Pawn: return 1;
            case ChessPieceType.Knight: return 3;
            case ChessPieceType.Bishop: return 3;
            case ChessPieceType.RookKing: return 5;
            case ChessPieceType.RookQueen: return 5;
            case ChessPieceType.RookPawn: return 5;
            case ChessPieceType.Queen: return 9;
            case ChessPieceType.King: return int.MaxValue;
            default: return 0;
         }
		}

      /// <summary>
		/// Returns a sequence of all positions that contain the given piece controlled by the given player.
		/// </summary>
		/// <returns>an empty sequence if the given player does not control any of the given piece type</returns>
		public IEnumerable<BoardPosition> GetPositionsOfPiece(ChessPieceType piece, int player) {
         // List of positions
         List<BoardPosition> posList = new List<BoardPosition>();

         // Iterate through board to find pieces
         foreach (int i in Enumerable.Range(0, 8)) { 
				foreach (int j in Enumerable.Range(0, 8)) {
               BoardPosition pos = new BoardPosition(i, j);
               ChessPiecePosition pieceAtPos = GetPieceAtPosition(pos);
               // If the piece is the specified piece and player
               if (pieceAtPos.Player.Equals(player) && pieceAtPos.PieceType.Equals(piece)) {
                  posList.Add(pos);
               }
				}
			}

         return posList;
		}

      /// <summary>
		/// True if the current player is in check and has no possible moves.
		/// </summary>
      public bool IsCheckmate {
			get {
				var kingPos = GetPositionsOfPiece(ChessPieceType.King, CurrentPlayer);
            if (kingPos.Count() == 0) {
               // Don't even bother
               return false;
            }
				var king = kingPos.First();
				var threatened = GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);
				int possMoves = GetPossibleMoves().Count();
				return threatened.Contains(king) && possMoves == 0;
			}
		}

		/// <summary>
		/// True if the game is a statemate because the current player has no moves, but is not in check.
		/// </summary>
		public bool IsStalemate {
			get {
				var kingPos = GetPositionsOfPiece(ChessPieceType.King, CurrentPlayer);
            if (kingPos.Count() == 0) {
               // Don't even bother
               return false;
            }
				var king = kingPos.First();
				var threatened = GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);
				int possMoves = GetPossibleMoves().Count();
				return !threatened.Contains(king) && possMoves == 0;
			}
		}

		/// <summary>
		/// True if the current player is in check but has at least one possible move.
		/// </summary>
		public bool IsCheck {
			get {
            var kingPos = GetPositionsOfPiece(ChessPieceType.King, CurrentPlayer);
            if (kingPos.Count() == 0) {
               // Don't even bother
               return false;
            }
				var king = kingPos.First();
				var threatened = GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);
				int possMoves = GetPossibleMoves().Count();
				return threatened.Contains(king) && possMoves > 0;
			}
		}

      public int staticBoardWeight() {
         int staticEval = Value;

         for (int row = 0; row < BOARD_SIZE; row++) {
            for (int col = 0; col < BOARD_SIZE; col++) {
               ChessPiecePosition check = GetPieceAtPosition(new BoardPosition(row, col));
               if (check.Player == 1 && check.PieceType == ChessPieceType.Pawn) {
                  staticEval += (6 - row);
               }
               else if (check.Player== 2 && check.PieceType == ChessPieceType.Pawn) {
                  staticEval -= (row - 1);
               }
            }
         }
         
			List<BoardPosition> list = GetThreatenedPositions(1) as List<BoardPosition>;
			foreach (var pos in list) {
            var checkPiece = GetPieceAtPosition(pos);
				if (checkPiece.PieceType == ChessPieceType.Bishop 
            || checkPiece.PieceType == ChessPieceType.Knight) {
                  staticEval += 1;
            }
            else if (checkPiece.PieceType == ChessPieceType.RookKing
            || checkPiece.PieceType == ChessPieceType.RookQueen
            || checkPiece.PieceType == ChessPieceType.RookPawn) {
               if (checkPiece.Player == 2)
                  staticEval += 2;
            }
            else if (checkPiece.PieceType == ChessPieceType.Queen) {
               if (checkPiece.Player == 2)
                  staticEval += 5;
            }
            else if (checkPiece.PieceType == ChessPieceType.King) {
               if (checkPiece.Player == 2)
                  staticEval += 4;
            }	
			}

        	List<BoardPosition> list2 = GetThreatenedPositions(2) as List<BoardPosition>;
			foreach (var pos in list2) {
            var checkPiece = GetPieceAtPosition(pos);
				if (checkPiece.PieceType == ChessPieceType.Bishop 
            || checkPiece.PieceType == ChessPieceType.Knight) {
                  staticEval -= 1;
            }
            else if (checkPiece.PieceType == ChessPieceType.RookKing
            || checkPiece.PieceType == ChessPieceType.RookQueen
            || checkPiece.PieceType == ChessPieceType.RookPawn) {
               if (checkPiece.Player == 1)
                  staticEval -= 2;
            }
            else if (checkPiece.PieceType == ChessPieceType.Queen) {
               if (checkPiece.Player == 1)
                  staticEval -= 5;
            }
            else if (checkPiece.PieceType == ChessPieceType.King) {
               if (checkPiece.Player == 1)
                  staticEval -= 4;
            }	
			}
			return staticEval;
      }


      public int Weight{
         get{
            return staticBoardWeight();
         }
      }

      public bool IsFinished {
         get {
            return IsCheckmate;
         }
      }

      /// <summary>
      /// Manually places the given piece at the given position.
      /// </summary>
      // This is used in the constructor
      private void SetPosition(BoardPosition position, ChessPiecePosition piece) {
			mBoard[position.Row, position.Col] = (sbyte)((int)piece.PieceType * (piece.Player == 2 ? -1 :
				 piece.Player));
		}
	}
}