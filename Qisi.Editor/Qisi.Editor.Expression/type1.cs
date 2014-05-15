using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace Qisi.Editor.Expression
{
	internal abstract class type1 : imageexpression
	{
		protected type1(Image f, lineexpression parent, Color color) : base(f, parent, color)
		{
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
			float num = CommonMethods.CalcAscentPixel(this.Font);
			base.FuhaoBaseLine = base.GSize.Height / 2f - this.Font.Size / 2f + num;
			float width = base.GSize.Width + base.Benti.Region.Width + 5f + 4f;
			float height = Math.Max(base.FuhaoBaseLine, base.Benti.BaseLine) + Math.Max(base.GSize.Height - base.FuhaoBaseLine, base.Benti.Region.Height - base.Benti.BaseLine);
			base.Region = new SizeF(width, height);
			base.BaseLine = Math.Max(base.FuhaoBaseLine, base.Benti.BaseLine);
		}
		public override void RefreshInputLocation()
		{
			base.Benti.InputLocation = new PointF(base.InputLocation.X + base.GSize.Width + 5f + 2f, base.InputLocation.Y + base.BaseLine - base.Benti.BaseLine);
			base.FuhaoLoc = new PointF(base.InputLocation.X + 2f, base.InputLocation.Y + base.BaseLine - base.FuhaoBaseLine);
			base.Benti.RefreshInputLocation();
		}
	}
}
