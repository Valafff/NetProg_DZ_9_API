using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetProg_DZ_9_API
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Root root = new Root();
		HttpClient HttpClient = new HttpClient();

		double Longitude;
		double Latitude;


		public MainWindow()
		{
			InitializeComponent();
			bt_5days.IsEnabled = false;
		}

		private async void Button_WeatherNow_Click(object sender, RoutedEventArgs e)
		{
			string uri = $"http://api.openweathermap.org/data/2.5/weather?q=Yaroslavl,{Region.Text}&APPID={tb_API.Text}&units=metric";
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					root = JsonSerializer.Deserialize<Root>(json);

					tb_Avarage.Text = $"Средняя температура {root.main.temp} град. Цельсия";
					tb_FeelLike.Text = $"Ощущается как {root.main.feels_like} град. Цельсия";
					tb_MaxTem.Text = $"Максимальная температура {root.main.temp_max} град. Цельсия";
					tb_MinTemp.Text = $"Минимальная температура {root.main.temp_min} град. Цельсия";

					Longitude = root.coord.lon; 
					Latitude = root.coord.lat;
					bt_5days.IsEnabled = true;
				}
				else
				{
					MessageBox.Show($"Ошибка при получении данных: {response.StatusCode}");
				}
			}
		}

		private async void Button_5Days_Click(object sender, RoutedEventArgs e)
		{
			string uri = $"http://api.openweathermap.org/data/2.5/forecast?lat={Latitude}&lon={Longitude}&APPID={tb_API.Text}&units=metric";
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync (uri);
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();

					NetProg_DZ_9_API_five.Root root = new NetProg_DZ_9_API_five.Root();
					root = JsonSerializer.Deserialize<NetProg_DZ_9_API_five.Root>(json);

					for (int i = 0; i < root.list.Count; i++)
					{		
						TextBlock newtextblock = new TextBlock() {Text = root.list[i].dt_txt + " Температура " + root.list[i].main.temp + " град. Цельсия"};
						Grid.SetRow(newtextblock, 1);
						myStack.Children.Add(newtextblock);
					}					
				}
				else
				{
					MessageBox.Show($"Ошибка при получении данных: {response.StatusCode}");
				}
			}

		}

		private void Button_Click_Exit(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}