using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class dian : dexpression
	{
		public dian(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.点;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			SolidBrush solidBrush = new SolidBrush(this.Color);
			g.FillEllipse(solidBrush, base.Benti.InputLocation.X + base.Benti.Region.Width / 2f - base.DotWidth / 2f, base.InputLocation.Y + base.Region.Height / 4f - base.Benti.Region.Height / 4f - base.DotWidth / 2f, base.DotWidth, base.DotWidth);
			solidBrush.Dispose();
		}
	}
}
