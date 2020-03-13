using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cecs475.BoardGames.Chess.View {
    /// <summary>
    /// Interaction logic for ChessView.xaml
    /// </summary>
    public partial class ChessView : UserControl {
        public static SolidColorBrush RED_BRUSH = new SolidColorBrush(Colors.Red);
        public static SolidColorBrush GREEN_BRUSH = new SolidColorBrush(Colors.Green);
        private bool pieceSelected;
        private ChessSquare squareStart;

        public ChessView() {
            InitializeComponent();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e) {
        if (!IsEnabled) { 
            return;
        }
            Border b = sender as Border;
            var square = b.DataContext as ChessSquare;
            var vm = FindResource("vm") as ChessViewModel;
            
            if (pieceSelected) {
                HashSet<BoardPosition> PossibleEndPos = new HashSet<BoardPosition>(
                    from ChessMove m in vm.PossibleMoves
                    where squareStart.Position.Equals(m.StartPosition)
                    select m.EndPosition
                );
                if (PossibleEndPos.Contains(square.Position)) {
                    square.IsHovered = true;
                }
            } else {
                if (vm.PossibleStartPos.Contains(square.Position) && !pieceSelected) {
                    square.IsHovered = true;
                }
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e) {
           if (!IsEnabled) { 
               return;
           }
            Border b = sender as Border;
            var square = b.DataContext as ChessSquare;
            square.IsHovered = false;
        }

        private async void Border_MouseUp(object sender, MouseButtonEventArgs e) {
         if (!IsEnabled) { 
            return;
         }
			Border b = sender as Border;
			var square = b.DataContext as ChessSquare;
			var vm = FindResource("vm") as ChessViewModel;

            if (pieceSelected) {
                HashSet<BoardPosition> PossibleEndPos = new HashSet<BoardPosition>(
                    from ChessMove m in vm.PossibleMoves
                    where squareStart.Position.Equals(m.StartPosition)
                    select m.EndPosition
                );
                if (PossibleEndPos.Contains(square.Position)) {
                    foreach(ChessMove m in vm.PossibleMoves) {
                        if (square.Position.Equals(m.EndPosition)) {
                            IsEnabled = false;
                            await vm.ApplyMove(new ChessMove(squareStart.Position, square.Position));
                        }
                    }
                }

                squareStart.IsSelected = false;
                square.IsHovered = false;
                pieceSelected = false;

                if (vm.History.Count() > 0) {
                    ChessMove lastMove = vm.History.Last() as ChessMove;
                    if (lastMove.Piece.PieceType.Equals(ChessPieceType.Pawn)) {
                        if (vm.CurrentPlayer == 1 && lastMove.EndPosition.Row.Equals(0)) {
                            PawnPromoteWhite window = new PawnPromoteWhite();
                            window.ShowDialog();
                            ChessPieceType promotion = window.selectedPiece;
                            await vm.ApplyMove(new ChessMove(lastMove.EndPosition, new BoardPosition(-1, (int)promotion)));
                        } else if (vm.CurrentPlayer == 2 && lastMove.EndPosition.Row.Equals(7)) {
                            PawnPromoteBlack window = new PawnPromoteBlack();
                            window.ShowDialog();
                            ChessPieceType promotion = window.selectedPiece;
                            await vm.ApplyMove(new ChessMove(lastMove.EndPosition, new BoardPosition(-1, (int)promotion)));
                        }
                    }
                }
                IsEnabled = true;
            }

            if (vm.PossibleStartPos.Contains(square.Position)) {
                pieceSelected = true;
                square.IsSelected = true;
                squareStart = square;
			}
		}
        
        public ChessViewModel Model {
            get { return FindResource("vm") as ChessViewModel; }
        }
    }

    public class ChessSquarePlayerConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            //BoardPosition pos = (BoardPosition)values[0];
            ChessPieceType type = (ChessPieceType)values[0];
            int player = (int)values[1];

            String color = player == 1 ? "White" : "Black";
            try {
                if (type == ChessPieceType.Pawn) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Pawn.png", UriKind.Relative));
                } else if (type == ChessPieceType.Knight) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Knight.png", UriKind.Relative));
                } else if (type == ChessPieceType.Bishop) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Bishop.png", UriKind.Relative));
                } else if (type == ChessPieceType.RookKing || type == ChessPieceType.RookQueen || type == ChessPieceType.RookPawn) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Rook.png", UriKind.Relative));
                } else if (type == ChessPieceType.Queen) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Queen.png", UriKind.Relative));
                } else if (type == ChessPieceType.King) {
                    //return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "King.png", UriKind.Relative));
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + (player == 1 ? "NealDT" : "Kelvin") + ".png", UriKind.Relative));
                } else {
                    return null;
                }
            } catch (Exception e) {
				return null;
            }
        }
        
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
