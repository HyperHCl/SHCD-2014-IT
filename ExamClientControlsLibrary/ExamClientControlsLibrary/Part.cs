using System;
using System.Collections.Generic;
using System.Xml;
namespace ExamClientControlsLibrary
{
	internal class Part
	{
		public string partname;
		private string partSubject;
		private XmlNode myNode;
		private List<Section> objSectionList;
		private List<Page> pageList;
		internal List<Page> PartPageList
		{
			get
			{
				return this.pageList;
			}
		}
		internal Part(XmlNode xmlnode)
		{
			this.objSectionList = new List<Section>();
			this.pageList = new List<Page>();
			if (xmlnode.Attributes["name"] != null)
			{
				this.partname = xmlnode.Attributes["name"].Value;
			}
			else
			{
				this.partname = "";
			}
			if (xmlnode.Attributes["subject"] != null)
			{
				this.partSubject = xmlnode.Attributes["subject"].Value;
			}
			else
			{
				this.partSubject = "";
			}
			this.myNode = xmlnode;
			if (this.partSubject == TestPaperPlayer.subject || this.partSubject == "" || TestPaperPlayer.subject == "")
			{
				XmlNodeList xmlNodeList = this.myNode.SelectNodes("section");
				foreach (XmlNode xmlnode2 in xmlNodeList)
				{
					Section item = new Section(xmlnode2);
					this.objSectionList.Add(item);
				}
			}
			foreach (Section current in this.objSectionList)
			{
				this.pageList.AddRange(current.SectionPageList);
			}
			foreach (Page current2 in this.pageList)
			{
				current2.SubjectName = this.partname;
			}
		}
	}
}
