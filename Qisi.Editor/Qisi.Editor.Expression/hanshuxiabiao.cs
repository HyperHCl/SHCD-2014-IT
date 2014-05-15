using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class hanshuxiabiao : structexpression
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
		public lineexpression Xiabiao
		{
			get
			{
				return base.Child[1];
			}
		}
		public hanshuxiabiao(string h, lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			this.Hanshu.ParentExpression = this;
			this.Benti.ParentExpression = this;
			this.Xiabiao.ParentExpression = this;
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
				this.Xiabiao.ToString(),
				"}{",
				this.Benti.ToString(),
				"}"
			});
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			float num = this.Benti.Font.Size;
			if (this.Xiabiao != null)
			{
				this.Xiabiao.ChangeFontSize(num);
			}
			this.Xiabiao.RefreshRegion(g);
			while (this.Xiabiao.Region.Height > this.Hanshu.Region.Height / 3f && num > expression.minfontsize)
			{
				num -= 0.5f;
				if (this.Xiabiao != null)
				{
					this.Xiabiao.ChangeFontSize(num);
				}
				this.Xiabiao.RefreshRegion(g);
			}
			this.Hanshu.ChangeFontSize(this.Font.Size);
			this.Hanshu.RefreshRegion(g);
			float width = this.Benti.Region.Width + Math.Max(this.Xiabiao.Region.Width, this.Hanshu.Region.Width) + 9f;
			float height = Math.Max(this.Hanshu.BaseLine, this.Benti.BaseLine) + Math.Max(this.Hanshu.Region.Height + this.Xiabiao.Region.Height - this.Hanshu.BaseLine, this.Benti.Region.Height - this.Benti.BaseLine);
			base.Region = new SizeF(width, height);
			base.BaseLine = Math.Max(this.Hanshu.BaseLine, this.Benti.BaseLine);
		}
		public override void RefreshInputLocation()
		{
			float x;
			if (this.Xiabiao.Region.Width < this.Hanshu.Region.Width)
			{
				this.Hanshu.InputLocation = new PointF(base.InputLocation.X + 2f, base.InputLocation.Y + base.BaseLine - this.Hanshu.BaseLine);
				this.Xiabiao.InputLocation = new PointF(this.Hanshu.InputLocation.X + this.Hanshu.Region.Width / 2f - this.Xiabiao.Region.Width / 2f, this.Hanshu.InputLocation.Y + this.Hanshu.Region.Height);
				x = base.InputLocation.X + 2f + this.Hanshu.Region.Width + 5f;
			}
			else
			{
				this.Hanshu.InputLocation = new PointF(base.InputLocation.X + 2f + this.Xiabiao.Region.Width / 2f - this.Hanshu.Region.Width / 2f, base.InputLocation.Y + base.BaseLine - this.Hanshu.BaseLine);
				this.Xiabiao.InputLocation = new PointF(base.InputLocation.X + 2f, this.Hanshu.InputLocation.Y + this.Hanshu.Region.Height);
				x = base.InputLocation.X + 2f + this.Xiabiao.Region.Width + 5f;
			}
			this.Benti.InputLocation = new PointF(x, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Xiabiao.RefreshInputLocation();
			this.Benti.RefreshInputLocation();
			this.Hanshu.RefreshInputLocation();
		}
	}
}
