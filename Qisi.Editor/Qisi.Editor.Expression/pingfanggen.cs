using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class pingfanggen : structexpression
	{
		public lineexpression Benti
		{
			get
			{
				return base.Child[0];
			}
		}
		private SizeF GSize
		{
			get;
			set;
		}
		private float Gintv
		{
			get;
			set;
		}
		public pingfanggen(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Type = FType.平方根;
			this.Benti.ParentExpression = this;
		}
		public override void DrawExpression(Graphics g)
		{
			base.DrawExpression(g);
			PointF pointF = new PointF(this.Benti.InputLocation.X - this.GSize.Width, this.Benti.InputLocation.Y - this.Gintv);
			PointF[] points = new PointF[]
			{
				new PointF(pointF.X, pointF.Y + 0.7f * this.GSize.Height),
				new PointF(pointF.X + 0.2f * this.GSize.Width, pointF.Y + 0.6f * this.GSize.Height),
				new PointF(pointF.X + 0.5f * this.GSize.Width, pointF.Y + this.GSize.Height),
				new PointF(pointF.X + this.GSize.Width, pointF.Y),
				new PointF(pointF.X + this.GSize.Width + this.Benti.Region.Width, pointF.Y)
			};
			Pen pen = new Pen(this.Color);
			g.DrawLines(pen, points);
			pen.Dispose();
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			this.GSize = g.MeasureString("1", this.Font, 0, genericTypographic);
			this.Gintv = 0f;
			if (this.Benti.Child != null)
			{
				foreach (expression current in this.Benti.Child)
				{
					if (current.Type == FType.一般根式 || current.Type == FType.平方根 || current.Type == FType.立方根)
					{
						this.Gintv = this.GSize.Height / 2f;
						break;
					}
				}
			}
			this.GSize = new SizeF(this.GSize.Width, this.Benti.Region.Height + this.Gintv);
			float width = this.GSize.Width + this.Benti.Region.Width;
			float height = this.GSize.Height;
			base.Region = new SizeF(width, height + this.Gintv);
			base.BaseLine = base.Region.Height - this.Benti.Region.Height + this.Benti.BaseLine;
		}
		public override void RefreshInputLocation()
		{
			this.Benti.InputLocation = new PointF(base.InputLocation.X + this.GSize.Width, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Benti.RefreshInputLocation();
		}
	}
}
