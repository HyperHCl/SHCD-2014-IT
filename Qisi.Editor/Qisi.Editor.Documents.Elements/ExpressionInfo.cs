using Qisi.Editor.Expression;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.Editor.Documents.Elements
{
	internal class ExpressionInfo : Element
	{
		private containerexpression containerExpr;
		internal static Padding padding = new Padding(2, 0, 2, 0);
		internal override PointF Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
				if (this.containerExpr != null)
				{
					this.containerExpr.InputLocation = new PointF(value.X + (float)this.Padding.Left, value.Y + (float)this.Padding.Top);
					this.containerExpr.RefreshInputLocation();
				}
			}
		}
		internal override SizeF Size
		{
			get
			{
				SizeF result;
				if (this.containerExpr != null)
				{
					result = new SizeF(this.containerExpr.Region.Width + (float)this.Padding.Horizontal, this.containerExpr.Region.Height + (float)this.Padding.Vertical);
				}
				else
				{
					result = new SizeF(0f, 0f);
				}
				return result;
			}
		}
		internal override float BaseLine
		{
			get
			{
				float result;
				if (this.containerExpr != null)
				{
					result = this.containerExpr.BaseLine + (float)this.Padding.Top;
				}
				else
				{
					result = 0f;
				}
				return result;
			}
		}
		internal Padding Padding
		{
			get
			{
				return ExpressionInfo.padding;
			}
		}
		internal containerexpression ContainerExpression
		{
			get
			{
				return this.containerExpr;
			}
			set
			{
				this.containerExpr = value;
			}
		}
		internal Color Color
		{
			get
			{
				return this.containerExpr.Color;
			}
			set
			{
				this.containerExpr.Color = value;
			}
		}
		internal ExpressionInfo(containerexpression expr, Font font) : base(font)
		{
			this.ContainerExpression = expr;
			this.ContainerExpression.Info = this;
			this.Sized = false;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.containerExpr.Dispose();
			}
			this.containerExpr = null;
			base.Dispose(disposing);
		}
		~ExpressionInfo()
		{
			this.Dispose(false);
		}
		internal override void Draw(Graphics g)
		{
			if (this.containerExpr != null)
			{
				base.Draw(g);
				this.containerExpr.DrawExpression(g);
			}
		}
	}
}
