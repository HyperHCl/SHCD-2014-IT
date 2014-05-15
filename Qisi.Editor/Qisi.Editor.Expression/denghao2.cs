using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class denghao2 : dh2
	{
		public denghao2(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.等号2;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + base.Fenzi.Region.Height + 3f, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + base.Fenzi.Region.Height + 3f);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + base.Fenzi.Region.Height + 7f, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + base.Fenzi.Region.Height + 7f);
			pen.Dispose();
		}
	}
}
