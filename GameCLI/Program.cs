using System;
using MasterMindLibrary;

namespace GameCLI
{
	// Pseudo kontroler aplikacji.
	class Program
	{
		static Game game = new Game();

		static void Main(string[] args)
		{
			game.Start();
			Console.WriteLine($"Witaj w grze Master Mind!\n\n" +
				$"Domyślnie w grze kod generowany jest z 6 literek i ma długość 4, czy chcesz zmienić te ustawienia? t/n");

			string change;

			// Pętla wykonuje się do momentu aż gracz wpisze 't' lub 'n';
			while (true)
			{
				change = Console.ReadLine();
				if (change.ToLower().Equals("t") || change.ToLower().Equals("n")) break;
				Console.WriteLine("Musisz wpisać literkę t (tak) lub n (nie)!");
			}

			if (change.ToLower().Equals("t"))
			{
				// Pętla wykonuje się do momentu, aż gracz poda prawidłowe ustawienia dotyczące gry.
				while (true)
				{
					try
					{
						Console.WriteLine("Podaj liczbę kolorów (liczba od 6 do 8):");
						int colors = int.Parse(Console.ReadLine());

						if (colors < Game.MIN_COLORS || colors > Game.MAX_COLORS)
						{
							WriteInColor($"Liczba kolorów nie może być mniejsza od {Game.MIN_COLORS} i większa od {Game.MAX_COLORS}!\n", ConsoleColor.Red);
							continue;
						}

						Console.WriteLine("Podaj długość kodu, który zostanie wylosowany (liczba od 4 do 6 - musi być ona jednak mniejsza od liczby kolorów):");
						int length = int.Parse(Console.ReadLine());

						if (length < Game.MIN_CODE_LENGTH || length > Game.MAX_CODE_LENGTH)
						{
							WriteInColor($"Długość kodu nie może być mniejsza od {Game.MIN_CODE_LENGTH} i większa od {Game.MAX_CODE_LENGTH}!\n", ConsoleColor.Red);
							continue;
						}

						if (length >= colors)
						{
							WriteInColor($"Długość kodu nie może być większa od liczby liter!\n", ConsoleColor.Red);
							continue;
						}

						game.SetLetters(colors);
						game.CodeLength = length;

						Console.WriteLine("Poprawnie ustawiono dane gry!");
						break;
					}
					catch (OverflowException)
					{
						WriteInColor("Podałeś ZDECYDOWANIE za dużą lub za małą liczbę!\n", ConsoleColor.Red);
						continue;
					}
					catch (FormatException)
					{
						WriteInColor("Musisz koniecznie podać liczbę!\n", ConsoleColor.Red);
						continue;
					}
					catch (Exception)
					{
						WriteInColor("!\n", ConsoleColor.Red);
						continue;
					}
				}
			}

			Console.WriteLine("Napisz, kto ma zgadywać kod, Ty (wpisz 1), czy komputer (wpisz 2)?");

			// Pętla wykonuje się dopóki gracz nie wpisze '1' lub '2'.
			while (true)
			{
				try
				{
					int answer = int.Parse(Console.ReadLine());
					Console.WriteLine();
					if (answer == 1)
						PlayerMode();
					if (answer == 2)
						ComputerMode();
				}
				catch (OverflowException)
				{
					WriteInColor("Podałeś ZDECYDOWANIE za dużą lub za małą liczbę!\n", ConsoleColor.Red);
					continue;
				}
				catch (FormatException)
				{
					WriteInColor("Musisz koniecznie podać liczbę!\n", ConsoleColor.Red);
					continue;
				}
				catch (Exception)
				{
					WriteInColor("!\n", ConsoleColor.Red);
					continue;
				}
			}
		}

		// Tryb, w którym zgaduje gracz.
		public static void PlayerMode()
		{
			Console.WriteLine($"Teraz zostanie dla Ciebie wygenerowany kod składający się z dokładnie {game.CodeLength} literek (kolorów).\n" +
			$"Podczas losowania kodu, brane będą pod uwagę wyłącznie literki \"{game.Letters}\".\n");

			game.SetRandomCode();

			Console.WriteLine($"Ok, wylosowano dla Ciebie kod, spróbuj go odgadnąć! Masz {Game.MAX_MOVES} ruchów!");

			// Pętla wykonuje się dopóki gra trwa.
			while (game.GameState == Game.State.InProgress)
			{
				string code = Console.ReadLine();

				if (!game.CheckCodeLength(code))
				{
					Console.WriteLine($"Kod musi składać się z dokładnie {game.CodeLength} literek!\n" +
						$"Spróbuj jeszcze raz!\n");
					continue;
				}

				int[] answer = game.CheckCode(code);

				Console.WriteLine();

				// Sprawdza, czy literki kodu użytkownika znajdują się w kodzie na tej samej pozycji, na innej, czy może nie ma ich w ogóle.
				for (int i = 0; i < answer.Length; i++)
				{
					if (answer[i] == -1)
					{
						WriteInColor("ŹLE", ConsoleColor.Red);
						Console.Write($" - literki {code[i]} nie ma w kodzie w ogóle.\n");
					}
					else if (answer[i] == 0)
					{
						WriteInColor("PRAWIE", ConsoleColor.Yellow);
						Console.WriteLine($" - literka {code[i]} znajduje się w kodzie, ale na innym miejscu.");
					}
					else if (answer[i] == 1)
					{
						WriteInColor("DOBRZE", ConsoleColor.Green);
						Console.WriteLine($" - literka {code[i]} znajduje się w kodzie na tej samej pozycji.");
					}
					else
					{
						Console.WriteLine("Coś poszło nie tak! :(");
					}
				}

				Console.WriteLine($"To był Twój {game.TotalMoves} ruch.\n");

				// Zatrzymuje grę (użytkownikowi nie udało się odgadnąć kodu w wyznaczonej liczbie ruchów).
				if (game.TotalMoves == Game.MAX_MOVES)
				{
					game.Stop();
					Console.WriteLine($"Niestety nie udało Ci się odgadnąć w określonej liczbie ({Game.MAX_MOVES}) ruchów! :(");
					Environment.Exit(0);
				}
			}

			Console.WriteLine($"Brawo, udało Ci się zgadnąć! Liczba poprawnych ruchów to {game.TotalMoves}.");
			Environment.Exit(0);
		}

		// Tryb, w którym zgaduje komputer.
		public static void ComputerMode()
		{
			Computer computer = new Computer();
			Console.WriteLine($"Twoim zadaniem będzie teraz podanie kodu, który będzie musiał odgadnąć \"komputer\".\n" +
				$"Pamiętaj jednak, że kod musi składać się wyłącznie z literek {game.Letters} i mieć długość równą dokładnie {game.CodeLength}.");

			string code = "";

			// Pętla wykonuje się do momentu, gdy użytkownik poda prawidłowy kod.
			while (true)
			{
				code = Console.ReadLine();
				if (!game.SetUserCode(code))
				{
					WriteInColor($"Kod musi składać się wyłącznie z literek {game.Letters} i mieć długość równą dokładnie {game.CodeLength}!\n", ConsoleColor.Red);
					continue;
				}
				break;
			}

			while (game.GameState == Game.State.InProgress)
			{
				int[] answer = game.CheckCode(computer.GetRandomCode(game.Letters, game.CodeLength));

				Console.WriteLine();

				// Sprawdza, czy literki kodu użytkownika znajdują się w kodzie na tej samej pozycji, na innej, czy może nie ma ich w ogóle.
				for (int i = 0; i < answer.Length; i++)
				{
					if (answer[i] == -1)
					{
						WriteInColor("ŹLE", ConsoleColor.Red);
						Console.Write($" - literki {code[i]} nie ma w kodzie w ogóle.\n");
					}
					else if (answer[i] == 0)
					{
						WriteInColor("PRAWIE", ConsoleColor.Yellow);
						Console.WriteLine($" - literka {code[i]} znajduje się w kodzie, ale na innym miejscu.");
					}
					else if (answer[i] == 1)
					{
						WriteInColor("DOBRZE", ConsoleColor.Green);
						Console.WriteLine($" - literka {code[i]} znajduje się w kodzie na tej samej pozycji.");
					}
					else
					{
						Console.WriteLine("Coś poszło nie tak! :(");
					}
				}

				Console.WriteLine($"To był {game.TotalMoves} ruch komputera.\n");
			}

			Console.WriteLine($"Komputer odgadł ustalony przez Ciebie dopiero po wykonaniu {game.TotalMoves} ruchów!");
			Environment.Exit(0);
		}

		// Wypisuje w konsoli tekst w konkretnym kolorze.
		public static void WriteInColor(string message, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(message);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
