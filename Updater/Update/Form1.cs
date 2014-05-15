using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
namespace Update
{
	public class Form1 : Form
	{
		private List<string> downloadList;
		private List<string> fileList;
		private bool error;
		private int downIndex;
		private IContainer components = null;
		private Button button1;
		private Button button2;
		private TextBox textBox1;
		private ProgressBar progressBar1;
		public Form1()
		{
			this.InitializeComponent();
			this.error = false;
			this.downloadList = new List<string>();
			this.fileList = new List<string>();
			string str = Program.URL.Substring(0, Program.URL.LastIndexOf("/"));
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(Program.URL);
				string[] files = Directory.GetFiles(Application.StartupPath, "*", SearchOption.AllDirectories);
				int i = 0;
				while (i < files.Length)
				{
					string text = files[i];
					string extension = Path.GetExtension(text);
					string text2 = extension;
					if (text2 == null)
					{
						goto IL_21A;
					}
					if (!(text2 == ".dll") && !(text2 == ".exe"))
					{
						goto IL_21A;
					}
					Version value = new Version(FileVersionInfo.GetVersionInfo(text).FileVersion);
					foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
					{
						if (xmlNode.Attributes["name"] != null && xmlNode.Attributes["version"] != null)
						{
							if (xmlNode.Attributes["name"].Value == text.Replace(Application.StartupPath, ""))
							{
								Version version = new Version(xmlNode.Attributes["version"].Value);
								if (version.CompareTo(value) > 0)
								{
									this.downloadList.Add(str + xmlNode.Attributes["name"].Value.Replace("\\", "/"));
									this.fileList.Add(text.Replace(Application.StartupPath, ""));
								}
							}
						}
					}
					IL_375:
					i++;
					continue;
					IL_21A:
					long ticks = File.GetCreationTimeUtc(text).Ticks;
					foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
					{
						if (xmlNode.Attributes["name"] != null && xmlNode.Attributes["time"] != null)
						{
							if (xmlNode.Attributes["name"].Value == text.Replace(Application.StartupPath, ""))
							{
								long num = Convert.ToInt64(xmlNode.Attributes["time"].Value);
								if (num > ticks)
								{
									this.downloadList.Add(str + xmlNode.Attributes["name"].Value.Replace("\\", "/"));
									this.fileList.Add(text.Replace(Application.StartupPath, ""));
								}
							}
						}
					}
					goto IL_375;
				}
				if (xmlDocument.DocumentElement.Attributes["details"] != null)
				{
					WebClient webClient = new WebClient();
					this.textBox1.Text = webClient.DownloadString(str + xmlDocument.DocumentElement.Attributes["details"].Value);
				}
			}
			catch
			{
				this.error = true;
			}
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			if (this.error)
			{
				MessageBox.Show("获取更新失败！");
				base.Close();
			}
			else
			{
				this.textBox1.SelectedText = "";
				this.textBox1.SelectionLength = 0;
				this.textBox1.SelectionStart = 0;
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this.progressBar1.Maximum = this.downloadList.Count;
			this.progressBar1.Value = 0;
			Process[] processes = Process.GetProcesses();
			Process[] array = processes;
			for (int i = 0; i < array.Length; i++)
			{
				Process process = array[i];
				if (process.ProcessName == "练习光盘")
				{
					try
					{
						if (process.Responding)
						{
							process.Kill();
							process.WaitForExit();
							Thread.Sleep(200);
							int num = 0;
							while (!process.HasExited && num < 5)
							{
								Thread.Sleep(200);
								num++;
							}
							Thread.Sleep(200);
						}
					}
					catch
					{
					}
				}
			}
			this.button1.Enabled = false;
			this.button2.Enabled = false;
			this.downIndex = 0;
			this.DownLoad();
		}
		private void DownLoad()
		{
			if (this.downIndex < this.downloadList.Count)
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.wc_DownloadFileCompleted);
				try
				{
					DirectoryInfo parent = Directory.GetParent(Path.Combine(Application.StartupPath, "update") + this.fileList[this.downIndex]);
					if (!parent.Exists)
					{
						parent.Create();
					}
					webClient.DownloadFileAsync(new Uri(this.downloadList[this.downIndex]), Path.Combine(Application.StartupPath, "update") + this.fileList[this.downIndex]);
				}
				catch
				{
					MessageBox.Show("更新过程发生错误！");
					base.Close();
				}
			}
		}
		private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			this.downIndex++;
			if (e.Error != null)
			{
				MessageBox.Show("更新过程发生错误！");
				try
				{
					Directory.Delete(Path.Combine(Application.StartupPath, "update"));
				}
				catch
				{
				}
				base.Close();
			}
			else
			{
				this.progressBar1.Value = this.downIndex;
				if (this.downIndex < this.downloadList.Count)
				{
					this.DownLoad();
				}
				else
				{
					MessageBox.Show("更新完成，现在启动练习光盘！");
					try
					{
						Form1.CopyFolder(Path.Combine(Application.StartupPath, "update"), Application.StartupPath, true);
						Directory.Delete(Path.Combine(Application.StartupPath, "update"), true);
					}
					catch
					{
					}
					Process.Start(Path.Combine(Application.StartupPath, "练习光盘.exe"));
					base.Close();
				}
			}
		}
		private void button2_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		public static void CopyFolder(string sourceFolder, string destFolder, bool overwrite = true)
		{
			if (Directory.Exists(sourceFolder))
			{
				if (!Directory.Exists(destFolder))
				{
					Directory.CreateDirectory(destFolder);
				}
				string[] files = Directory.GetFiles(sourceFolder);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					string fileName = Path.GetFileName(text);
					string text2 = Path.Combine(destFolder, fileName);
					try
					{
						File.Copy(text, text2, overwrite);
					}
					catch (Exception var_4_58)
					{
					}
				}
				string[] directories = Directory.GetDirectories(sourceFolder);
				array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					string text3 = array[i];
					string fileName = Path.GetFileName(text3);
					string text2 = Path.Combine(destFolder, fileName);
					Form1.CopyFolder(text3, text2, overwrite);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
			this.button1 = new Button();
			this.button2 = new Button();
			this.textBox1 = new TextBox();
			this.progressBar1 = new ProgressBar();
			base.SuspendLayout();
			this.button1.AutoSize = true;
			this.button1.FlatStyle = FlatStyle.Flat;
			this.button1.Location = new Point(126, 237);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "开始更新";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.AutoSize = true;
			this.button2.FlatStyle = FlatStyle.Flat;
			this.button2.Location = new Point(229, 237);
			this.button2.Name = "button2";
			this.button2.Size = new Size(96, 24);
			this.button2.TabIndex = 2;
			this.button2.Text = "不了，谢谢！";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.textBox1.Dock = DockStyle.Top;
			this.textBox1.Font = new Font("微软雅黑", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.textBox1.HideSelection = false;
			this.textBox1.Location = new Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = ScrollBars.Vertical;
			this.textBox1.Size = new Size(337, 207);
			this.textBox1.TabIndex = 0;
			this.progressBar1.Location = new Point(0, 213);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new Size(337, 12);
			this.progressBar1.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(337, 262);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox1);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "Form1";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "软件更新";
			base.Load += new EventHandler(this.Form1_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
