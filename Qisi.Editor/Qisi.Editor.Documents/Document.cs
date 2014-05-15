using Qisi.Editor.Documents.Elements;
using Qisi.Editor.Documents.Table;
using Qisi.Editor.Expression;
using Qisi.General;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace Qisi.Editor.Documents
{
	internal class Document : IDisposable
	{
		private const float lineInterval = 8f;
		private const float minWidth = 10f;
		private List<Line> lines;
		private List<Element> elements;
		private List<Blank> blanks;
		private List<Options> optionss;
		private Pic_Tab parent;
		private float docWidth;
		private float docHeight;
		private PointF docLocation;
		private Color highLightColor;
		internal event EventHandler ContentChanged;
		internal event EventHandler HeightChanged;
		internal event EventHandler OperateClicked;
		internal event EventHandler OperateDone;
		internal List<Line> Lines
		{
			get
			{
				return this.lines;
			}
			set
			{
				this.lines = value;
			}
		}
		internal List<Element> Elements
		{
			get
			{
				return this.elements;
			}
			set
			{
				this.elements = value;
			}
		}
		internal List<Blank> Blanks
		{
			get
			{
				return this.blanks;
			}
			set
			{
				this.blanks = value;
			}
		}
		internal List<Options> Optionss
		{
			get
			{
				return this.optionss;
			}
			set
			{
				this.optionss = value;
			}
		}
		internal Pic_Tab Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}
		internal Line LastLine
		{
			get
			{
				Line result;
				if (this.lines == null || this.lines.Count == 0)
				{
					result = null;
				}
				else
				{
					result = this.lines[this.lines.Count - 1];
				}
				return result;
			}
		}
		internal Element LastElement
		{
			get
			{
				Element result;
				if (this.elements != null && this.elements.Count > 0)
				{
					result = this.elements[this.elements.Count - 1];
				}
				else
				{
					result = null;
				}
				return result;
			}
		}
		internal int AnswerCount
		{
			get
			{
				int num = 0;
				foreach (Blank current in this.Blanks)
				{
					num++;
				}
				foreach (Options current2 in this.optionss)
				{
					num++;
				}
				foreach (Element current3 in this.Elements)
				{
					if (current3 is TableInfo)
					{
						TableInfo tableInfo = current3 as TableInfo;
						foreach (Cell current4 in tableInfo.Items)
						{
							num += current4.AnswerCount;
						}
					}
					else
					{
						if (current3 is PictureInfo)
						{
							PictureInfo pictureInfo = current3 as PictureInfo;
							foreach (Document current5 in pictureInfo.Documents)
							{
								num += current5.AnswerCount;
							}
						}
						else
						{
							if (current3 is OperationInfo)
							{
								num++;
							}
							else
							{
								if (current3 is DrawInfo)
								{
									num++;
								}
							}
						}
					}
				}
				return num;
			}
		}
		internal float LineInterval
		{
			get
			{
				return 8f;
			}
		}
		internal Padding Margin
		{
			get;
			set;
		}
		internal virtual float DocWidth
		{
			get
			{
				return this.docWidth;
			}
			set
			{
				if (this.docWidth != Math.Max(10f, value))
				{
					this.docWidth = Math.Max(10f, value);
					if (this.elements.Count > 0)
					{
						this.elements[0].Settled = false;
					}
				}
			}
		}
		internal virtual float DocHeight
		{
			get
			{
				return this.docHeight;
			}
			set
			{
				if (this.docHeight != value)
				{
					this.docHeight = value;
				}
			}
		}
		internal float OutWidth
		{
			get
			{
				return this.DocWidth + (float)this.Margin.Horizontal;
			}
		}
		internal virtual float OutHeight
		{
			get
			{
				return this.docHeight + (float)this.Margin.Vertical;
			}
		}
		public PointF DocLocation
		{
			get
			{
				return this.docLocation;
			}
			set
			{
				if (this.docLocation != value)
				{
					foreach (Element current in this.elements)
					{
						current.Location = new PointF(current.Location.X - this.docLocation.X + value.X, current.Location.Y - this.docLocation.Y + value.Y);
					}
					foreach (Line current2 in this.lines)
					{
						current2.Top = current2.Top - this.docLocation.Y + value.Y;
						current2.Left = current2.Left - this.docLocation.X + value.X;
						current2.Right = current2.Right - this.docLocation.X + value.X;
					}
					this.docLocation = value;
				}
			}
		}
		public Color HighLightColor
		{
			get
			{
				return this.highLightColor;
			}
		}
		public Font DefaultFont
		{
			get;
			set;
		}
		internal Document(Padding margin, Font font, Pic_Tab parentObj, float width, PointF location, Color backcolor)
		{
			this.elements = new List<Element>();
			this.lines = new List<Line>();
			this.blanks = new List<Blank>();
			this.optionss = new List<Options>();
			this.parent = parentObj;
			this.Margin = margin;
			this.DocWidth = width - (float)this.Margin.Horizontal;
			this.DocHeight = 0f;
			this.docLocation = location;
			this.DefaultFont = font;
			this.highLightColor = NativeMethods.MixColor(NativeMethods.getRevColor(backcolor), SystemColors.Highlight);
			this.AppendLine(this.docLocation.Y + (float)this.Margin.Top, 0);
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.lines != null)
				{
					foreach (Line current in this.lines)
					{
						current.Dispose();
					}
				}
				if (this.elements != null)
				{
					foreach (Element current2 in this.elements)
					{
						current2.Dispose();
					}
				}
				if (this.blanks != null)
				{
					foreach (Blank current3 in this.blanks)
					{
						current3.Dispose();
					}
				}
				if (this.optionss != null)
				{
					foreach (Options current4 in this.optionss)
					{
						current4.Dispose();
					}
				}
			}
			if (this.lines != null)
			{
				for (int i = 0; i < this.lines.Count; i++)
				{
					this.lines[i] = null;
				}
			}
			this.lines = null;
			if (this.elements != null)
			{
				for (int i = 0; i < this.elements.Count; i++)
				{
					this.elements[i] = null;
				}
			}
			this.elements = null;
			if (this.blanks != null)
			{
				for (int i = 0; i < this.blanks.Count; i++)
				{
					this.blanks[i] = null;
				}
			}
			this.blanks = null;
			if (this.optionss != null)
			{
				for (int i = 0; i < this.optionss.Count; i++)
				{
					this.optionss[i] = null;
				}
			}
			this.optionss = null;
			this.parent = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Document()
		{
			this.Dispose(false);
		}
		internal bool InsertElement(int index, Element element, bool readOnly)
		{
			bool result;
			foreach (Options current in this.optionss)
			{
				if (index >= current.StartIndex && index <= current.StartIndex + current.Count)
				{
					result = false;
					return result;
				}
			}
			if (readOnly)
			{
				bool flag = false;
				foreach (Blank current2 in this.blanks)
				{
					if (current2.StartIndex <= index && current2.StartIndex + current2.Count >= index)
					{
						if (current2.Count < current2.MaxCharsCount)
						{
							if (!(element is CharInfo) || (element as CharInfo).Char != '\r' || current2.AllowCR)
							{
								current2.Count++;
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					result = false;
					return result;
				}
			}
			else
			{
				bool flag = false;
				foreach (Blank current2 in this.blanks)
				{
					if (current2.StartIndex <= index && current2.StartIndex + current2.Count >= index)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					result = false;
					return result;
				}
			}
			foreach (Blank current2 in this.blanks)
			{
				if (current2.StartIndex > index)
				{
					current2.StartIndex++;
				}
			}
			foreach (Options current in this.optionss)
			{
				if (current.StartIndex > index)
				{
					foreach (Option current3 in current.OptionList)
					{
						current3.StartIndex++;
					}
				}
			}
			this.elements.Insert(index, element);
			this.elements[index].Settled = false;
			if (this.ContentChanged != null)
			{
				this.ContentChanged(this, new EventArgs());
			}
			result = true;
			return result;
		}
		internal bool InsertChar(int index, char chr, Font font, bool readOnly)
		{
			return this.InsertElement(index, new CharInfo(chr, font), readOnly);
		}
		internal bool InsertExpression(int index, containerexpression containerExpr, Font font, bool readOnly)
		{
			return this.InsertElement(index, new ExpressionInfo(containerExpr, font), readOnly);
		}
		internal bool InsertPic(int index, Image image, Font font, bool readOnly)
		{
			return this.InsertElement(index, new PictureInfo(image, font), readOnly);
		}
		internal bool InsertTable(int index, Point tableSize, Font font, bool readOnly)
		{
			return this.InsertElement(index, new TableInfo(tableSize, this.DocWidth, font, 1f), readOnly);
		}
		internal bool InsertOperation(int index, OperationInfo.OperationType operType, Font font, string dirpath, string rtf, string gif, bool readOnly)
		{
			return this.InsertElement(index, new OperationInfo(operType, font, dirpath, rtf, gif), readOnly);
		}
		internal bool DeleteElement(int index, bool readOnly)
		{
			bool result;
			foreach (Options current in this.optionss)
			{
				if (index >= current.StartIndex - 1 && index <= current.StartIndex + current.Count)
				{
					result = false;
					return result;
				}
			}
			foreach (Blank current2 in this.blanks)
			{
				if (index == current2.StartIndex - 1 || index == current2.StartIndex + current2.Count)
				{
					result = false;
					return result;
				}
			}
			bool flag = false;
			foreach (Blank current2 in this.blanks)
			{
				if (current2.StartIndex <= index && current2.StartIndex + current2.Count > index)
				{
					flag = true;
				}
			}
			if (readOnly ^ flag)
			{
				result = false;
			}
			else
			{
				foreach (Blank current2 in this.Blanks)
				{
					if (current2.StartIndex <= index && current2.StartIndex + current2.Count > index)
					{
						current2.Count--;
					}
					else
					{
						if (current2.StartIndex > index)
						{
							current2.StartIndex--;
						}
					}
				}
				foreach (Options current in this.optionss)
				{
					if (current.StartIndex > index)
					{
						foreach (Option current3 in current.OptionList)
						{
							current3.StartIndex--;
						}
					}
				}
				this.elements.RemoveAt(index);
				if (index < this.elements.Count)
				{
					this.elements[index].Settled = false;
				}
				if (this.ContentChanged != null)
				{
					this.ContentChanged(this, new EventArgs());
				}
				result = true;
			}
			return result;
		}
		internal int DeleteElement(int start, int count, bool readOnly)
		{
			int num = 0;
			int num2 = start;
			while (count > 0)
			{
				bool flag = false;
				foreach (Options current in this.optionss)
				{
					if (num2 >= current.StartIndex - 1 && num2 <= current.StartIndex + current.Count)
					{
						flag = true;
					}
				}
				foreach (Blank current2 in this.blanks)
				{
					if (num2 == current2.StartIndex - 1 || num2 == current2.StartIndex + current2.Count)
					{
						flag = true;
					}
				}
				if (flag)
				{
					num++;
					num2++;
					count--;
				}
				else
				{
					bool flag2 = false;
					foreach (Blank current2 in this.blanks)
					{
						if (current2.StartIndex <= num2 && current2.StartIndex + current2.Count > num2)
						{
							flag2 = true;
						}
					}
					if (readOnly ^ flag2)
					{
						num++;
						num2++;
						count--;
					}
					else
					{
						foreach (Blank current2 in this.Blanks)
						{
							if (current2.StartIndex <= num2 && current2.StartIndex + current2.Count > num2)
							{
								current2.Count--;
							}
							else
							{
								if (current2.StartIndex > num2)
								{
									current2.StartIndex--;
								}
							}
						}
						foreach (Options current in this.optionss)
						{
							if (current.StartIndex > num2)
							{
								foreach (Option current3 in current.OptionList)
								{
									current3.StartIndex--;
								}
							}
						}
						this.elements.RemoveAt(num2);
						count--;
					}
				}
			}
			if (start < this.elements.Count)
			{
				this.elements[start].Settled = false;
			}
			if (this.ContentChanged != null)
			{
				this.ContentChanged(this, new EventArgs());
			}
			return num;
		}
		internal void ClearAll()
		{
			this.elements = new List<Element>();
			this.lines = new List<Line>();
			this.blanks = new List<Blank>();
			this.optionss = new List<Options>();
			this.AppendLine(this.docLocation.Y + (float)this.Margin.Top, 0);
		}
		internal void Checked(PointF MousePos)
		{
			foreach (Options current in this.optionss)
			{
				if (current.Region.IsVisible(MousePos))
				{
					Option option = null;
					foreach (Option current2 in current.OptionList)
					{
						if (current2.Region.IsVisible(MousePos))
						{
							option = current2;
						}
					}
					if (option != null)
					{
						if (current.Multiple)
						{
							option.Checked = !option.Checked;
						}
						else
						{
							foreach (Option current2 in current.OptionList)
							{
								if (current2 != option)
								{
									current2.Checked = false;
								}
								else
								{
									current2.Checked = !option.Checked;
								}
							}
						}
					}
				}
			}
			foreach (Element current3 in this.elements)
			{
				if (current3 is TableInfo)
				{
					TableInfo tableInfo = current3 as TableInfo;
					foreach (Cell current4 in tableInfo.Items)
					{
						current4.Checked(MousePos);
					}
				}
				else
				{
					if (current3 is PictureInfo)
					{
						PictureInfo pictureInfo = current3 as PictureInfo;
						foreach (Document current5 in pictureInfo.Documents)
						{
							current5.Checked(MousePos);
						}
					}
				}
			}
		}
		internal void Operate(PointF MousePos)
		{
			foreach (Element current in this.elements)
			{
				if (current != null && current is OperationInfo && current.Region.IsVisible(MousePos))
				{
					OperationInfo operationInfo = current as OperationInfo;
					if (operationInfo.ReDoButtonRegion.IsVisible(MousePos) && operationInfo.Opened)
					{
						operationInfo.ReDo();
						if (this.OperateClicked != null)
						{
							this.OperateClicked(operationInfo, new EventArgs());
						}
					}
					else
					{
						if (operationInfo.DoButtonRegion.IsVisible(MousePos) || operationInfo.ImageRegion.IsVisible(MousePos))
						{
							if (this.OperateClicked != null)
							{
								this.OperateClicked(operationInfo, new EventArgs());
							}
						}
					}
				}
			}
			foreach (Element current in this.elements)
			{
				if (current is TableInfo)
				{
					TableInfo tableInfo = current as TableInfo;
					foreach (Cell current2 in tableInfo.Items)
					{
						current2.Checked(MousePos);
					}
				}
				else
				{
					if (current is PictureInfo)
					{
						PictureInfo pictureInfo = current as PictureInfo;
						foreach (Document current3 in pictureInfo.Documents)
						{
							current3.Checked(MousePos);
						}
					}
				}
			}
		}
		internal void OperateFinished(OperationInfo operate)
		{
			if (this.OperateDone != null)
			{
				this.OperateDone(operate, new EventArgs());
			}
			if (this.ContentChanged != null)
			{
				this.ContentChanged(this, new EventArgs());
			}
		}
		private void LayOut(int index)
		{
			if (this.elements != null && index < this.elements.Count)
			{
				Element element = this.elements[index];
				foreach (Blank current in this.blanks)
				{
					if (index == current.StartIndex + current.Count)
					{
						this.RefreshBlank(current);
						break;
					}
				}
				foreach (Options current2 in this.optionss)
				{
					if (index == current2.StartIndex + current2.Count)
					{
						this.RefreshOptions(current2);
						break;
					}
				}
				Line lastLine = this.LastLine;
				element.LineContainer = lastLine;
				element.Location = new PointF(lastLine.Right, lastLine.Top + lastLine.BaseLine - element.BaseLine);
				lastLine.ResetVertical(element);
				if (element.OutSize.Width + element.OutLocation.X > this.DocLocation.X + (float)this.Margin.Left + this.DocWidth)
				{
					if (element.OutSize.Width < this.docWidth || lastLine.ElementCount != 0)
					{
						lastLine.ResetVertical();
						lastLine.Separate();
						this.AppendLine(lastLine.Top + lastLine.Height + 8f, index);
						this.LayOut(index);
						return;
					}
				}
				if (element is CharInfo && lastLine.ElementCount == 0 && lastLine != this.Lines[0])
				{
					int num = 0;
					Line line = this.Lines[this.Lines.IndexOf(lastLine) - 1];
					Element element2 = element;
					while (num < line.ElementCount && element2 is CharInfo && !CharInfo.IsPunctuationLegalTOL((element2 as CharInfo).Char))
					{
						num++;
						element2 = this.elements[index - num];
					}
					if (num < line.ElementCount)
					{
						element2 = this.elements[index - num - 1];
						while (num < line.ElementCount && element2 is CharInfo && !CharInfo.IsPunctuationLegalEOL(((CharInfo)element2).Char))
						{
							num++;
							if (index - num > 0)
							{
								element2 = this.elements[index - num - 1];
							}
						}
						if (num < line.ElementCount && num > 0)
						{
							line.ElementCount -= num;
							line.Separate();
							List<Element> list = new List<Element>();
							this.lines.Remove(lastLine);
							this.DocHeight = this.LastLine.Top + this.LastLine.Height - this.DocLocation.Y - (float)this.Margin.Top;
							this.AppendLine(line.Top + line.Height + 8f, line.StartIndex + line.ElementCount);
							for (int i = 0; i < num; i++)
							{
								this.LayOut(line.StartIndex + line.ElementCount + i);
							}
							this.LayOut(index);
							return;
						}
					}
				}
				element.DocumentContainer = this;
				element.LineContainer = lastLine;
				element.LineContainer.ElementCount++;
				element.LineContainer.Right = element.OutLocation.X + element.OutSize.Width;
				element.Settled = true;
				lastLine.ResetVertical();
				if (element is CharInfo && (element as CharInfo).Char == '\r')
				{
					this.AppendLine(lastLine.Top + lastLine.Height + 8f, index + 1);
				}
			}
		}
		private void RefreshBlank(Blank blank)
		{
			Line lastLine = this.LastLine;
			blank.UnderLines = new List<UnderLine>();
			float num = 0f;
			for (int i = 0; i < blank.Count; i++)
			{
				Element element = this.Elements[i + blank.StartIndex];
				if (element is CharInfo && (element as CharInfo).Char == '\r')
				{
					float width = this.docWidth - element.OutLocation.X - element.OutWidth;
					foreach (RectangleF current in new List<RectangleF>
					{
						new RectangleF(element.OutLocation.X + element.OutWidth, element.OutLocation.Y, width, element.OutSize.Height)
					})
					{
						blank.UnderLines.Add(new UnderLine(element.LineContainer, current.X, current.Right));
						num += current.Width;
					}
				}
				else
				{
					num += element.OutSize.Width;
					blank.UnderLines.Add(new UnderLine(element.LineContainer, element.OutLocation.X, element.OutLocation.X + element.OutSize.Width));
				}
			}
			float num2 = blank.MinLength - num;
			while (num2 > 0f)
			{
				if (lastLine.Right >= this.docLocation.X + this.docWidth + (float)this.Margin.Left)
				{
					this.AppendLine(lastLine.Top + lastLine.Height + 8f, blank.StartIndex + blank.Count);
					lastLine = this.LastLine;
				}
				float width = Math.Min(num2, this.docLocation.X + (float)this.Margin.Left + this.docWidth - lastLine.Right);
				foreach (RectangleF current2 in new List<RectangleF>
				{
					new RectangleF(lastLine.Right, lastLine.Top, width, lastLine.Height)
				})
				{
					blank.UnderLines.Add(new UnderLine(lastLine, current2.X, current2.Right));
					num2 -= current2.Width;
					lastLine.Right = Math.Max(lastLine.Right, current2.Right);
				}
			}
			if (blank.AllowCR)
			{
				blank.UnderLines.Add(new UnderLine(lastLine, lastLine.Right, this.docLocation.X + (float)this.Margin.Left + this.docWidth));
				lastLine.Right = this.docWidth + this.docLocation.X + (float)this.Margin.Left;
			}
			blank.Refreshed = true;
		}
		private void RefreshOptions(Options options)
		{
			if (options.StartIndex == 0)
			{
				this.lines = new List<Line>();
				this.DocHeight = 0f;
			}
			else
			{
				Line lineContainer = this.elements[options.StartIndex - 1].LineContainer;
				int num = this.Lines.IndexOf(lineContainer);
				this.lines.RemoveRange(num + 1, this.Lines.Count - num - 1);
				this.DocHeight = this.LastLine.Top + this.LastLine.Height - this.docLocation.Y - (float)this.Margin.Top;
			}
			foreach (Option option in options.OptionList)
			{
				Option option1;
				option1.Width = 0f;
				option1.Region.MakeEmpty();
				for (int i = option.StartIndex; i < option.StartIndex + option.Count; i++)
				{
					option1.Width += this.Elements[i].Size.Width;
				}
			}
			bool flag = true;
			foreach (Option option in options.OptionList)
			{
				Option option2;
				if (option2.Width + option.Font.Size * 3f > this.docWidth / (float)options.OptionList.Count)
				{
					flag = false;
					break;
				}
			}
			float top;
			if (this.LastLine == null)
			{
				top = this.docLocation.Y + (float)this.Margin.Top;
			}
			else
			{
				top = this.LastLine.Top + this.LastLine.Height + 8f;
			}
			if (flag)
			{
				this.AppendLine(top, options.StartIndex);
				Line lastLine = this.LastLine;
				for (int i = 0; i < options.OptionList.Count; i++)
				{
					Option option = options.OptionList[i];
					float num2 = this.docLocation.X + (float)this.Margin.Left + (float)i * this.docWidth / (float)options.OptionList.Count + option.Font.Size * 1.5f;
					option.Left = this.docLocation.X + (float)this.Margin.Left + (float)i * this.docWidth / (float)options.OptionList.Count;
					option.Line = lastLine;
					for (int j = option.StartIndex; j < option.StartIndex + option.Count; j++)
					{
						this.Elements[j].Location = new PointF(num2, lastLine.Top);
						this.Elements[j].LineContainer = lastLine;
						this.Elements[j].OutWidth = this.Elements[j].Size.Width;
						lastLine.ElementCount++;
						num2 += this.Elements[j].Size.Width;
					}
				}
				lastLine.ResetVertical();
				for (int i = 0; i < options.OptionList.Count; i++)
				{
					Option option = options.OptionList[i];
					option.Region.Union(new RectangleF(option.Left, option.Line.Top, option.Font.Size * 1.5f, option.Line.Height));
					for (int j = option.StartIndex; j < option.StartIndex + option.Count; j++)
					{
						option.Region.Union(this.Elements[j].Region);
					}
				}
			}
			else
			{
				bool flag2 = true;
				foreach (Option option in options.OptionList)
				{
					Option option3;
					if (option3.Width + option3.Font.Size * 3f > this.docWidth / 2f)
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					this.AppendLine(top, options.StartIndex);
					Line lastLine = this.LastLine;
					for (int i = 0; i < options.OptionList.Count; i++)
					{
						Option option = options.OptionList[i];
						float num2 = this.docLocation.X + (float)this.Margin.Left + (float)(i % 2) * this.docWidth / 2f + option.Font.Size * 1.5f;
						option.Left = this.docLocation.X + (float)this.Margin.Left + (float)(i % 2) * this.docWidth / 2f;
						option.Line = lastLine;
						for (int j = option.StartIndex; j < option.StartIndex + option.Count; j++)
						{
							this.Elements[j].Location = new PointF(num2, lastLine.Top);
							this.Elements[j].LineContainer = lastLine;
							this.Elements[j].OutWidth = this.Elements[j].Size.Width;
							lastLine.ElementCount++;
							num2 += this.Elements[j].Size.Width;
						}
						if (i % 2 == 1 && i + 1 < options.OptionList.Count)
						{
							lastLine.ResetVertical();
							this.AppendLine(lastLine.Top + lastLine.Height + 8f, options.OptionList[i + 1].StartIndex);
							lastLine = this.LastLine;
						}
					}
					lastLine.ResetVertical();
					for (int i = 0; i < options.OptionList.Count; i++)
					{
						Option option = options.OptionList[i];
						option.Region.Union(new RectangleF(option.Left, option.Line.Top, option.Font.Size * 1.5f, option.Line.Height));
						for (int j = option.StartIndex; j < option.StartIndex + option.Count; j++)
						{
							option.Region.Union(this.Elements[j].Region);
						}
					}
				}
				else
				{
					this.AppendLine(top, options.StartIndex);
					Line lastLine = this.LastLine;
					for (int i = 0; i < options.OptionList.Count; i++)
					{
						Option option = options.OptionList[i];
						float num2 = this.DocLocation.X + (float)this.Margin.Left + option.Font.Size * 1.5f;
						option.Left = this.DocLocation.X + (float)this.Margin.Left;
						option.Line = lastLine;
						for (int j = option.StartIndex; j < option.StartIndex + option.Count; j++)
						{
							if (num2 + this.Elements[j].Size.Width > this.DocWidth + this.DocLocation.X + (float)this.Margin.Left)
							{
								num2 = this.DocLocation.X + (float)this.Margin.Left + option.Font.Size * 1.5f;
								lastLine.ResetVertical();
								this.AppendLine(lastLine.Top + lastLine.Height + 8f, j);
								lastLine = this.LastLine;
							}
							this.Elements[j].Location = new PointF(num2, lastLine.Top);
							this.Elements[j].LineContainer = lastLine;
							this.Elements[j].OutWidth = this.Elements[j].Size.Width;
							lastLine.ElementCount++;
							num2 += this.Elements[j].Size.Width;
						}
						if (i + 1 < options.OptionList.Count)
						{
							lastLine.ResetVertical();
							this.AppendLine(lastLine.Top + lastLine.Height + 8f, options.OptionList[i + 1].StartIndex);
							lastLine = this.LastLine;
						}
					}
					lastLine.ResetVertical();
					for (int i = 0; i < options.OptionList.Count; i++)
					{
						Option option = options.OptionList[i];
						option.Region.Union(new RectangleF(option.Left, option.Line.Top, option.Font.Size * 1.5f, option.Line.Height));
						for (int j = option.StartIndex; j < option.StartIndex + option.Count; j++)
						{
							option.Region.Union(this.Elements[j].Region);
						}
					}
				}
			}
			options.Handled = true;
		}
		internal void AppendLine(float top, int startIndex)
		{
			new Line(top, this.DefaultFont, startIndex, this, this.docWidth, this.docLocation.X + (float)this.Margin.Left);
		}
		internal void InsertBlank(int index, int maxCharsCount, float width, bool allowCR)
		{
			foreach (Options current in this.optionss)
			{
				if (index >= current.StartIndex && index <= current.StartIndex + current.Count)
				{
					return;
				}
			}
			foreach (Blank current2 in this.blanks)
			{
				if (index >= current2.StartIndex && index <= current2.StartIndex + current2.Count)
				{
					return;
				}
			}
			int num = -1;
			for (int i = 0; i < this.blanks.Count; i++)
			{
				if (this.blanks[i].StartIndex < index)
				{
					num = i;
				}
			}
			this.blanks.Insert(num + 1, new Blank(index, 0, maxCharsCount, width, allowCR));
			if (this.ContentChanged != null)
			{
				this.ContentChanged(this, new EventArgs());
			}
		}
		internal void InsertOptions(int index, Options optionsToInsert, List<Element> elementList)
		{
			foreach (Options current in this.optionss)
			{
				if (index >= current.StartIndex && index <= current.StartIndex + current.Count)
				{
					return;
				}
			}
			foreach (Blank current2 in this.blanks)
			{
				if (index >= current2.StartIndex && index <= current2.StartIndex + current2.Count)
				{
					return;
				}
			}
			for (int i = 0; i < elementList.Count; i++)
			{
				elementList[i].Settled = false;
				this.InsertElement(index + i, elementList[i], false);
			}
			int num = index;
			for (int i = 0; i < optionsToInsert.OptionList.Count; i++)
			{
				optionsToInsert.OptionList[i].StartIndex = num;
				num += optionsToInsert.OptionList[i].Count;
			}
			int num2 = -1;
			for (int i = 0; i < this.optionss.Count; i++)
			{
				if (this.optionss[i].StartIndex < index)
				{
					num2 = i;
				}
			}
			this.optionss.Insert(num2 + 1, optionsToInsert);
			if (this.ContentChanged != null)
			{
				this.ContentChanged(this, new EventArgs());
			}
		}
		private void MoveBlankOptionsRight(int index, bool addFront)
		{
			if (addFront)
			{
				foreach (Options current in this.optionss)
				{
					if (current.StartIndex >= index)
					{
						foreach (Option current2 in current.OptionList)
						{
							current2.StartIndex++;
						}
					}
				}
				foreach (Blank current3 in this.blanks)
				{
					if (current3.StartIndex >= index)
					{
						current3.StartIndex++;
					}
				}
			}
			else
			{
				foreach (Options current in this.optionss)
				{
					if (current.StartIndex > index)
					{
						foreach (Option current2 in current.OptionList)
						{
							current2.StartIndex++;
						}
					}
				}
				foreach (Blank current3 in this.blanks)
				{
					if (current3.StartIndex > index)
					{
						current3.StartIndex++;
					}
				}
			}
		}
		private bool CheckOptionsAndBlank()
		{
			bool result;
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (this.elements[i] is OperationInfo)
				{
					OperationInfo operationInfo = this.elements[i] as OperationInfo;
					if (i > 0 && (!(this.elements[i - 1] is CharInfo) || (this.elements[i - 1] as CharInfo).Char != '\r'))
					{
						CharInfo item = new CharInfo('\r', this.elements[i - 1].Font);
						this.elements.Insert(i, item);
						this.MoveBlankOptionsRight(i, true);
						result = false;
					}
					else
					{
						if (i >= this.elements.Count - 1 || (this.elements[i + 1] is CharInfo && (this.elements[i + 1] as CharInfo).Char == '\r'))
						{
							goto IL_149;
						}
						CharInfo item = new CharInfo('\r', this.elements[i + 1].Font);
						this.elements.Insert(i + 1, item);
						this.MoveBlankOptionsRight(i + 1, true);
						result = false;
					}
					return result;
				}
				IL_149:;
			}
			foreach (Options current in this.optionss)
			{
				if (current.StartIndex > 0)
				{
					if (!(this.elements[current.StartIndex - 1] is CharInfo) || (this.elements[current.StartIndex - 1] as CharInfo).Char != '\r')
					{
						CharInfo item = new CharInfo('\r', this.elements[current.StartIndex - 1].Font);
						this.elements.Insert(current.StartIndex, item);
						this.MoveBlankOptionsRight(current.StartIndex, true);
						result = false;
						return result;
					}
				}
				if (current.StartIndex + current.Count < this.elements.Count)
				{
					if (!(this.elements[current.StartIndex + current.Count] is CharInfo) || (this.elements[current.StartIndex + current.Count] as CharInfo).Char != '\r')
					{
						CharInfo item = new CharInfo('\r', this.elements[current.StartIndex + current.Count].Font);
						this.elements.Insert(current.StartIndex + current.Count, item);
						this.MoveBlankOptionsRight(current.StartIndex + current.Count, false);
						result = false;
						return result;
					}
				}
				else
				{
					if (current.StartIndex + current.Count == this.elements.Count)
					{
						CharInfo item = new CharInfo('\r', this.DefaultFont);
						this.elements.Insert(current.StartIndex + current.Count, item);
						result = false;
						return result;
					}
				}
			}
			foreach (Blank current2 in this.blanks)
			{
				if (current2.StartIndex > 0)
				{
					if (current2.AllowCR)
					{
						if (!(this.elements[current2.StartIndex - 1] is CharInfo) || (this.elements[current2.StartIndex - 1] as CharInfo).Char != '\r')
						{
							CharInfo item = new CharInfo('\r', this.elements[current2.StartIndex - 1].Font);
							this.elements.Insert(current2.StartIndex, item);
							this.MoveBlankOptionsRight(current2.StartIndex, true);
							result = false;
							return result;
						}
					}
					else
					{
						if (!(this.elements[current2.StartIndex - 1] is CharInfo) || (this.elements[current2.StartIndex - 1] as CharInfo).Char != ' ')
						{
							CharInfo item = new CharInfo(' ', this.elements[current2.StartIndex - 1].Font);
							this.elements.Insert(current2.StartIndex, item);
							this.MoveBlankOptionsRight(current2.StartIndex, true);
							result = false;
							return result;
						}
					}
				}
				if (current2.StartIndex + current2.Count < this.elements.Count)
				{
					if (current2.AllowCR)
					{
						if (!(this.elements[current2.StartIndex + current2.Count] is CharInfo) || (this.elements[current2.StartIndex + current2.Count] as CharInfo).Char != '\r')
						{
							CharInfo item = new CharInfo('\r', this.elements[current2.StartIndex + current2.Count].Font);
							this.elements.Insert(current2.StartIndex + current2.Count, item);
							this.MoveBlankOptionsRight(current2.StartIndex + current2.Count, false);
							result = false;
							return result;
						}
					}
					else
					{
						if (!(this.elements[current2.StartIndex + current2.Count] is CharInfo) || (this.elements[current2.StartIndex + current2.Count] as CharInfo).Char != ' ')
						{
							CharInfo item = new CharInfo(' ', this.elements[current2.StartIndex + current2.Count].Font);
							this.elements.Insert(current2.StartIndex + current2.Count, item);
							this.MoveBlankOptionsRight(current2.StartIndex + current2.Count, false);
							result = false;
							return result;
						}
					}
				}
				else
				{
					if (current2.StartIndex + current2.Count == this.elements.Count)
					{
						CharInfo item;
						if (current2.AllowCR)
						{
							item = new CharInfo('\r', this.DefaultFont);
							this.elements.Insert(current2.StartIndex + current2.Count, item);
							result = false;
							return result;
						}
						item = new CharInfo(' ', this.DefaultFont);
						this.elements.Insert(current2.StartIndex + current2.Count, item);
						result = false;
						return result;
					}
				}
			}
			result = true;
			return result;
		}
		internal void PrepareToDraw(Graphics g)
		{
			while (!this.CheckOptionsAndBlank())
			{
			}
			int num = this.elements.Count;
			for (int i = 0; i < this.elements.Count; i++)
			{
				Element element = this.elements[i];
				if (!element.Sized)
				{
					if (element is CharInfo)
					{
						element.Size = CharInfo.CalCharSize(g, element as CharInfo);
					}
					else
					{
						if (element is ExpressionInfo)
						{
							(element as ExpressionInfo).ContainerExpression.RefreshRegion(g);
						}
						else
						{
							if (element is OperationInfo)
							{
								(element as OperationInfo).LayOut(g);
							}
							else
							{
								if (element is TableInfo)
								{
									(element as TableInfo).LayOut(g);
								}
							}
						}
					}
					element.OutWidth = element.Size.Width;
					element.Sized = true;
					element.Settled = false;
					num = Math.Min(i, num);
				}
			}
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (!this.elements[i].Settled)
				{
					num = Math.Min(i, num);
					break;
				}
			}
			if (num < this.elements.Count)
			{
				if (num == 0)
				{
					this.lines = new List<Line>();
					this.DocHeight = 0f;
					this.AppendLine(this.docLocation.Y + (float)this.Margin.Top, 0);
				}
				else
				{
					int num2 = this.lines.IndexOf(this.elements[num - 1].LineContainer);
					if (this.elements[num - 1] is CharInfo && (this.elements[num - 1] as CharInfo).Char == '\r')
					{
						this.lines.RemoveRange(num2 + 2, this.lines.Count - num2 - 2);
						this.DocHeight = this.LastLine.Top + this.LastLine.Height - this.docLocation.Y - (float)this.Margin.Top;
						this.LastLine.Right = this.docLocation.X + (float)this.Margin.Left;
						this.LastLine.ElementCount = 0;
						this.LastLine.ResetVertical();
					}
					else
					{
						this.lines.RemoveRange(num2 + 1, this.lines.Count - num2 - 1);
						this.DocHeight = this.LastLine.Top + this.LastLine.Height - this.docLocation.Y - (float)this.Margin.Top;
						this.LastLine.Right = this.elements[num - 1].OutLocation.X + this.elements[num - 1].OutSize.Width;
						this.LastLine.ElementCount = num - this.LastLine.StartIndex;
						this.LastLine.ResetVertical();
					}
				}
				foreach (Blank current in this.blanks)
				{
					if (current.StartIndex + current.Count >= num)
					{
						current.Refreshed = false;
					}
				}
				foreach (Options current2 in this.optionss)
				{
					if (current2.StartIndex + current2.Count >= num)
					{
						current2.Handled = false;
					}
				}
				for (int i = num; i < this.elements.Count; i++)
				{
					this.LayOut(i);
				}
				foreach (Blank current in this.blanks)
				{
					if (!current.Refreshed)
					{
						this.RefreshBlank(current);
					}
				}
				foreach (Options current2 in this.optionss)
				{
					if (!current2.Handled)
					{
						this.RefreshOptions(current2);
					}
				}
				if (this.HeightChanged != null)
				{
					this.HeightChanged(this, new EventArgs());
				}
			}
			else
			{
				this.LastLine.ElementCount = this.elements.Count - this.LastLine.StartIndex;
				this.LastLine.ResetVertical();
				this.DocHeight = this.LastLine.Top + this.LastLine.Height - this.docLocation.Y - (float)this.Margin.Top;
			}
		}
		internal void Draw(Graphics g)
		{
			this.PrepareToDraw(g);
			g.Clip = new Region(new RectangleF(this.docLocation.X, this.docLocation.Y, this.OutWidth, this.OutHeight));
			foreach (Element current in this.elements)
			{
				current.Draw(g);
			}
			foreach (Blank current2 in this.blanks)
			{
				current2.Draw(g);
			}
			foreach (Options current3 in this.optionss)
			{
				current3.Draw(g);
			}
			g.ResetClip();
		}
		internal void DrawHighLight(Graphics g, int start, int count)
		{
			for (int i = start; i < start + count; i++)
			{
				this.elements[i].DrawHighLight(g);
			}
		}
		internal void DrawHighLight(Graphics g)
		{
			foreach (Element current in this.elements)
			{
				current.DrawHighLight(g);
			}
		}
		internal void LoadXmlFromFile(string file)
		{
			string directoryName = Path.GetDirectoryName(file);
			XmlDocument xmlDocument = new XmlDocument();
			string xml = File.ReadAllText(file, Encoding.UTF8);
			xmlDocument.LoadXml(xml);
			this.LoadFromXml(xmlDocument.ChildNodes[0], directoryName, true);
		}
		internal void LoadFromXml(XmlNode xml, string dir, bool newOutLine = true)
		{
			this.elements = new List<Element>();
			this.lines = new List<Line>();
			this.blanks = new List<Blank>();
			this.optionss = new List<Options>();
			FontStyle fontStyle = FontStyle.Regular;
			string familyName = "宋体";
			float emSize = 13f;
			if (!newOutLine)
			{
				if (xml.Attributes["padding"] != null)
				{
					string[] array = xml.Attributes["padding"].Value.Split(new char[]
					{
						','
					});
					if (array.Length == 4)
					{
						this.Margin = new Padding(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), Convert.ToInt32(array[3]));
					}
				}
				else
				{
					if (xml.Attributes["Padding"] != null)
					{
						string[] array = xml.Attributes["Padding"].Value.Split(new char[]
						{
							','
						});
						if (array.Length == 4)
						{
							this.Margin = new Padding(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), Convert.ToInt32(array[3]));
						}
					}
				}
				if (xml.Attributes["style"] != null)
				{
					string value = xml.Attributes["style"].Value;
					if (value.Contains("Bold"))
					{
						fontStyle |= FontStyle.Bold;
					}
					if (value.Contains("Italic"))
					{
						fontStyle |= FontStyle.Italic;
					}
					if (value.Contains("Strikeout"))
					{
						fontStyle |= FontStyle.Strikeout;
					}
					if (value.Contains("Underline"))
					{
						fontStyle |= FontStyle.Underline;
					}
				}
				else
				{
					if (xml.Attributes["Style"] != null)
					{
						string value = xml.Attributes["Style"].Value;
						if (value.Contains("Bold"))
						{
							fontStyle |= FontStyle.Bold;
						}
						if (value.Contains("Italic"))
						{
							fontStyle |= FontStyle.Italic;
						}
						if (value.Contains("Strikeout"))
						{
							fontStyle |= FontStyle.Strikeout;
						}
						if (value.Contains("Underline"))
						{
							fontStyle |= FontStyle.Underline;
						}
					}
				}
				if (xml.Attributes["font"] != null)
				{
					familyName = xml.Attributes["font"].Value;
				}
				else
				{
					if (xml.Attributes["Font"] != null)
					{
						familyName = xml.Attributes["Font"].Value;
					}
				}
				if (xml.Attributes["fontsize"] != null)
				{
					emSize = Convert.ToSingle(xml.Attributes["fontsize"].Value);
				}
				else
				{
					if (xml.Attributes["FontSize"] != null)
					{
						emSize = Convert.ToSingle(xml.Attributes["FontSize"].Value);
					}
				}
				this.DefaultFont = new Font(familyName, emSize, fontStyle, GraphicsUnit.Pixel);
				this.docWidth = 10f;
				if (xml.Attributes["width"] != null)
				{
					this.docWidth = Convert.ToSingle(xml.Attributes["width"].Value);
				}
				else
				{
					if (xml.Attributes["Width"] != null)
					{
						this.docWidth = Convert.ToSingle(xml.Attributes["Width"].Value);
					}
				}
				if (this.parent == null)
				{
					this.docLocation = new PointF(0f, 0f);
				}
				else
				{
					this.docLocation = this.parent.OutLocation;
				}
				if (xml.Attributes["location"] != null)
				{
					string[] array = xml.Attributes["location"].Value.Split(new char[]
					{
						','
					});
					if (array.Length == 2 && this.Parent != null)
					{
						this.docLocation = new PointF(Convert.ToSingle(array[0]) + this.Parent.OutLocation.X, Convert.ToSingle(array[1]) + this.Parent.OutLocation.Y);
					}
				}
				else
				{
					if (xml.Attributes["Location"] != null)
					{
						string[] array = xml.Attributes["Location"].Value.Split(new char[]
						{
							','
						});
						if (array.Length == 2 && this.Parent != null)
						{
							this.docLocation = new PointF(Convert.ToSingle(array[0]) + this.Parent.OutLocation.X, Convert.ToSingle(array[1]) + this.Parent.OutLocation.Y);
						}
					}
				}
				this.highLightColor = NativeMethods.MixColor(Color.White, SystemColors.Highlight);
				if (xml.Attributes["color"] != null)
				{
					string value2 = xml.Attributes["color"].Value;
					try
					{
						this.highLightColor = NativeMethods.MixColor(Color.FromArgb(Convert.ToInt32(value2, 16)), SystemColors.Highlight);
					}
					catch
					{
						this.highLightColor = NativeMethods.MixColor(Color.White, SystemColors.Highlight);
					}
				}
				else
				{
					if (xml.Attributes["Color"] != null)
					{
						string value2 = xml.Attributes["Color"].Value;
						try
						{
							this.highLightColor = NativeMethods.MixColor(Color.FromArgb(Convert.ToInt32(value2, 16)), SystemColors.Highlight);
						}
						catch
						{
							this.highLightColor = NativeMethods.MixColor(Color.White, SystemColors.Highlight);
						}
					}
				}
			}
			Font font = this.DefaultFont;
			this.AppendLine(this.docLocation.Y + (float)this.Margin.Top, 0);
			foreach (XmlNode xmlNode in xml.ChildNodes)
			{
				if (xmlNode.Attributes["style"] != null)
				{
					string value = xmlNode.Attributes["style"].Value;
					if (value.Contains("Bold"))
					{
						fontStyle |= FontStyle.Bold;
					}
					if (value.Contains("Italic"))
					{
						fontStyle |= FontStyle.Italic;
					}
					if (value.Contains("Strikeout"))
					{
						fontStyle |= FontStyle.Strikeout;
					}
					if (value.Contains("Underline"))
					{
						fontStyle |= FontStyle.Underline;
					}
				}
				else
				{
					if (xmlNode.Attributes["Style"] != null)
					{
						string value = xmlNode.Attributes["Style"].Value;
						if (value.Contains("Bold"))
						{
							fontStyle |= FontStyle.Bold;
						}
						if (value.Contains("Italic"))
						{
							fontStyle |= FontStyle.Italic;
						}
						if (value.Contains("Strikeout"))
						{
							fontStyle |= FontStyle.Strikeout;
						}
						if (value.Contains("Underline"))
						{
							fontStyle |= FontStyle.Underline;
						}
					}
				}
				if (xmlNode.Attributes["font"] != null)
				{
					familyName = xmlNode.Attributes["font"].Value;
				}
				else
				{
					if (xmlNode.Attributes["Font"] != null)
					{
						familyName = xmlNode.Attributes["Font"].Value;
					}
				}
				if (xmlNode.Attributes["fontsize"] != null)
				{
					emSize = Convert.ToSingle(xmlNode.Attributes["fontsize"].Value);
				}
				else
				{
					if (xmlNode.Attributes["FontSize"] != null)
					{
						emSize = Convert.ToSingle(xmlNode.Attributes["FontSize"].Value);
					}
				}
				font = new Font(familyName, emSize, fontStyle, GraphicsUnit.Pixel);
				if (xmlNode.Name == "text")
				{
					string text = Document.FromEscape(xmlNode.InnerText);
					char[] array2 = text.ToCharArray();
					for (int i = 0; i < array2.Length; i++)
					{
						char ch = array2[i];
						this.elements.Add(new CharInfo(ch, font));
					}
				}
				else
				{
					if (xmlNode.Name == "expr")
					{
						foreach (XmlNode node in xmlNode.ChildNodes)
						{
							containerexpression expr = new containerexpression(node, font);
							this.elements.Add(new ExpressionInfo(expr, font));
						}
					}
					else
					{
						if (xmlNode.Name == "img")
						{
							FileStream fileStream = File.OpenRead(Path.Combine(dir, xmlNode.Attributes["src"].Value));
							int num = (int)fileStream.Length;
							Image image = Image.FromStream(fileStream);
							fileStream.Close();
							float num2;
							if (xmlNode.Attributes["width"] != null)
							{
								num2 = Convert.ToSingle(xmlNode.Attributes["width"].Value);
							}
							else
							{
								if (xmlNode.Attributes["Width"] != null)
								{
									num2 = Convert.ToSingle(xmlNode.Attributes["Width"].Value);
								}
								else
								{
									num2 = (float)image.Width;
								}
							}
							float height;
							if (xmlNode.Attributes["height"] != null)
							{
								height = Convert.ToSingle(xmlNode.Attributes["height"].Value);
							}
							else
							{
								if (xmlNode.Attributes["Height"] != null)
								{
									height = Convert.ToSingle(xmlNode.Attributes["Height"].Value);
								}
								else
								{
									height = (float)image.Height;
								}
							}
							PictureInfo pictureInfo = new PictureInfo(image, font, new SizeF(num2, height));
							this.elements.Add(pictureInfo);
							for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
							{
								Document document = new Document(default(Padding), font, pictureInfo, 0f, new PointF(0f, 0f), Color.Transparent);
								document.LoadFromXml(xmlNode.ChildNodes[j], dir, true);
							}
						}
						else
						{
							if (xmlNode.Name == "table")
							{
								int k = 0;
								int num3 = 0;
								if (xmlNode.Attributes["x"] != null)
								{
									k = Convert.ToInt32(xmlNode.Attributes["x"].Value);
								}
								else
								{
									if (xmlNode.Attributes["X"] != null)
									{
										k = Convert.ToInt32(xmlNode.Attributes["X"].Value);
									}
								}
								if (xmlNode.Attributes["y"] != null)
								{
									num3 = Convert.ToInt32(xmlNode.Attributes["y"].Value);
								}
								else
								{
									if (xmlNode.Attributes["Y"] != null)
									{
										num3 = Convert.ToInt32(xmlNode.Attributes["Y"].Value);
									}
								}
								float num2 = 0f;
								float num4 = 0f;
								if (xmlNode.Attributes["width"] != null)
								{
									num2 = Convert.ToSingle(xmlNode.Attributes["width"].Value);
								}
								else
								{
									if (xmlNode.Attributes["Width"] != null)
									{
										num2 = Convert.ToSingle(xmlNode.Attributes["Width"].Value);
									}
								}
								if (xmlNode.Attributes["linewidth"] != null)
								{
									num4 = Convert.ToSingle(xmlNode.Attributes["linewidth"].Value);
								}
								else
								{
									if (xmlNode.Attributes["LineWidth"] != null)
									{
										num4 = Convert.ToSingle(xmlNode.Attributes["LineWidth"].Value);
									}
								}
								if (k > 0 && num3 > 0 && num2 > 0f && num4 > 0f)
								{
									Point tableSize = new Point(k, num3);
									TableInfo tableInfo = new TableInfo(tableSize, num2, font, num4);
									this.elements.Add(tableInfo);
									for (int j = 0; j < Math.Min(tableInfo.Items.Count, xmlNode.ChildNodes.Count); j++)
									{
										tableInfo.Items[j].LoadFromXml(xmlNode.ChildNodes[j], dir, false);
									}
								}
							}
							else
							{
								if (xmlNode.Name == "operate")
								{
									if (xmlNode.Attributes["Engine"] != null && xmlNode.Attributes["Path"] != null && xmlNode.Attributes["RTF"] != null)
									{
										string value3 = xmlNode.Attributes["Engine"].Value;
										string value4 = xmlNode.Attributes["Path"].Value;
										string value5 = xmlNode.Attributes["RTF"].Value;
										string gif;
										if (xmlNode.Attributes["GIF"] != null)
										{
											gif = Path.Combine(dir, xmlNode.Attributes["GIF"].Value);
										}
										else
										{
											gif = "";
										}
										if (value3.ToLower() == "flash")
										{
											this.elements.Add(new OperationInfo(OperationInfo.OperationType.Flash, font, Path.Combine(dir, value4), Path.Combine(dir, value5), gif));
										}
										else
										{
											if (value3.ToLower() == "vb")
											{
												this.elements.Add(new OperationInfo(OperationInfo.OperationType.VisualBasic, font, Path.Combine(dir, value4), Path.Combine(dir, value5), gif));
											}
											else
											{
												if (value3.ToLower() == "access")
												{
													this.elements.Add(new OperationInfo(OperationInfo.OperationType.Access, font, Path.Combine(dir, value4), Path.Combine(dir, value5), gif));
												}
												else
												{
													if (value3.ToLower() == "photoshop")
													{
														this.elements.Add(new OperationInfo(OperationInfo.OperationType.PhotoShop, font, Path.Combine(dir, value4), Path.Combine(dir, value5), gif));
													}
												}
											}
										}
									}
								}
								else
								{
									if (xmlNode.Name == "edit")
									{
										int num5;
										if (xmlNode.Attributes["width"] != null)
										{
											num5 = Convert.ToInt32(xmlNode.Attributes["width"].Value);
										}
										else
										{
											if (xmlNode.Attributes["Width"] != null)
											{
												num5 = Convert.ToInt32(xmlNode.Attributes["Width"].Value);
											}
											else
											{
												num5 = -1;
											}
										}
										int num6;
										if (xmlNode.Attributes["minlength"] != null)
										{
											num6 = Convert.ToInt32(xmlNode.Attributes["minlength"].Value);
										}
										else
										{
											if (xmlNode.Attributes["MinLength"] != null)
											{
												num6 = Convert.ToInt32(xmlNode.Attributes["MinLength"].Value);
											}
											else
											{
												if (num5 <= 0)
												{
													num6 = 50;
												}
												else
												{
													num6 = num5 * 10;
												}
											}
										}
										bool cr = false;
										if (xmlNode.Attributes["allowcr"] != null)
										{
											cr = Convert.ToBoolean(xmlNode.Attributes["allowcr"].Value);
										}
										else
										{
											if (xmlNode.Attributes["AllowCR"] != null)
											{
												cr = Convert.ToBoolean(xmlNode.Attributes["AllowCR"].Value);
											}
										}
										int l;
										if (xmlNode.Attributes["count"] != null)
										{
											l = Convert.ToInt32(xmlNode.Attributes["count"].Value);
										}
										else
										{
											if (xmlNode.Attributes["Count"] != null)
											{
												l = Convert.ToInt32(xmlNode.Attributes["Count"].Value);
											}
											else
											{
												l = 0;
											}
										}
										this.Blanks.Add(new Blank(this.Elements.Count, l, num5, (float)num6, cr));
									}
									else
									{
										if (xmlNode.Name == "select")
										{
											bool multiple;
											if (xmlNode.Attributes["multiple"] != null)
											{
												multiple = Convert.ToBoolean(xmlNode.Attributes["multiple"].Value);
											}
											else
											{
												multiple = (xmlNode.Attributes["Multiple"] != null && Convert.ToBoolean(xmlNode.Attributes["Multiple"].Value));
											}
											bool randomized;
											if (xmlNode.Attributes["randomized"] != null)
											{
												randomized = Convert.ToBoolean(xmlNode.Attributes["randomized"].Value);
											}
											else
											{
												randomized = (xmlNode.Attributes["Randomized"] != null && Convert.ToBoolean(xmlNode.Attributes["Randomized"].Value));
											}
											Options options = new Options(multiple, randomized);
											int num7 = this.Elements.Count;
											options.RandOrder = "";
											for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
											{
												int l;
												if (xmlNode.ChildNodes[j].Attributes["count"] != null)
												{
													l = Convert.ToInt32(xmlNode.ChildNodes[j].Attributes["count"].Value);
												}
												else
												{
													if (xmlNode.ChildNodes[j].Attributes["Count"] != null)
													{
														l = Convert.ToInt32(xmlNode.ChildNodes[j].Attributes["Count"].Value);
													}
													else
													{
														l = 0;
													}
												}
												Option option = new Option(num7, l, ((char)(65 + j)).ToString());
												bool @checked;
												if (xmlNode.ChildNodes[j].Attributes["checked"] != null)
												{
													@checked = Convert.ToBoolean(xmlNode.ChildNodes[j].Attributes["checked"].Value);
												}
												else
												{
													@checked = (xmlNode.ChildNodes[j].Attributes["Checked"] != null && Convert.ToBoolean(xmlNode.ChildNodes[j].Attributes["Checked"].Value));
												}
												option.Checked = @checked;
												options.OptionList.Add(option);
												num7 += l;
												Options expr_158B = options;
												expr_158B.RandOrder += j.ToString();
											}
											this.optionss.Add(options);
										}
										else
										{
											if (xmlNode.Name == "judge")
											{
												TrueOrFalse item = new TrueOrFalse(this.Elements.Count);
												this.optionss.Add(item);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			foreach (Options options in this.optionss)
			{
				Options options4;
				if (options4.Randomized)
				{
					int startIndex = options4.StartIndex;
					int l = options.Count;
					List<Element> list = new List<Element>();
					List<Option> list2 = new List<Option>();
					list2.AddRange(options4.OptionList);
					while (l > 0)
					{
						list.Add(this.elements[startIndex]);
						this.elements.RemoveAt(startIndex);
						l--;
					}
					long ticks = DateTime.Now.Ticks;
					Random random = new Random((int)(ticks & (long)((ulong)-1)) | (int)(ticks >> 32));
					options4.OptionList = new List<Option>();
					options4.RandOrder = "";
					for (int k = 0; k < list2.Count; k++)
					{
						Option option = list2[k];
						int num8 = random.Next(options.OptionList.Count + 1);
						int num9 = startIndex;
						for (int j = 0; j < num8; j++)
						{
							num9 += options.OptionList[j].Count;
						}
						for (int j = 0; j < option.Count; j++)
						{
							this.elements.Insert(j + num9, list[option.StartIndex - startIndex + j]);
						}
						option.StartIndex = num9;
						for (int j = num8; j < options.OptionList.Count; j++)
						{
							options.OptionList[j].StartIndex += option.Count;
						}
						options.OptionList.Insert(num8, option);
						options.RandOrder = options.RandOrder.Insert(num8, k.ToString());
					}
				}
			}
		}
		internal string toXml(string PathToSave)
		{
			string text = "";
			float num;
			float num2;
			if (this.Parent == null)
			{
				num = 0f;
				num2 = 0f;
			}
			else
			{
				num = this.docLocation.X - this.Parent.OutLocation.X;
				num2 = this.docLocation.Y - this.Parent.OutLocation.Y;
			}
			text = string.Concat(new string[]
			{
				"<doc Padding=\"",
				this.Margin.Left.ToString(),
				",",
				this.Margin.Top.ToString(),
				",",
				this.Margin.Right.ToString(),
				",",
				this.Margin.Bottom.ToString(),
				"\""
			});
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				" Font=\"",
				this.DefaultFont.FontFamily.Name,
				"\" FontSize=\"",
				this.DefaultFont.Size.ToString(),
				"\" Style=\"",
				this.DefaultFont.Style.ToString(),
				"\""
			});
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				" Width=\"",
				this.DocWidth.ToString(),
				"\" Location=\"",
				num.ToString(),
				",",
				num2.ToString(),
				"\""
			});
			text += ">";
			if (this.Elements.Count > 0)
			{
				ContentType contentType;
				if (this.elements[0] is CharInfo)
				{
					contentType = ContentType.Text;
				}
				else
				{
					if (this.elements[0] is ExpressionInfo)
					{
						contentType = ContentType.Expr;
					}
					else
					{
						if (this.elements[0] is TableInfo)
						{
							contentType = ContentType.Table;
						}
						else
						{
							contentType = ContentType.Image;
						}
					}
				}
				Font font = this.elements[0].Font;
				Color right = Color.Black;
				foreach (Blank current in this.blanks)
				{
					if (current.StartIndex == 0)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"<edit Width=\"",
							current.MaxCharsCount.ToString(),
							"\" MinLength=\"",
							current.MinLength.ToString(),
							"\" Count=\"",
							current.Count.ToString(),
							"\"/>"
						});
						break;
					}
				}
				foreach (Options current2 in this.optionss)
				{
					if (current2.StartIndex == 0)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"<select Multiple=\"",
							current2.Multiple.ToString(),
							"\" Randomized=\"",
							current2.Randomized.ToString(),
							"\">"
						});
						for (int i = 0; i < current2.OptionList.Count; i++)
						{
							object obj = text;
							text = string.Concat(new object[]
							{
								obj,
								"<option Count=\"",
								current2.OptionList[i].Count,
								"\" Checked=\"",
								current2.OptionList[i].Checked,
								"\"/>"
							});
						}
						text += "</select>";
						break;
					}
				}
				Element element = this.Elements[0];
				if (element is ExpressionInfo)
				{
					contentType = ContentType.Expr;
					text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"<expr Font=\"",
						element.Font.FontFamily.Name,
						"\" FontSize=\"",
						element.Font.Size.ToString(),
						"\" Style=\"",
						element.Font.Style.ToString(),
						"\">",
						(element as ExpressionInfo).ContainerExpression.ToXml(),
						"</expr>"
					});
				}
				else
				{
					if (element is TableInfo)
					{
						contentType = ContentType.Table;
						TableInfo tableInfo = element as TableInfo;
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"<table X=\"",
							tableInfo.TableSize.X.ToString(),
							"\" Y=\"",
							tableInfo.TableSize.Y.ToString(),
							"\" Width=\"",
							tableInfo.Width.ToString(),
							"\" LineWidth=\"",
							tableInfo.LineWidth.ToString(),
							"\" Font=\"",
							tableInfo.Font.FontFamily.Name,
							"\" FontSize=\"",
							tableInfo.Font.Size.ToString(),
							"\" Style=\"",
							tableInfo.Font.Style.ToString(),
							"\">"
						});
						foreach (Cell current3 in tableInfo.Items)
						{
							text += current3.toXml(PathToSave);
						}
						text += "</table>";
					}
					else
					{
						if (element is PictureInfo)
						{
							contentType = ContentType.Image;
							PictureInfo pictureInfo = element as PictureInfo;
							string path = "tmp";
							pictureInfo.Image.Save(Path.Combine(PathToSave, path), pictureInfo.Image.RawFormat);
							string mD5HashFromFile = NativeMethods.GetMD5HashFromFile(Path.Combine(PathToSave, path));
							File.Move(Path.Combine(PathToSave, path), Path.Combine(PathToSave, mD5HashFromFile));
							text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								"<img src=\"",
								mD5HashFromFile,
								"\" Width=\"",
								pictureInfo.ImageShowSize.Width.ToString(),
								"\" Height=\"",
								pictureInfo.ImageShowSize.Height.ToString(),
								"\" Font=\"",
								pictureInfo.Font.FontFamily.Name,
								"\" FontSize=\"",
								pictureInfo.Font.Size.ToString(),
								"\" Style=\"",
								pictureInfo.Font.Style.ToString(),
								"\">"
							});
							foreach (Document current4 in pictureInfo.Documents)
							{
								text += current4.toXml(PathToSave);
							}
							text += "</img>";
						}
						else
						{
							contentType = ContentType.Text;
							CharInfo charInfo = element as CharInfo;
							right = charInfo.Color;
							text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								"<text Font=\"",
								charInfo.Font.FontFamily.Name,
								"\" FontSize=\"",
								charInfo.Font.Size.ToString(),
								"\" Style=\"",
								charInfo.Font.Style.ToString(),
								"\" Color=\"",
								charInfo.Color.ToArgb().ToString("x8"),
								"\">",
								Document.ToEscape(charInfo.Char)
							});
						}
					}
				}
				for (int i = 1; i < this.Elements.Count; i++)
				{
					bool flag = false;
					foreach (Blank current in this.blanks)
					{
						if (current.StartIndex == i)
						{
							if (contentType == ContentType.Text)
							{
								text += "</text>";
							}
							text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								"<edit Width=\"",
								current.MaxCharsCount.ToString(),
								"\" MinLength=\"",
								current.MinLength.ToString(),
								"\" Count=\"",
								current.Count.ToString(),
								"\"/>"
							});
							flag = true;
						}
					}
					foreach (Options current2 in this.optionss)
					{
						if (current2.StartIndex == i)
						{
							if (contentType == ContentType.Text)
							{
								text += "</text>";
							}
							text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								"<select Multiple=\"",
								current2.Multiple.ToString(),
								"\" Randomized=\"",
								current2.Randomized.ToString(),
								"\">"
							});
							for (int j = 0; j < current2.OptionList.Count; j++)
							{
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									"<option Count=\"",
									current2.OptionList[j].Count,
									"\" Checked=\"",
									current2.OptionList[j].Checked,
									"\"/>"
								});
							}
							text += "</select>";
							flag = true;
						}
					}
					element = this.Elements[i];
					if (contentType == ContentType.Text && !(element is CharInfo) && !flag)
					{
						text += "</text>";
					}
					if (element is ExpressionInfo)
					{
						text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							"<expr Font=\"",
							element.Font.FontFamily.Name,
							"\" FontSize=\"",
							element.Font.Size.ToString(),
							"\" Style=\"",
							element.Font.Style.ToString(),
							"\">",
							(element as ExpressionInfo).ContainerExpression.ToXml(),
							"</expr>"
						});
					}
					else
					{
						if (element is TableInfo)
						{
							TableInfo tableInfo = element as TableInfo;
							text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								"<table X=\"",
								tableInfo.TableSize.X.ToString(),
								"\" Y=\"",
								tableInfo.TableSize.Y.ToString(),
								"\" Width=\"",
								tableInfo.Width.ToString(),
								"\" LineWidth=\"",
								tableInfo.LineWidth.ToString(),
								"\" Font=\"",
								tableInfo.Font.FontFamily.Name,
								"\" FontSize=\"",
								tableInfo.Font.Size.ToString(),
								"\" Style=\"",
								tableInfo.Font.Style.ToString(),
								"\">"
							});
							foreach (Cell current3 in tableInfo.Items)
							{
								text += current3.toXml(PathToSave);
							}
							text += "</table>";
						}
						else
						{
							if (element is PictureInfo)
							{
								PictureInfo pictureInfo = element as PictureInfo;
								string path = "tmp";
								pictureInfo.Image.Save(Path.Combine(PathToSave, path), pictureInfo.Image.RawFormat);
								string mD5HashFromFile = NativeMethods.GetMD5HashFromFile(Path.Combine(PathToSave, path));
								File.Move(Path.Combine(PathToSave, path), Path.Combine(PathToSave, mD5HashFromFile));
								text2 = text;
								text = string.Concat(new string[]
								{
									text2,
									"<img src=\"",
									mD5HashFromFile,
									"\" Width=\"",
									pictureInfo.ImageShowSize.Width.ToString(),
									"\" Height=\"",
									pictureInfo.ImageShowSize.Height.ToString(),
									"\" Font=\"",
									pictureInfo.Font.FontFamily.Name,
									"\" FontSize=\"",
									pictureInfo.Font.Size.ToString(),
									"\" Style=\"",
									pictureInfo.Font.Style.ToString(),
									"\">"
								});
								foreach (Document current4 in pictureInfo.Documents)
								{
									text += current4.toXml(PathToSave);
								}
								text += "</img>";
							}
							else
							{
								CharInfo charInfo2 = element as CharInfo;
								if (!flag)
								{
									if (charInfo2.Font == font && charInfo2.Color == right && contentType == ContentType.Text)
									{
										text += Document.ToEscape(charInfo2.Char);
									}
									else
									{
										if (contentType == ContentType.Text)
										{
											text += "</text>";
										}
										font = charInfo2.Font;
										right = charInfo2.Color;
										text2 = text;
										text = string.Concat(new string[]
										{
											text2,
											"<text Font=\"",
											charInfo2.Font.FontFamily.Name,
											"\" FontSize=\"",
											charInfo2.Font.Size.ToString(),
											"\" Style=\"",
											charInfo2.Font.Style.ToString(),
											"\" Color=\"",
											charInfo2.Color.ToArgb().ToString("x8"),
											"\">",
											Document.ToEscape(charInfo2.Char)
										});
									}
								}
								else
								{
									font = charInfo2.Font;
									right = charInfo2.Color;
									text2 = text;
									text = string.Concat(new string[]
									{
										text2,
										"<text Font=\"",
										charInfo2.Font.FontFamily.Name,
										"\" FontSize=\"",
										charInfo2.Font.Size.ToString(),
										"\" Style=\"",
										charInfo2.Font.Style.ToString(),
										"\" Color=\"",
										charInfo2.Color.ToArgb().ToString("x8"),
										"\">",
										Document.ToEscape(charInfo2.Char)
									});
								}
							}
						}
					}
					if (element is CharInfo)
					{
						contentType = ContentType.Text;
					}
					else
					{
						if (element is ExpressionInfo)
						{
							contentType = ContentType.Expr;
						}
						else
						{
							if (element is TableInfo)
							{
								contentType = ContentType.Table;
							}
							else
							{
								if (element is PictureInfo)
								{
									contentType = ContentType.Image;
								}
							}
						}
					}
				}
				if (contentType == ContentType.Text)
				{
					text += "</text>";
				}
			}
			foreach (Blank current in this.blanks)
			{
				if (current.StartIndex == this.elements.Count)
				{
					text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"<edit Width=\"",
						current.MaxCharsCount.ToString(),
						"\" MinLength=\"",
						current.MinLength.ToString(),
						"\" Count=\"",
						current.Count.ToString(),
						"\"/>"
					});
					break;
				}
			}
			foreach (Options current2 in this.optionss)
			{
				if (current2.StartIndex == this.elements.Count)
				{
					text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"<select Multiple=\"",
						current2.Multiple.ToString(),
						"\" Randomized=\"",
						current2.Randomized.ToString(),
						"\"/>"
					});
					for (int i = 0; i < current2.OptionList.Count; i++)
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							"<option Count=\"",
							current2.OptionList[i].Count,
							"\" Checked=\"",
							current2.OptionList[i].Checked,
							"\"/>"
						});
					}
					text += "</select>";
					break;
				}
			}
			return text + "</doc>";
		}
		internal string getContent()
		{
			List<int> list = new List<int>();
			List<string> list2 = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Blank current in this.blanks)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int i = current.StartIndex; i < current.StartIndex + current.Count; i++)
				{
					if (this.Elements[i] is CharInfo)
					{
						CharInfo charInfo = this.Elements[i] as CharInfo;
						if (charInfo.Char == '\r')
						{
							stringBuilder2.Append('\u001d');
						}
						else
						{
							if (charInfo.Char != ' ' || !stringBuilder2.ToString().EndsWith(" "))
							{
								stringBuilder2.Append(charInfo.Char);
							}
						}
					}
					else
					{
						if (this.Elements[i] is ExpressionInfo)
						{
							stringBuilder2.Append((this.Elements[i] as ExpressionInfo).ContainerExpression.ToString());
						}
					}
				}
				list.Add(current.StartIndex);
				list2.Add(stringBuilder2.ToString());
			}
			foreach (Options current2 in this.optionss)
			{
				string text = "";
				for (int i = 0; i < current2.OptionList.Count; i++)
				{
					if (current2.OptionList[i].Checked)
					{
						text += ('A' + current2.RandOrder[i] - '0').ToString();
					}
				}
				list.Add(current2.StartIndex);
				list2.Add(text);
			}
			foreach (Element current3 in this.elements)
			{
				if (current3 is OperationInfo)
				{
					string text;
					if ((current3 as OperationInfo).Opened)
					{
						text = (current3 as OperationInfo).OperationID + ".zip";
					}
					else
					{
						text = "";
					}
					list.Add(current3.Index);
					list2.Add(text);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list.Count - 1 - i; j++)
				{
					if (list[j] > list[j + 1])
					{
						int value = list[j];
						list[j] = list[j + 1];
						list[j + 1] = value;
						string value2 = list2[j];
						list2[j] = list2[j + 1];
						list2[j + 1] = value2;
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append(list2[i] + '\u001e'.ToString());
			}
			foreach (Element current3 in this.elements)
			{
				if (current3 is TableInfo)
				{
					TableInfo tableInfo = current3 as TableInfo;
					foreach (Cell current4 in tableInfo.Items)
					{
						string text = current4.getContent();
						if (text != "")
						{
							stringBuilder.Append(text + '\u001e'.ToString());
						}
					}
				}
				else
				{
					if (current3 is PictureInfo)
					{
						PictureInfo pictureInfo = current3 as PictureInfo;
						foreach (Document current5 in pictureInfo.Documents)
						{
							string text = current5.getContent();
							if (text != "")
							{
								stringBuilder.Append(text + '\u001e'.ToString());
							}
						}
					}
				}
			}
			string text2 = stringBuilder.ToString();
			if (text2.Length > 0)
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			return text2;
		}
		internal string getContentXml()
		{
			List<int> list = new List<int>();
			List<string> list2 = new List<string>();
			string text = "";
			foreach (Blank current in this.Blanks)
			{
				string text2 = "";
				if (current.Count > 0)
				{
					ContentType contentType;
					if (this.elements[current.StartIndex] is CharInfo)
					{
						contentType = ContentType.Text;
					}
					else
					{
						if (this.elements[current.StartIndex] is ExpressionInfo)
						{
							contentType = ContentType.Expr;
						}
						else
						{
							if (this.elements[current.StartIndex] is TableInfo)
							{
								contentType = ContentType.Table;
							}
							else
							{
								contentType = ContentType.Image;
							}
						}
					}
					Font font = this.elements[current.StartIndex].Font;
					Color right = Color.Black;
					Element element = this.Elements[current.StartIndex];
					if (element is ExpressionInfo)
					{
						contentType = ContentType.Expr;
						string text3 = text2;
						text2 = string.Concat(new string[]
						{
							text3,
							"<expr Font=\"",
							element.Font.FontFamily.Name,
							"\" FontSize=\"",
							element.Font.Size.ToString(),
							"\" Style=\"",
							element.Font.Style.ToString(),
							"\">",
							(element as ExpressionInfo).ContainerExpression.ToXml(),
							"</expr>"
						});
					}
					else
					{
						if (element is CharInfo)
						{
							contentType = ContentType.Text;
							CharInfo charInfo = element as CharInfo;
							right = charInfo.Color;
							string text3 = text2;
							text2 = string.Concat(new string[]
							{
								text3,
								"<text Font=\"",
								charInfo.Font.FontFamily.Name,
								"\" FontSize=\"",
								charInfo.Font.Size.ToString(),
								"\" Style=\"",
								charInfo.Font.Style.ToString(),
								"\" Color=\"",
								charInfo.Color.ToArgb().ToString("x8"),
								"\">",
								Document.ToEscape(charInfo.Char)
							});
						}
					}
					for (int i = current.StartIndex + 1; i < current.StartIndex + current.Count; i++)
					{
						element = this.Elements[i];
						if (contentType == ContentType.Text && !(element is CharInfo))
						{
							text2 += "</text>";
						}
						if (element is ExpressionInfo)
						{
							string text3 = text2;
							text2 = string.Concat(new string[]
							{
								text3,
								"<expr Font=\"",
								element.Font.FontFamily.Name,
								"\" FontSize=\"",
								element.Font.Size.ToString(),
								"\" Style=\"",
								element.Font.Style.ToString(),
								"\">",
								(element as ExpressionInfo).ContainerExpression.ToXml(),
								"</expr>"
							});
						}
						else
						{
							if (element is CharInfo)
							{
								CharInfo charInfo2 = element as CharInfo;
								if (charInfo2.Font == font && charInfo2.Color == right && contentType == ContentType.Text)
								{
									text2 += Document.ToEscape(charInfo2.Char);
								}
								else
								{
									if (contentType == ContentType.Text)
									{
										text2 += "</text>";
									}
									font = charInfo2.Font;
									right = charInfo2.Color;
									string text3 = text2;
									text2 = string.Concat(new string[]
									{
										text3,
										"<text Font=\"",
										charInfo2.Font.FontFamily.Name,
										"\" FontSize=\"",
										charInfo2.Font.Size.ToString(),
										"\" Style=\"",
										charInfo2.Font.Style.ToString(),
										"\" Color=\"",
										charInfo2.Color.ToArgb().ToString("x8"),
										"\">",
										Document.ToEscape(charInfo2.Char)
									});
								}
							}
						}
						if (element is CharInfo)
						{
							contentType = ContentType.Text;
						}
						else
						{
							if (element is ExpressionInfo)
							{
								contentType = ContentType.Expr;
							}
							else
							{
								if (element is TableInfo)
								{
									contentType = ContentType.Table;
								}
								else
								{
									if (element is PictureInfo)
									{
										contentType = ContentType.Image;
									}
								}
							}
						}
					}
					if (contentType == ContentType.Text)
					{
						text2 += "</text>";
					}
				}
				list.Add(current.StartIndex);
				list2.Add(text2);
			}
			foreach (Options current2 in this.optionss)
			{
				string text2 = "rand:" + current2.RandOrder + " answer:";
				for (int i = 0; i < current2.OptionList.Count; i++)
				{
					if (current2.OptionList[i].Checked)
					{
						text2 += ('A' + current2.RandOrder[i] - '0').ToString();
					}
				}
				list.Add(current2.StartIndex);
				list2.Add(text2);
			}
			foreach (Element element in this.elements)
			{
				Element element1;
				if (element1 is OperationInfo)
				{
					string text2;
					if ((element1 as OperationInfo).Opened)
					{
						text2 = (element1 as OperationInfo).OperationID + ".zip";
					}
					else
					{
						text2 = "";
					}
					list.Add(element1.Index);
					list2.Add(text2);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list.Count - 1 - i; j++)
				{
					if (list[j] > list[j + 1])
					{
						int value = list[j];
						list[j] = list[j + 1];
						list[j + 1] = value;
						string value2 = list2[j];
						list2[j] = list2[j + 1];
						list2[j + 1] = value2;
					}
				}
			}
			text = "";
			for (int i = 0; i < list.Count; i++)
			{
				text = text + list2[i] + '\u001e'.ToString();
			}
			foreach (Element element in this.Elements)
			{
				Element element2;
				if (element2 is TableInfo)
				{
					TableInfo tableInfo = element2 as TableInfo;
					foreach (Cell current3 in tableInfo.Items)
					{
						string text2 = current3.getContentXml();
						if (text2 != "")
						{
							text = text + text2 + '\u001e'.ToString();
						}
					}
				}
				else
				{
					if (element2 is PictureInfo)
					{
						PictureInfo pictureInfo = element2 as PictureInfo;
						foreach (Document current4 in pictureInfo.Documents)
						{
							string text2 = current4.getContentXml();
							if (text2 != "")
							{
								text = text + text2 + '\u001e'.ToString();
							}
						}
					}
				}
			}
			if (text.Length > 0)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}
		internal void LoadOptionSTD(string answer)
		{
			string[] array = answer.Split(new char[]
			{
				'\u001d'
			});
			for (int i = 0; i < Math.Min(this.optionss.Count, array.Length); i++)
			{
				Options options = this.optionss[i];
				for (int j = 0; j < options.OptionList.Count; j++)
				{
					if (array[i].Contains((options.RandOrder[j] + 'A' - '0').ToString()))
					{
						Option option = options.OptionList[j];
						option.Answer = true;
						for (int k = option.StartIndex; k < option.StartIndex + option.Count; k++)
						{
							if (this.elements[k] is CharInfo)
							{
								(this.elements[k] as CharInfo).Color = Color.Red;
								this.elements[k].Settled = false;
							}
							else
							{
								if (this.elements[k] is ExpressionInfo)
								{
									(this.elements[k] as ExpressionInfo).Color = Color.Red;
									this.elements[k].Settled = false;
								}
							}
						}
					}
				}
			}
		}
		internal void FillIn(string[] strs, string filepath, string id)
		{
			if (strs.Length == this.AnswerCount)
			{
				List<int> list = new List<int>();
				List<object> list2 = new List<object>();
				foreach (Blank current in this.blanks)
				{
					while (current.Count > 0)
					{
						this.DeleteElement(current.StartIndex, true);
					}
				}
				foreach (Options current2 in this.optionss)
				{
					foreach (Option current3 in current2.OptionList)
					{
						current3.Checked = false;
					}
				}
				foreach (Blank current in this.blanks)
				{
					list2.Add(current);
					list.Add(current.StartIndex);
				}
				foreach (Options current2 in this.optionss)
				{
					list.Add(current2.StartIndex);
					list2.Add(current2);
				}
				foreach (Element current4 in this.elements)
				{
					if (current4 is OperationInfo)
					{
						list.Add(current4.Index);
						list2.Add(current4 as OperationInfo);
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = 0; j < list.Count - 1 - i; j++)
					{
						if (list[j] > list[j + 1])
						{
							int value = list[j];
							list[j] = list[j + 1];
							list[j + 1] = value;
							object value2 = list2[j];
							list2[j] = list2[j + 1];
							list2[j + 1] = value2;
						}
					}
				}
				for (int i = 0; i < list2.Count; i++)
				{
					if (list2[i] is Options)
					{
						this.setOption(list2[i] as Options, strs[i]);
					}
					else
					{
						if (list2[i] is Blank)
						{
							this.setBlank(list2[i] as Blank, strs[i]);
						}
						else
						{
							if (list2[i] is OperationInfo)
							{
								this.setOperate(list2[i] as OperationInfo, strs[i], filepath, id);
							}
						}
					}
				}
				int k = list2.Count;
				foreach (Element current4 in this.Elements)
				{
					if (current4 is TableInfo)
					{
						TableInfo tableInfo = current4 as TableInfo;
						foreach (Cell current5 in tableInfo.Items)
						{
							int answerCount = current5.AnswerCount;
							string[] array = new string[answerCount];
							int i = k;
							while (k < answerCount)
							{
								array[i - k] = strs[k];
								i++;
							}
							current5.FillIn(array, filepath, id);
							k += answerCount;
						}
					}
					else
					{
						if (current4 is PictureInfo)
						{
							PictureInfo pictureInfo = current4 as PictureInfo;
							foreach (Document current6 in pictureInfo.Documents)
							{
								int answerCount = current6.AnswerCount;
								string[] array = new string[answerCount];
								int i = k;
								while (k < answerCount)
								{
									array[i - k] = strs[k];
									i++;
								}
								current6.FillIn(array, filepath, id);
								k += answerCount;
							}
						}
					}
				}
			}
		}
		internal void setOption(Options options, string str)
		{
			string text = "";
			if (str.Contains("rand:") && str.Contains(" "))
			{
				int num = str.IndexOf("rand:");
				int num2 = str.IndexOf(" ");
				text = str.Substring(num + 5, num2 - num - 5);
				string randOrder = options.RandOrder;
				options.RandOrder = "";
				for (int i = 0; i < Math.Min(text.Length, randOrder.Length); i++)
				{
					options.RandOrder += randOrder.IndexOf(text[i]).ToString();
				}
			}
			else
			{
				options.RandOrder = "";
				for (int i = 0; i < options.OptionList.Count; i++)
				{
					options.RandOrder += i.ToString();
				}
			}
			int num3 = options.StartIndex;
			int startIndex = options.StartIndex;
			int j = options.Count;
			List<Element> list = new List<Element>();
			List<Option> list2 = new List<Option>();
			list2.AddRange(options.OptionList);
			while (j > 0)
			{
				list.Add(this.elements[num3]);
				this.elements.RemoveAt(num3);
				j--;
			}
			options.OptionList = new List<Option>();
			for (int k = 0; k < options.RandOrder.Length; k++)
			{
				int index = Convert.ToInt32(options.RandOrder[k].ToString());
				Option option = list2[index];
				for (int i = 0; i < option.Count; i++)
				{
					list[option.StartIndex - startIndex + i].Settled = false;
					this.elements.Insert(i + num3, list[option.StartIndex - startIndex + i]);
				}
				option.StartIndex = num3;
				options.OptionList.Add(option);
				num3 += option.Count;
			}
			options.RandOrder = text;
			string text2;
			if (str.Contains("answer:"))
			{
				int num = str.IndexOf("answer:");
				text2 = str.Substring(num + 7);
			}
			else
			{
				text2 = "";
			}
			for (int i = 0; i < options.OptionList.Count; i++)
			{
				if (text2.Contains((options.RandOrder[i] + 'A' - '0').ToString()))
				{
					options.OptionList[i].Checked = true;
				}
				else
				{
					options.OptionList[i].Checked = false;
				}
			}
			options.Handled = false;
		}
		public void setBlank(Blank blank, string strr)
		{
			strr = strr.Replace('\u001d', '\r');
			if (strr != "")
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.LoadXml("<doc>" + strr + "</doc>");
				}
				catch
				{
				}
				StringFormat genericTypographic = StringFormat.GenericTypographic;
				genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
				FontStyle fontStyle = FontStyle.Regular;
				string familyName = "宋体";
				float emSize = 13f;
				Font font = SystemFonts.DefaultFont;
				foreach (XmlNode xmlNode in xmlDocument.ChildNodes[0].ChildNodes)
				{
					if (xmlNode.Attributes["style"] != null)
					{
						string value = xmlNode.Attributes["style"].Value;
						if (value.Contains("Bold"))
						{
							fontStyle |= FontStyle.Bold;
						}
						if (value.Contains("Italic"))
						{
							fontStyle |= FontStyle.Italic;
						}
						if (value.Contains("Strikeout"))
						{
							fontStyle |= FontStyle.Strikeout;
						}
						if (value.Contains("Underline"))
						{
							fontStyle |= FontStyle.Underline;
						}
					}
					else
					{
						if (xmlNode.Attributes["Style"] != null)
						{
							string value = xmlNode.Attributes["Style"].Value;
							if (value.Contains("Bold"))
							{
								fontStyle |= FontStyle.Bold;
							}
							if (value.Contains("Italic"))
							{
								fontStyle |= FontStyle.Italic;
							}
							if (value.Contains("Strikeout"))
							{
								fontStyle |= FontStyle.Strikeout;
							}
							if (value.Contains("Underline"))
							{
								fontStyle |= FontStyle.Underline;
							}
						}
					}
					if (xmlNode.Attributes["font"] != null)
					{
						familyName = xmlNode.Attributes["font"].Value;
					}
					else
					{
						if (xmlNode.Attributes["Font"] != null)
						{
							familyName = xmlNode.Attributes["Font"].Value;
						}
					}
					if (xmlNode.Attributes["fontsize"] != null)
					{
						emSize = Convert.ToSingle(xmlNode.Attributes["fontsize"].Value);
					}
					else
					{
						if (xmlNode.Attributes["FontSize"] != null)
						{
							emSize = Convert.ToSingle(xmlNode.Attributes["FontSize"].Value);
						}
					}
					font = new Font(familyName, emSize, fontStyle, GraphicsUnit.Pixel);
					if (xmlNode.Name == "text")
					{
						string text = Document.FromEscape(xmlNode.InnerText);
						char[] array = text.ToCharArray();
						for (int i = 0; i < array.Length; i++)
						{
							char chr = array[i];
							this.InsertChar(blank.StartIndex + blank.Count, chr, font, true);
						}
					}
					else
					{
						if (xmlNode.Name == "expr")
						{
							foreach (XmlNode node in xmlNode.ChildNodes)
							{
								containerexpression containerExpr = new containerexpression(node, font);
								this.InsertExpression(blank.StartIndex + blank.Count, containerExpr, font, true);
							}
						}
					}
				}
			}
		}
		internal void setOperate(OperationInfo operate, string str, string filepath, string id)
		{
			operate.Review = true;
			string path = Path.Combine(filepath, id + operate.OperationID + ".zip");
			if (File.Exists(path))
			{
				byte[] buffer = File.ReadAllBytes(path);
				List<byte[]> filebyets;
				List<string> filenames;
				Qisi.General.CommonMethods.Unzip(buffer, out filebyets, out filenames, "CKKC37F423");
				operate.LoadAnswer(filebyets, filenames);
			}
		}
		private static string ToEscape(char chr)
		{
			string text = "";
			if (chr == '&')
			{
				text += "&amp;";
			}
			else
			{
				if (chr == '<')
				{
					text += "&lt;";
				}
				else
				{
					if (chr == '>')
					{
						text += "&gt;";
					}
					else
					{
						if (chr == '"')
						{
							text += "&quot;";
						}
						else
						{
							if (chr == '\'')
							{
								text += "&apos;";
							}
							else
							{
								if (chr == ' ')
								{
									text += "\\s";
								}
								else
								{
									if (chr == '\\')
									{
										text += "\\\\";
									}
									else
									{
										if (chr == '\r')
										{
											text += "\\n";
										}
										else
										{
											if (chr == '\n')
											{
												text += "\\n";
											}
											else
											{
												text += chr.ToString();
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return text;
		}
		internal static string FromEscape(string str)
		{
			str = str.Replace("&amp;", "&");
			str = str.Replace("&lt;", "<");
			str = str.Replace("&gt;", ">");
			str = str.Replace("&quot;", "\"");
			str = str.Replace("&apos;", "'");
			str = str.Replace("\\s", " ");
			str = str.Replace("\\\\", "\\");
			str = str.Replace("\\r", "\r");
			str = str.Replace("\\n", "\r");
			return str;
		}
	}
}
