using Qisi.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
namespace ExamClientControlsLibrary
{
	internal class TestPaper : IDisposable
	{
		internal static string id;
		private XmlNode testpaperNode;
		private string testpaperName;
		private string subject;
		private int handintime;
		private int totaltime;
		private int endtiptime;
		private string lockWin;
		public string formulaEditType;
		private List<Page> pageList;
		private int currentPageindex;
		internal static bool allowExplorer;
		private TreeNode paperTreeTopNode;
		internal static int questionCount;
		private int pageChangeTime;
		private Thread t;
		private bool threadstarted;
		public event EventHandler ShowWindow;
		public event EventHandler HideWindow;
		public event Page.IndexEventHandler IndexChanged;
		public event Question.BoolEventHandler IsDown;
		internal int TotalTime
		{
			get
			{
				return this.totaltime;
			}
		}
		internal int HandinTime
		{
			get
			{
				return this.handintime;
			}
		}
		internal int EndTipTime
		{
			get
			{
				return this.endtiptime;
			}
		}
		internal TreeNode TestPaperTreeNode
		{
			get
			{
				return this.paperTreeTopNode;
			}
		}
		internal FlowLayoutPanel FirstPanel
		{
			get
			{
				this.pageList[this.currentPageindex].ResetIndexFont();
				this.currentPageindex = 0;
				if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
				{
					this.pageChangeTime = TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft;
				}
				this.pageList[this.currentPageindex].SetIndexFont(0);
				return this.pageList[this.currentPageindex].PagePanel;
			}
		}
		internal Page Currentpanel
		{
			get
			{
				return this.pageList[this.currentPageindex];
			}
		}
		internal bool HasPrev
		{
			get
			{
				return this.currentPageindex != 0;
			}
		}
		internal bool HasNext
		{
			get
			{
				return this.currentPageindex != this.pageList.Count - 1;
			}
		}
		internal int CurrentPageIndex
		{
			get
			{
				return this.currentPageindex;
			}
		}
		private void releaseControl(Control control)
		{
			foreach (Control control2 in control.Controls)
			{
				this.releaseControl(control2);
				control2.Dispose();
			}
			control.Dispose();
			control = null;
		}
		internal TestPaper(XmlNode xmlnode)
		{
			this.testpaperNode = xmlnode;
			if (this.testpaperNode.Attributes["name"] != null)
			{
				this.testpaperName = this.testpaperNode.Attributes["name"].Value;
			}
			else
			{
				this.testpaperName = "";
			}
			if (this.testpaperNode.Attributes["subject"] != null)
			{
				this.subject = this.testpaperNode.Attributes["subject"].Value;
			}
			else
			{
				this.subject = "";
			}
			if (this.testpaperNode.Attributes["id"] != null)
			{
				TestPaper.id = this.testpaperNode.Attributes["id"].Value;
			}
			else
			{
				TestPaper.id = "";
			}
			if (this.testpaperNode.Attributes["handintime"] != null)
			{
				string[] array = this.testpaperNode.Attributes["handintime"].Value.Split(new char[]
				{
					':'
				});
				this.handintime = 0;
				for (int i = 0; i < array.Length; i++)
				{
					int num = 1;
					for (int j = 0; j < i; j++)
					{
						num *= 60;
					}
					this.handintime += num * Convert.ToInt32(array[array.Length - i - 1]);
				}
			}
			else
			{
				this.handintime = 0;
			}
			if (this.testpaperNode.Attributes["endtiptime"] != null)
			{
				string[] array = this.testpaperNode.Attributes["endtiptime"].Value.Split(new char[]
				{
					':'
				});
				this.endtiptime = 0;
				for (int i = 0; i < array.Length; i++)
				{
					int num = 1;
					for (int j = 0; j < i; j++)
					{
						num *= 60;
					}
					this.endtiptime += num * Convert.ToInt32(array[array.Length - i - 1]);
				}
			}
			else
			{
				this.endtiptime = 0;
			}
			if (this.testpaperNode.Attributes["totaltime"] != null)
			{
				string[] array = this.testpaperNode.Attributes["totaltime"].Value.Split(new char[]
				{
					':'
				});
				this.totaltime = 0;
				for (int i = 0; i < array.Length; i++)
				{
					int num = 1;
					for (int j = 0; j < i; j++)
					{
						num *= 60;
					}
					this.totaltime += num * Convert.ToInt32(array[array.Length - i - 1]);
				}
			}
			else
			{
				this.totaltime = 0;
			}
			if (this.testpaperNode.Attributes["lockwinds"] != null)
			{
				this.lockWin = this.testpaperNode.Attributes["lockwinds"].Value;
			}
			else
			{
				this.lockWin = "";
			}
			if (this.testpaperNode.Attributes["fetype"] != null)
			{
				this.formulaEditType = this.testpaperNode.Attributes["fetype"].Value;
			}
			else
			{
				this.formulaEditType = "";
			}
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "PAPERID", TestPaper.id);
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "DoTime", this.totaltime);
				TestPaperPlayer.answerINI.WriteValue("ANSWER", "Course", this.subject);
			}
			TestPaper.questionCount = 0;
			this.pageList = new List<Page>();
			XmlNodeList xmlNodeList = this.testpaperNode.SelectNodes("part");
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				string a;
				if (xmlNode.Attributes["subject"] != null)
				{
					a = xmlNode.Attributes["subject"].Value;
				}
				else
				{
					a = "";
				}
				if (a == TestPaperPlayer.subject || a == "" || TestPaperPlayer.subject == "")
				{
					Part part = new Part(xmlNode);
					this.pageList.AddRange(part.PartPageList);
				}
			}
			int num2 = 1;
			this.paperTreeTopNode = new TreeNode();
			for (int i = 0; i < this.pageList.Count; i++)
			{
				int num3 = num2;
				this.pageList[i].IndexChanged += new Page.IndexEventHandler(this.TestPaper_IndexChanged);
				this.pageList[i].IsDown += new Page.BoolIndexEventHandler(this.TestPaper_isDown);
				this.pageList[i].setQuestionIndex(ref num2);
				this.pageList[i].HideWindow += new EventHandler(this.TestPaper_HideWindow);
				this.pageList[i].ShowWindow += new EventHandler(this.TestPaper_ShowWindow);
				if (!this.paperTreeTopNode.Nodes.ContainsKey(this.pageList[i].SubjectName))
				{
					this.paperTreeTopNode.Nodes.Add(this.pageList[i].SubjectName, this.pageList[i].SubjectName, "undo", "undo");
				}
				if (this.pageList[i].IsQuestion)
				{
					for (int j = num3; j < num2; j++)
					{
						this.paperTreeTopNode.Nodes[this.pageList[i].SubjectName].Nodes.Add(i.ToString(), "第" + j.ToString().PadLeft(2, '0') + "题", "undo");
					}
				}
			}
			this.currentPageindex = 0;
			TestPaper.allowExplorer = false;
			this.pageChangeTime = 0;
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				TestPaperPlayer.answerINI.WriteValue("Answer", "NUM", (num2 - 1).ToString());
			}
			this.t = new Thread(new ThreadStart(this.closeWin));
			this.t.Start();
			this.threadstarted = true;
			foreach (Page current in this.pageList)
			{
				current.PagePanel.TabStop = false;
				this.disableTab(current.PagePanel);
			}
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (Page current in this.pageList)
				{
					current.Dispose();
				}
			}
			for (int i = 0; i < this.pageList.Count; i++)
			{
				this.pageList[i] = null;
			}
			this.pageList = null;
			this.threadstarted = false;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~TestPaper()
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
		internal void FreshTree()
		{
			foreach (Page current in this.pageList)
			{
				current.FreshTree();
			}
		}
		private void closeWin()
		{
			while (this.threadstarted)
			{
				if (TestPaper.allowExplorer)
				{
					NativeMethods.ShutdownForms(this.lockWin, "考生目录");
				}
				else
				{
					NativeMethods.ShutdownForms(this.lockWin);
				}
				Thread.Sleep(2000);
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (TestPaper.allowExplorer)
			{
				NativeMethods.ShutdownForms(this.lockWin, "考生目录");
			}
			else
			{
				NativeMethods.ShutdownForms(this.lockWin);
			}
		}
		internal FlowLayoutPanel getPanel(int pageIndex, int questionIndex)
		{
			string text = "";
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				TestPaperPlayer.answerINI.ReadValue("LOG", "PAGE_" + this.CurrentPageIndex.ToString(), text);
				TestPaperPlayer.answerINI.WriteValue("LOG", "PAGE_" + this.CurrentPageIndex.ToString(), string.Concat(new string[]
				{
					text,
					"[",
					this.pageChangeTime.ToString(),
					"-",
					(TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft).ToString(),
					"]"
				}));
			}
			this.pageList[this.currentPageindex].ResetIndexFont();
			this.currentPageindex = pageIndex;
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				this.pageChangeTime = TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft;
			}
			this.pageList[pageIndex].SetIndexFont(questionIndex);
			return this.pageList[pageIndex].PagePanel;
		}
		private void TestPaper_IndexChanged(object sender, IndexEventArgs e)
		{
			this.IndexChanged(this, e);
		}
		private void TestPaper_isDown(object sender, BoolIndexEventArgs e)
		{
			if (this.IsDown != null)
			{
				this.IsDown(this, new BoolEventArgs(e.Message));
			}
		}
		[DllImport("winmm.dll")]
		private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, int hwndCallback);
		internal void Play()
		{
			if (this.testpaperNode.Attributes["audio"] != null)
			{
				TestPaper.Play(Path.Combine(TestPaperPlayer.paperPath, this.testpaperNode.Attributes["audio"].Value), false);
			}
		}
		private static void Play(string MP3_FileName, bool Repeat)
		{
			TestPaper.mciSendString("open \"" + MP3_FileName + "\" type mpegvideo alias MediaFile", null, 0, 0);
			TestPaper.mciSendString("play MediaFile" + (Repeat ? " repeat" : string.Empty), null, 0, 0);
		}
		private void TestPaper_ShowWindow(object sender, EventArgs e)
		{
			if (this.ShowWindow != null)
			{
				this.ShowWindow(sender, e);
			}
		}
		private void TestPaper_HideWindow(object sender, EventArgs e)
		{
			if (this.HideWindow != null)
			{
				this.HideWindow(sender, e);
			}
		}
	}
}
