using ExamClientControlsLibrary;
using Qisi.General;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace SHCD
{
	public class score : Form
	{
		private string mypaper;
		private Form myowner;
		private IContainer components = null;
		private Label label2;
		private Label label1;
		private Button button1;
		private Button button2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label label10;
		private Label label11;
		private Label label12;
		private Label label13;
		private Label label14;
		public score(string paper)
		{
			this.InitializeComponent();
			this.mypaper = paper;
			MemoryIniFile memoryIniFile = new MemoryIniFile();
			memoryIniFile.LoadFromEncodedFile(Path.Combine(Program.tempAnswerDir, "Answer.ini"));
			this.label1.Text = "<" + paper + ">  " + memoryIniFile.ReadValue("ANSWER", "SUBJECT", "");
			int num = memoryIniFile.ReadValue("ANSWER", "NUM", 0);
			int num2 = 0;
			float num3 = 0f;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			float num7 = 0f;
			MemoryIniFile memoryIniFile2 = new MemoryIniFile();
			memoryIniFile2.LoadFromFile(Path.Combine(Application.StartupPath, "STD\\" + paper + "\\stds.ini"));
			for (int i = 0; i < num; i++)
			{
				string sectionName = memoryIniFile.ReadValue(i.ToString(), "ID", "");
				string text = memoryIniFile2.ReadValue(sectionName, "std", "");
				if (text != "")
				{
					num2++;
					float num8 = memoryIniFile2.ReadValue(sectionName, "SCORE", 0f);
					num3 += num8;
					string a = memoryIniFile.ReadValue(i.ToString(), "Answer", "");
					if (a == "" || a == "[]")
					{
						num4++;
					}
					else
					{
						if (a == "[" + text + "]")
						{
							num5++;
							num7 += num8;
						}
						else
						{
							num6++;
						}
					}
				}
			}
			this.label3.Text = "题数";
			this.label4.Text = num2.ToString();
			this.label5.Text = "满分";
			this.label6.Text = num3.ToString();
			this.label7.Text = "未作答";
			this.label8.Text = num4.ToString();
			this.label9.Text = "答对";
			this.label10.Text = num5.ToString();
			this.label11.Text = "答错";
			this.label12.Text = num6.ToString();
			this.label13.Text = "得分";
			this.label14.Text = num7.ToString();
		}
		private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void button1_Click(object sender, EventArgs e)
		{
			FormTemplate formTemplate = new FormTemplate(TestPaperPlayer.RunMode.CDAnalysis, this.mypaper);
			this.myowner = base.Owner;
			base.Owner.Hide();
			base.Hide();
			formTemplate.Disposed += new EventHandler(this.t_Disposed);
			formTemplate.Show();
		}
		public void t_Disposed(object sender, EventArgs e)
		{
			if (this.myowner != null)
			{
				this.myowner.Show();
				base.Owner = this.myowner;
				base.ShowDialog(this.myowner);
			}
		}
		private void button2_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void button3_Click(object sender, EventArgs e)
		{
			upload upload = new upload(this.mypaper);
			upload.ShowDialog();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.components != null)
				{
					this.components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(score));
			this.label1 = new Label();
			this.label2 = new Label();
			this.button1 = new Button();
			this.button2 = new Button();
			this.label3 = new Label();
			this.label4 = new Label();
			this.label5 = new Label();
			this.label6 = new Label();
			this.label7 = new Label();
			this.label8 = new Label();
			this.label9 = new Label();
			this.label10 = new Label();
			this.label11 = new Label();
			this.label12 = new Label();
			this.label13 = new Label();
			this.label14 = new Label();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.BackColor = Color.Transparent;
			this.label1.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label1.ForeColor = Color.White;
			this.label1.Location = new Point(1, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(56, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			this.label2.AutoSize = true;
			this.label2.BackColor = Color.Transparent;
			this.label2.ForeColor = Color.White;
			this.label2.Location = new Point(1, 32);
			this.label2.Name = "label2";
			this.label2.Size = new Size(41, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "选择题";
			this.button1.Cursor = Cursors.Hand;
			this.button1.FlatStyle = FlatStyle.Flat;
			this.button1.Location = new Point(69, 227);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "分析";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Cursor = Cursors.Hand;
			this.button2.FlatStyle = FlatStyle.Flat;
			this.button2.Location = new Point(199, 227);
			this.button2.Name = "button2";
			this.button2.Size = new Size(75, 23);
			this.button2.TabIndex = 4;
			this.button2.Text = "退出";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.label3.AutoSize = true;
			this.label3.BackColor = Color.Transparent;
			this.label3.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label3.ForeColor = Color.White;
			this.label3.Location = new Point(12, 54);
			this.label3.Name = "label3";
			this.label3.Size = new Size(56, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "label3";
			this.label4.AutoSize = true;
			this.label4.BackColor = Color.Transparent;
			this.label4.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label4.ForeColor = Color.White;
			this.label4.Location = new Point(196, 54);
			this.label4.Name = "label4";
			this.label4.Size = new Size(56, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "label4";
			this.label5.AutoSize = true;
			this.label5.BackColor = Color.Transparent;
			this.label5.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label5.ForeColor = Color.White;
			this.label5.Location = new Point(12, 80);
			this.label5.Name = "label5";
			this.label5.Size = new Size(56, 16);
			this.label5.TabIndex = 7;
			this.label5.Text = "label5";
			this.label6.AutoSize = true;
			this.label6.BackColor = Color.Transparent;
			this.label6.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label6.ForeColor = Color.White;
			this.label6.Location = new Point(196, 80);
			this.label6.Name = "label6";
			this.label6.Size = new Size(56, 16);
			this.label6.TabIndex = 8;
			this.label6.Text = "label6";
			this.label7.AutoSize = true;
			this.label7.BackColor = Color.Transparent;
			this.label7.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label7.ForeColor = Color.White;
			this.label7.Location = new Point(12, 106);
			this.label7.Name = "label7";
			this.label7.Size = new Size(56, 16);
			this.label7.TabIndex = 9;
			this.label7.Text = "label7";
			this.label8.AutoSize = true;
			this.label8.BackColor = Color.Transparent;
			this.label8.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label8.ForeColor = Color.White;
			this.label8.Location = new Point(196, 106);
			this.label8.Name = "label8";
			this.label8.Size = new Size(56, 16);
			this.label8.TabIndex = 10;
			this.label8.Text = "label8";
			this.label9.AutoSize = true;
			this.label9.BackColor = Color.Transparent;
			this.label9.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label9.ForeColor = Color.White;
			this.label9.Location = new Point(12, 132);
			this.label9.Name = "label9";
			this.label9.Size = new Size(56, 16);
			this.label9.TabIndex = 11;
			this.label9.Text = "label9";
			this.label10.AutoSize = true;
			this.label10.BackColor = Color.Transparent;
			this.label10.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label10.ForeColor = Color.White;
			this.label10.Location = new Point(196, 132);
			this.label10.Name = "label10";
			this.label10.Size = new Size(64, 16);
			this.label10.TabIndex = 12;
			this.label10.Text = "label10";
			this.label11.AutoSize = true;
			this.label11.BackColor = Color.Transparent;
			this.label11.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label11.ForeColor = Color.White;
			this.label11.Location = new Point(12, 158);
			this.label11.Name = "label11";
			this.label11.Size = new Size(64, 16);
			this.label11.TabIndex = 13;
			this.label11.Text = "label11";
			this.label12.AutoSize = true;
			this.label12.BackColor = Color.Transparent;
			this.label12.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label12.ForeColor = Color.White;
			this.label12.Location = new Point(196, 158);
			this.label12.Name = "label12";
			this.label12.Size = new Size(64, 16);
			this.label12.TabIndex = 14;
			this.label12.Text = "label12";
			this.label13.AutoSize = true;
			this.label13.BackColor = Color.Transparent;
			this.label13.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label13.ForeColor = Color.White;
			this.label13.Location = new Point(12, 184);
			this.label13.Name = "label13";
			this.label13.Size = new Size(64, 16);
			this.label13.TabIndex = 15;
			this.label13.Text = "label13";
			this.label14.AutoSize = true;
			this.label14.BackColor = Color.Transparent;
			this.label14.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label14.ForeColor = Color.White;
			this.label14.Location = new Point(196, 184);
			this.label14.Name = "label14";
			this.label14.Size = new Size(64, 16);
			this.label14.TabIndex = 16;
			this.label14.Text = "label14";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(69, 137, 148);
			base.ClientSize = new Size(371, 289);
			base.Controls.Add(this.label14);
			base.Controls.Add(this.label13);
			base.Controls.Add(this.label12);
			base.Controls.Add(this.label11);
			base.Controls.Add(this.label10);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			this.DoubleBuffered = true;
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "score";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "分数";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
