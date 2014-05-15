using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Qisi.Editor.Expression
{
	internal class jianyinfuhao : dexpression
	{
		public jianyinfuhao(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.尖音符号;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			float width = g.MeasureString("`", this.Font, 0, genericTypographic).Width;
			SolidBrush solidBrush = new SolidBrush(this.Color);
			g.ScaleTransform(-1f, 1f, MatrixOrder.Prepend);
			g.DrawString("`", this.Font, solidBrush, -(base.InputLocation.X + base.Benti.Region.Width / 2f) - width / 2f, base.InputLocation.Y - width / 2f, genericTypographic);
			g.ResetTransform();
			solidBrush.Dispose();
		}
	}
}
