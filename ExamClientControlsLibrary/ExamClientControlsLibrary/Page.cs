using Qisi.General;
using Qisi.General.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
namespace ExamClientControlsLibrary
{
	internal class Page : IDisposable
	{
		internal delegate void IndexEventHandler(object sender, IndexEventArgs e);
		internal delegate void BoolIndexEventHandler(object sender, BoolIndexEventArgs e);
		private int indexLabelFontSize;
		private XmlNode myNode;
		private FlowLayoutPanel pagePanel;
		private bool pageIsQuestion;
		private List<Label> indexLabel;
		private List<Question> questionList;
		private List<string> answerList;
		private List<Label> answerLabelList;
		private string subject;
		private bool showVolumnControl = false;
		private bool showFormuleEditor = false;
		internal event Page.IndexEventHandler IndexChanged;
		internal event Page.BoolIndexEventHandler IsDown;
		internal event EventHandler HideWindow;
		internal event EventHandler ShowWindow;
		internal string SubjectName
		{
			get
			{
				return this.subject;
			}
			set
			{
				this.subject = value;
			}
		}
		internal FlowLayoutPanel PagePanel
		{
			get
			{
				return this.pagePanel;
			}
		}
		internal bool IsQuestion
		{
			get
			{
				return this.pageIsQuestion;
			}
		}
		internal bool ShowVolumnControl
		{
			get
			{
				return this.showVolumnControl;
			}
		}
		internal bool ShowFormuleEditor
		{
			get
			{
				return this.showFormuleEditor;
			}
		}
		internal Page(XmlNode xmlnode)
		{
			this.pagePanel = new FlowLayoutPanel();
			this.indexLabelFontSize = 12;
			this.pageIsQuestion = false;
			this.indexLabel = new List<Label>();
			this.questionList = new List<Question>();
			this.answerList = new List<string>();
			this.answerLabelList = new List<Label>();
			this.myNode = xmlnode;
			this.pagePanel.AutoSize = true;
			this.pagePanel.MaximumSize = new Size(TestPaperPlayer.mainPanelSize.Width, 0);
			this.pagePanel.Margin = new Padding(0, 0, 0, 0);
			this.pagePanel.Padding = new Padding(0, 0, 0, 0);
			if (this.myNode.Attributes["showvc"] != null && this.myNode.Attributes["showvc"].Value.ToLower() == "true")
			{
				this.showVolumnControl = true;
			}
			if (this.myNode.Attributes["showfe"] != null && this.myNode.Attributes["showfe"].Value.ToLower() == "true")
			{
				this.showFormuleEditor = true;
			}
			if (this.myNode.SelectNodes("question").Count != 0 || this.myNode.SelectNodes("questions").Count != 0)
			{
				this.pageIsQuestion = true;
			}
			foreach (XmlNode xmlNode in this.myNode.ChildNodes)
			{
				if (xmlNode.Name == "richtext")
				{
					ReadOnlyRichTextBox readOnlyRichTextBox = new ReadOnlyRichTextBox();
					readOnlyRichTextBox.ContentsResized += new ContentsResizedEventHandler(this.RTF_ContentsResized);
					readOnlyRichTextBox.Width = TestPaperPlayer.mainPanelSize.Width - readOnlyRichTextBox.Margin.Left - readOnlyRichTextBox.Margin.Right;
					readOnlyRichTextBox.ReadOnly = true;
					readOnlyRichTextBox.WordWrap = true;
					readOnlyRichTextBox.BackColor = Color.White;
					readOnlyRichTextBox.BorderStyle = BorderStyle.None;
					readOnlyRichTextBox.Multiline = true;
					readOnlyRichTextBox.LoadFile(TestPaperPlayer.paperPath + xmlNode.Attributes["src"].Value);
					readOnlyRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
					this.pagePanel.Controls.Add(readOnlyRichTextBox);
					this.pagePanel.SetFlowBreak(readOnlyRichTextBox, true);
				}
				else
				{
					if (xmlNode.Name == "question")
					{
						int num = this.indexLabelFontSize * 7;
						int num2 = TestPaperPlayer.mainPanelSize.Width - num;
						Question question = new Question(xmlNode, num2, num);
						question.IndexChanged += new EventHandler(this.q_IndexChanged);
						question.HideWindow += new EventHandler(this.q_HideWindow);
						question.ShowWindow += new EventHandler(this.q_ShowWindow);
						question.isDone += new Question.BoolEventHandler(this.q_isDone);
						this.pagePanel.Controls.Add(question.QuestionID);
						this.pagePanel.Controls.Add(question.question);
						this.indexLabel.Add(question.QuestionID);
						this.questionList.Add(question);
						if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDExercise)
						{
							Label label = new Label();
							label.BackColor = Color.Transparent;
							label.Text = "";
							label.AutoSize = false;
							label.Size = new Size(num, num);
							this.pagePanel.Controls.Add(label);
							Label label2 = new Label();
							label2.Font = new Font("宋体", 20f, FontStyle.Underline, GraphicsUnit.Pixel);
							label2.ForeColor = Color.Blue;
							label2.Text = "参考答案";
							label2.Cursor = Cursors.Hand;
							label2.Click += new EventHandler(this.label_Click);
							this.pagePanel.Controls.Add(label2);
							this.pagePanel.SetFlowBreak(label2, true);
							this.answerList.Add(TestPaperPlayer.stdINI.ReadValue(xmlNode.Attributes["id"].Value, "std", ""));
							this.answerLabelList.Add(label2);
						}
						else
						{
							if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDAnalysis)
							{
								Label label = new Label();
								label.BackColor = Color.Transparent;
								label.Text = "";
								label.AutoSize = false;
								label.Size = new Size(num, num);
								this.pagePanel.Controls.Add(label);
								string text = TestPaperPlayer.stdINI.ReadValue(xmlNode.Attributes["id"].Value, "std", "");
								if (text.EndsWith(".rtf"))
								{
									ReadOnlyRichTextBox readOnlyRichTextBox2 = new ReadOnlyRichTextBox();
									readOnlyRichTextBox2.Width = num2 - 20;
									readOnlyRichTextBox2.BackColor = Color.White;
									readOnlyRichTextBox2.ScrollBars = RichTextBoxScrollBars.None;
									readOnlyRichTextBox2.ContentsResized += new ContentsResizedEventHandler(this.RTF_ContentsResized);
									readOnlyRichTextBox2.BorderStyle = BorderStyle.None;
									readOnlyRichTextBox2.LoadFile(Path.Combine(TestPaperPlayer.stdAnswerDir, TestPaperPlayer.stdINI.ReadValue(xmlNode.Attributes["id"].Value, "std", "")));
									this.pagePanel.Controls.Add(readOnlyRichTextBox2);
									this.pagePanel.SetFlowBreak(readOnlyRichTextBox2, true);
								}
								else
								{
									question.LoadSTD(text);
								}
							}
						}
					}
					else
					{
						if (xmlNode.Name == "questions")
						{
							List<XmlNode> list = new List<XmlNode>();
							if (xmlNode.Attributes["randone"] != null && xmlNode.Attributes["randone"].Value.ToLower() == "true")
							{
								Random random = new Random(DateTime.Now.Millisecond);
								int i = random.Next(xmlNode.ChildNodes.Count);
								if (xmlNode.ChildNodes[i].Name == "question")
								{
									list.Add(xmlNode.ChildNodes[i]);
								}
								else
								{
									XmlNodeList xmlNodeList = xmlNode.ChildNodes[i].SelectNodes("question");
									foreach (XmlNode item in xmlNodeList)
									{
										list.Add(item);
									}
								}
							}
							else
							{
								XmlNodeList xmlNodeList = xmlNode.SelectNodes("question");
								foreach (XmlNode item in xmlNodeList)
								{
									list.Add(item);
								}
							}
							int num = this.indexLabelFontSize * 6;
							for (int j = 0; j < list.Count; j++)
							{
								Question question = new Question(list[j], this.pagePanel.ClientSize.Width - num - 6, num);
								question.IndexChanged += new EventHandler(this.q_IndexChanged);
								question.HideWindow += new EventHandler(this.q_HideWindow);
								question.ShowWindow += new EventHandler(this.q_ShowWindow);
								question.isDone += new Question.BoolEventHandler(this.q_isDone);
								this.pagePanel.Controls.Add(question.QuestionID);
								this.pagePanel.Controls.Add(question.question);
								this.pagePanel.SetFlowBreak(question.question, true);
								this.indexLabel.Add(question.QuestionID);
								this.questionList.Add(question);
								if (TestPaperPlayer.mode == TestPaperPlayer.RunMode.CDExercise)
								{
									Label label = new Label();
									label.BackColor = Color.Transparent;
									label.Text = "";
									label.AutoSize = false;
									label.Size = new Size(num, num);
									this.pagePanel.Controls.Add(label);
									ReadOnlyRichTextBox readOnlyRichTextBox2 = new ReadOnlyRichTextBox();
									readOnlyRichTextBox2.LoadFile(Path.Combine(TestPaperPlayer.stdAnswerDir, TestPaperPlayer.stdINI.ReadValue(xmlNode.Attributes["id"].Value, "std", "")));
									this.pagePanel.Controls.Add(readOnlyRichTextBox2);
									this.pagePanel.SetFlowBreak(readOnlyRichTextBox2, true);
								}
							}
						}
					}
				}
			}
		}
		private void label_Click(object sender, EventArgs e)
		{
			if (sender is Label)
			{
				int num = this.answerLabelList.IndexOf(sender as Label);
				if (num >= 0)
				{
					if (this.answerList[num].EndsWith(".rtf"))
					{
						FormRTF formRTF = new FormRTF(Path.Combine(TestPaperPlayer.stdAnswerDir, this.answerList[num]));
						formRTF.ShowDialog();
					}
					else
					{
						this.questionList[num].LoadSTD(this.answerList[num]);
					}
				}
			}
		}
		private void q_ShowWindow(object sender, EventArgs e)
		{
			if (this.ShowWindow != null)
			{
				this.ShowWindow(sender, e);
			}
		}
		private void q_HideWindow(object sender, EventArgs e)
		{
			if (this.HideWindow != null)
			{
				this.HideWindow(sender, e);
			}
		}
		private void RTF_ContentsResized(object sender, ContentsResizedEventArgs e)
		{
			if (sender is ReadOnlyRichTextBox)
			{
				ReadOnlyRichTextBox readOnlyRichTextBox = sender as ReadOnlyRichTextBox;
				int num = readOnlyRichTextBox.Height - readOnlyRichTextBox.ClientSize.Height;
				readOnlyRichTextBox.Height = Math.Max(readOnlyRichTextBox.Height, e.NewRectangle.Height + num);
			}
		}
		internal void setQuestionIndex(ref int index)
		{
			for (int i = 0; i < this.indexLabel.Count; i++)
			{
				this.indexLabel[i].Text = index++.ToString() + ".";
			}
		}
		internal void SetIndexFont(int index)
		{
			if (index < this.indexLabel.Count)
			{
				this.indexLabel[index].Font = new Font("黑体", (float)(this.indexLabelFontSize * 2), FontStyle.Bold);
				this.indexLabel[index].Refresh();
			}
		}
		internal void ResetIndexFont()
		{
			for (int i = 0; i < this.indexLabel.Count; i++)
			{
				this.indexLabel[i].Font = new Font("黑体", (float)this.indexLabelFontSize, FontStyle.Bold);
				this.indexLabel[i].Refresh();
			}
		}
		private void q_IndexChanged(object sender, EventArgs e)
		{
			if (sender is Question)
			{
				Question item = sender as Question;
				int num = this.questionList.IndexOf(item);
				if (num >= 0 && num < this.indexLabel.Count)
				{
					this.IndexChanged(this, new IndexEventArgs(num));
				}
			}
		}
		private void q_isDone(object sender, BoolEventArgs e)
		{
			if (sender is Question)
			{
				Question item = sender as Question;
				int num = this.questionList.IndexOf(item);
				if (num >= 0 && num < this.indexLabel.Count)
				{
					this.IsDown(this, new BoolIndexEventArgs(num, e.Message));
				}
			}
		}
		internal void FreshTree()
		{
			foreach (Question current in this.questionList)
			{
				current.freshtree();
			}
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.pagePanel != null)
				{
					this.pagePanel.Dispose();
				}
				foreach (Question current in this.questionList)
				{
					current.QuestionID.Dispose();
					current.question.Dispose();
				}
			}
			this.pagePanel = null;
			this.indexLabel = null;
			this.questionList = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Page()
		{
			this.Dispose(false);
		}
	}
}
