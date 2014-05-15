using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Qisi.Editor.Expression
{
	internal class daikuanggongshi : structexpression
	{
		protected lineexpression Benti
		{
			get
			{
				return base.Child[0];
			}
		}
		private float DotWidth
		{
			get;
			set;
		}
		public daikuanggongshi(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Type = FType.带框公式;
			this.Benti.ParentExpression = this;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.DrawRectangle(pen, base.InputLocation.X + this.DotWidth / 3f + 1f, base.InputLocation.Y + this.DotWidth / 3f - 1f, this.Benti.Region.Width + this.DotWidth * 4f / 3f, this.Benti.Region.Height + this.DotWidth * 4f / 3f);
			pen.Dispose();
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			this.DotWidth = g.MeasureString("A", this.Font, 0, genericTypographic).Height / 3f;
			base.Region = new SizeF(this.Benti.Region.Width + this.DotWidth * 2f + 4f, this.Benti.Region.Height + this.DotWidth * 2f);
			base.BaseLine = this.Benti.BaseLine + this.DotWidth;
		}
		public override void RefreshInputLocation()
		{
			this.Benti.InputLocation = new PointF(base.InputLocation.X + this.DotWidth + 2f, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Benti.RefreshInputLocation();
		}
	}
}
