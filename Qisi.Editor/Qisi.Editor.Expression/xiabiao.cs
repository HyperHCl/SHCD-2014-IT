using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class xiabiao : structexpression
	{
		public lineexpression Benti
		{
			get
			{
				return base.Child[0];
			}
		}
		public lineexpression Xiabiao
		{
			get
			{
				return base.Child[1];
			}
		}
		public xiabiao(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Type = FType.下标;
			this.Benti.ParentExpression = this;
			this.Xiabiao.ParentExpression = this;
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
			while (this.Xiabiao.Region.Height > this.Benti.Region.Height / 2f && num > expression.minfontsize)
			{
				num -= 0.5f;
				if (this.Xiabiao != null)
				{
					this.Xiabiao.ChangeFontSize(num);
				}
				this.Xiabiao.RefreshRegion(g);
			}
			float width = base.Child[0].Region.Width + base.Child[1].Region.Width;
			float height = Math.Max(base.Child[1].Region.Height + base.Child[0].Region.Height / 2f, base.Child[1].Region.Height / 2f + base.Child[0].Region.Height);
			base.Region = new SizeF(width, height);
			base.BaseLine = this.Benti.BaseLine;
		}
		public override void RefreshInputLocation()
		{
			this.Benti.InputLocation = new PointF(base.InputLocation.X, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Benti.RefreshInputLocation();
			this.Xiabiao.InputLocation = new PointF(base.InputLocation.X + this.Benti.Region.Width, base.InputLocation.Y + base.Region.Height - this.Xiabiao.Region.Height);
			this.Xiabiao.RefreshInputLocation();
		}
	}
}
