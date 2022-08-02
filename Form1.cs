using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BullsAndCows
{
	public partial class Form1 : Form
	{
		uint HiddenNum;
		int dvcount = 0;
		int NumCount;
		int AttemptCount;
		string Username = "";
		class Score
		{
			public byte GuessedNumCount = 0;
			public byte GuessedOrderCount = 0;
			public override string ToString()
			{
				return GuessedNumCount.ToString() + ":" + GuessedOrderCount.ToString();
			}
			public void Print(DataGridView dv, int dvcount)
			{
				dv.Rows.Add();
				dv.Rows[dvcount].Cells[1].Value = ToString();
			}
		}

		public Form1()
		{
			InitializeComponent();
		}
		public Form1(string user, int ncount)
		{
			InitializeComponent();
			NumCount = ncount;
			Username = user;
			this.Text = "Быки и коровы (" + Username + ")";
			StartNewGame(NumCount);
		}

		static uint InitRandNum(int ncount)
		{
			Random r = new Random();
			uint RandNum;
			int a = Convert.ToInt32(Math.Pow(10, (ncount - 1)));
			int b = Convert.ToInt32(Math.Pow(10, ncount) - 1);
			do
			{
				RandNum = (uint)r.Next(a, b);
			}
			while (!(CheckNumForRepeat(RandNum.ToString())));
			return RandNum;
		}

		// Для загаданного числа
		static bool CheckNumForRepeat(string strnum)
		{
			uint[] arr = ToNumArrray(strnum);
			if (arr == null)
				return false;
			var hset = new HashSet<uint>();
			foreach (uint item in arr)
				if (!hset.Add(item))
					return false;
			return true;
		}

		// Для предполагаемого числа
		static bool CheckNumForValid(string strnum, int NumCount)
		{
			uint[] arr = ToNumArrray(strnum);
			if( (arr == null) || (strnum.Length != NumCount) )
			{
				MessageBox.Show($"Введите натуральное {NumCount}-значное число");
				return false;
			}
			else if (!(CheckNumForRepeat(strnum)))
			{
				MessageBox.Show("Цифры в числе не должны повторяться");
				return false;
			}
			return true;
		}

		// Строка в число
		static uint[] ToNumArrray(string strnum)
		{
			uint[] arr = null;
			uint unum = ToNum(strnum);
			if (unum != 0)
			{
				arr = new uint[strnum.Length];
				for (int i = strnum.Length - 1; i >= 0; i--)
				{
					arr[i] = unum % 10;
					unum /= 10;
				}
			}
			return arr;
		}

		// Проверка на натуральное число
		static bool IsNum(string str) 
		{
			if (str[0] == '0')
				return false;
			foreach (char ch in str)
				if (!(Char.IsDigit(ch)))
					return false;
			return true;
		}

		// Строка в число
		static uint ToNum(string str)
		{
			uint unum = 0;
			if (IsNum(str))
				unum = (uint)Convert.ToInt32(str);
			return unum;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string SupposedStr = Convert.ToString(textBox2.Text);
			if (CheckNumForValid(SupposedStr, NumCount))
				CalculateScore(ToNum(SupposedStr), HiddenNum);
		}

		void CalculateScore(uint SupposedNum, uint GuessedNum)
		{
			AttemptCount++;
			Score score = new Score();
			uint[] SupposedArr = ToNumArrray(SupposedNum.ToString());
			uint[] GuessedArr = ToNumArrray(GuessedNum.ToString());
			for (int g = 0; g < GuessedArr.Length; g++)
			{
				for (int s = 0; s < SupposedArr.Length; s++)
				{
					if (SupposedArr[s] == GuessedArr[g])
					{
						score.GuessedNumCount++;
						if (g == s)
							score.GuessedOrderCount++;
					}
				}
			}
			score.Print(dataGridView1, dvcount);
			dataGridView1.Rows[dvcount].Cells[0].Value = SupposedNum;
			if ((score.GuessedNumCount == score.GuessedOrderCount) && (score.GuessedOrderCount == GuessedNum.ToString().Length))
			{
				MessageBox.Show("Вы угадали число!!\nКоличество попыток - " + AttemptCount);
				AskNewGame();
			}
			else
				dvcount++;
		}

		void AskNewGame() 
		{
			DialogResult result = MessageBox.Show(
			"Вы хотитете начать игру заново?",
			"Новая игра",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Question,
			MessageBoxDefaultButton.Button1,
			MessageBoxOptions.DefaultDesktopOnly);
			if (result == DialogResult.Yes)
			{
				StartNewGame(NumCount);
				this.TopMost = true;
			}
			else
				this.Close();
		}

		void StartNewGame(int ncount) {
			HiddenNum = InitRandNum(ncount);
			textBox1.Text = HiddenNum.ToString();
			dvcount = 0;
			AttemptCount = 0;
			dataGridView1.ColumnCount = 2;
			dataGridView1.Rows.Clear();
			textBox2.Clear();
		}

		private void Surrender(object sender, EventArgs e)
		{
			MessageBox.Show("Какой же вы слабый!\nЗагаданное число: " + HiddenNum.ToString());
			AskNewGame();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			Form ifrm = Application.OpenForms[0];
			ifrm.StartPosition = FormStartPosition.Manual; // меняем параметр StartPosition у Form1, иначе она будет использовать тот, который у неё прописан в настройках и всегда будет открываться по центру экрана
			ifrm.Left = this.Left; // задаём открываемой форме позицию слева равную позиции текущей формы
			ifrm.Top = this.Top;
			ifrm.TopMost = true;// задаём открываемой форме позицию сверху равную позиции текущей формы
			ifrm.Show();
		}
	}
}
