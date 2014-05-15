using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class zuojiantouzaishang : dexpression
	{
		public zuojiantouzaishang(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.左箭头在上;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			g.DrawLine(pen, base.InputLocation.X, base.InputLocation.Y + base.DotWidth / 2f, base.InputLocation.X + base.Region.Width, base.InputLocation.Y + base.DotWidth / 2f);
			g.DrawLine(pen, base.InputLocation.X, base.InputLocation.Y + base.DotWidth / 2f, base.InputLocation.X + base.DotWidth / 2f, base.InputLocation.Y);
			g.DrawLine(pen, base.InputLocation.X, base.InputLocation.Y + base.DotWidth / 2f, base.InputLocation.X + base.DotWidth / 2f, base.InputLocation.Y + base.DotWidth);
			pen.Dispose();
		}
	}
}
