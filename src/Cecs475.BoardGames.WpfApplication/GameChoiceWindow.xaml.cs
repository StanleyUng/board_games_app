using Cecs475.BoardGames.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Cecs475.BoardGames.WpfApplication {
	/// <summary>
	/// Interaction logic for GameChoiceWindow.xaml
	/// </summary>
	public partial class GameChoiceWindow : Window {
		public GameChoiceWindow() {
			InitializeComponent();
			//a
			Type gameType = typeof(IGameType);
			//b
         
         var dllFiles = Directory.EnumerateFiles("lib", "*.dll");
         foreach (string s in dllFiles) {
            string name = s.Replace("lib\\", "").Trim();
            name = name.Replace(".dll", "").Trim();
               
            string loadDLL =  name + ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=68e71c13048d452a";
            Assembly.Load(loadDLL);
         }
    
         //Assembly.Load("Cecs475.BoardGames.Chess.View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=68e71c13048d452a");
         //Assembly.Load("Cecs475.BoardGames.Chess.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9e2b69c9b6db84f3");
         
			//c, d, e
			var boardTypes = AppDomain.CurrentDomain.GetAssemblies()
				 .SelectMany(t => t.GetTypes())
				 .Where(t => gameType.IsAssignableFrom(t) && t.IsClass);
			Type[] gameTypeList = Type.EmptyTypes;
			List<object> types = new List<object>();
			foreach (var g in boardTypes) {
				ConstructorInfo ctor = g.GetConstructor(gameTypeList);
				object instance = ctor.Invoke(new object[] { });
				types.Add(instance);		
			}
			this.Resources["GameTypes"] = types;
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			Button b = sender as Button;
			IGameType gameType = b.DataContext as IGameType;
			var gameWindow = new MainWindow(gameType, 
				mHumanBtn.IsChecked.Value ? NumberOfPlayers.Two : NumberOfPlayers.One) {
				Title = gameType.GameName
			};
			gameWindow.Closed += GameWindow_Closed;

			gameWindow.Show();
			this.Hide();
		}

		private void GameWindow_Closed(object sender, EventArgs e) {
			this.Show();
		}
	}
}
