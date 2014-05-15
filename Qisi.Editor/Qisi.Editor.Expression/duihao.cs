using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Qisi.Editor.Expression
{
	internal class duihao : dexpression
	{
		public duihao(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.对号;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			float width = g.MeasureString("^", this.Font, 0, genericTypographic).Width;
			SolidBrush solidBrush = new SolidBrush(this.Color);
			float num = Math.Min(base.Benti.Region.Width / width, 4f);
			g.ScaleTransform(num, -1f, MatrixOrder.Prepend);
			g.DrawString("^", this.Font, solidBrush, (base.InputLocation.X + base.Benti.Region.Width / 2f) / num - width / 2f, -base.InputLocation.Y - base.DotWidth - width / 2f, genericTypographic);
			g.ResetTransform();
			solidBrush.Dispose();
		}
	}
}
