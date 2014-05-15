using Qisi.General;
using Qisi.General.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
namespace SHCD
{
	public class upload : Form
	{
		private string mypaper;
		private IContainer components = null;
		private Label label1;
		private TextBox textBox1;
		private Button button1;
		public upload(string paper)
		{
			this.InitializeComponent();
			this.mypaper = paper;
		}
		private void button1_Click(object sender, EventArgs e)
		{
			if (this.textBox1.Text.Contains("&") || this.textBox1.Text.Contains("="))
			{
				FlatMessageBox.Show(this, "邮箱输入错误，请重新输入邮箱地址！", "提示", FlatMessageBox.KeysButtons.YesNo, FlatMessageBox.KeysIcon.Information);
			}
			else
			{
				if (FlatMessageBox.Show(this, "每张模拟卷只能获取一次评估报告，你确定现在就要获取评估报告吗？", "提示", FlatMessageBox.KeysButtons.YesNo, FlatMessageBox.KeysIcon.Information) == DialogResult.Yes)
				{
					if (FlatMessageBox.Show(this, "你的邮箱是：" + this.textBox1.Text + "\r\n确定吗?", "提示", FlatMessageBox.KeysButtons.YesNo, FlatMessageBox.KeysIcon.Information) != DialogResult.No)
					{
						string text = "";
						string text2 = Environment.GetEnvironmentVariable("USERPROFILE") + "\\SHCD.ini";
						string text3 = "";
						try
						{
							text3 = File.ReadAllText(Environment.GetEnvironmentVariable("USERPROFILE") + "\\SHCD.ini");
						}
						catch
						{
							text3 = "";
						}
						finally
						{
							byte[] bytes = Convert.FromBase64String(text3);
							text3 = Encoding.ASCII.GetString(bytes);
							if (text3.Length >= 34)
							{
								text = text3.Substring(0, 18);
							}
						}
						WebClient webClient = new WebClient();
						string a = "";
						try
						{
							a = webClient.DownloadString(string.Concat(new string[]
							{
								"http://www.keys-edu.com/register/upload.asp?LISTCODE=",
								text,
								"&PAPERNAME=",
								this.mypaper,
								"&EMAIL=",
								this.textBox1.Text
							}));
						}
						catch
						{
						}
						if (a == "1")
						{
							if (FlatMessageBox.Show(this, "点击“确定”开始上传你的答题数据！", "提示", FlatMessageBox.KeysButtons.YesNo, FlatMessageBox.KeysIcon.Information) == DialogResult.Yes)
							{
								FTPClient fTPClient = new FTPClient("www.keys-edu.com", "shcd", "Keys0801");
								if (fTPClient.login())
								{
									fTPClient.makeDir(text);
									fTPClient.Upload(Path.Combine(Program.answerDir, this.mypaper + ".dat"));
								}
								FlatMessageBox.Show(this, "数据上传完成，我们会在两周内返回你的评估报告。", "提示", FlatMessageBox.KeysButtons.OK, FlatMessageBox.KeysIcon.Information);
								base.DialogResult = DialogResult.OK;
							}
						}
						else
						{
							FlatMessageBox.Show(this, "你可能已经获取过评估报告，如有任何疑问，请拨打咨询热线。", "错误", FlatMessageBox.KeysButtons.OK, FlatMessageBox.KeysIcon.Information);
						}
					}
				}
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
			this.textBox1 = new TextBox();
			this.button1 = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label1.ForeColor = Color.White;
			this.label1.Location = new Point(1, 32);
			this.label1.Name = "label1";
			this.label1.Size = new Size(512, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "请输入你的邮箱地址，我们将在2周内，将评估报告发往你填写的邮箱。";
			this.textBox1.Location = new Point(68, 67);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(384, 21);
			this.textBox1.TabIndex = 1;
			this.button1.Cursor = Cursors.Hand;
			this.button1.FlatStyle = FlatStyle.Flat;
			this.button1.Location = new Point(217, 115);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "确定";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(69, 137, 148);
			base.ClientSize = new Size(509, 169);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "upload";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "upload";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
