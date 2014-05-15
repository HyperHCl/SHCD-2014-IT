using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class youjiantou2 : dh2
	{
		public youjiantou2(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.右箭头2;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			g.DrawLine(pen, base.InputLocation.X + 1f, base.InputLocation.Y + base.Fenzi.Region.Height + 5f, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + base.Fenzi.Region.Height + 5f);
			g.DrawLine(pen, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + base.Fenzi.Region.Height + 5f, base.InputLocation.X + base.Region.Width - 5f, base.InputLocation.Y + base.Fenzi.Region.Height + 2f);
			g.DrawLine(pen, base.InputLocation.X + base.Region.Width - 2f, base.InputLocation.Y + base.Fenzi.Region.Height + 5f, base.InputLocation.X + base.Region.Width - 5f, base.InputLocation.Y + base.Fenzi.Region.Height + 8f);
			pen.Dispose();
		}
	}
}
