using System;
using System.Text;

namespace MasterMindLibrary
{
	/// <summary>
	/// Główna klasa (model) gry, który można bardzo łatwo zmodyfikować, tak by pozwalał na generowanie jeszcze dłuższych i bardziej skomplikowanych kodów i dawał użytkownikowi więcej mozliwości.
	/// </summary>
	public class Game
	{
		/// <summary>
		/// Tablica literek (kolorów), z któych składać się może kod:
		/// - r - red,
		/// - y - yellow,
		/// - g - green,
		/// - b - blue,
		/// - c - cyan,
		/// - m - magenta,
		/// - p - purple,
		/// - o - orange
		/// </summary>
		public readonly static char[] POSSIBLY_LETTERS = { 'r', 'y', 'g', 'b', 'c', 'm', 'p', 'o' };

		/// <summary>
		/// Minimalna liczba kolorów, z których składać się może kod.
		/// </summary>
		public readonly static int MIN_COLORS = 6;

		/// <summary>
		/// Maksymalna liczba kolorów równa liczbie literek, z których składać się może kod.
		/// </summary>
		public readonly static int MAX_COLORS = POSSIBLY_LETTERS.Length;
		/// <summary>
		/// Minimalna długość kodu.
		/// </summary>
		public readonly static int MIN_CODE_LENGTH = 4;
		/// <summary>
		/// Maksymalna długość kodu.
		/// </summary>
		public readonly static int MAX_CODE_LENGTH = 8;
		/// <summary>
		/// Maksymalna liczba ruchów.
		/// </summary>
		public readonly static int MAX_MOVES = 9;

		/// <summary>
		/// Property, które pozwala na zwrócenie lub ustawienie kodu.
		/// </summary>
		public string Code
		{
			private get;
			set;
		}

		/// <summary>
		/// Property, które pozwala na zwrócenie lub ustawienie długości kodu. Domyślnie długość kodu = 4.
		/// </summary>
		public int CodeLength
		{
			get;
			set;
		} = 4;

		/// <summary>
		/// Property, które pozwala na zwrócenie lub ustawienie literek, które będą brane pod uwagę przy losowaniu kodu. Domyślne literki to r, y, g, b, c, m.
		/// </summary>
		public string Letters
		{
			get;
			private set;
		} = "rygbcm";

		/// <summary>
		/// Property, które pozwala na zwrócenie lub ustawienie liczby wykonanych ruchów.
		/// </summary>
		public int TotalMoves
		{
			get;
			private set;
		}

		/// <summary>
		/// Property, które pozwala na zwrócenie lub ustawienie stanu gry.
		/// </summary>
		public State GameState
		{
			get;
			private set;
		}

		// Rozpoczyna grę.
		public void Start()
		{
			TotalMoves = 0;
			GameState = State.InProgress;
		}

		/// <summary>
		/// Kończy grę z niepowodzeniem - gracz poddał się lub nie udało mu się odgadnąć kodu w wyznaczonej liczbie ruchów.
		/// </summary>
		public void Stop()
		{
			GameState = State.Over;
		}

		/// <summary>
		/// Kończy grę - użytkownik pomyślnie odgadnął kod.
		/// </summary>
		private void Finish()
		{
			GameState = State.Finished;
		}

		/// <summary>
		/// Ustawia literki (na podstawie podanej długości), które będą brane pod uwagę podczas losowania kodu.
		/// </summary>
		/// <param name="length">Długość kodu.</param>
		public void SetLetters(int length)
		{
			char[] letters = new char[length];

			for (int i = 0; i < letters.Length; i++)
			{
				letters[i] = POSSIBLY_LETTERS[i];
			}

			Letters = new string(letters);
		}

		/// <summary>
		/// Sprawdza, czy długość podanego przez gracza kodu jest prawidłowa.
		/// </summary>
		/// <param name="code">Kod.</param>
		/// <returns>
		/// - true, jeśli długość kodu jest odpowiednia,
		/// - false jeśli długość kodu jest nieodpowiednia.</returns>
		public bool CheckCodeLength(string code)
		{
			return code.Length == CodeLength;
		}

		/// <summary>
		/// Sprawdza kod i zwraca tablicę zawierającą ilość liczb równą ustalonej długości kodu.
		/// </summary>
		/// <param name="code">Kod.</param>
		/// <returns>
		/// Dla każdej literki (każdego indeksu) kodu zwraca:
		/// - -1 - jeżeli literka na danej pozycji (danym indeksie) w ogóle nie znajduje się w podanym przez użytkownika kodzie,
		/// - 0 - jeżeli literka znajduje się w podanym przez użytkownika kodzie, ale na innym miejscu,
		/// - 1 - jeżeli literka znajduje się w podanym przez użytkownika kodzie na dokładnie tym samym miejscu.
		/// </returns>
		public int[] CheckCode(string code)
		{
			int[] output = new int[CodeLength];
			for (int i = 0; i < code.Length; i++)
			{
				if (Code[i] == code[i])
					output[i] = 1;
				else if (Code.Contains(code[i].ToString()))
					output[i] = 0;
				else
					output[i] = -1;
			}
			
			TotalMoves++;

			if (code.Equals(Code)) Finish();

			return output;
		}

		/// <summary>
		/// Ustawia losowy kod do odgadnięcia.
		/// </summary>
		public void SetRandomCode()
		{
			Random r = new Random();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < CodeLength; i++)
				sb.Append(Letters[r.Next(Letters.Length)]);
			Code = sb.ToString();
		}

		/// <summary>
		/// Ustawia kod podany przez gracza, który będzie mógł zostać odgadnięty przez innego gracza / komputer.
		/// </summary>
		/// <param name="code">Kod</param>
		/// <returns>
		/// - false, gdy długość kodu podanego przez gracza jest inna od tego, który obsługuje w konkretnej sytuacji gra lub gdy w podanym przez użytkownia kodzie znajdują się literki nieobsługiwane przez grę,
		/// - zwraca true, gdy kod spełnia wszystkie wymagania.
		/// </returns>
		public bool SetUserCode(string code)
		{
			if (code.Length != CodeLength) return false;
			
			// Jeżli którakolwiek z literek kodu zawiera nieobsługiwaną literkę, zwraca false.
			for (int i = 0; i < code.Length; i++)
				if (!Letters.Contains(code[i].ToString()))
					return false;
			
			// Przypisuje kod do zmiennej, gdy wszystko jest okej.
			Code = code;
			return true;
		}

		/// <summary>
		/// Pomocniczy enum pozwalający na określenie stanu gry.
		/// </summary>
		public enum State
		{
			InProgress, Over, Finished
		}
	}
}
 