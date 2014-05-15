using ExamClientControlsLibrary.Properties;
using Qisi.Editor.Controls;
using Qisi.General;
using Qisi.General.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
namespace ExamClientControlsLibrary
{
	public class TestPaperPlayer : Control
	{
		public enum RunMode
		{
			Exam,
			CDExercise,
			CDSimulate,
			CDAnalysis,
			Player
		}
		private delegate void setControlTextCallBack(Control control, string text);
		private delegate void setControlColorCallBack(Control control, Color color);
		private delegate void setControlEnabledCallBack(Control control, bool enabled);
		private delegate void setControlVisibleCallBack(Control control, bool visible);
		private delegate void setPictureBoxImageCallBack(PictureBox picturebox, Bitmap bitmap);
		private delegate void performClickCallBack(Button button);
		private delegate void setPaperTreeImageKeyCallBack(bool isDone);
		private System.Timers.Timer timer;
		private Label clockLabel;
		private QisiTreeView paperTree;
		private FlowLayoutPanel mainPanel;
		private Label subjectTitleLabel;
		private Button submit;
		private VolumeControl volumeControl;
		private FLabelList formulaEditor;
		private Panel title;
		private Panel foot;
		private imeBar imeBar;
		private Label topPre;
		private Label topNext;
		private Label bottomPre;
		private Label bottomNext;
		private TestPaper testpaper;
		internal static string paperPath;
		internal static string stuDataPath;
		private string stuAnswerFile;
		internal static string answerPath;
		internal static MemoryIniFile answerINI;
		internal static string stdAnswerDir;
		internal static MemoryIniFile stdINI;
		internal static int examTotalTime;
		internal static int examTimeLeft;
		internal static int endTipTime;
		internal static int handinTime;
		internal static int tickCount;
		internal static string subject;
		internal static Size mainPanelSize;
		internal static TestPaperPlayer.RunMode mode;
		internal static Image stuBMP;
		internal static string stuInfo;
		internal static bool inOperate;
		public event EventHandler HandOn;
		public event MessageEventHandler QuestionChanged;
		public TestPaperPlayer(Size controlSize, List<string> information, string selectdeSubject, TestPaperPlayer.RunMode runMode, string unzippedPaperPath, string studentDataPath, string studentAnswerPath, Image studentPic, string keyName, string studentAnswerFileName, string stdAnswer = "", string stdDir = "")
		{
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.Selectable, false);
			this.DoubleBuffered = true;
			this.BackColor = Color.White;
			base.Size = controlSize;
			if (unzippedPaperPath == null || unzippedPaperPath == "")
			{
				throw new Exception("路径错误", new Exception("试卷路径为空"));
			}
			if (!unzippedPaperPath.EndsWith("\\"))
			{
				unzippedPaperPath += "\\";
			}
			if (studentAnswerPath == null || studentAnswerPath == "")
			{
				throw new Exception("路径错误", new Exception("答案保存路径为空"));
			}
			if (!studentAnswerPath.EndsWith("\\"))
			{
				studentAnswerPath += "\\";
			}
			if (studentDataPath == null || studentDataPath == "")
			{
				throw new Exception("路径错误", new Exception("考生目录路径为空"));
			}
			if (!studentDataPath.EndsWith("\\"))
			{
				studentDataPath += "\\";
			}
			if (!Directory.Exists(unzippedPaperPath))
			{
				throw new Exception("路径错误", new Exception("试卷路径不存在"));
			}
			TestPaperPlayer.mode = runMode;
			TestPaperPlayer.paperPath = unzippedPaperPath;
			TestPaperPlayer.stuDataPath = studentDataPath;
			TestPaperPlayer.answerPath = studentAnswerPath;
			TestPaperPlayer.answerINI = new MemoryIniFile();
			this.stuAnswerFile = studentAnswerFileName;
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDAnalysis)
			{
				if (!Directory.Exists(TestPaperPlayer.answerPath))
				{
					throw new Exception("路径错误", new Exception("答案保存路径不存在"));
				}
				if (!File.Exists(TestPaperPlayer.answerPath + this.stuAnswerFile))
				{
					throw new Exception("路径错误", new Exception("指定路径中答案不存在"));
				}
				TestPaperPlayer.answerINI.LoadFromEncodedFile(TestPaperPlayer.answerPath + this.stuAnswerFile);
				TestPaperPlayer.subject = TestPaperPlayer.answerINI.ReadValue("ANSWER", "SUBJECT", "");
			}
			else
			{
				if (!Directory.Exists(TestPaperPlayer.answerPath))
				{
					try
					{
						Directory.CreateDirectory(TestPaperPlayer.answerPath);
					}
					catch
					{
						throw new Exception("路径错误", new Exception("创建答案保存路径时出错"));
					}
				}
				TestPaperPlayer.subject = selectdeSubject;
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "SUBJECT", TestPaperPlayer.subject);
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "DoTimeLen", "0");
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "MENU", "1");
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "ReDo", "0");
			}
			TestPaperPlayer.stdINI = new MemoryIniFile();
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDAnalysis || TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDExercise)
			{
				if (stdAnswer != "" && File.Exists(stdAnswer))
				{
					TestPaperPlayer.stdINI.LoadFromFile(stdAnswer);
					if (stdDir == null || stdDir == "")
					{
						throw new Exception("路径错误", new Exception("考生目录路径为空"));
					}
					if (!stdDir.EndsWith("\\"))
					{
						stdDir += "\\";
					}
					TestPaperPlayer.stdAnswerDir = stdDir;
				}
			}
			int num = 180;
			int height = 270;
			int num2 = 50;
			int num3 = 30;
			int num4 = 40;
			int num5 = 40;
			int y = 10;
			Size size = new Size(100, 132);
			Panel panel = new Panel();
			panel.Padding = new Padding(0, 0, 0, 0);
			panel.Margin = new Padding(0, 0, 0, 0);
			panel.BackColor = Color.SkyBlue;
			panel.Size = new Size(num, height);
			panel.Location = new Point(0, 0);
			base.Controls.Add(panel);
			PictureBox pictureBox = new PictureBox();
			pictureBox.Padding = new Padding(0, 0, 0, 0);
			pictureBox.Margin = new Padding(0, 0, 0, 0);
			pictureBox.Location = new Point((num - size.Width) / 2, y);
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.BackColor = Color.Transparent;
			pictureBox.Size = size;
			if (studentPic != null)
			{
				pictureBox.Image = studentPic;
			}
			else
			{
				if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.Player)
				{
					pictureBox.Image = Resources.logo;
				}
				else
				{
					pictureBox.Image = Resources.noPic;
				}
			}
			pictureBox.Visible = true;
			panel.Controls.Add(pictureBox);
			TestPaperPlayer.stuBMP = pictureBox.Image;
			int num6 = 10;
			Label label = new Label();
			label.Font = new Font("黑体", 15f, FontStyle.Bold, GraphicsUnit.Pixel);
			label.Location = new Point(0, pictureBox.Bottom + num6);
			label.AutoSize = true;
			label.MaximumSize = new Size(num, 0);
			label.Visible = true;
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "TESTID", information[3]);
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "GENDER", information[1]);
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "STUDENTNAME", information[0]);
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "SEATNO", information[2]);
				label.Text = label.Text + "姓名：" + information[0] + "\r\n";
				label.Text = label.Text + "性别：" + information[1] + "\r\n";
				label.Text = label.Text + "座位号：" + information[2] + "\r\n";
				label.Text = string.Concat(new string[]
				{
					label.Text,
					keyName,
					"：",
					information[3],
					"\r\n"
				});
			}
			else
			{
				label.Text = label.Text + "姓名：" + TestPaperPlayer.answerINI.ReadValue("ANSWER", "STUDENTNAME", "") + "\r\n";
				label.Text = label.Text + "性别：" + TestPaperPlayer.answerINI.ReadValue("ANSWER", "GENDER", "") + "\r\n";
				label.Text = label.Text + "座位号：" + TestPaperPlayer.answerINI.ReadValue("ANSWER", "SEATNO", "") + "\r\n";
				label.Text = string.Concat(new string[]
				{
					label.Text,
					keyName,
					"：",
					TestPaperPlayer.answerINI.ReadValue("ANSWER", "TESTID", ""),
					"\r\n"
				});
			}
			label.Text = label.Text + "选考模块：" + TestPaperPlayer.subject;
			panel.Controls.Add(label);
			TestPaperPlayer.stuInfo = label.Text;
			Clock clock = new Clock();
			base.Controls.Add(clock);
			clock.Size = new Size(num2, num2);
			clock.Location = new Point(15, base.Height - num2);
			clock.Visible = true;
			this.clockLabel = new Label();
			this.clockLabel.Location = new Point(clock.Right, clock.Top);
			this.clockLabel.Size = new Size(num - clock.Right, num2);
			this.clockLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.clockLabel.Font = new Font("黑体", 20f, FontStyle.Bold, GraphicsUnit.Pixel);
			base.Controls.Add(this.clockLabel);
			this.timer = new System.Timers.Timer(500.0);
			this.timer.Elapsed += new ElapsedEventHandler(this.timer1_Tick);
			this.timer.AutoReset = true;
			this.submit = new Button();
			this.submit.BackColor = Color.White;
			this.submit.Size = new Size(num, num3);
			this.submit.Location = new Point(0, clock.Top - num3);
			this.submit.Font = new Font("微软雅黑", 18f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.submit.Cursor = Cursors.Hand;
			this.submit.TextAlign = ContentAlignment.MiddleCenter;
			base.Controls.Add(this.submit);
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDExercise || TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDAnalysis)
			{
				this.submit.Text = "退出";
				this.submit.Enabled = true;
			}
			else
			{
				if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.Player)
				{
					this.submit.Text = "交卷";
					this.submit.Enabled = true;
				}
				else
				{
					this.submit.Text = "交卷";
					this.submit.Enabled = false;
				}
			}
			this.submit.Click += new EventHandler(this.submit_Click);
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.Player)
			{
				Label label2 = new Label();
				label2.Font = new Font("微软雅黑", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
				label2.Text = "字体";
				label2.Location = new Point(0, this.submit.Top - num4);
				label2.Size = new Size(num4, num4);
				label2.AutoSize = false;
				label2.TextAlign = ContentAlignment.MiddleCenter;
				base.Controls.Add(label2);
				TrackBar trackBar = new TrackBar();
				trackBar.Location = new Point(label2.Right, label2.Top);
				trackBar.AutoSize = false;
				trackBar.Size = new Size(num - num4, num4);
				trackBar.Minimum = 0;
				trackBar.Maximum = 100;
				trackBar.Value = 50;
				trackBar.TickStyle = TickStyle.None;
				trackBar.Cursor = Cursors.Hand;
				base.Controls.Add(trackBar);
				Label label3 = new Label();
				label3.Font = new Font("微软雅黑", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
				label3.Text = "颜色";
				label3.Location = new Point(0, label2.Top - num4);
				label3.Size = new Size(num5, num5);
				label3.AutoSize = false;
				label3.TextAlign = ContentAlignment.MiddleCenter;
				base.Controls.Add(label3);
				int num7 = (num - num5) / 8;
				Color[] array = new Color[]
				{
					Color.FromArgb(255, 255, 255),
					Color.FromArgb(250, 249, 222),
					Color.FromArgb(255, 242, 226),
					Color.FromArgb(253, 230, 224),
					Color.FromArgb(227, 237, 205),
					Color.FromArgb(220, 226, 241),
					Color.FromArgb(233, 235, 254),
					Color.FromArgb(234, 234, 239)
				};
				for (int i = 0; i < array.Length; i++)
				{
					Label label4 = new Label();
					label4.Text = "";
					label4.BorderStyle = BorderStyle.FixedSingle;
					label4.BackColor = array[i];
					label4.Size = new Size(num7, num7);
					label4.Location = new Point(label3.Right + i * num7, label3.Top + label3.Height / 2 - num7 / 2);
					label4.Cursor = Cursors.Hand;
					base.Controls.Add(label4);
				}
			}
			this.paperTree = new QisiTreeView();
			this.paperTree.Location = new Point(0, panel.Bottom);
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.Player)
			{
				this.paperTree.Size = new Size(num, this.submit.Top - num4 - num4 - panel.Bottom);
			}
			else
			{
				this.paperTree.Size = new Size(num, this.submit.Top - panel.Bottom);
			}
			this.paperTree.ImageList = new ImageList();
			this.paperTree.ImageList.Images.Add("undo", Resources.undo);
			this.paperTree.ImageList.Images.Add("done", Resources.done);
			this.paperTree.Font = new Font("微软雅黑", 16f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.paperTree.ImeMode = ImeMode.Disable;
			this.paperTree.ShowPlusMinus = false;
			this.paperTree.ShowLines = false;
			this.paperTree.ShowRootLines = false;
			base.Controls.Add(this.paperTree);
			int x = 20;
			int num8 = 50;
			int height2 = 45;
			this.title = new Panel();
			this.title.Location = new Point(num, 0);
			this.title.Size = new Size(base.Width - num, height2);
			this.title.Paint += new PaintEventHandler(this.Title_Paint);
			this.subjectTitleLabel = new Label();
			this.subjectTitleLabel.Font = new Font("宋体", 28f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.subjectTitleLabel.BackColor = Color.Transparent;
			this.subjectTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
			this.subjectTitleLabel.AutoSize = true;
			this.subjectTitleLabel.Location = new Point(x, 0);
			this.title.Controls.Add(this.subjectTitleLabel);
			this.topNext = new Label();
			this.topNext.Text = "下一页";
			this.topNext.Enabled = false;
			this.topNext.Font = new Font("宋体", 20f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
			this.topNext.ForeColor = Color.Blue;
			this.topNext.BackColor = Color.Transparent;
			this.topNext.Visible = true;
			this.topNext.Cursor = Cursors.Hand;
			this.topNext.AutoSize = true;
			this.topNext.Click += new EventHandler(this.Next_Click);
			this.topPre = new Label();
			this.topPre.Text = "上一页";
			this.topPre.Enabled = false;
			this.topPre.Font = new Font("宋体", 20f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
			this.topPre.ForeColor = Color.Blue;
			this.topPre.BackColor = Color.Transparent;
			this.topPre.Visible = true;
			this.topPre.Cursor = Cursors.Hand;
			this.topPre.AutoSize = true;
			this.topPre.Click += new EventHandler(this.Pre_Click);
			this.title.Controls.Add(this.topNext);
			this.topNext.Location = new Point(this.title.Width - this.topNext.Width - num8, this.title.ClientSize.Height / 2 - this.topNext.Height / 2);
			this.title.Controls.Add(this.topPre);
			this.topPre.Location = new Point(this.topNext.Left - this.topPre.Width, this.title.ClientSize.Height / 2 - this.topPre.Height / 2);
			base.Controls.Add(this.title);
			int num9 = 20;
			this.foot = new Panel();
			base.Controls.Add(this.foot);
			this.foot.Location = new Point(num, base.Height - num9);
			this.foot.Size = new Size(base.Width - num, num9);
			this.foot.Paint += new PaintEventHandler(this.Title_Paint);
			this.bottomNext = new Label();
			this.bottomNext.Text = "下一页";
			this.bottomNext.Font = new Font("宋体", 16f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
			this.bottomNext.ForeColor = Color.Blue;
			this.bottomNext.BackColor = Color.Transparent;
			this.bottomNext.Visible = true;
			this.bottomNext.Cursor = Cursors.Hand;
			this.bottomNext.AutoSize = true;
			this.bottomNext.Click += new EventHandler(this.Next_Click);
			this.bottomPre = new Label();
			this.bottomPre.Text = "上一页";
			this.bottomPre.Enabled = false;
			this.bottomPre.Font = new Font("宋体", 16f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
			this.bottomPre.ForeColor = Color.Blue;
			this.bottomPre.BackColor = Color.Transparent;
			this.bottomPre.Visible = true;
			this.bottomPre.Cursor = Cursors.Hand;
			this.bottomPre.AutoSize = true;
			this.bottomPre.Click += new EventHandler(this.Pre_Click);
			this.foot.Controls.Add(this.bottomNext);
			this.bottomNext.Location = new Point(this.foot.Width - this.bottomNext.Width - num8, this.foot.ClientSize.Height / 2 - this.bottomNext.Height / 2);
			this.foot.Controls.Add(this.bottomPre);
			this.bottomPre.Location = new Point(this.bottomNext.Left - this.bottomPre.Width, this.foot.ClientSize.Height / 2 - this.bottomPre.Height / 2);
			this.imeBar = new imeBar();
			this.imeBar.Height = this.foot.ClientSize.Height;
			this.imeBar.Width = Math.Max(200, this.bottomPre.Left - (base.Width - num) / 2);
			this.imeBar.Top = 1;
			this.imeBar.Left = this.bottomPre.Left - this.imeBar.Width - 20;
			this.imeBar.Visible = true;
			this.foot.Controls.Add(this.imeBar);
			this.volumeControl = new VolumeControl();
			this.volumeControl.Top = 0;
			this.volumeControl.Left = 30;
			this.volumeControl.Height = this.foot.ClientSize.Height;
			this.volumeControl.Visible = false;
			this.foot.Controls.Add(this.volumeControl);
			int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
			TestPaperPlayer.mainPanelSize = new Size(controlSize.Width - num - verticalScrollBarWidth, this.foot.Top - this.title.Bottom);
			this.mainPanel = new FlowLayoutPanel();
			this.mainPanel.Margin = new Padding(0, 0, 0, 0);
			this.mainPanel.Padding = new Padding(0, 0, 0, 0);
			this.mainPanel.Size = new Size(controlSize.Width - num, this.foot.Top - this.title.Bottom);
			this.mainPanel.AutoScroll = true;
			XmlDocument xmlDocument = new XmlDocument();
			string xml = File.ReadAllText(TestPaperPlayer.paperPath + "testpaper.xml", Encoding.UTF8);
			xmlDocument.LoadXml(xml);
			this.testpaper = new TestPaper(xmlDocument.DocumentElement);
			this.testpaper.IndexChanged += new Page.IndexEventHandler(this.testpaper_IndexChanged);
			this.testpaper.IsDown += new Question.BoolEventHandler(this.testpaper_isDown);
			this.testpaper.ShowWindow += new EventHandler(this.testpaper_ShowWindow);
			this.testpaper.HideWindow += new EventHandler(this.testpaper_HideWindow);
			TestPaperPlayer.examTotalTime = this.testpaper.TotalTime;
			TestPaperPlayer.examTimeLeft = TestPaperPlayer.examTotalTime;
			TestPaperPlayer.handinTime = this.testpaper.HandinTime;
			TestPaperPlayer.endTipTime = this.testpaper.EndTipTime;
			TestPaperPlayer.tickCount = 0;
			if (this.testpaper.formulaEditType != "")
			{
				this.formulaEditor = new FLabelList(this.testpaper.formulaEditType, TestPaperPlayer.mainPanelSize.Width, true);
				this.formulaEditor.Visible = false;
				this.formulaEditor.Location = new Point(num, this.title.Bottom);
				this.formulaEditor.VisibleChanged += new EventHandler(this.fe_VisibleChanged);
				this.formulaEditor.SizeChanged += new EventHandler(this.fe_SizeChanged);
				this.formulaEditor.Visible = this.testpaper.Currentpanel.ShowFormuleEditor;
				base.Controls.Add(this.formulaEditor);
			}
			else
			{
				this.formulaEditor = null;
			}
			this.mainPanel.Controls.Add(this.testpaper.FirstPanel);
			this.volumeControl.Visible = this.testpaper.Currentpanel.ShowVolumnControl;
			base.Controls.Add(this.mainPanel);
			this.mainPanel.Location = new Point(num, this.title.Bottom);
			this.subjectTitleLabel.Text = this.testpaper.Currentpanel.SubjectName;
			for (int i = 0; i < this.testpaper.TestPaperTreeNode.Nodes.Count; i++)
			{
				this.paperTree.Nodes.Add(this.testpaper.TestPaperTreeNode.Nodes[i]);
			}
			this.paperTree.ExpandAll();
			this.paperTree.AfterSelect += new TreeViewEventHandler(this.PaperTree_AfterSelect);
			this.bottomNext.Enabled = this.testpaper.HasNext;
			this.topNext.Enabled = this.testpaper.HasNext;
			this.bottomPre.Enabled = this.testpaper.HasPrev;
			this.topPre.Enabled = this.testpaper.HasPrev;
			TestPaperPlayer.inOperate = false;
			base.TabStop = false;
			this.disableTab(this);
			this.Refresh();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.timer != null)
				{
					this.timer.Stop();
					this.timer.Dispose();
					this.timer = null;
				}
				if (TestPaperPlayer.answerINI != null)
				{
					TestPaperPlayer.answerINI.Dispose();
					TestPaperPlayer.answerINI = null;
				}
				if (TestPaperPlayer.stdINI != null)
				{
					TestPaperPlayer.stdINI.Dispose();
					TestPaperPlayer.stdINI = null;
				}
				if (this.testpaper != null)
				{
					this.testpaper.Dispose();
					this.testpaper = null;
				}
				if (TestPaperPlayer.stuBMP != null)
				{
					TestPaperPlayer.stuBMP.Dispose();
					TestPaperPlayer.stuBMP = null;
				}
				if (this.clockLabel != null)
				{
					this.clockLabel.Dispose();
					this.clockLabel = null;
				}
				if (this.paperTree != null)
				{
					this.paperTree.Dispose();
					this.paperTree = null;
				}
				if (this.submit != null)
				{
					this.submit.Dispose();
					this.submit = null;
				}
				if (this.subjectTitleLabel != null)
				{
					this.subjectTitleLabel.Dispose();
					this.subjectTitleLabel = null;
				}
				if (this.volumeControl != null)
				{
					this.volumeControl.Dispose();
					this.volumeControl = null;
				}
				if (this.formulaEditor != null)
				{
					this.formulaEditor.Dispose();
					this.formulaEditor = null;
				}
				if (this.imeBar != null)
				{
					this.imeBar.Dispose();
					this.imeBar = null;
				}
				if (this.topPre != null)
				{
					this.topPre.Dispose();
					this.topPre = null;
				}
				if (this.topNext != null)
				{
					this.topNext.Dispose();
					this.topNext = null;
				}
				if (this.bottomPre != null)
				{
					this.bottomPre.Dispose();
					this.bottomPre = null;
				}
				if (this.bottomNext != null)
				{
					this.bottomNext.Dispose();
					this.bottomNext = null;
				}
				if (this.title != null)
				{
					this.title.Dispose();
					this.title = null;
				}
				if (this.foot != null)
				{
					this.foot.Dispose();
					this.foot = null;
				}
				if (this.mainPanel != null)
				{
					this.mainPanel.Dispose();
					this.mainPanel = null;
				}
			}
			TestPaperPlayer.paperPath = "";
			TestPaperPlayer.stuDataPath = "";
			this.stuAnswerFile = "";
			TestPaperPlayer.answerPath = "";
			TestPaperPlayer.stdAnswerDir = "";
			TestPaperPlayer.examTotalTime = 0;
			TestPaperPlayer.examTimeLeft = 0;
			TestPaperPlayer.endTipTime = 0;
			TestPaperPlayer.handinTime = 0;
			TestPaperPlayer.tickCount = 0;
			TestPaperPlayer.subject = "";
			TestPaperPlayer.stuInfo = "";
			base.Dispose(disposing);
		}
		~TestPaperPlayer()
		{
			this.Dispose(false);
		}
		private void disableTab(Control control)
		{
			foreach (Control control2 in control.Controls)
			{
				control2.TabStop = false;
				this.disableTab(control2);
			}
		}
		private void fe_VisibleChanged(object sender, EventArgs e)
		{
			if (this.formulaEditor != null && this.formulaEditor.Visible)
			{
				this.mainPanel.Location = new Point(this.mainPanel.Left, this.formulaEditor.Bottom);
				this.mainPanel.Size = new Size(this.mainPanel.Size.Width, this.foot.Top - this.formulaEditor.Bottom);
			}
			else
			{
				this.mainPanel.Location = new Point(this.mainPanel.Left, this.title.Bottom);
				this.mainPanel.Size = new Size(this.mainPanel.Size.Width, this.foot.Top - this.title.Bottom);
			}
		}
		private void fe_SizeChanged(object sender, EventArgs e)
		{
			if (this.formulaEditor != null)
			{
				this.mainPanel.Location = new Point(this.mainPanel.Left, this.formulaEditor.Bottom);
				this.mainPanel.Size = new Size(this.mainPanel.Size.Width, this.foot.Top - this.formulaEditor.Bottom);
			}
		}
		private void PaperTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			e.Cancel = true;
		}
		private void Title_Paint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Color white = Color.White;
			Color green = Color.Green;
			Brush brush = new LinearGradientBrush(((Panel)sender).ClientRectangle, white, green, LinearGradientMode.ForwardDiagonal);
			graphics.FillRectangle(brush, ((Panel)sender).ClientRectangle);
		}
		private void PaperTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			int num = 0;
			int num2;
			if (e.Node.Level == 0)
			{
				num2 = Convert.ToInt32(e.Node.Nodes[0].Name) - 1;
			}
			else
			{
				num2 = Convert.ToInt32(e.Node.Name);
				if (e.Node.PrevNode != null)
				{
					TreeNode treeNode = e.Node;
					while (treeNode.PrevNode != null && treeNode.PrevNode.Name == e.Node.Name)
					{
						num++;
						treeNode = treeNode.PrevNode;
					}
				}
			}
			if (num2 != this.testpaper.CurrentPageIndex)
			{
				this.mainPanel.Controls.Clear();
				this.mainPanel.Controls.Add(this.testpaper.getPanel(num2, num));
				this.volumeControl.Visible = this.testpaper.Currentpanel.ShowVolumnControl;
				if (this.formulaEditor != null)
				{
					this.formulaEditor.Visible = this.testpaper.Currentpanel.ShowFormuleEditor;
				}
			}
			else
			{
				this.testpaper.getPanel(num2, num);
			}
			this.bottomNext.Enabled = this.testpaper.HasNext;
			this.topNext.Enabled = this.testpaper.HasNext;
			this.bottomPre.Enabled = this.testpaper.HasPrev;
			this.topPre.Enabled = this.testpaper.HasPrev;
			this.subjectTitleLabel.Text = this.testpaper.Currentpanel.SubjectName;
			if (this.QuestionChanged != null)
			{
				this.QuestionChanged(this, new MessageEventArgs(this.paperTree.SelectedNode.Text));
			}
		}
		private void testpaper_IndexChanged(object sender, IndexEventArgs e)
		{
			if (this.paperTree != null)
			{
				TreeNode[] array = this.paperTree.Nodes.Find(this.testpaper.CurrentPageIndex.ToString(), true);
				if (array.Length > e.Index && this.paperTree.SelectedNode != array[e.Index])
				{
					this.paperTree.SelectedNode = array[e.Index];
				}
			}
		}
		private void testpaper_isDown(object sender, BoolEventArgs e)
		{
			this.setPaperTreeImageKey(e.Message);
		}
		private void timer1_Tick(object sender, ElapsedEventArgs e)
		{
			TestPaperPlayer.tickCount++;
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDAnalysis || TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDExercise)
			{
				int num = TestPaperPlayer.tickCount / 2 % 60;
				int num2 = (TestPaperPlayer.tickCount / 2 - num) / 60 % 60;
				int num3 = ((TestPaperPlayer.tickCount / 2 - num) / 60 - num2) / 60;
				this.setControlText(this.clockLabel, string.Concat(new string[]
				{
					num3.ToString().PadLeft(2, '0'),
					":",
					num2.ToString().PadLeft(2, '0'),
					":",
					num.ToString().PadLeft(2, '0')
				}));
				if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDExercise)
				{
					TestPaperPlayer.answerINI.WriteValue("ANSWER", "DoTimeLen", TestPaperPlayer.tickCount.ToString());
					TestPaperPlayer.answerINI.SaveToEncryptedFile(TestPaperPlayer.answerPath + this.stuAnswerFile);
				}
			}
			else
			{
				if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDSimulate || TestPaperPlayer.mode == TestPaperPlayer.RunMode.Exam)
				{
					TestPaperPlayer.tickCount %= 2;
					if (TestPaperPlayer.examTimeLeft <= 0 && TestPaperPlayer.tickCount == 1)
					{
						int num = TestPaperPlayer.examTimeLeft % 60;
						int num2 = (TestPaperPlayer.examTimeLeft - num) / 60 % 60;
						int num3 = ((TestPaperPlayer.examTimeLeft - num) / 60 - num2) / 60;
						this.setControlText(this.clockLabel, string.Concat(new string[]
						{
							num3.ToString().PadLeft(2, '0'),
							":",
							num2.ToString().PadLeft(2, '0'),
							":",
							num.ToString().PadLeft(2, '0')
						}));
						TestPaperPlayer.answerINI.WriteValue("ANSWER", "DoTimeLen", (TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft).ToString());
						TestPaperPlayer.answerINI.SaveToEncryptedFile(TestPaperPlayer.answerPath + this.stuAnswerFile);
						this.PerformClick(this.submit);
					}
					else
					{
						if (TestPaperPlayer.examTimeLeft > TestPaperPlayer.endTipTime && TestPaperPlayer.tickCount == 1)
						{
							int num = TestPaperPlayer.examTimeLeft % 60;
							int num2 = (TestPaperPlayer.examTimeLeft - num) / 60 % 60;
							int num3 = ((TestPaperPlayer.examTimeLeft - num) / 60 - num2) / 60;
							this.setControlText(this.clockLabel, string.Concat(new string[]
							{
								num3.ToString().PadLeft(2, '0'),
								":",
								num2.ToString().PadLeft(2, '0'),
								":",
								num.ToString().PadLeft(2, '0')
							}));
							TestPaperPlayer.examTimeLeft--;
							TestPaperPlayer.answerINI.WriteValue("ANSWER", "DoTimeLen", (TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft).ToString());
							TestPaperPlayer.answerINI.SaveToEncryptedFile(TestPaperPlayer.answerPath + this.stuAnswerFile);
						}
						else
						{
							if (TestPaperPlayer.examTimeLeft <= TestPaperPlayer.endTipTime && TestPaperPlayer.tickCount == 1)
							{
								int num = TestPaperPlayer.examTimeLeft % 60;
								int num2 = (TestPaperPlayer.examTimeLeft - num) / 60 % 60;
								int num3 = ((TestPaperPlayer.examTimeLeft - num) / 60 - num2) / 60;
								this.setControlColor(this.clockLabel, Color.Red);
								this.setControlText(this.clockLabel, string.Concat(new string[]
								{
									num3.ToString().PadLeft(2, '0'),
									":",
									num2.ToString().PadLeft(2, '0'),
									":",
									num.ToString().PadLeft(2, '0')
								}));
								TestPaperPlayer.examTimeLeft--;
								TestPaperPlayer.answerINI.WriteValue("ANSWER", "DoTimeLen", (TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft).ToString());
								TestPaperPlayer.answerINI.SaveToEncryptedFile(TestPaperPlayer.answerPath + this.stuAnswerFile);
							}
							else
							{
								if (TestPaperPlayer.examTimeLeft <= TestPaperPlayer.endTipTime && TestPaperPlayer.tickCount == 0)
								{
									this.setControlText(this.clockLabel, "");
								}
							}
						}
					}
					if (TestPaperPlayer.examTimeLeft <= TestPaperPlayer.handinTime)
					{
						this.setControlEnabled(this.submit, true);
					}
				}
			}
		}
		private void submit_Click(object sender, EventArgs e)
		{
			if (FlatMessageBox.Show(this, "确定要" + this.submit.Text + "吗?", "提示", FlatMessageBox.KeysButtons.OKCancel, FlatMessageBox.KeysIcon.Information) == DialogResult.OK)
			{
				while (TestPaperPlayer.inOperate)
				{
					Thread.Sleep(500);
				}
				this.timer.Stop();
				if (this.HandOn != null)
				{
					this.HandOn(this, new EventArgs());
				}
			}
		}
		private void setPaperTreeImageKey(bool isDone)
		{
			if (this.paperTree.InvokeRequired)
			{
				TestPaperPlayer.setPaperTreeImageKeyCallBack method = new TestPaperPlayer.setPaperTreeImageKeyCallBack(this.setPaperTreeImageKey);
				this.paperTree.Invoke(method, new object[]
				{
					isDone
				});
			}
			else
			{
				if (isDone)
				{
					this.paperTree.SelectedNode.ImageKey = "done";
					this.paperTree.SelectedNode.SelectedImageKey = "done";
				}
				else
				{
					this.paperTree.SelectedNode.ImageKey = "undo";
					this.paperTree.SelectedNode.SelectedImageKey = "undo";
				}
			}
		}
		private void setControlText(Control control, string text)
		{
			if (control.InvokeRequired)
			{
				TestPaperPlayer.setControlTextCallBack method = new TestPaperPlayer.setControlTextCallBack(this.setControlText);
				control.BeginInvoke(method, new object[]
				{
					control,
					text
				});
			}
			else
			{
				control.Text = text;
			}
		}
		private void PerformClick(Button b)
		{
			if (b.InvokeRequired)
			{
				TestPaperPlayer.performClickCallBack method = new TestPaperPlayer.performClickCallBack(this.PerformClick);
				b.BeginInvoke(method, new object[]
				{
					b
				});
			}
			else
			{
				b.PerformClick();
			}
		}
		private void setControlEnabled(Control c, bool b)
		{
			if (c.InvokeRequired)
			{
				TestPaperPlayer.setControlEnabledCallBack method = new TestPaperPlayer.setControlEnabledCallBack(this.setControlEnabled);
				c.BeginInvoke(method, new object[]
				{
					c,
					b
				});
			}
			else
			{
				c.Enabled = b;
				c.ForeColor = Color.Black;
			}
		}
		private void setControlColor(Control control, Color color)
		{
			if (control.InvokeRequired)
			{
				TestPaperPlayer.setControlColorCallBack method = new TestPaperPlayer.setControlColorCallBack(this.setControlColor);
				control.BeginInvoke(method, new object[]
				{
					control,
					color
				});
			}
			else
			{
				control.ForeColor = color;
			}
		}
		private void setPictureBoxImage(PictureBox pic, Bitmap bmp)
		{
			if (pic.InvokeRequired)
			{
				TestPaperPlayer.setPictureBoxImageCallBack method = new TestPaperPlayer.setPictureBoxImageCallBack(this.setPictureBoxImage);
				pic.BeginInvoke(method, new object[]
				{
					pic,
					bmp
				});
			}
			else
			{
				pic.Image = bmp;
				pic.Refresh();
			}
		}
		private void Next_Click(object sender, EventArgs e)
		{
			if (this.paperTree.SelectedNode == null)
			{
				this.paperTree.SelectedNode = this.paperTree.Nodes[0];
			}
			else
			{
				if (this.paperTree.SelectedNode.Level == 0)
				{
					if (this.paperTree.SelectedNode.Nodes.Count > 0)
					{
						this.paperTree.SelectedNode = this.paperTree.SelectedNode.Nodes[0];
					}
				}
				else
				{
					if (this.paperTree.SelectedNode.NextNode != null)
					{
						TreeNode treeNode = this.paperTree.SelectedNode;
						while (treeNode.NextNode != null && treeNode.NextNode.Name == treeNode.Name)
						{
							treeNode = treeNode.NextNode;
						}
						if (treeNode.NextNode != null)
						{
							treeNode = treeNode.NextNode;
						}
						else
						{
							if (treeNode.Parent != null && treeNode.Parent.NextNode != null)
							{
								treeNode = treeNode.Parent.NextNode;
							}
						}
						this.paperTree.SelectedNode = treeNode;
					}
					else
					{
						if (this.paperTree.SelectedNode.Parent.NextNode != null)
						{
							this.paperTree.SelectedNode = this.paperTree.SelectedNode.Parent.NextNode;
						}
					}
				}
			}
		}
		private void Pre_Click(object sender, EventArgs e)
		{
			if (this.paperTree.SelectedNode == null)
			{
				this.paperTree.SelectedNode = this.paperTree.Nodes[0];
			}
			else
			{
				if (this.paperTree.SelectedNode.Level == 0)
				{
					if (this.paperTree.SelectedNode.PrevNode != null && this.paperTree.SelectedNode.PrevNode.Nodes.Count > 0)
					{
						this.paperTree.SelectedNode = this.paperTree.SelectedNode.PrevNode.LastNode;
					}
					else
					{
						if (this.paperTree.SelectedNode.PrevNode != null)
						{
							this.paperTree.SelectedNode = this.paperTree.SelectedNode.PrevNode;
						}
						else
						{
							this.mainPanel.Controls.Clear();
							this.mainPanel.Controls.Add(this.testpaper.getPanel(0, 0));
							this.bottomNext.Enabled = this.testpaper.HasNext;
							this.topNext.Enabled = this.testpaper.HasNext;
							this.bottomPre.Enabled = this.testpaper.HasPrev;
							this.topPre.Enabled = this.testpaper.HasPrev;
							this.subjectTitleLabel.Text = this.testpaper.Currentpanel.SubjectName;
						}
					}
				}
				else
				{
					if (this.paperTree.SelectedNode.PrevNode != null)
					{
						TreeNode treeNode = this.paperTree.SelectedNode;
						while (treeNode.PrevNode != null && treeNode.PrevNode.Name == treeNode.Name)
						{
							treeNode = treeNode.PrevNode;
						}
						if (treeNode.PrevNode != null)
						{
							treeNode = treeNode.PrevNode;
						}
						else
						{
							if (treeNode.Parent != null)
							{
								treeNode = treeNode.Parent;
							}
						}
						this.paperTree.SelectedNode = treeNode;
					}
					else
					{
						if (this.paperTree.SelectedNode.Parent != null)
						{
							this.paperTree.SelectedNode = this.paperTree.SelectedNode.Parent;
						}
					}
				}
			}
		}
		public void StartCounting()
		{
			this.timer.Start();
			this.testpaper.Play();
		}
		public void StopCounting()
		{
			this.timer.Stop();
		}
		private void testpaper_ShowWindow(object sender, EventArgs e)
		{
			if (base.Parent != null)
			{
				this.setControlVisible(base.Parent, true);
			}
		}
		private void setControlVisible(Control control, bool visible)
		{
			if (control.InvokeRequired)
			{
				TestPaperPlayer.setControlVisibleCallBack method = new TestPaperPlayer.setControlVisibleCallBack(this.setControlVisible);
				base.Parent.Invoke(method, new object[]
				{
					control,
					visible
				});
			}
			else
			{
				control.Visible = visible;
			}
		}
		private void testpaper_HideWindow(object sender, EventArgs e)
		{
			base.Parent.Hide();
		}
	}
}
