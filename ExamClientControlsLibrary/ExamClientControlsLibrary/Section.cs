using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
namespace ExamClientControlsLibrary
{
	public class Section
	{
		private XmlNode myNode;
		private bool rand = false;
		private List<Page> objPageList = null;
		private string commentText = null;
		internal List<Page> SectionPageList
		{
			get
			{
				return this.objPageList;
			}
		}
		public Section(XmlNode xmlnode)
		{
			this.objPageList = new List<Page>();
			this.myNode = xmlnode;
			if (this.myNode.Attributes["randomize"] != null && this.myNode.Attributes["randomize"].Value == "true")
			{
				this.rand = true;
			}
			else
			{
				this.rand = false;
			}
			XmlNodeList xmlNodeList = this.myNode.SelectNodes("page");
			foreach (XmlNode xmlnode2 in xmlNodeList)
			{
				Page item = new Page(xmlnode2);
				this.objPageList.Add(item);
			}
			if (xmlNodeList.Count > 0 && xmlNodeList[0].SelectSingleNode("text") != null)
			{
				this.commentText = xmlNodeList[0].SelectSingleNode("text").InnerText;
			}
			else
			{
				this.commentText = "";
			}
			if (this.rand)
			{
				Random random = new Random(DateTime.Now.Millisecond);
				for (int i = 0; i < this.objPageList.Count; i++)
				{
					Page item2 = this.objPageList[i];
					int index = random.Next(this.objPageList.Count);
					this.objPageList.Remove(item2);
					this.objPageList.Insert(index, item2);
				}
			}
			if (this.commentText != "")
			{
				Label label = new Label();
				label.Visible = true;
				label.Margin = new Padding(50, 10, 0, 30);
				label.AutoSize = true;
				label.MaximumSize = new Size(TestPaperPlayer.mainPanelSize.Width, 0);
				label.Font = new Font("宋体", 20f, FontStyle.Regular, GraphicsUnit.Pixel);
				label.Text = this.commentText;
				this.objPageList[0].PagePanel.Controls.Add(label);
				this.objPageList[0].PagePanel.SetFlowBreak(label, true);
				this.objPageList[0].PagePanel.Controls.SetChildIndex(label, 0);
			}
		}
	}
}
