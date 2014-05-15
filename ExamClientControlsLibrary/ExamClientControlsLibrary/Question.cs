using Qisi.Editor.Controls;
using Qisi.General;
using System;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Xml;
namespace ExamClientControlsLibrary
{
	public class Question
	{
		public delegate void BoolEventHandler(object sender, BoolEventArgs e);
		private int indexlabelfontsize = 12;
		private string id;
		private string answerid;
		private string answer;
		private Label indexLabel;
		private static int questionChangeTime;
		private SuperBox superbox;
		public event EventHandler IndexChanged;
		public event EventHandler HideWindow;
		public event EventHandler ShowWindow;
		public event Question.BoolEventHandler isDone;
		internal bool IsCurrent
		{
			get;
			set;
		}
		internal bool Done
		{
			get;
			set;
		}
		public SuperBox question
		{
			get
			{
				return this.superbox;
			}
		}
		public Label QuestionID
		{
			get
			{
				return this.indexLabel;
			}
		}
		private void QuestionPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (!this.IsCurrent)
			{
				this.IndexChanged(this, new EventArgs());
				this.superbox.Focus();
			}
		}
		private void QuestionPanel_Enter(object sender, EventArgs e)
		{
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				Question.questionChangeTime = TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft;
			}
		}
		private void QuestionPanel_Leave(object sender, EventArgs e)
		{
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				string text = "";
				TestPaperPlayer.answerINI.WriteValue("LOG", this.id, text);
				TestPaperPlayer.answerINI.ReadValue("LOG", this.id, string.Concat(new string[]
				{
					text,
					"[",
					Question.questionChangeTime.ToString(),
					"-",
					(TestPaperPlayer.examTotalTime - TestPaperPlayer.examTimeLeft).ToString(),
					"]"
				}));
			}
		}
		public Question(XmlNode xmlnode, int panelwidth, int QindexWidth)
		{
			this.indexLabel = new Label();
			this.id = xmlnode.Attributes["id"].Value;
			this.indexLabel.AutoSize = false;
			this.indexLabel.Font = new Font("黑体", (float)this.indexlabelfontsize, FontStyle.Bold, GraphicsUnit.Pixel);
			this.indexLabel.Size = new Size(QindexWidth, this.indexlabelfontsize * 3);
			this.indexLabel.TextAlign = ContentAlignment.MiddleRight;
			this.indexLabel.Text = "";
			this.indexLabel.Margin = new Padding(0, 0, 0, 0);
			this.superbox = new SuperBox(Math.Max(0, panelwidth - 30));
			this.superbox.ReadOnly = true;
			this.superbox.ContentChanged += new EventHandler(this.sb_ContentChanged);
			string text = Path.Combine(TestPaperPlayer.paperPath, TestPaper.id + "_" + this.id);
			text = Path.Combine(text, "Question.xml");
			this.superbox.LoadFromXml(text);
			this.superbox.Margin = new Padding(0, 0, 0, 0);
			this.superbox.OperateClicked += new EventHandler(this.superbox_OperateClicked);
			this.superbox.OperateDone += new SuperBox.MessageEventHandler(this.superbox_OperateDone);
			this.Done = false;
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDAnalysis)
			{
				int num = TestPaperPlayer.answerINI.ReadValue("Answer", "NUM", 0);
				for (int i = 0; i < num; i++)
				{
					if (TestPaperPlayer.answerINI.ReadValue(i.ToString(), "ID", "") == xmlnode.Attributes["id"].Value)
					{
						string text2 = TestPaperPlayer.answerINI.ReadValue(i.ToString(), "AnswerXML", "");
						if (text2.StartsWith("["))
						{
							text2 = text2.Substring(1);
						}
						if (text2.EndsWith("]"))
						{
							text2 = text2.Substring(0, text2.Length - 1);
						}
						this.superbox.FillIn(text2.Split(new char[]
						{
							'\u001e'
						}), TestPaperPlayer.answerPath, this.id);
					}
				}
			}
			this.superbox.MouseMove += new MouseEventHandler(this.QuestionPanel_MouseMove);
			this.answerid = TestPaper.questionCount.ToString();
			TestPaper.questionCount++;
			if (TestPaperPlayer.mode != TestPaperPlayer.RunMode.CDAnalysis)
			{
				this.superbox.Enter += new EventHandler(this.QuestionPanel_Enter);
				this.superbox.Leave += new EventHandler(this.QuestionPanel_Leave);
				TestPaperPlayer.answerINI.WriteValue(this.answerid, "ID", xmlnode.Attributes["id"].Value);
				TestPaperPlayer.answerINI.WriteValue(this.answerid, "Answer", "[" + this.superbox.getContent() + "]");
				TestPaperPlayer.answerINI.WriteValue(this.answerid, "AnswerXML", "[" + this.superbox.getContentXml() + "]");
			}
		}
		private void superbox_OperateClicked(object sender, EventArgs e)
		{
			CommonMethods.ClearDirectory(TestPaperPlayer.stuDataPath);
			this.HideWindow(this, new EventArgs());
			TestPaper.allowExplorer = true;
			TestPaperPlayer.inOperate = true;
			if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDAnalysis)
			{
				this.superbox.DoOperate(sender, TestPaperPlayer.stuBMP, TestPaperPlayer.stuInfo, TestPaperPlayer.stuDataPath, TestPaperPlayer.tickCount, -1, Path.Combine(TestPaperPlayer.stdAnswerDir, this.id));
			}
			else
			{
				if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDExercise)
				{
					this.superbox.DoOperate(sender, TestPaperPlayer.stuBMP, TestPaperPlayer.stuInfo, TestPaperPlayer.stuDataPath, TestPaperPlayer.tickCount, -1, Path.Combine(TestPaperPlayer.answerPath, ""));
				}
				else
				{
					this.superbox.DoOperate(sender, TestPaperPlayer.stuBMP, TestPaperPlayer.stuInfo, TestPaperPlayer.stuDataPath, TestPaperPlayer.examTimeLeft, TestPaperPlayer.endTipTime, "");
				}
			}
		}
		private void superbox_OperateDone(object sender, MessageEventArgs e)
		{
			DirectorySecurity directorySecurity = new DirectorySecurity();
			InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
			FileSystemAccessRule rule = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inheritanceFlags, PropagationFlags.None, AccessControlType.Allow);
			bool flag;
			directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out flag);
			string text = Path.Combine(TestPaperPlayer.answerPath, this.id + e.Message);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text, directorySecurity);
			}
			string text2 = Path.Combine(TestPaperPlayer.answerPath, this.id + e.Message + ".zip");
			CommonMethods.ClearDirectory(text);
			CommonMethods.CopyFolder(TestPaperPlayer.stuDataPath, text, true);
			if (File.Exists(text2))
			{
				FileInfo fileInfo = new FileInfo(text2);
				if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					fileInfo.Attributes = FileAttributes.Normal;
				}
				FileSecurity accessControl = fileInfo.GetAccessControl(AccessControlSections.All);
				InheritanceFlags inheritanceFlags2 = InheritanceFlags.None;
				FileSystemAccessRule rule2 = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inheritanceFlags2, PropagationFlags.None, AccessControlType.Allow);
				bool flag2;
				accessControl.ModifyAccessRule(AccessControlModification.Add, rule2, out flag2);
				fileInfo.SetAccessControl(accessControl);
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
			}
			CommonMethods.Zip(text, text2, "CKKC37F423");
			CommonMethods.ClearDirectory(TestPaperPlayer.stuDataPath);
			TestPaperPlayer.inOperate = false;
			this.ShowWindow(this, new EventArgs());
			TestPaperPlayer.answerINI.WriteValue(this.answerid, "Answer", "[" + this.superbox.getContent() + "]");
			TestPaperPlayer.answerINI.WriteValue(this.answerid, "AnswerXML", "[" + this.superbox.getContentXml() + "]");
			string content = this.superbox.getContent();
			string[] array = content.Split(new char[]
			{
				'\u001e'
			});
			bool flag3 = true;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string a = array2[i];
				if (a == "")
				{
					flag3 = false;
				}
			}
			if (flag3 != this.Done)
			{
				this.Done = flag3;
				if (this.isDone != null)
				{
					this.isDone(this, new BoolEventArgs(this.Done));
				}
			}
			TestPaper.allowExplorer = false;
		}
		private void sb_ContentChanged(object sender, EventArgs e)
		{
			TestPaperPlayer.answerINI.WriteValue(this.answerid, "Answer", "[" + this.superbox.getContent() + "]");
			TestPaperPlayer.answerINI.WriteValue(this.answerid, "AnswerXML", "[" + this.superbox.getContentXml() + "]");
			string content = this.superbox.getContent();
			string[] array = content.Split(new char[]
			{
				'\u001e'
			});
			bool flag = true;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string a = array2[i];
				if (a == "")
				{
					flag = false;
				}
			}
			if (flag != this.Done)
			{
				this.Done = flag;
				if (this.isDone != null)
				{
					this.isDone(this, new BoolEventArgs(this.Done));
				}
			}
		}
		internal void freshtree()
		{
			string content = this.superbox.getContent();
			string[] array = content.Split(new char[]
			{
				'\u001e'
			});
			bool flag = true;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string a = array2[i];
				if (a == "")
				{
					flag = false;
				}
			}
			if (flag != this.Done)
			{
				this.Done = flag;
				if (this.isDone != null)
				{
					this.isDone(this, new BoolEventArgs(this.Done));
				}
			}
		}
		internal void LoadSTD(string OptionAnswer)
		{
			this.superbox.LoadOptionSTD(OptionAnswer);
		}
		private void showAnswer_Click(object sender, EventArgs e)
		{
			this.ShowAnwser();
		}
		private void ShowAnwser()
		{
		}
		private void ShowStudentAnwser()
		{
		}
	}
}
