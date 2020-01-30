using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Graphics;
using System;
using MasterMindLibrary;
using System.Text;

namespace GameXamarinAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Color[] colors = { Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Cyan, Color.Magenta};
        private int[] colorNr = { -1, -1, -1, -1 };
		private Game game = new Game();
		private List<Button> btns;
		private List<Button> btnsResult;
		private Button btnCheck;
		private Button btnStart;
		private Button btnStop;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            game.SetLetters(6);
            game.CodeLength = 4;

			Button btn1 = FindViewById<Button>(Resource.Id.btn_0);
			Button btn2 = FindViewById<Button>(Resource.Id.btn_1);
			Button btn3 = FindViewById<Button>(Resource.Id.btn_2);
			Button btn4 = FindViewById<Button>(Resource.Id.btn_3);

			Button btnA1 = FindViewById<Button>(Resource.Id.btn_a_0);
			Button btnA2 = FindViewById<Button>(Resource.Id.btn_a_1);
			Button btnA3 = FindViewById<Button>(Resource.Id.btn_a_2);
			Button btnA4 = FindViewById<Button>(Resource.Id.btn_a_3);

			btnCheck = (Button)FindViewById(Resource.Id.btn_check);
			btnStart = (Button)FindViewById(Resource.Id.btn_start);
			btnStop = (Button)FindViewById(Resource.Id.btn_stop);

			btnCheck.Click += OnCheckButtonClicked;
			btnStart.Click += OnStartButtonClicked;
			btnStop.Click += OnStopButtonClicked;

			btnCheck.Enabled = false;
			btnStop.Enabled = false;

			btns = new List<Button> { btn1, btn2, btn3, btn4 };
			btnsResult = new List<Button> { btnA1, btnA2, btnA3, btnA4 };

			for (int i = 0; i < btns.Count; i++)
				btns[i].Tag = i;

            for (int i = 0; i < btns.Count; i++)
				btns[i].Click += ButtonClicked;
        }

        public void ButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            int tag = int.Parse(button.Tag.ToString());
            colorNr[tag]++;
            if (colorNr[tag] > colors.Length - 1)
                colorNr[tag] = 0;
            button.SetBackgroundColor(colors[colorNr[tag]]);
        }

		public void OnStartButtonClicked(object sender, EventArgs args) => GameStart();

		public void OnStopButtonClicked(object sender, EventArgs args) => GameStop();

		public void OnCheckButtonClicked(object sender, EventArgs args)
        {
			if (game.GameState != Game.State.InProgress) return;

			string code = CreateCode();
			
			if (code == null)
			{
				Toast.MakeText(this, "Musisz wybrać wszystkie kolory!", ToastLength.Short).Show();
				return;
			}

			if (!game.CheckCodeLength(code))
			{
				Toast.MakeText(this, $"Musisz wybrać wszystkie kolory!", ToastLength.Short).Show();
				return;
			}

			int[] answer = game.CheckCode(code);

			for (int i = 0; i < answer.Length; i++)
			{
				if (answer[i] == -1)
				{
					btnsResult[i].Visibility = Android.Views.ViewStates.Invisible;
				}
				else if (answer[i] == 0)
				{
					btnsResult[i].SetBackgroundColor(Color.White);
					btnsResult[i].Visibility = Android.Views.ViewStates.Visible;
				}
				else if (answer[i] == 1)
				{
					btnsResult[i].SetBackgroundColor(Color.Black);
					btnsResult[i].Visibility = Android.Views.ViewStates.Visible;
				}
				else
				{
					Toast.MakeText(this, "Coś poszło nie tak! :(", ToastLength.Short).Show();
				}
			}

			Toast.MakeText(this, $"To był Twój {game.TotalMoves.ToString()} ruch na 9 możliwych.", ToastLength.Short).Show();

			if (game.GameState == Game.State.Finished)
			{
				Toast.MakeText(this, $"Brawo! Udało Ci odgadnąć kod w {game.TotalMoves} ruchach!", ToastLength.Short).Show();
				GameStop();
			}

			if (game.TotalMoves == 9)
			{
				Toast.MakeText(this, "To był Twój ostatni ruch! Nie udało Ci się odgadnąć! :(", ToastLength.Short).Show();
				GameStop();
			}
		}

		private void GameStart()
		{
			btnStart.Enabled = false;
			btnStop.Enabled = true;
			btnCheck.Enabled = true;
			game.SetRandomCode();
			game.Start();
		}

		private void GameStop()
		{
			btnCheck.Enabled = false;
			btnStop.Enabled = false;
			btnStart.Enabled = true;

			foreach (Button b in btnsResult)
				b.Visibility = Android.Views.ViewStates.Invisible;

			SetDefalts();
			game.Stop();
		}

		public void SetDefalts()
		{
			for (int i = 0; i < colorNr.Length; i++)
				colorNr[i] = -1;
			for (int i = 0; i < btns.Count; i++)
				btns[i].SetBackgroundColor(Color.GhostWhite);
		}

		private string CreateCode()
		{
			StringBuilder sb = new StringBuilder();

			try
			{
				for (int i = 0; i < btns.Count; i++)
				{
					switch (colorNr[i])
					{
						// Red.
						case 0:
							sb.Append("r");
							break;
						// Yellow.
						case 1:
							sb.Append("y");
							break;
						// Green.
						case 2:
							sb.Append("g");
							break;
						// Blue;
						case 3:
							sb.Append("b");
							break;
						// Cyan.
						case 4:
							sb.Append("c");
							break;
						// Magenta
						case 5:
							sb.Append("m");
							break;
					}
				}
			}
			catch (NullReferenceException)
			{
				return null;
			}

			Toast.MakeText(this, sb.ToString(), ToastLength.Short);

			return sb.ToString();
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}