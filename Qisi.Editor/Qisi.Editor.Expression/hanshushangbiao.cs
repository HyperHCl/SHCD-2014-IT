using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class hanshushangbiao : structexpression
	{
		public lineexpression Hanshu
		{
			get
			{
				return base.Child[0];
			}
		}
		public lineexpression Benti
		{
			get
			{
				return base.Child[2];
			}
		}
		public lineexpression Cishu
		{
			get
			{
				return base.Child[1];
			}
		}
		public hanshushangbiao(string h, lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			this.Hanshu.ParentExpression = this;
			this.Benti.ParentExpression = this;
			this.Cishu.ParentExpression = this;
			this.Hanshu.Child = new List<structexpression>();
			char[] array = h.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				this.Hanshu.Child.Add(new charexpression(c.ToString(), this.Hanshu, color));
			}
		}
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"\\",
				this.Hanshu.ToString(),
				"^{",
				this.Cishu.ToString(),
				"}{",
				this.Benti.ToString(),
				"}"
			});
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			float num = this.Font.Size;
			if (this.Cishu != null)
			{
				this.Cishu.ChangeFontSize(num);
			}
			this.Cishu.RefreshRegion(g);
			while (this.Cishu.Region.Height > this.Hanshu.Region.Height / 3f && num > expression.minfontsize)
			{
				num -= 0.5f;
				if (this.Cishu != null)
				{
					this.Cishu.ChangeFontSize(num);
				}
				this.Cishu.RefreshRegion(g);
			}
			this.Hanshu.ChangeFontSize(this.Font.Size);
			this.Hanshu.RefreshRegion(g);
			float width = this.Benti.Region.Width + this.Cishu.Region.Width + this.Hanshu.Region.Width + 9f;
			float num2 = Math.Max(this.Cishu.Region.Height + this.Hanshu.Region.Height / 2f, this.Hanshu.Region.Height + this.Cishu.Region.Height / 2f);
			base.BaseLine = num2 - this.Hanshu.Region.Height + this.Hanshu.BaseLine;
			num2 = Math.Max(base.BaseLine, this.Benti.BaseLine) + Math.Max(num2 - base.BaseLine, this.Benti.Region.Height - this.Benti.BaseLine);
			base.Region = new SizeF(width, num2);
			base.BaseLine = Math.Max(base.BaseLine, this.Benti.BaseLine);
		}
		public override void RefreshInputLocation()
		{
			if (this.Cishu.Region.Height < this.Hanshu.Region.Height)
			{
				this.Hanshu.InputLocation = new PointF(base.InputLocation.X + 2f, base.InputLocation.Y + base.BaseLine - this.Hanshu.BaseLine);
				this.Cishu.InputLocation = new PointF(base.InputLocation.X + this.Hanshu.Region.Width + 2f, this.Hanshu.InputLocation.Y - this.Cishu.Region.Height / 2f);
			}
			else
			{
				this.Hanshu.InputLocation = new PointF(base.InputLocation.X + 2f, base.InputLocation.Y + base.BaseLine - this.Hanshu.BaseLine);
				this.Cishu.InputLocation = new PointF(base.InputLocation.X + this.Hanshu.Region.Width + 2f, this.Hanshu.InputLocation.Y + this.Hanshu.Region.Height / 2f - this.Cishu.Region.Height);
			}
			this.Benti.InputLocation = new PointF(base.InputLocation.X + this.Hanshu.Region.Width + this.Cishu.Region.Width + 2f + 5f, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Cishu.RefreshInputLocation();
			this.Benti.RefreshInputLocation();
			this.Hanshu.RefreshInputLocation();
		}
	}
}
