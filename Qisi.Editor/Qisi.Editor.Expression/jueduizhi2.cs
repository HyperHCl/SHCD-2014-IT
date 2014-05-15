using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class jueduizhi2 : kuohaoexpression
	{
		public jueduizhi2(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.绝对值2;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			float num = base.GSize / 2f + base.Region.Height * base.Region.Height / base.GSize / 8f;
			float num2 = (float)(Math.Atan((double)(base.Region.Height / 2f / (base.Region.Height * base.Region.Height / 8f / base.GSize - base.GSize / 2f))) / 3.1415926535897931 * 180.0);
			g.DrawLine(pen, base.InputLocation.X + 2f + base.GSize / 4f, base.InputLocation.Y, base.InputLocation.X + 2f + base.GSize / 4f, base.InputLocation.Y + base.Region.Height);
			g.DrawLine(pen, base.InputLocation.X + base.Region.Width - 2f - base.GSize / 4f, base.InputLocation.Y, base.InputLocation.X + base.Region.Width - 2f - base.GSize / 4f, base.InputLocation.Y + base.Region.Height);
			g.DrawLine(pen, base.InputLocation.X + 2f + base.GSize / 4f * 3f, base.InputLocation.Y, base.InputLocation.X + 2f + base.GSize / 4f * 3f, base.InputLocation.Y + base.Region.Height);
			g.DrawLine(pen, base.InputLocation.X + base.Region.Width - 2f - base.GSize / 4f * 3f, base.InputLocation.Y, base.InputLocation.X + base.Region.Width - 2f - base.GSize / 4f * 3f, base.InputLocation.Y + base.Region.Height);
			pen.Dispose();
		}
	}
}
