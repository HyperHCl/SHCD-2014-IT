using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class sandian : dexpression
	{
		public sandian(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.三点;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			SolidBrush solidBrush = new SolidBrush(this.Color);
			g.FillEllipse(solidBrush, base.Benti.InputLocation.X + base.Benti.Region.Width / 2f - base.DotWidth / 2f, base.InputLocation.Y + base.Region.Height / 4f - base.Benti.Region.Height / 4f - base.DotWidth / 2f, base.DotWidth, base.DotWidth);
			g.FillEllipse(solidBrush, base.Benti.InputLocation.X + base.Benti.Region.Width / 2f - base.DotWidth / 2f * 5f, base.InputLocation.Y + base.Region.Height / 4f - base.Benti.Region.Height / 4f - base.DotWidth / 2f, base.DotWidth, base.DotWidth);
			g.FillEllipse(solidBrush, base.Benti.InputLocation.X + base.Benti.Region.Width / 2f + base.DotWidth / 2f * 3f, base.InputLocation.Y + base.Region.Height / 4f - base.Benti.Region.Height / 4f - base.DotWidth / 2f, base.DotWidth, base.DotWidth);
			solidBrush.Dispose();
		}
	}
}
