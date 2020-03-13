using Cecs475.BoardGames.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cecs475.BoardGames.ComputerOpponent;
using System.Windows;

namespace Cecs475.BoardGames.Chess.View {
   public class ChessSquare : INotifyPropertyChanged {
        private int mPlayer;
		public int Player {
			get { return mPlayer; }
			set {
				if (value != mPlayer) {
					mPlayer = value;
					OnPropertyChanged(nameof(Player));
				}
			}
		}

        private bool mIsHovered;
		public bool IsHovered {
			get { return mIsHovered; }
			set {
				if (value != mIsHovered) {
					mIsHovered = value;
					OnPropertyChanged(nameof(IsHovered));
				}
			}
		}

        private bool mIsSelected;
		public bool IsSelected {
			get { return mIsSelected; }
			set {
				if (value != mIsSelected) {
					mIsSelected = value;
					OnPropertyChanged(nameof(IsSelected));
				}
			}
		}

		private bool mIsCheck;
		public bool IsCheck {
			get { return mIsCheck; }
			set {
				if (value != mIsCheck) {
					mIsCheck = value;
					OnPropertyChanged(nameof(IsCheck));
				}
			}
		}

		public BoardPosition Position {
			get; set;
		}

        private ChessPieceType mPieceType;
		public ChessPieceType PieceType {
			get { return mPieceType; }
			set {
				if (value != mPieceType) {
					mPieceType = value;
					OnPropertyChanged(nameof(PieceType));
				}
			}
		} 

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
   }

   public class ChessViewModel : INotifyPropertyChanged, IGameViewModel {
         private const int MAX_AI_DEPTH = 4; 
         private ChessBoard mBoard;
         private ObservableCollection<ChessSquare> mSquares;
         private IGameAi mGameAi = new MinimaxAi(MAX_AI_DEPTH);

	      public event PropertyChangedEventHandler PropertyChanged;
	      public event EventHandler GameFinished;

          private void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	      }

          public ChessViewModel() {
             mBoard = new ChessBoard();
             mSquares = new ObservableCollection<ChessSquare>(
				    from pos in (
					    from r in Enumerable.Range(0, 8)
					    from c in Enumerable.Range(0, 8)
					    select new BoardPosition(r, c)
				    )
				    select new ChessSquare() {
					    Position = pos,
					    Player = mBoard.GetPieceAtPosition(pos).Player,
                        PieceType = mBoard.GetPieceAtPosition(pos).PieceType
				    }
			    );
                PossibleMoves = new HashSet<ChessMove>(
				       from ChessMove m in mBoard.GetPossibleMoves()
				       select m
                );
                PossibleStartPos = new HashSet<BoardPosition>(
                  from ChessMove m in mBoard.GetPossibleMoves()
                  select m.StartPosition
                );
          }

          public async Task ApplyMove(ChessMove move) {
                  var possMoves = mBoard.GetPossibleMoves() as IEnumerable<ChessMove>;
		            foreach (var m in possMoves) {
					       if (m.StartPosition.Equals(move.StartPosition) && m.EndPosition.Equals(move.EndPosition)) {
						       mBoard.ApplyMove(m);
                         //MessageBox.Show("Weight: " + mBoard.Weight.ToString());
						       break;
					       }
				      }

                  RebindState();

                  if (Players == NumberOfPlayers.One && CurrentPlayer == 2 && !mBoard.IsFinished) {
				         var bestMove = await Task.Run(() => mGameAi.FindBestMove(mBoard));
				         if (bestMove != null) {
					         mBoard.ApplyMove(bestMove);
                        RebindState();
				         }
			         }

            if (mBoard.IsCheckmate || mBoard.IsStalemate) {
                GameFinished?.Invoke(this, new EventArgs());
            }
        } 

        public void RebindState() {
             PossibleMoves = new HashSet<ChessMove>(
					      from ChessMove m in mBoard.GetPossibleMoves()
					      select m
				      );
                  PossibleStartPos = new HashSet<BoardPosition>(
                     from ChessMove m in mBoard.GetPossibleMoves()
                     select m.StartPosition
                  );
				    var newSquares =
					    from r in Enumerable.Range(0, 8)
					    from c in Enumerable.Range(0, 8)
					    select new BoardPosition(r, c);
				    int i = 0;
				    foreach (var pos in newSquares) {
					    mSquares[i].Player = mBoard.GetPieceAtPosition(pos).Player;
                   mSquares[i].PieceType = mBoard.GetPieceAtPosition(pos).PieceType;
                        
                        // King Check
                        mSquares[i].IsCheck = false;
                        if (mSquares[i].PieceType.Equals(ChessPieceType.King) && mSquares[i].Player.Equals(CurrentPlayer)) {
                            var kingPos = mBoard.GetPositionsOfPiece(ChessPieceType.King, CurrentPlayer);
                            if (kingPos.Count() == 0) {
                                mSquares[i].IsCheck = false;
                            } else { 
				                var king = kingPos.First();
				                var threatened = mBoard.GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);
				                mSquares[i].IsCheck = threatened.Contains(king) && PossibleMoves.Count() > 0;
                            }
                        }
					    i++;
				    }
			    OnPropertyChanged(nameof(BoardValue));
			    OnPropertyChanged(nameof(CurrentPlayer));
			    OnPropertyChanged(nameof(CanUndo));
        }

        public void UndoMove() {
            if (mBoard.MoveHistory.Count() > 0 && CurrentPlayer == 1) {
                ChessMove lastMove = mBoard.MoveHistory.Last() as ChessMove;
                if (lastMove.MoveType.Equals(ChessMoveType.PawnPromote)) {
                    mBoard.UndoLastMove();
                }
                mBoard.UndoLastMove();

                if (Players == NumberOfPlayers.One) {
				         mBoard.UndoLastMove();
			       }

                PossibleMoves = new HashSet<ChessMove>(
					    from ChessMove m in mBoard.GetPossibleMoves()
					    select m
				    );
                PossibleStartPos = new HashSet<BoardPosition>(
                        from ChessMove m in mBoard.GetPossibleMoves()
                        select m.StartPosition
                    );
			       var newSquares =
				       from r in Enumerable.Range(0, 8)
				       from c in Enumerable.Range(0, 8)
				       select new BoardPosition(r, c);
			       int i = 0;
			       foreach (var pos in newSquares) {
				        mSquares[i].Player = mBoard.GetPieceAtPosition(pos).Player;
                        mSquares[i].PieceType = mBoard.GetPieceAtPosition(pos).PieceType;

                        // King Check
                        mSquares[i].IsCheck = false;
                        if (mSquares[i].PieceType.Equals(ChessPieceType.King) && mSquares[i].Player.Equals(CurrentPlayer)) {
                            var kingPos = mBoard.GetPositionsOfPiece(ChessPieceType.King, CurrentPlayer);
                            if (kingPos.Count() == 0) {
                                mSquares[i].IsCheck = false;
                            } else {
				                var king = kingPos.First();
				                var threatened = mBoard.GetThreatenedPositions(CurrentPlayer == 1 ? 2 : 1);
				                mSquares[i].IsCheck = threatened.Contains(king) && PossibleMoves.Count() > 0;
                            }
                        }
				        i++;
			       }
                OnPropertyChanged(nameof(BoardValue));
                OnPropertyChanged(nameof(CurrentPlayer));
                OnPropertyChanged(nameof(CanUndo));
            }
        }

        public ObservableCollection<ChessSquare> Squares {
			   get { return mSquares; }
		  }

		  public HashSet<ChessMove> PossibleMoves {
		      get; private set;
		  }

        public HashSet<BoardPosition> PossibleStartPos {
            get; private set;
        }

        public IEnumerable<IGameMove> History { get { return mBoard.MoveHistory; } }
        public int BoardValue { get { return mBoard.Value; } }
	     public int CurrentPlayer { get { return mBoard.CurrentPlayer; } }
        public bool CanUndo { get { return mBoard.MoveHistory.Count > 0; } }

        public NumberOfPlayers Players { get; set; }
   }
}
