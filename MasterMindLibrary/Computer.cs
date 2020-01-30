using System;
using System.Text;

namespace MasterMindLibrary
{
	/// <summary>
	/// Klasa komputera wykorzystywana do generowania (na różnych zasadzach) kodów.
	/// </summary>
	public class Computer
	{
		/// <summary>
		/// Zwraca losowy kod.
		/// </summary>
		/// <param name="letters">Ciąg znaków zawierający literki, z których składać się ma wylosowany kod.</param>
		/// <param name="length">Długość kodu</param>
		/// <returns>Losowy kod.</returns>
		public string GetRandomCode(string letters, int length)
		{
			Random r = new Random();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < length; i++)
				sb.Append(letters[r.Next(letters.Length)]);
			return sb.ToString();
		}

		// TODO: metoda BruteForce.
	}
}
