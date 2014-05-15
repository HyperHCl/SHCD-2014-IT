using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Qisi.Editor.Expression
{
	internal class genshi : structexpression
	{
		public lineexpression Benti
		{
			get
			{
				return base.Child[1];
			}
		}
		public lineexpression Gen
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
		public genshi(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			base.Child.Add(new lineexpression(this.Font));
			base.Type = FType.一般根式;
			this.Benti.ParentExpression = this;
			this.Gen.ParentExpression = this;
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
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.DrawLines(pen, points);
			pen.Dispose();
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			float num = this.Benti.Font.Size;
			if (this.Gen != null)
			{
				this.Gen.ChangeFontSize(num);
			}
			this.Gen.RefreshRegion(g);
			while (this.Gen.Region.Height > this.Benti.Region.Height / 3f && num > expression.minfontsize)
			{
				num -= 0.5f;
				if (this.Gen != null)
				{
					this.Gen.ChangeFontSize(num);
				}
				this.Gen.RefreshRegion(g);
			}
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
			float width = Math.Max(this.Gen.Region.Width + this.GSize.Width * 2f / 3f, this.GSize.Width) + this.Benti.Region.Width;
			float num2 = Math.Max(this.Gen.Region.Height + this.GSize.Height / 5f * 4f, this.GSize.Height);
			base.Region = new SizeF(width, num2 + this.Gintv);
			base.BaseLine = base.Region.Height - this.Benti.Region.Height + this.Benti.BaseLine;
		}
		public override void RefreshInputLocation()
		{
			PointF inputLocation = default(PointF);
			PointF inputLocation2 = default(PointF);
			if (this.Gen.Region.Height + this.GSize.Height / 5f * 4f > this.GSize.Height)
			{
				inputLocation.Y = base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine;
				inputLocation2.Y = base.InputLocation.Y;
			}
			else
			{
				inputLocation.Y = base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine;
				inputLocation2.Y = base.InputLocation.Y + this.GSize.Height * 2f / 5f - this.Gen.Region.Height;
			}
			if (this.Gen.Region.Width + this.GSize.Width * 2f / 3f > this.GSize.Width)
			{
				inputLocation.X = base.InputLocation.X + this.Gen.Region.Width + this.GSize.Width * 2f / 3f;
				inputLocation2.X = base.InputLocation.X;
			}
			else
			{
				inputLocation.X = base.InputLocation.X + this.GSize.Width;
				inputLocation2.X = base.InputLocation.X + this.GSize.Width / 3f - this.Gen.Region.Width;
			}
			this.Benti.InputLocation = inputLocation;
			this.Benti.RefreshInputLocation();
			this.Gen.InputLocation = inputLocation2;
			this.Gen.RefreshInputLocation();
		}
	}
}
