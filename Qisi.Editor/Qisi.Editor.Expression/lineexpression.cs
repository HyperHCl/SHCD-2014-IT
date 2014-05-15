using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
namespace Qisi.Editor.Expression
{
	internal class lineexpression : expression
	{
		private Font _font;
		public override Color Color
		{
			get
			{
				return (this.ParentExpression == null) ? Color.Black : this.ParentExpression.Color;
			}
			set
			{
				foreach (structexpression current in this.Child)
				{
					current.Color = value;
				}
			}
		}
		public new structexpression ParentExpression
		{
			get;
			set;
		}
		public new List<structexpression> Child
		{
			get;
			set;
		}
		public override Font Font
		{
			get
			{
				return this._font;
			}
		}
		public lineexpression UpLineExpression
		{
			get;
			set;
		}
		public lineexpression DownLineExpression
		{
			get;
			set;
		}
		public lineexpression(Font font)
		{
			this._font = font;
			this.Child = new List<structexpression>();
		}
		public lineexpression(XmlNode node, Font font)
		{
			this._font = font;
			if (node.HasChildNodes)
			{
				this.Child = new List<structexpression>();
				foreach (XmlNode xmlNode in node.ChildNodes)
				{
					Color color;
					if (xmlNode.Attributes["Color"] != null)
					{
						string value = xmlNode.Attributes["Color"].Value;
						try
						{
							color = Color.FromArgb(Convert.ToInt32(value, 16));
						}
						catch
						{
							color = Color.Black;
						}
					}
					else
					{
						color = Color.Black;
					}
					int matrixX = 2;
					int matrixY = 1;
					if (xmlNode.Name == "矩阵")
					{
						if (xmlNode.Attributes["X"] != null)
						{
							matrixX = Convert.ToInt32(xmlNode.Attributes["X"].Value);
						}
						if (xmlNode.Attributes["Y"] != null)
						{
							matrixY = Convert.ToInt32(xmlNode.Attributes["Y"].Value);
						}
					}
					structexpression structexpression = CommonMethods.CreateExpr(xmlNode.Name, this, color, xmlNode.InnerText, matrixX, matrixY);
					this.Child.Add(structexpression);
					if (xmlNode.HasChildNodes && structexpression.Child != null)
					{
						for (int i = 0; i < Math.Min(xmlNode.ChildNodes.Count, structexpression.Child.Count); i++)
						{
							XmlNode xmlNode2 = xmlNode.ChildNodes[i];
							FontStyle fontStyle = FontStyle.Regular;
							if (xmlNode2.Attributes["Style"] != null)
							{
								string value2 = xmlNode2.Attributes["Style"].Value;
								if (value2.Contains("Bold"))
								{
									fontStyle |= FontStyle.Bold;
								}
								if (value2.Contains("Italic"))
								{
									fontStyle |= FontStyle.Italic;
								}
								if (value2.Contains("Strikeout"))
								{
									fontStyle |= FontStyle.Strikeout;
								}
								if (value2.Contains("Underline"))
								{
									fontStyle |= FontStyle.Underline;
								}
							}
							Font font2 = new Font(font.Name, font.Size, fontStyle, font.Unit);
							lineexpression lineexpression = new lineexpression(xmlNode2, font2);
							structexpression.Child[i] = lineexpression;
							lineexpression.ParentExpression = structexpression;
						}
					}
				}
			}
		}
		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._font.Dispose();
			}
		}
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~lineexpression()
		{
			this.Dispose(false);
		}
		public override string ToString()
		{
			string result;
			if (this.Child == null)
			{
				result = "";
			}
			else
			{
				string text = "";
				foreach (expression current in this.Child)
				{
					text += current.ToString();
				}
				result = text;
			}
			return result;
		}
		public override expression PointInChild(Point point)
		{
			expression result;
			if (this.Child != null)
			{
				foreach (structexpression current in this.Child)
				{
					if (current.PointInOrNot(point))
					{
						result = current.PointInChild(point);
						return result;
					}
				}
			}
			result = this;
			return result;
		}
		public override void RefreshRegion(Graphics g)
		{
			if (this.Child.Count == 0)
			{
				g.PageUnit = GraphicsUnit.Pixel;
				StringFormat genericTypographic = StringFormat.GenericTypographic;
				genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
				base.Region = g.MeasureString("  ", this.Font, 0, genericTypographic);
				base.BaseLine = base.Region.Height / 2f - this._font.Size / 2f + CommonMethods.CalcAscentPixel(this._font);
			}
			else
			{
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				foreach (expression current in this.Child)
				{
					current.RefreshRegion(g);
					num += current.Region.Width;
					if (current.BaseLine > num2)
					{
						num2 = current.BaseLine;
					}
					if (current.Region.Height - current.BaseLine > num3)
					{
						num3 = current.Region.Height - current.BaseLine;
					}
				}
				float height = num2 + num3;
				base.Region = new SizeF(num, height);
				base.BaseLine = num2;
			}
		}
		public override void RefreshInputLocation()
		{
			if (this.Child.Count != 0)
			{
				this.Child[0].InputLocation = new PointF(base.InputLocation.X, base.InputLocation.Y + base.BaseLine - this.Child[0].BaseLine);
				this.Child[0].RefreshInputLocation();
				for (int i = 1; i < this.Child.Count; i++)
				{
					this.Child[i].InputLocation = new PointF(this.Child[i - 1].InputLocation.X + this.Child[i - 1].Region.Width, base.InputLocation.Y + base.BaseLine - this.Child[i].BaseLine);
					this.Child[i].RefreshInputLocation();
				}
			}
		}
		public override void DrawExpression(Graphics g)
		{
			if (this.Child.Count == 0)
			{
				Pen pen = new Pen(this.Color);
				pen.DashStyle = DashStyle.Dot;
				g.DrawRectangle(pen, base.InputLocation.X, base.InputLocation.Y, base.Region.Width, base.Region.Height);
				pen.Dispose();
			}
			else
			{
				foreach (structexpression current in this.Child)
				{
					current.DrawExpression(g);
				}
			}
		}
		public void ChangeFontSize(float size)
		{
			this._font = new Font(this._font.Name, Math.Max(size, expression.minfontsize), this._font.Style, this._font.Unit);
		}
		public override string ToXml()
		{
			string text = "";
			text = "<线性 Style=\"" + this.Font.Style.ToString() + "\">";
			if (this.Child != null)
			{
				foreach (structexpression current in this.Child)
				{
					text += current.ToXml();
				}
			}
			text += "</线性>";
			return text;
		}
	}
}
