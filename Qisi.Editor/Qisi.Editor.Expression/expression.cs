using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal abstract class expression : IDisposable
	{
		protected static float minfontsize = 8f;
		public abstract Color Color
		{
			get;
			set;
		}
		public PointF InputLocation
		{
			get;
			set;
		}
		public expression ParentExpression
		{
			get;
			set;
		}
		public List<expression> Child
		{
			get;
			set;
		}
		public float BaseLine
		{
			get;
			set;
		}
		public SizeF Region
		{
			get;
			set;
		}
		public abstract Font Font
		{
			get;
		}
		public FType Type
		{
			get;
			set;
		}
		public expression()
		{
		}
		public virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~expression()
		{
			this.Dispose(false);
		}
		public new abstract string ToString();
		public expression PointInExpression(Point point)
		{
			expression result;
			if (this.PointInOrNot(point))
			{
				result = this.PointInChild(point);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public abstract expression PointInChild(Point point);
		public bool PointInOrNot(Point point)
		{
			return this.InputLocation.X <= (float)point.X && this.InputLocation.X + this.Region.Width - 1f >= (float)point.X && this.InputLocation.Y <= (float)point.Y && this.InputLocation.Y + this.Region.Height - 1f >= (float)point.Y;
		}
		public abstract void RefreshRegion(Graphics g);
		public abstract void RefreshInputLocation();
		public abstract void DrawExpression(Graphics g);
		public abstract string ToXml();
	}
}
