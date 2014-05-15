using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class juzheng : structexpression
	{
		private Point Size = default(Point);
		public juzheng(Point P, lineexpression parent, Color color) : base(parent, color, true)
		{
			this.Size = P;
			for (int i = 0; i < P.X * P.Y; i++)
			{
				base.Child.Add(new lineexpression(this.Font));
			}
			base.Type = FType.矩阵;
			for (int i = 0; i < P.X * P.Y; i++)
			{
				base.Child[i].ParentExpression = this;
			}
			this.refreshUpDown();
		}
		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < base.Child.Count - 1; i++)
			{
				if ((i + 1) % this.Size.Y == 0)
				{
					text = text + base.Child[i].ToString() + "\\\\";
				}
				else
				{
					if ((i + 1) % this.Size.Y == 0)
					{
						text = text + base.Child[i].ToString() + "&";
					}
				}
			}
			text += base.Child[base.Child.Count - 1].ToString();
			return CommonMethods.ExprToString(base.Type.ToString()).Replace("<0>", text);
		}
		public override void RefreshRegion(Graphics g)
		{
			foreach (lineexpression current in base.Child)
			{
				current.ChangeFontSize(this.Font.Size);
				current.RefreshRegion(g);
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this.Size.X; i++)
			{
				float num3 = 0f;
				float num4 = 0f;
				for (int j = 0; j < this.Size.Y; j++)
				{
					if (num3 < base.Child[i * this.Size.Y + j].BaseLine)
					{
						num3 = base.Child[i * this.Size.Y + j].BaseLine;
					}
					if (num4 < base.Child[i * this.Size.Y + j].Region.Height - base.Child[i * this.Size.Y + j].BaseLine)
					{
						num4 = base.Child[i * this.Size.Y + j].Region.Height - base.Child[i * this.Size.Y + j].BaseLine;
					}
				}
				num2 += num3 + num4;
			}
			for (int j = 0; j < this.Size.Y; j++)
			{
				float num5 = 0f;
				for (int i = 0; i < this.Size.X; i++)
				{
					if (num5 < base.Child[i * this.Size.Y + j].Region.Width)
					{
						num5 = base.Child[i * this.Size.Y + j].Region.Width;
					}
				}
				num += num5;
			}
			num += (float)((this.Size.Y - 1) * this.Font.Height / 2);
			num2 += (float)((this.Size.X - 1) * this.Font.Height / 2);
			base.Region = new SizeF(num, num2);
			float num6 = CommonMethods.CalcAscentPixel(this.Font);
			base.BaseLine = base.Region.Height / 2f - this.Font.Size / 2f + num6;
		}
		public override void RefreshInputLocation()
		{
			float num = base.InputLocation.X;
			float num2 = base.InputLocation.Y;
			for (int i = 0; i < this.Size.X; i++)
			{
				float num3 = 0f;
				float num4 = 0f;
				for (int j = 0; j < this.Size.Y; j++)
				{
					if (num3 < base.Child[i * this.Size.Y + j].BaseLine)
					{
						num3 = base.Child[i * this.Size.Y + j].BaseLine;
					}
					if (num4 < base.Child[i * this.Size.Y + j].Region.Height - base.Child[i * this.Size.Y + j].BaseLine)
					{
						num4 = base.Child[i * this.Size.Y + j].Region.Height - base.Child[i * this.Size.Y + j].BaseLine;
					}
				}
				for (int j = 0; j < this.Size.Y; j++)
				{
					base.Child[i * this.Size.Y + j].InputLocation = new PointF(base.Child[i * this.Size.Y + j].InputLocation.X, num2 + num3 - base.Child[i * this.Size.Y + j].BaseLine);
				}
				num2 += num3 + num4 + (float)(this.Font.Height / 2);
			}
			for (int j = 0; j < this.Size.Y; j++)
			{
				float num5 = 0f;
				for (int i = 0; i < this.Size.X; i++)
				{
					if (num5 < base.Child[i * this.Size.Y + j].Region.Width)
					{
						num5 = base.Child[i * this.Size.Y + j].Region.Width;
					}
				}
				for (int i = 0; i < this.Size.X; i++)
				{
					base.Child[i * this.Size.Y + j].InputLocation = new PointF(num, base.Child[i * this.Size.Y + j].InputLocation.Y);
				}
				num += num5 + (float)(this.Font.Height / 2);
			}
			for (int i = 0; i < this.Size.X; i++)
			{
				for (int j = 0; j < this.Size.Y; j++)
				{
					base.Child[i * this.Size.Y + j].RefreshInputLocation();
				}
			}
		}
		public void addLine(int count)
		{
			this.Size = new Point(this.Size.X + count, this.Size.Y);
			for (int i = 0; i < count * this.Size.Y; i++)
			{
				base.Child.Add(new lineexpression(this.Font));
				base.Child[base.Child.Count - 1].ParentExpression = this;
			}
			this.refreshUpDown();
		}
		public void insertRowBefore(int Index)
		{
			int index = Index / this.Size.Y * this.Size.Y;
			for (int i = 0; i < this.Size.Y; i++)
			{
				base.Child.Insert(index, new lineexpression(this.Font));
				base.Child[index].ParentExpression = this;
			}
			this.Size = new Point(this.Size.X + 1, this.Size.Y);
			this.refreshUpDown();
		}
		public void insertRowAfter(int Index)
		{
			int index = Index / this.Size.Y * this.Size.Y + this.Size.Y;
			for (int i = 0; i < this.Size.Y; i++)
			{
				base.Child.Insert(index, new lineexpression(this.Font));
				base.Child[index].ParentExpression = this;
			}
			this.Size = new Point(this.Size.X + 1, this.Size.Y);
			this.refreshUpDown();
		}
		public void insertColumnBefore(int Index)
		{
			int num = Index % this.Size.Y;
			this.Size = new Point(this.Size.X, this.Size.Y + 1);
			for (int i = 0; i < this.Size.X; i++)
			{
				base.Child.Insert(i * this.Size.Y + num, new lineexpression(this.Font));
				base.Child[i * this.Size.Y + num].ParentExpression = this;
			}
			this.refreshUpDown();
		}
		public void insertColumnAfter(int Index)
		{
			int num = Index % this.Size.Y + 1;
			this.Size = new Point(this.Size.X, this.Size.Y + 1);
			for (int i = 0; i < this.Size.X; i++)
			{
				base.Child.Insert(i * this.Size.Y + num, new lineexpression(this.Font));
				base.Child[i * this.Size.Y + num].ParentExpression = this;
			}
			this.refreshUpDown();
		}
		public void deleteRow(int Index)
		{
			int index = Index / this.Size.Y * this.Size.Y;
			for (int i = 0; i < this.Size.Y; i++)
			{
				base.Child.RemoveAt(index);
			}
			this.Size = new Point(this.Size.X - 1, this.Size.Y);
			if (this.Size.X == 0 || this.Size.Y == 0)
			{
				base.Child = null;
			}
			this.refreshUpDown();
		}
		public void deleteCol(int Index)
		{
			int num = Index % this.Size.Y;
			int num2 = Index / this.Size.Y;
			for (int i = this.Size.X - 1; i >= 0; i--)
			{
				base.Child.RemoveAt(i * this.Size.Y + num);
			}
			this.Size = new Point(this.Size.X, this.Size.Y - 1);
			if (this.Size.X == 0 || this.Size.Y == 0)
			{
				base.Child = null;
			}
			this.refreshUpDown();
		}
		private void refreshUpDown()
		{
			for (int i = 0; i < this.Size.X * this.Size.Y; i++)
			{
				base.Child[i].UpLineExpression = ((i - this.Size.Y >= 0) ? base.Child[i - this.Size.Y] : null);
				base.Child[i].DownLineExpression = ((i + this.Size.Y < this.Size.X * this.Size.Y) ? base.Child[i + this.Size.Y] : null);
			}
		}
		public override string ToXml()
		{
			string text = "";
			text = string.Concat(new string[]
			{
				"<",
				base.Type.ToString(),
				"X=\"",
				this.Size.X.ToString(),
				"\" Y=\"",
				this.Size.Y.ToString(),
				"\" Color=\"",
				this.Color.ToArgb().ToString("x8"),
				"\">"
			});
			if (base.Child != null)
			{
				foreach (lineexpression current in base.Child)
				{
					text += current.ToXml();
				}
			}
			text = text + "</" + base.Type.ToString() + ">";
			return text;
		}
	}
}
