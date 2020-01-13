using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MasterMindLibrary;

namespace GameWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Game game;
		Dictionary<string, char> colors;
		ComboBox[] comboArray;
		Ellipse[] ellipseArray;
		Ellipse[] ellipseResultArray;

		public MainWindow()
		{
			InitializeComponent();

			game = new Game();
			
			// Dodaję kolory do słownika, ponieważ potrzebna będzie dla modelu pierwsza literka angielskiego odpowiednika nazwy koloru.
			colors = new Dictionary<string, char>()
			{
				{ "Czerwony", 'r' },
				{ "Żółty", 'y'},
				{ "Zielony", 'g' },
				{ "Niebieski", 'b' },
				{ "Cyjanowy", 'c' },
				{ "Magenta", 'm' },
				{ "Purpurowy", 'p' },
				{ "Pomarańczowy", 'o' }
			};

			comboArray = new ComboBox[] { c0, c1, c2, c3, c4, c5 };
			ellipseArray = new Ellipse[] { e0, e1, e2, e3, e4, e5 };
			ellipseResultArray = new Ellipse[] { eR0, eR1, eR2, eR3, eR4, eR5 };


			SetCheckBoxListener();
		}

		private void OnCheckButtonClick(object sender, RoutedEventArgs e)
		{
			if (game.GameState != Game.State.InProgress) return;
			
			string code = CreateCode();
			
			// Kod równy null oznacza, że nie wszystkie kolory zostały wybrane.
			if (code == null)
			{
				MessageBox.Show("Musisz wybrać wszystkie kolory!");
				return;
			}

			if (!game.CheckCodeLength(code))
			{
				MessageBox.Show($"Kod ma nieodpowiednią długość: {code.Length}");
				return;
			}

			int[] answer = game.CheckCode(code);

			// Sprawdza, czy literki kodu użytkownika znajdują się w kodzie na tej samej pozycji, na innej, czy może nie ma ich w ogóle.
			for (int i = 0; i < answer.Length; i++)
			{
				if (answer[i] == -1)
				{
					ellipseResultArray[i].Visibility = Visibility.Hidden;
				}
				else if (answer[i] == 0)
				{
					ellipseResultArray[i].Fill = new SolidColorBrush(Colors.White);
					ellipseResultArray[i].Visibility = Visibility.Visible;
				}
				else if (answer[i] == 1)
				{
					ellipseResultArray[i].Fill = new SolidColorBrush(Colors.Black);
					ellipseResultArray[i].Visibility = Visibility.Visible;
				}
				else
				{
					MessageBox.Show("Coś poszło nie tak! :(");
				}
			}

			lblMoves.Content = game.TotalMoves.ToString();

			if (game.GameState == Game.State.Finished)
			{
				MessageBox.Show($"Brawo! Udało Ci odgadnąć kod w {game.TotalMoves} ruchach!");
				GameStop();
			}

			if (game.TotalMoves == 9)
			{
				MessageBox.Show("To był Twój ostatni ruch! Nie udało Ci się odgadnąć! :(");
				GameStop();
			}
		}

		// Ustawia odpowiednio grę i rozpoczyna ją. Ustawia też odpowiednio właściwości elementów GUI.
		private void GameStart()
		{
			int amountOfColors = GetAmountOfColors();
			int codeLength = GetCodeLength();

			if (codeLength >= amountOfColors)
			{
				MessageBox.Show("Liczba kolorów musi być większa od długości kodów!");
				return;
			}

			game.SetLetters(amountOfColors);
			game.CodeLength = codeLength;

			// Wstawiam dane (kolory) do combo boksów.
			for (int i = 0; i < game.CodeLength; i++)
			{
				for (int k = 0; k < game.Letters.Length; k++)
				{
					string color = colors.ElementAt(k).Key;
					comboArray[i].Items.Add(color);
				}

				comboArray[i].Visibility = Visibility.Visible;
				ellipseArray[i].Visibility = Visibility.Visible;
			}

			lblMoves.Content = 0;

			stopBtn.IsEnabled = true;
			sliderCodeLen.IsEnabled = false;
			sliderColors.IsEnabled = false;
			startBtn.IsEnabled = false;
			checkBtn.IsEnabled = true;
			lblMoves.Visibility = Visibility.Visible;
			lblMovesTitle.Visibility = Visibility.Visible;

			for (int i = 0; i < comboArray.Length; i++)
				comboArray[i].IsEnabled = true;

			game.SetRandomCode();
			game.Start();
		}

		// Zatrzymuje grę i ustawia odpowiednio właściwości elementów GUI.
		private void GameStop()
		{
			game.Stop();
		
			sliderCodeLen.IsEnabled = true;
			sliderColors.IsEnabled = true;
			startBtn.IsEnabled = true;
			stopBtn.IsEnabled = false;
			checkBtn.IsEnabled = false;
			lblMoves.Visibility = Visibility.Hidden;
			lblMovesTitle.Visibility = Visibility.Hidden;

			for (int i = 0; i < comboArray.Length; i++)
			{
				ellipseArray[i].Visibility = Visibility.Hidden;
				ellipseResultArray[i].Visibility = Visibility.Hidden;
				comboArray[i].Visibility = Visibility.Hidden;
				comboArray[i].Items.Clear();
			}

			SetDefaultColors();
		}

		// Ustawia domyślne kolory wszystkim elipsom.
		private void SetDefaultColors()
		{
			for (int i = 0; i < ellipseArray.Length; i++)
				ellipseArray[i].Fill = new SolidColorBrush(Colors.LightGray);
		}

		// Zwraca kod utworzony na podstawie wybranych kolorów.
		private string CreateCode()
		{
			StringBuilder sb = new StringBuilder();

			try
			{
				for (int i = 0; i < game.CodeLength; i++)
					sb.Append(colors[comboArray[i].SelectedValue.ToString()]);

			}
			catch (NullReferenceException)
			{
				return null;
			}

			return sb.ToString();
		}

		// Zwraca długość kodu wybraną przez gracza.
		private int GetCodeLength() => int.Parse(sliderCodeLen.Value.ToString());

		// Zwraca ilość kolorów wybraną przez gracza.
		private int GetAmountOfColors() => int.Parse(sliderColors.Value.ToString());

		// Nadaje odpowiedni kolor elipsom.
		private void SetColor(Ellipse ellipse, string color)
		{
			switch (color)
			{
				case "Czerwony":
					ellipse.Fill = new SolidColorBrush(Colors.Red);
					break;
				case "Żółty":
					ellipse.Fill = new SolidColorBrush(Colors.Yellow);
					break;
				case "Zielony":
					ellipse.Fill = new SolidColorBrush(Colors.Green);
					break;
				case "Niebieski":
					ellipse.Fill = new SolidColorBrush(Colors.Blue);
					break;
				case "Cyjanowy":
					ellipse.Fill = new SolidColorBrush(Colors.Cyan);
					break;
				case "Magenta":
					ellipse.Fill = new SolidColorBrush(Colors.Magenta);
					break;
				case "Purpurowy":
					ellipse.Fill = new SolidColorBrush(Colors.Purple);
					break;
				case "Pomarańczowy":
					ellipse.Fill = new SolidColorBrush(Colors.Orange);
					break;
				default:
					MessageBox.Show("Coś poszło nie tak! :(");
					break;
			}
		}

		// Ustawia listenery dla wszystkich combo boksów.
		private void SetCheckBoxListener()
		{
			for (int i = 0; i < comboArray.Length; i++)
				comboArray[i].SelectionChanged += ChangeColorListener;
		}

		// Zmienia kolor elipsy odpowiadającej danemu combo boksowi.
		private void ChangeColorListener(object sender, SelectionChangedEventArgs e)
		{
			ComboBox cb = (ComboBox) e.Source;

			if (cb.SelectedValue == null)
				return;

			string color = cb.SelectedValue.ToString();
			int i = int.Parse(cb.Tag.ToString());

			SetColor(ellipseArray[i], color);
		}

		private void OnStartButtonClick(object sender, RoutedEventArgs e) => GameStart();

		private void OnStopButtonClick(object sender, RoutedEventArgs e) => GameStop();

		// Eventy dla sliderów. Zmiana powoduje ustawienie odpowiedniej wartości dla elementu label.
		private void OnSliderCodeLenValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (lblCodeLen != null)
				lblCodeLen.Content = sliderCodeLen.Value;
		}
		
		private void OnSliderColorsValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (lblCodeLen != null)
				lblColors.Content = sliderColors.Value;
		}
	}
}
