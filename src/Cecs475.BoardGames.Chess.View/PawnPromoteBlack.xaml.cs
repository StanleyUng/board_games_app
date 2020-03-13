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
    /// Interaction logic for PawnPromoteBlack.xaml
    /// </summary>
    public partial class PawnPromoteBlack : Window {
        public PawnPromoteBlack() {
            InitializeComponent();
        }

        // Public property to access the promotion piece
        public ChessPieceType selectedPiece { get; set; }
        public int mPlayer;

        private void promoteChoice(object sender, RoutedEventArgs e) {
            if (sender == BlackQueenPromote) {
                selectedPiece = ChessPieceType.Queen;
            } else if (sender == BlackRookPromote) {
                selectedPiece = ChessPieceType.RookPawn;
            } else if (sender == BlackBishopPromote) {
                selectedPiece = ChessPieceType.Bishop;
            } else if (sender == BlackKnightPromote) {
                selectedPiece = ChessPieceType.Knight;
            }
            Close();
        }
    }
}
