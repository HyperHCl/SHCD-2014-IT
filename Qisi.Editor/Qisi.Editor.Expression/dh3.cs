using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal abstract class dh3 : structexpression
	{
		protected lineexpression Fenzi
		{
			get
			{
				return base.Child[0];
			}
		}
		public dh3(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			this.Fenzi.ParentExpression = this;
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Fenzi.ChangeFontSize(this.Font.Size / 2f);
			this.Fenzi.RefreshRegion(g);
			float width = Math.Max(this.Font.Size, base.Child[0].Region.Width) + 12f;
			float height = base.Child[0].Region.Height + 10f;
			base.Region = new SizeF(width, height);
			float num = CommonMethods.CalcAscentPixel(this.Font);
			base.BaseLine = 5f - this.Font.Size / 2f + num;
		}
		public override void RefreshInputLocation()
		{
			this.Fenzi.InputLocation = new PointF(base.InputLocation.X + (base.Region.Width - this.Fenzi.Region.Width) / 2f, base.InputLocation.Y + 10f);
			this.Fenzi.RefreshInputLocation();
		}
	}
}
