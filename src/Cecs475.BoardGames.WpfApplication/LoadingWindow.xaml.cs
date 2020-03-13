using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;

namespace Cecs475.BoardGames.WpfApplication {
   /// <summary>
   /// Interaction logic for LoadingWindow.xaml
   /// </summary>
   public partial class LoadingWindow : Window {

      public LoadingWindow() {
         this.Loaded += LoadGames;
         InitializeComponent();
      }

      public class File {
          public string FileName { get; set; }
          public Uri Url { get; set; }
          public string PublicKey { get; set; }
          public string Version { get; set; }
      }

      public class GameDLL {
          public string Name { get; set; }
          public File[] Files { get; set; }
      }

      private async void LoadGames(object sender, RoutedEventArgs e) {

         // Loading in the game from this site
         var client = new RestClient("http://cecs475-boardgames.azurewebsites.net/");
         var request = new RestRequest("api/games", Method.GET);
         var response = await client.ExecuteTaskAsync(request);
         var gameList = JsonConvert.DeserializeObject<List<GameDLL>>(response.Content);
			if (response.StatusCode == System.Net.HttpStatusCode.NotFound) {
				MessageBox.Show("Game not found");
			} else {
            WebClient wc = new WebClient();
            foreach(var g in gameList) {
               foreach(var file in g.Files) {
                  await wc.DownloadFileTaskAsync(file.Url, "lib/" + file.FileName);
               }
            }
			}
         
         // Create a new GameChoiceWindow
         GameChoiceWindow gw = new GameChoiceWindow();
         gw.Show();
         this.Close();
      }
   }
}