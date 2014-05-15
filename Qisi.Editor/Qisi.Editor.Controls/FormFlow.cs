using Qisi.General.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Forms;
namespace Qisi.Editor.Controls
{
	internal class FormFlow : Form
	{
		private delegate void SetColorCallBack(ToolStripStatusLabel control, Color color);
		private delegate void SetTextCallBack(ToolStripStatusLabel control, string text);
		private delegate void SetEnabledCallBack(Control control, bool enabled);
		private delegate void pexitcallback();
		private const int WM_SYSCOMMAND = 274;
		private const int SC_CLOSE = 61536;
		private const int SC_MINIMIZE = 61472;
		private const int SC_MAXIMIZE = 61488;
		private int myHeight;
		private int timecount;
		private int backcount;
		private int leftTime;
		private int tickCount;
		private int endTipTime;
		private DateTime lastbacktime;
		private System.Timers.Timer mytimer;
		private string dataDir;
		private IContainer components = null;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStripStatusLabel toolStripStatusLabel2;
		private ToolStripStatusLabel toolStripStatusLabel3;
		private Panel panel1;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private Button btnBack;
		private Button btnOpenDataPath;
		private ReadOnlyRichTextBox readOnlyRichTextBox1;
		internal bool hasProcess
		{
			get;
			set;
		}
		internal FormFlow(Image stuImg, string stuInfo, string rtfPath, string dataPath, int examLeftTime, int endtiptime, string answerPath, string gifPath, int countMode)
		{
			this.InitializeComponent();
			this.myHeight = 0;
			this.timecount = 0;
			this.backcount = 0;
			this.leftTime = examLeftTime;
			if (countMode == -1)
			{
				this.tickCount = 0;
			}
			else
			{
				this.tickCount = examLeftTime;
			}
			this.endTipTime = endtiptime;
			this.hasProcess = false;
			this.btnBack.Enabled = false;
			this.DoubleBuffered = true;
			this.panel1.BackColor = Color.SkyBlue;
			this.panel1.Location = new Point(0, 0);
			PictureBox pictureBox = new PictureBox();
			pictureBox.Image = stuImg;
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			base.TopMost = true;
			pictureBox.Size = new Size(100, 132);
			this.panel1.Controls.Add(pictureBox);
			pictureBox.Location = new Point(0, 0);
			Label label = new Label();
			label.Text = stuInfo;
			label.Font = new Font("黑体", 9f, FontStyle.Bold);
			label.Visible = true;
			label.AutoSize = true;
			this.panel1.Controls.Add(label);
			label.Top = pictureBox.Bottom + 10;
			FileStream fileStream = File.OpenRead(rtfPath);
			this.readOnlyRichTextBox1.LoadFile(fileStream, RichTextBoxStreamType.RichText);
			fileStream.Close();
			this.dataDir = dataPath;
			this.toolStripStatusLabel3.Text = "考生目录：" + this.dataDir;
			this.toolStripStatusLabel1.Font = new Font("黑体", 9f, FontStyle.Bold);
			this.toolStripStatusLabel2.Font = new Font("黑体", 9f, FontStyle.Bold);
			this.toolStripStatusLabel3.Font = new Font("黑体", 9f, FontStyle.Bold);
			if (countMode == -1)
			{
				this.mytimer = new System.Timers.Timer(500.0);
				this.mytimer.Elapsed += new ElapsedEventHandler(this.timer1_Tick);
				this.mytimer.AutoReset = true;
				this.mytimer.Start();
			}
			else
			{
				if (countMode == 0)
				{
					this.toolStripStatusLabel2.Text = "";
				}
				else
				{
					this.mytimer = new System.Timers.Timer(500.0);
					this.mytimer.Elapsed += new ElapsedEventHandler(this.timer1_Tick_1);
					this.mytimer.AutoReset = true;
					this.mytimer.Start();
				}
			}
			this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
			this.btnOpenDataPath.Click += new EventHandler(this.button2_Click);
			this.btnBack.Click += new EventHandler(this.button1_Click);
			if (gifPath != null && gifPath != "")
			{
				TabPage tabPage = new TabPage();
				Button button = new Button();
				button.AutoSize = true;
				button.Text = "播放";
				tabPage.Controls.Add(button);
				button.Location = new Point(3, 3);
				button.Click += new EventHandler(this.play_Click);
				Button button2 = new Button();
				button2.AutoSize = true;
				button2.Text = "停止";
				tabPage.Controls.Add(button2);
				button2.Location = new Point(3 + button.Width, 3);
				button2.Click += new EventHandler(this.stop_Click);
				animateImage animateImage = new animateImage(gifPath);
				tabPage.Controls.Add(animateImage);
				animateImage.Location = new Point(3, button.Bottom + 3);
				tabPage.Text = "点此观看参考样例";
				this.tabControl1.TabPages.Add(tabPage);
			}
			if (answerPath != null && answerPath != "")
			{
				TabPage tabPage = new TabPage();
				ReadOnlyRichTextBox readOnlyRichTextBox = new ReadOnlyRichTextBox();
				readOnlyRichTextBox.ReadOnly = true;
				readOnlyRichTextBox.LoadFile(answerPath);
				readOnlyRichTextBox.Dock = DockStyle.Fill;
				tabPage.Controls.Add(readOnlyRichTextBox);
				tabPage.Text = "分析与解答";
				this.tabControl1.TabPages.Add(tabPage);
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
		}
		private void button2_Click(object sender, EventArgs e)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			process.StandardInput.WriteLine("start  " + this.dataDir);
		}
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = this.tabControl1.SelectedIndex;
			if (selectedIndex != 0)
			{
				if (this.tabControl1.TabPages[selectedIndex].Controls.Count >= 3 && this.tabControl1.TabPages[selectedIndex].Controls[2].GetType() == Type.GetType("ExamClientControlsLibrary.animateImage"))
				{
					((animateImage)this.tabControl1.TabPages[selectedIndex].Controls[2]).Play();
				}
			}
		}
		private void stop_Click(object sender, EventArgs e)
		{
			try
			{
				int index = ((Button)sender).Parent.Controls.IndexOf((Button)sender) + 1;
				((animateImage)((Button)sender).Parent.Controls[index]).Stop();
			}
			finally
			{
			}
		}
		private void play_Click(object sender, EventArgs e)
		{
			try
			{
				int index = ((Button)sender).Parent.Controls.IndexOf((Button)sender) + 2;
				((animateImage)((Button)sender).Parent.Controls[index]).Play();
			}
			finally
			{
			}
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 274)
			{
				if (m.WParam.ToInt32() == 61472)
				{
					this.myHeight = Math.Max(200, base.ClientSize.Height);
					base.Height -= base.ClientSize.Height;
					base.MaximizeBox = true;
					base.MinimizeBox = false;
					return;
				}
				if (m.WParam.ToInt32() == 61488)
				{
					base.Height += this.myHeight;
					base.MaximizeBox = false;
					base.MinimizeBox = true;
					return;
				}
			}
			base.WndProc(ref m);
		}
		private void FlowForm_Load(object sender, EventArgs e)
		{
			foreach (TabPage tabPage in this.tabControl1.TabPages)
			{
				if (this.tabControl1.TabPages.IndexOf(tabPage) != 0)
				{
					foreach (Control control in tabPage.Controls)
					{
						this.tabControl1.Width = Math.Max(this.tabControl1.Width, control.Right + 6);
						this.tabControl1.Height = Math.Max(this.tabControl1.Height, control.Bottom + 18);
					}
				}
			}
			foreach (Control control in this.panel1.Controls)
			{
				this.panel1.Height = Math.Max(this.panel1.Height, control.Bottom);
				this.panel1.Width = Math.Max(this.panel1.Width, control.Right);
			}
			if (this.panel1.Height > this.tabControl1.Height)
			{
				this.tabControl1.Height = this.panel1.Height;
			}
			else
			{
				this.panel1.Height = this.tabControl1.Height;
			}
			this.btnBack.Top = this.panel1.Bottom;
			this.btnOpenDataPath.Top = this.btnBack.Top;
			this.panel1.Controls[0].Left = (this.panel1.Width - this.panel1.Controls[0].Width) / 2;
			this.tabControl1.Location = new Point(this.panel1.Right, 0);
			base.ClientSize = new Size(this.tabControl1.Right, this.btnBack.Bottom + this.statusStrip1.Height);
			base.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - base.Width, Screen.PrimaryScreen.WorkingArea.Height - base.Height);
		}
		private void FlowForm_SizeChanged(object sender, EventArgs e)
		{
			if (base.ClientSize.Height > 0)
			{
				base.MaximizeBox = false;
				base.MinimizeBox = true;
			}
			this.btnBack.Top = this.statusStrip1.Top - this.btnBack.Height;
			this.btnOpenDataPath.Top = this.btnBack.Top;
			this.panel1.Height = this.btnBack.Top;
			this.tabControl1.Height = this.panel1.Height;
			this.tabControl1.Width = base.ClientSize.Width - this.tabControl1.Left;
		}
		private void SetEnabled(Control c, bool Enabled)
		{
			if (c.InvokeRequired)
			{
				FormFlow.SetEnabledCallBack method = new FormFlow.SetEnabledCallBack(this.SetEnabled);
				c.BeginInvoke(method, new object[]
				{
					c,
					Enabled
				});
			}
			else
			{
				c.Enabled = Enabled;
			}
		}
		private void timer1_Tick_1(object sender, ElapsedEventArgs e)
		{
			this.tickCount++;
			if (this.timecount < 10)
			{
				this.timecount++;
			}
			else
			{
				if (this.timecount == 10)
				{
					this.SetEnabled(this.btnBack, true);
				}
			}
			this.tickCount %= 2;
			int num = this.leftTime % 60;
			int num2 = (this.leftTime - num) / 60 % 60;
			int num3 = ((this.leftTime - num) / 60 - num2) / 60;
			if (this.leftTime > this.endTipTime && this.tickCount == 1)
			{
				this.SetText(this.toolStripStatusLabel2, string.Concat(new string[]
				{
					num3.ToString().PadLeft(2, '0'),
					":",
					num2.ToString().PadLeft(2, '0'),
					":",
					num.ToString().PadLeft(2, '0')
				}));
				this.leftTime++;
			}
		}
		private void timer1_Tick(object sender, ElapsedEventArgs e)
		{
			this.tickCount++;
			if (this.timecount < 10)
			{
				this.timecount++;
			}
			else
			{
				if (this.timecount == 10)
				{
					this.SetEnabled(this.btnBack, true);
				}
			}
			this.tickCount %= 2;
			int num = this.leftTime % 60;
			int num2 = (this.leftTime - num) / 60 % 60;
			int num3 = ((this.leftTime - num) / 60 - num2) / 60;
			if (this.leftTime <= 0 && this.tickCount == 1)
			{
				this.mytimer.Stop();
				this.pexit();
			}
			if (this.leftTime > this.endTipTime && this.tickCount == 1)
			{
				this.SetText(this.toolStripStatusLabel2, string.Concat(new string[]
				{
					num3.ToString().PadLeft(2, '0'),
					":",
					num2.ToString().PadLeft(2, '0'),
					":",
					num.ToString().PadLeft(2, '0')
				}));
			}
			else
			{
				if (this.leftTime <= this.endTipTime && this.tickCount == 1)
				{
					this.SetColor(this.toolStripStatusLabel2, Color.Red);
					this.SetText(this.toolStripStatusLabel2, string.Concat(new string[]
					{
						num3.ToString().PadLeft(2, '0'),
						":",
						num2.ToString().PadLeft(2, '0'),
						":",
						num.ToString().PadLeft(2, '0')
					}));
				}
				else
				{
					if (this.leftTime <= this.endTipTime && this.tickCount == 0)
					{
						this.SetColor(this.toolStripStatusLabel2, Color.White);
						this.SetText(this.toolStripStatusLabel2, string.Concat(new string[]
						{
							num3.ToString().PadLeft(2, '0'),
							":",
							num2.ToString().PadLeft(2, '0'),
							":",
							num.ToString().PadLeft(2, '0')
						}));
					}
				}
			}
			if (this.tickCount == 1)
			{
				this.leftTime--;
			}
		}
		private void pexit()
		{
			if (base.InvokeRequired)
			{
				FormFlow.pexitcallback method = new FormFlow.pexitcallback(this.pexit);
				if (this != null && !base.IsDisposed)
				{
					base.Invoke(method);
				}
			}
			else
			{
				if (this != null && !base.IsDisposed)
				{
					base.Dispose();
				}
			}
		}
		private void SetColor(ToolStripStatusLabel c, Color col)
		{
			if (base.InvokeRequired)
			{
				FormFlow.SetColorCallBack method = new FormFlow.SetColorCallBack(this.SetColor);
				base.BeginInvoke(method, new object[]
				{
					c,
					col
				});
			}
			else
			{
				c.ForeColor = col;
			}
		}
		private void SetText(ToolStripStatusLabel c, string text)
		{
			if (base.InvokeRequired)
			{
				FormFlow.SetTextCallBack method = new FormFlow.SetTextCallBack(this.SetText);
				base.BeginInvoke(method, new object[]
				{
					c,
					text
				});
			}
			else
			{
				c.Text = text;
			}
		}
		private void FlowForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.btnBack.Enabled)
			{
				e.Cancel = true;
			}
			else
			{
				if (this.hasProcess)
				{
					bool flag = 1 == 0;
					TimeSpan timeSpan = new TimeSpan(this.lastbacktime.Ticks);
					TimeSpan ts = new TimeSpan(DateTime.Now.Ticks);
					if (timeSpan.Subtract(ts).Duration().Seconds < 10)
					{
						this.backcount++;
						if (this.backcount >= 4)
						{
							if (MessageBox.Show("真的要强制返回主界面吗？", "注意", MessageBoxButtons.YesNo) == DialogResult.Yes)
							{
								this.mytimer.Enabled = false;
								return;
							}
							this.backcount = 0;
						}
						this.lastbacktime = DateTime.Now;
						MessageBox.Show("请关闭操作软件窗口。\n关闭操作软件窗口就可以返回答题主界面。\n或连续点击四次“返回”按钮，强制返回答题主界面。", "注意", MessageBoxButtons.OK);
					}
					else
					{
						this.backcount = 1;
						this.lastbacktime = DateTime.Now;
						MessageBox.Show("请关闭操作软件窗口。\n关闭操作软件窗口就可以返回答题主界面。\n或连续点击四次“返回”按钮，强制返回答题主界面。", "注意", MessageBoxButtons.OK);
					}
					e.Cancel = true;
				}
				else
				{
					if (MessageBox.Show("确定要返回主界面吗？未保存的结果将会丢失！", "注意", MessageBoxButtons.YesNo) != DialogResult.Yes)
					{
						e.Cancel = true;
					}
					else
					{
						this.mytimer.Enabled = false;
					}
				}
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.mytimer != null)
			{
				this.mytimer.Dispose();
			}
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormFlow));
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new ToolStripStatusLabel();
			this.toolStripStatusLabel3 = new ToolStripStatusLabel();
			this.panel1 = new Panel();
			this.tabControl1 = new TabControl();
			this.tabPage1 = new TabPage();
			this.readOnlyRichTextBox1 = new ReadOnlyRichTextBox();
			this.btnBack = new Button();
			this.btnOpenDataPath = new Button();
			this.statusStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			base.SuspendLayout();
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel1,
				this.toolStripStatusLabel2,
				this.toolStripStatusLabel3
			});
			this.statusStrip1.Location = new Point(0, 271);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new Padding(1, 0, 19, 0);
			this.statusStrip1.Size = new Size(653, 25);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabel1.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.toolStripStatusLabel1.Font = new Font("黑体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new Size(126, 20);
			this.toolStripStatusLabel1.Text = "请及时保存文件";
			this.toolStripStatusLabel1.TextAlign = ContentAlignment.MiddleLeft;
			this.toolStripStatusLabel2.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new Size(71, 20);
			this.toolStripStatusLabel2.Text = "00:00:00";
			this.toolStripStatusLabel2.TextAlign = ContentAlignment.MiddleLeft;
			this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			this.toolStripStatusLabel3.Size = new Size(405, 20);
			this.toolStripStatusLabel3.Spring = true;
			this.toolStripStatusLabel3.TextAlign = ContentAlignment.MiddleLeft;
			this.panel1.Location = new Point(0, 0);
			this.panel1.Margin = new Padding(4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(181, 229);
			this.panel1.TabIndex = 1;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new Point(185, 0);
			this.tabControl1.Margin = new Padding(0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(468, 240);
			this.tabControl1.TabIndex = 2;
			this.tabPage1.Controls.Add(this.readOnlyRichTextBox1);
			this.tabPage1.Location = new Point(4, 25);
			this.tabPage1.Margin = new Padding(4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(4);
			this.tabPage1.Size = new Size(460, 211);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "操作提示";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.readOnlyRichTextBox1.Cursor = Cursors.Arrow;
			this.readOnlyRichTextBox1.Dock = DockStyle.Fill;
			this.readOnlyRichTextBox1.Location = new Point(4, 4);
			this.readOnlyRichTextBox1.Margin = new Padding(4);
			this.readOnlyRichTextBox1.Name = "readOnlyRichTextBox1";
			this.readOnlyRichTextBox1.ReadOnly = true;
			this.readOnlyRichTextBox1.Size = new Size(452, 203);
			this.readOnlyRichTextBox1.TabIndex = 0;
			this.readOnlyRichTextBox1.Text = "";
			this.btnBack.Cursor = Cursors.Hand;
			this.btnBack.Location = new Point(0, 236);
			this.btnBack.Margin = new Padding(4);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new Size(100, 29);
			this.btnBack.TabIndex = 3;
			this.btnBack.Text = "返回";
			this.btnBack.UseVisualStyleBackColor = true;
			this.btnOpenDataPath.AutoSize = true;
			this.btnOpenDataPath.Cursor = Cursors.Hand;
			this.btnOpenDataPath.Location = new Point(120, 236);
			this.btnOpenDataPath.Margin = new Padding(4);
			this.btnOpenDataPath.Name = "btnOpenDataPath";
			this.btnOpenDataPath.Size = new Size(148, 29);
			this.btnOpenDataPath.TabIndex = 4;
			this.btnOpenDataPath.Text = "打开考生目录";
			this.btnOpenDataPath.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(8f, 15f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(653, 296);
			base.Controls.Add(this.btnOpenDataPath);
			base.Controls.Add(this.btnBack);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.statusStrip1);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Margin = new Padding(4);
			base.MaximizeBox = false;
			base.Name = "FormFlow";
			this.Text = "操作提示";
			base.TopMost = true;
			base.FormClosing += new FormClosingEventHandler(this.FlowForm_FormClosing);
			base.Load += new EventHandler(this.FlowForm_Load);
			base.SizeChanged += new EventHandler(this.FlowForm_SizeChanged);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
