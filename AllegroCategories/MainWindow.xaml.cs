using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AllegroCategories
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string AuthUrl = "https://allegro.pl/auth/oauth/token";
        private string CategoriesUrl = "https://api.allegro.pl/sale/categories";
        private string Token;
        private List<Category> Categories = new List<Category>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GenerateTxt_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Token)) await getToken();
            await getCategories();
            await writeToFile();
            MessageBox.Show("File created!");
        }

        private async Task getToken()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(values);

            var authenticationString = $"{ClientID.Text}:{Secret.Text}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, AuthUrl);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;
            
            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            AuthResponse authResponse = JsonSerializer.Deserialize<AuthResponse>(responseBody);

            Token = authResponse.AccessToken;
        }

        private async Task getCategories(string categoryID = null, string parentName = null)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/vnd.allegro.public.v1+json"));

                var url = CategoriesUrl;
                if (!string.IsNullOrEmpty(categoryID))
                {
                    url += $"?parent.id={categoryID}";
                }

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var categoriesResponse = JsonSerializer.Deserialize<CategoriesResponse>(responseBody);
                foreach (var category in categoriesResponse.Categories)
                {
                    category.Name = $"{parentName} \\ {category.Name}";
                    Categories.Add(category);
                    await getCategories(category.Id, category.Name);
                }

            }
            catch (Exception e)
            {
            }
        }

        private async Task writeToFile()
        {
            await File.WriteAllLinesAsync("Categories.txt", Categories.Select(x => $"{x.Id} | {x.Name}").ToArray());
        }
    }
}
