using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal abstract class dexpression : structexpression
	{
		protected lineexpression Benti
		{
			get
			{
				return base.Child[0];
			}
		}
		protected float DotWidth
		{
			get;
			set;
		}
		public dexpression(lineexpression parent, Color color) : base(parent, color, true)
		{
			base.Child.Add(new lineexpression(this.Font));
			this.Benti.ParentExpression = this;
		}
		public override void RefreshRegion(Graphics g)
		{
			this.Benti.ChangeFontSize(this.Font.Size);
			this.Benti.RefreshRegion(g);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			this.DotWidth = g.MeasureString("A", this.Font, 0, genericTypographic).Height / 3f;
			base.Region = new SizeF(this.Benti.Region.Width, this.Benti.Region.Height + this.DotWidth);
			base.BaseLine = base.Region.Height - this.Benti.Region.Height + this.Benti.BaseLine;
		}
		public override void RefreshInputLocation()
		{
			this.Benti.InputLocation = new PointF(base.InputLocation.X, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Benti.RefreshInputLocation();
		}
	}
}
