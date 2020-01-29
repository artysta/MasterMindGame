using System;
using System.Text;

namespace MasterMindLibrary
{
	// Główna klasa (model) gry.
	// Model można bardzo łatwo zmodyfikować, tak by pozwalał na generowanie jeszcze dłuższych i bardziej skomplikowanych kodów i dawał użytkownikowi więcej mozliwości.
	public class Game
	{
		/*
		 * Tablica literek (kolorów), z któych składać się może kod:
		 * - r - red,
		 * - y - yellow,
		 * - g - green,
		 * - b - blue
		 * - c - cyan,
		 * - m - magenta,
		 * - p - purple (dodatkowy),
		 * - o - orange (dodatkowy).
		 */
		public readonly static char[] POSSIBLY_LETTERS = { 'r', 'y', 'g', 'b', 'c', 'm', 'p', 'o' };
		// Minimalna liczba kolorów, z których składać się może kod.
		public readonly static int MIN_COLORS = 6;
		// Maksymalna liczba kolorów równa liczbie literek, z których składać się może kod.
		public readonly static int MAX_COLORS = POSSIBLY_LETTERS.Length;
		// Minimalna długość kodu do odgadnięcia.
		public readonly static int MIN_CODE_LENGTH = 4;
		// Maksymalna długość kodu do odgadnięcia.
		public readonly static int MAX_CODE_LENGTH = 8;
		// Maksymalna liczba ruchów.
		public readonly static int MAX_MOVES = 9;

		// Zwraca / ustawia kod do odgadnięcia.
		public string Code
		{
			private get;
			set;
		}

		// Zwraca / ustawia długość kodu.
		public int CodeLength
		{
			get;
			set;
		} = 4;

		// Zwraca / ustawia literki brane pod uwagę przy losowaniu kodu.
		public string Letters
		{
			get;
			private set;
		} = "rygbcm";

		// Zwraca / ustawia liczbę ruchów.
		public int TotalMoves
		{
			get;
			private set;
		}

		// Zwraca / ustawia stan gry.
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

		// Kończy grę z niepowodzeniem - gracz poddał się lub nie udało mu się odgadnąć kodu w wyznaczonej liczbie ruchów.
		public void Stop()
		{
			GameState = State.Over;
		}

		// Kończy grę - użytkownik pomyślnie odgadnął kod.
		private void Finish()
		{
			GameState = State.Finished;
		}

		// Ustawia literki (na podstawie podanej długości), które będą brane pod uwagę podczas losowania kodu.
		public void SetLetters(int length)
		{
			char[] letters = new char[length];

			for (int i = 0; i < letters.Length; i++)
			{
				letters[i] = POSSIBLY_LETTERS[i];
			}

			Letters = new string(letters);
		}

		// Sprawdza, czy długość podanego przez gracza kodu jest prawidłowa.
		public bool CheckCodeLength(string code)
		{
			return code.Length == CodeLength;
		}

		/* Sprawdza kod i zwraca tablicę zawierającą ilość liczb równą ustalonej długości kodu.
		 * Dla każdej literki (każdego indeksu) kodu zwraca:
		 * - -1 - jeżeli literka na danej pozycji (danym indeksie) w ogóle nie znajduje się w podanym przez użytkownika kodzie,
		 * -  0 - jeżeli literka znajduje się w podanym przez użytkownika kodzie, ale na innym miejscu,
		 * -  1 - jeżeli literka znajduje się w podanym przez użytkownika kodzie na dokładnie tym samym miejscu.
		 */
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

		// Ustawia losowy kod do odgadnięcia.
		public void SetRandomCode()
		{
			Random r = new Random();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < CodeLength; i++)
				sb.Append(Letters[r.Next(Letters.Length)]);
			Code = sb.ToString();
		}

		/*
		 * Ustawia kod podany przez gracza, który będzie mógł zostać odgadnięty przez innego gracza / komputer.
		 * Zwraca:
		 *  - false, gdy długość kodu podanego przez gracza jest inna od tego, który obsługuje w konkretnej sytuacji gra.
		 *  - false również w sytuacji, gdy w podanym przez użytkownia kodzie znajdują się literki nieobsługiwane przez grę.
		 *  - zwraca true, gdy kod spełnia wszystkie wymagania.
		 *  Taka weryfikacja kodu pozwala uniknąć m.in. zapętlenia się gry.
		 */
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

		// Pomocniczy enum pozwalający na określenie stanu gry.
		public enum State
		{
			InProgress, Over, Finished
		}
	}
}
 