using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace SHCD
{
	public class Login : MdiBase
	{
		private IContainer components = null;
		private Label label1;
		private Label label2;
		private TextBox textBox1;
		private Button button1;
		private Button button2;
		public event EventHandler Logon;
		public event EventHandler Exit;
		public string ID
		{
			get
			{
				return this.textBox1.Text;
			}
			set
			{
				this.textBox1.Text = value;
			}
		}
		public Login()
		{
			this.InitializeComponent();
		}
		public Login(string text)
		{
			this.InitializeComponent();
			Label expr_1C = this.label2;
			expr_1C.Text += text;
		}
		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				this.Exit(this, e);
			}
			catch
			{
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			if (this.textBox1.Text.Trim().Length == 13)
			{
				this.Logon(this, e);
			}
			else
			{
				MessageBox.Show("请输入正确的报名号！");
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.label1 = new Label();
			this.label2 = new Label();
			this.textBox1 = new TextBox();
			this.button1 = new Button();
			this.button2 = new Button();
			base.SuspendLayout();
			this.label1.BackColor = Color.Transparent;
			this.label1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label1.ForeColor = Color.SteelBlue;
			this.label1.Location = new Point(161, 20);
			this.label1.Name = "label1";
			this.label1.Size = new Size(160, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "考试系统登录";
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			this.label2.BackColor = Color.Transparent;
			this.label2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label2.ForeColor = Color.Black;
			this.label2.Location = new Point(0, 76);
			this.label2.Name = "label2";
			this.label2.Size = new Size(464, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "请输入报名号";
			this.label2.TextAlign = ContentAlignment.MiddleCenter;
			this.textBox1.Font = new Font("黑体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.textBox1.ImeMode = ImeMode.Off;
			this.textBox1.Location = new Point(47, 113);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(353, 29);
			this.textBox1.TabIndex = 3;
			this.button1.AutoSize = true;
			this.button1.Cursor = Cursors.Hand;
			this.button1.FlatStyle = FlatStyle.Flat;
			this.button1.Font = new Font("黑体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.button1.Location = new Point(83, 206);
			this.button1.Name = "button1";
			this.button1.Size = new Size(89, 31);
			this.button1.TabIndex = 4;
			this.button1.Text = "确定";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.AutoSize = true;
			this.button2.Cursor = Cursors.Hand;
			this.button2.FlatStyle = FlatStyle.Flat;
			this.button2.Font = new Font("黑体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.button2.Location = new Point(264, 206);
			this.button2.Name = "button2";
			this.button2.Size = new Size(89, 31);
			this.button2.TabIndex = 5;
			this.button2.Text = "退出";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.GradientInactiveCaption;
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Name = "Login";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
