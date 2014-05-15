using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class fenshi : structexpression
	{
		public lineexpression Fenzi
		{
			get
			{
				return base.Child[0];
			}
		}
		public lineexpression Fenmu
		{
			get
			{
				return base.Child[1];
			}
		}
		public fenshi(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Type = FType.分式;
			this.Fenzi.ParentExpression = this;
			this.Fenmu.ParentExpression = this;
			this.Fenzi.DownLineExpression = this.Fenmu;
			this.Fenmu.UpLineExpression = this.Fenzi;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			Pen pen = new Pen(this.Color);
			g.DrawLine(pen, base.InputLocation.X + 2f, this.Fenzi.InputLocation.Y + this.Fenzi.Region.Height + 2f, base.InputLocation.X + base.Region.Width - 5f, this.Fenzi.InputLocation.Y + this.Fenzi.Region.Height + 2f);
			pen.Dispose();
		}
		public override void RefreshRegion(Graphics g)
		{
			foreach (expression current in base.Child)
			{
				current.RefreshRegion(g);
			}
			float width = Math.Max(base.Child[0].Region.Width, base.Child[1].Region.Width) + Math.Max(12f, 12f * this.Font.Size / 12f);
			float height = base.Child[0].Region.Height + base.Child[1].Region.Height + 10f;
			base.Region = new SizeF(width, height);
			base.BaseLine = this.Fenzi.Region.Height + 5f - this.Font.Size / 2f + CommonMethods.CalcAscentPixel(this.Font);
		}
		public override void RefreshInputLocation()
		{
			this.Fenzi.InputLocation = new PointF(base.InputLocation.X + (base.Region.Width - this.Fenzi.Region.Width) / 2f, base.InputLocation.Y + 3f);
			this.Fenzi.RefreshInputLocation();
			this.Fenmu.InputLocation = new PointF(base.InputLocation.X + (base.Region.Width - this.Fenmu.Region.Width) / 2f, base.InputLocation.Y + base.Region.Height - 3f - base.Child[1].Region.Height);
			this.Fenmu.RefreshInputLocation();
		}
	}
}
