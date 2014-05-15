using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal abstract class structexpression : expression
	{
		private Color _color;
		public override Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				if (this.Child != null)
				{
					foreach (lineexpression current in this.Child)
					{
						current.Color = value;
					}
				}
			}
		}
		public new lineexpression ParentExpression
		{
			get;
			set;
		}
		public new List<lineexpression> Child
		{
			get;
			set;
		}
		public override Font Font
		{
			get
			{
				return (this.ParentExpression != null) ? this.ParentExpression.Font : SystemFonts.DefaultFont;
			}
		}
		public lineexpression DefaultChild
		{
			get
			{
				lineexpression result;
				if (this.Child != null && this.Child.Count > 0)
				{
					result = this.Child[0];
				}
				else
				{
					result = null;
				}
				return result;
			}
		}
		public structexpression(lineexpression parent, Color color, bool haschild = true)
		{
			this.ParentExpression = parent;
			if (haschild)
			{
				this.Child = new List<lineexpression>();
			}
			else
			{
				this.Child = null;
			}
			this.Color = color;
		}
		~structexpression()
		{
			this.Dispose(false);
		}
		public override string ToString()
		{
			string text = CommonMethods.ExprToString(base.Type.ToString());
			string result;
			if (this.Child == null)
			{
				result = text;
			}
			else
			{
				for (int i = 0; i < this.Child.Count; i++)
				{
					text = text.Replace("<" + i.ToString() + ">", this.Child[i].ToString());
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
				foreach (lineexpression current in this.Child)
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
		public override void DrawExpression(Graphics g)
		{
			foreach (lineexpression current in this.Child)
			{
				current.DrawExpression(g);
			}
		}
		public override string ToXml()
		{
			string text = "";
			text = string.Concat(new string[]
			{
				"<",
				base.Type.ToString(),
				" Color=\"",
				this.Color.ToArgb().ToString("x8"),
				"\">"
			});
			if (this.Child != null)
			{
				foreach (lineexpression current in this.Child)
				{
					text += current.ToXml();
				}
			}
			text = text + "</" + base.Type.ToString() + ">";
			return text;
		}
	}
}
