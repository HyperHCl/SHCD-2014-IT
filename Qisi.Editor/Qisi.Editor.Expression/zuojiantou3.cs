using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class zuojiantou3 : dh3
	{
		public zuojiantou3(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.左箭头3;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + 5f, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + 5f);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + 5f, base.InputLocation.X + 4f, base.InputLocation.Y + 2f);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + 5f, base.InputLocation.X + 4f, base.InputLocation.Y + 8f);
			pen.Dispose();
		}
	}
}
