using System;
using System.Text;

namespace MasterMindLibrary
{
	// Klasa komputera wykorzystywana do generowania (na różnych zasadzach) kodów.
	public class Computer
	{
		// Zwraca losowy kod.
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
