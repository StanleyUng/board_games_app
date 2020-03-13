using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Cecs475.BoardGames.Chess.View {
    public class ChessSquareColorConverter : IMultiValueConverter {
        private static SolidColorBrush YELLOW_BRUSH = new SolidColorBrush(Colors.Yellow);
        private static SolidColorBrush RED_BRUSH = new SolidColorBrush(Colors.Red);
        private static SolidColorBrush CHECK_COLOR = new SolidColorBrush(Colors.Lime);
        private static SolidColorBrush PINK_BRUSH = new SolidColorBrush(Colors.Magenta);
		private static SolidColorBrush BLUE_BRUSH = new SolidColorBrush(Colors.SkyBlue);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            BoardPosition pos = (BoardPosition)values[0];
			   bool isHovered = (bool)values[1];
            bool isSelected = (bool)values[2];
            bool isCheck = (bool)values[3];
            
            // Hovered squares have a specific color.
			   if (isHovered) {
				   return YELLOW_BRUSH;
			   }

            if (isSelected) {
                return RED_BRUSH;
            }

            if (isCheck) {
                return CHECK_COLOR;
            }

            // Add the position row and col to determine if it is light or dark.
            if ((pos.Row + pos.Col) % 2 == 0) {
                return PINK_BRUSH;
            } else {
                return BLUE_BRUSH;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
