using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace Qisi.Editor.Expression
{
	internal abstract class type3 : imageexpression
	{
		private bool offset;
		public float offsetX
		{
			get;
			set;
		}
		public lineexpression Shangbiao
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
		public type3(Image f, lineexpression parent, Color color, bool off = false) : base(f, parent, color)
		{
			this.offset = off;
			base.Child.Insert(0, new lineexpression(this.Font));
			base.Child.Insert(0, new lineexpression(this.Font));
			this.Shangbiao.ParentExpression = this;
			this.Xiabiao.ParentExpression = this;
			this.Shangbiao.DownLineExpression = this.Xiabiao;
			this.Xiabiao.UpLineExpression = this.Shangbiao;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			ImageAttributes imageAttributes = new ImageAttributes();
			ColorMap[] map = new ColorMap[]
			{
				new ColorMap
				{
					OldColor = Color.Black,
					NewColor = this.Color
				}
			};
			imageAttributes.SetRemapTable(map, ColorAdjustType.Bitmap);
			g.DrawImage(this.fuhao, new Rectangle((int)base.FuhaoLoc.X, (int)base.FuhaoLoc.Y, (int)base.GSize.Width, (int)base.GSize.Height), 0, 0, this.fuhao.Width, this.fuhao.Height, GraphicsUnit.Pixel, imageAttributes);
		}
		public override void RefreshRegion(Graphics g)
		{
			base.Benti.ChangeFontSize(this.Font.Size);
			base.Benti.RefreshRegion(g);
			float num = this.Font.Size;
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
			while (this.Shangbiao.Region.Height > base.GSize.Height / 3f && num > expression.minfontsize)
			{
				num -= 0.5f;
				if (this.Shangbiao != null)
				{
					this.Shangbiao.ChangeFontSize(num);
				}
				this.Shangbiao.RefreshRegion(g);
			}
			num = base.Benti.Font.Size;
			while (this.Xiabiao.Region.Height > base.GSize.Height / 3f && num > expression.minfontsize)
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
			if (this.offset)
			{
				this.offsetX = 16f * this.Font.Size / 50f;
			}
			else
			{
				this.offsetX = 0f;
			}
			PointF pointF = default(PointF);
			PointF pointF2 = default(PointF);
			PointF pointF3 = default(PointF);
			pointF.X = -this.Shangbiao.Region.Width / 2f + this.offsetX;
			pointF.Y = pointF.X + this.Shangbiao.Region.Width;
			pointF2.X = -this.Xiabiao.Region.Width / 2f - this.offsetX;
			pointF2.Y = pointF2.X + this.Xiabiao.Region.Width;
			pointF3.X = -base.GSize.Width / 2f;
			pointF3.Y = pointF3.X + base.GSize.Width;
			float num2 = Math.Max(pointF3.Y, Math.Max(pointF.Y, pointF2.Y)) - Math.Min(Math.Min(pointF3.X, pointF.X), pointF2.X);
			float num3 = base.GSize.Height + this.Shangbiao.Region.Height + this.Xiabiao.Region.Height;
			float num4 = CommonMethods.CalcAscentPixel(this.Font);
			base.FuhaoBaseLine = base.GSize.Height / 2f + this.Shangbiao.Region.Height - this.Font.Size / 2f + num4;
			num2 += base.Benti.Region.Width + 5f + 4f;
			num3 = Math.Max(num3 - base.FuhaoBaseLine, base.Benti.Region.Height - base.Benti.BaseLine) + Math.Max(base.FuhaoBaseLine, base.Benti.BaseLine);
			base.Region = new SizeF(num2, num3);
			base.BaseLine = Math.Max(base.FuhaoBaseLine, base.Benti.BaseLine);
		}
		public override void RefreshInputLocation()
		{
			PointF pointF = default(PointF);
			PointF pointF2 = default(PointF);
			PointF pointF3 = default(PointF);
			pointF.X = -this.Shangbiao.Region.Width / 2f + this.offsetX;
			pointF.Y = pointF.X + this.Shangbiao.Region.Width;
			pointF2.X = -this.Xiabiao.Region.Width / 2f - this.offsetX;
			pointF2.Y = pointF2.X + this.Xiabiao.Region.Width;
			pointF3.X = -base.GSize.Width / 2f;
			pointF3.Y = pointF3.X + base.GSize.Width;
			float num = Math.Max(pointF3.Y, Math.Max(pointF.Y, pointF2.Y)) - Math.Min(Math.Min(pointF3.X, pointF.X), pointF2.X);
			base.FuhaoLoc = new PointF(base.InputLocation.X + 2f + num / 2f - base.GSize.Width / 2f, base.InputLocation.Y + base.BaseLine - base.FuhaoBaseLine + this.Shangbiao.Region.Height);
			base.Benti.InputLocation = new PointF(base.InputLocation.X + 7f + num, base.InputLocation.Y + base.BaseLine - base.Benti.BaseLine);
			this.Shangbiao.InputLocation = new PointF(base.InputLocation.X + 2f + num / 2f - this.Shangbiao.Region.Width / 2f + this.offsetX, base.FuhaoLoc.Y - this.Shangbiao.Region.Height);
			this.Xiabiao.InputLocation = new PointF(base.InputLocation.X + 2f + num / 2f - this.Xiabiao.Region.Width / 2f - this.offsetX, base.FuhaoLoc.Y + base.GSize.Height);
			this.Shangbiao.RefreshInputLocation();
			this.Xiabiao.RefreshInputLocation();
			base.Benti.RefreshInputLocation();
		}
	}
}
