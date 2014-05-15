using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class shangxiabiaoyou : structexpression
	{
		public lineexpression Benti
		{
			get
			{
				return base.Child[0];
			}
		}
		public lineexpression Shangbiao
		{
			get
			{
				return base.Child[1];
			}
		}
		public lineexpression Xiabiao
		{
			get
			{
				return base.Child[2];
			}
		}
		public shangxiabiaoyou(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Type = FType.上下标右;
			base.Child[0].ParentExpression = this;
			base.Child[1].ParentExpression = this;
			base.Child[2].ParentExpression = this;
			this.Shangbiao.DownLineExpression = this.Xiabiao;
			this.Xiabiao.UpLineExpression = this.Shangbiao;
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			float num = this.Benti.Font.Size;
			if (this.Shangbiao != null)
			{
				this.Shangbiao.ChangeFontSize(num);
			}
			if (this.Xiabiao != null)
			{
				this.Xiabiao.ChangeFontSize(num);
			}
			this.Shangbiao.RefreshRegion(g);
			this.Xiabiao.RefreshRegion(g);
			while (this.Shangbiao.Region.Height > this.Benti.Region.Height / 3f && num > expression.minfontsize)
			{
				num -= 0.5f;
				if (this.Shangbiao != null)
				{
					this.Shangbiao.ChangeFontSize(num);
				}
				this.Shangbiao.RefreshRegion(g);
			}
			num = this.Benti.Font.Size;
			while (this.Xiabiao.Region.Height > this.Benti.Region.Height / 3f && num > expression.minfontsize)
			{
				num -= 0.5f;
				if (this.Xiabiao != null)
				{
					this.Xiabiao.ChangeFontSize(num);
				}
				this.Xiabiao.RefreshRegion(g);
			}
			if (this.Xiabiao.Font.Size < this.Shangbiao.Font.Size)
			{
				this.Shangbiao.ChangeFontSize(this.Xiabiao.Font.Size);
				this.Shangbiao.RefreshRegion(g);
			}
			else
			{
				this.Xiabiao.ChangeFontSize(this.Shangbiao.Font.Size);
				this.Xiabiao.RefreshRegion(g);
			}
			float width = base.Child[0].Region.Width + Math.Max(base.Child[1].Region.Width, base.Child[2].Region.Width);
			float num2;
			if (this.Shangbiao.Region.Height < this.Benti.Region.Height)
			{
				num2 = this.Shangbiao.Region.Height / 2f;
			}
			else
			{
				num2 = this.Shangbiao.Region.Height - this.Benti.Region.Height / 2f;
			}
			base.BaseLine = num2 + this.Benti.BaseLine;
			if (this.Xiabiao.Region.Height < this.Benti.Region.Height)
			{
				num2 += this.Xiabiao.Region.Height / 2f;
			}
			else
			{
				num2 += this.Xiabiao.Region.Height - this.Benti.Region.Height / 2f;
			}
			num2 += this.Benti.Region.Height;
			base.Region = new SizeF(width, num2);
		}
		public override void RefreshInputLocation()
		{
			this.Benti.InputLocation = new PointF(base.InputLocation.X, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Shangbiao.InputLocation = new PointF(base.InputLocation.X + this.Benti.Region.Width, base.InputLocation.Y);
			this.Xiabiao.InputLocation = new PointF(base.InputLocation.X + this.Benti.Region.Width, base.InputLocation.Y + base.Region.Height - this.Xiabiao.Region.Height);
			this.Benti.RefreshInputLocation();
			this.Shangbiao.RefreshInputLocation();
			this.Xiabiao.RefreshInputLocation();
		}
	}
}
