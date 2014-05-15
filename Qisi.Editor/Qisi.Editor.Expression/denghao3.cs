using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class denghao3 : dh3
	{
		public denghao3(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.等号3;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + 3f, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + 3f);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + 7f, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + 7f);
			pen.Dispose();
		}
	}
}
