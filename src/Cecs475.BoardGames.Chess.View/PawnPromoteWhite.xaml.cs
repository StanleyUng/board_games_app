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
    /// Interaction logic for PawnPromoteWhite.xaml
    /// </summary>
    public partial class PawnPromoteWhite : Window {
        public PawnPromoteWhite() {
            InitializeComponent();
        }

        // Public property to access the promotion piece
        public ChessPieceType selectedPiece { get; set; }
        public int mPlayer;

        private void promoteChoice(object sender, RoutedEventArgs e) {
            if (sender == WhiteQueenPromote) {
                selectedPiece = ChessPieceType.Queen;
            } else if (sender == WhiteRookPromote) {
                selectedPiece = ChessPieceType.RookPawn;
            } else if (sender == WhiteBishopPromote) {
                selectedPiece = ChessPieceType.Bishop;
            } else if (sender == WhiteKnightPromote) {
                selectedPiece = ChessPieceType.Knight;
            }
            Close();
        }
    }

    public class PawnPromotionConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            ChessPieceType type = (ChessPieceType)values[0];
            int player = (int)values[1];

            String color = player == 1 ? "White" : "Black";
            try {
                if (type == ChessPieceType.Queen) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Queen.png", UriKind.Relative));
                } else if (type == ChessPieceType.Knight) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Knight.png", UriKind.Relative));
                } else if (type == ChessPieceType.Bishop) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Bishop.png", UriKind.Relative));
                } else if (type == ChessPieceType.RookKing || type == ChessPieceType.RookQueen || type == ChessPieceType.RookPawn) {
                    return new BitmapImage(new Uri("/Cecs475.BoardGames.Chess.View;component/Resources/" + color + "Rook.png", UriKind.Relative));
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
