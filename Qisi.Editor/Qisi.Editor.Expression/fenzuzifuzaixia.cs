using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class fenzuzifuzaixia : structexpression
	{
		public lineexpression Benti
		{
			get
			{
				return base.Child[0];
			}
		}
		public lineexpression Fenzu
		{
			get
			{
				return base.Child[1];
			}
		}
		private float DotWidth
		{
			get;
			set;
		}
		public fenzuzifuzaixia(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Type = FType.分组字符在下;
			this.Benti.ParentExpression = this;
			this.Fenzu.ParentExpression = this;
			this.Benti.DownLineExpression = this.Fenzu;
			this.Fenzu.UpLineExpression = this.Benti;
		}
		public override void DrawExpression(Graphics g)
		{
			foreach (expression current in base.Child)
			{
				current.DrawExpression(g);
			}
			Pen pen = new Pen(this.Color);
			g.DrawArc(pen, base.InputLocation.X + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f * 5f - this.Fenzu.Region.Height, this.DotWidth / 3f * 4f, this.DotWidth / 3f * 4f, 90f, 90f);
			g.DrawLine(pen, base.InputLocation.X + this.DotWidth * 2f / 3f + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f - this.Fenzu.Region.Height, base.InputLocation.X + this.Benti.Region.Width / 2f - 1.732f * this.DotWidth / 3f + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f - this.Fenzu.Region.Height);
			g.DrawArc(pen, base.InputLocation.X + this.Benti.Region.Width / 2f - 3.732f * this.DotWidth / 3f + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f - this.Fenzu.Region.Height, this.DotWidth / 3f * 4f, this.DotWidth / 3f * 4f, -30f, -60f);
			g.DrawArc(pen, base.InputLocation.X + this.Benti.Region.Width - 4f * this.DotWidth / 3f + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f * 5f - this.Fenzu.Region.Height, this.DotWidth / 3f * 4f, this.DotWidth / 3f * 4f, 0f, 90f);
			g.DrawLine(pen, base.InputLocation.X + this.Benti.Region.Width - this.DotWidth * 2f / 3f + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f - this.Fenzu.Region.Height, base.InputLocation.X + this.Benti.Region.Width / 2f + 1.732f * this.DotWidth / 3f + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f - this.Fenzu.Region.Height);
			g.DrawArc(pen, base.InputLocation.X + this.Benti.Region.Width / 2f - 0.286f * this.DotWidth / 3f + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.DotWidth / 3f - this.Fenzu.Region.Height, this.DotWidth / 3f * 4f, this.DotWidth / 3f * 4f, -90f, -60f);
			pen.Dispose();
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			this.Fenzu.ChangeFontSize(this.Font.Size - 2f);
			this.Fenzu.RefreshRegion(g);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			this.DotWidth = g.MeasureString("A", this.Font, 0, genericTypographic).Height / 3f;
			if (7.464 * (double)this.DotWidth / 3.0 > (double)this.Benti.Region.Width)
			{
				this.DotWidth = this.Benti.Region.Width * 3f / 2f / 3.73199987f;
			}
			base.Region = new SizeF(Math.Max(this.Benti.Region.Width, this.Fenzu.Region.Width), this.Benti.Region.Height + this.DotWidth + this.Fenzu.Region.Height);
			base.BaseLine = this.Benti.BaseLine;
		}
		public override void RefreshInputLocation()
		{
			this.Fenzu.InputLocation = new PointF(base.InputLocation.X + base.Region.Width / 2f - this.Fenzu.Region.Width / 2f, base.InputLocation.Y + base.Region.Height - this.Fenzu.Region.Height);
			this.Fenzu.RefreshInputLocation();
			this.Benti.InputLocation = new PointF(base.InputLocation.X + base.Region.Width / 2f - this.Benti.Region.Width / 2f, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Benti.RefreshInputLocation();
		}
	}
}
