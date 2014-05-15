using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Qisi.Editor.Expression
{
	internal class duanyinfuhao : dexpression
	{
		public duanyinfuhao(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.短音符号;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			SizeF sizeF = g.MeasureString("(", this.Font, 0, genericTypographic);
			SolidBrush solidBrush = new SolidBrush(this.Color);
			g.RotateTransform(-90f);
			float num = Math.Min(base.Benti.Region.Width / sizeF.Height, 4f);
			g.ScaleTransform(1f, num, MatrixOrder.Prepend);
			g.DrawString("(", this.Font, solidBrush, -base.InputLocation.Y - sizeF.Width, (base.InputLocation.X + base.Benti.Region.Width / 2f) / num - sizeF.Height / 2f - sizeF.Width / 3f, genericTypographic);
			g.ResetTransform();
			solidBrush.Dispose();
		}
	}
}
