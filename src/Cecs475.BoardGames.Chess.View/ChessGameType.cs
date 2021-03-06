﻿using System;
using System.Windows.Controls;
using Cecs475.BoardGames.View;
using System.Windows.Data;

namespace Cecs475.BoardGames.Chess.View {
	public class ChessGameType : IGameType {
		public string GameName {
			get {
				return "Chess";
			}
		}

		public IValueConverter CreateBoardValueConverter() {
			return new ChessValueConverter();
		}

		public IValueConverter CreateCurrentPlayerConverter() {
			return new ChessCurrentPlayerConverter();
		}

		public Tuple<Control, IGameViewModel> CreateViewAndViewModel(NumberOfPlayers players) {
			var view = new ChessView();
			var model = view.Model;
         model.Players = players;
			return new Tuple<Control, IGameViewModel>(view, model);
		}
	}
}