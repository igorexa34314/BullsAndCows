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
	public partial class StartForm : Form
	{
		string Username = "";
		int NumCount = 0;
		public StartForm()
		{
			InitializeComponent();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Username = Convert.ToString(textBox1.Text);
			NumCount = Convert.ToInt32(numericUpDown1.Value);

			Form1 f1 = new Form1(Username, NumCount);
			f1.Left = this.Left; // задаём открываемой форме позицию слева равную позиции текущей формы
			f1.Top = this.Top; // задаём открываемой форме позицию сверху равную позиции текущей формы
			f1.Show(); // отображаем Form2
			this.Hide(); // скрываем Form1 (this - текущая форма)
		}

		private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
	}
}
