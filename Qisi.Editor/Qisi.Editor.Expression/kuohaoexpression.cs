using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal abstract class kuohaoexpression : structexpression
	{
		protected float GSize
		{
			get;
			set;
		}
		protected lineexpression Benti
		{
			get
			{
				return base.Child[0];
			}
		}
		public kuohaoexpression(lineexpression parent, Color color) : base(parent, color, true)
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
			this.GSize = g.MeasureString("{", this.Font, 0, genericTypographic).Width;
			if (7.464 * (double)this.GSize / 3.0 > (double)this.Benti.Region.Height)
			{
				this.GSize = this.Benti.Region.Height * 3f / 2f / 3.73199987f;
			}
			float width = this.Benti.Region.Width + this.GSize * 2f + 4f;
			float num = CommonMethods.CalcAscentPixel(this.Font);
			float num2 = this.Benti.BaseLine - num + this.Font.Size / 2f;
			float height = Math.Max(num2 * 2f, (this.Benti.Region.Height - num2) * 2f);
			base.Region = new SizeF(width, height);
			base.BaseLine = base.Region.Height / 2f - this.Font.Size / 2f + num;
		}
		public override void RefreshInputLocation()
		{
			this.Benti.InputLocation = new PointF(base.InputLocation.X + this.GSize + 2f, base.InputLocation.Y + base.BaseLine - this.Benti.BaseLine);
			this.Benti.RefreshInputLocation();
		}
	}
}
