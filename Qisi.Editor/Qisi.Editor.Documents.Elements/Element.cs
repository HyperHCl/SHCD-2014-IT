using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Qisi.Editor.Documents.Elements
{
	internal class Element : IDisposable
	{
		private Line lineContainer;
		private Document document;
		private int lineIndex;
		private Region region;
		private Font font;
		internal Line LineContainer
		{
			get
			{
				return this.lineContainer;
			}
			set
			{
				this.lineContainer = value;
			}
		}
		internal int Index
		{
			get
			{
				return this.document.Elements.IndexOf(this);
			}
		}
		internal Document DocumentContainer
		{
			get
			{
				return this.document;
			}
			set
			{
				this.document = value;
			}
		}
		internal float OutWidth
		{
			get;
			set;
		}
		internal virtual PointF Location
		{
			get;
			set;
		}
		internal PointF OutLocation
		{
			get
			{
				PointF result;
				if (this.lineContainer != null)
				{
					result = new PointF(this.Location.X, this.lineContainer.Top);
				}
				else
				{
					result = new PointF(this.Location.X, 0f);
				}
				return result;
			}
		}
		internal SizeF OutSize
		{
			get
			{
				SizeF result;
				if (this.lineContainer != null)
				{
					result = new SizeF(this.OutWidth, this.lineContainer.Height);
				}
				else
				{
					result = new SizeF(this.OutWidth, 0f);
				}
				return result;
			}
		}
		internal virtual Region Region
		{
			get
			{
				this.region = new Region(new RectangleF(this.OutLocation, this.OutSize));
				return this.region;
			}
		}
		internal int LineIndex
		{
			get
			{
				int result;
				if (this.lineContainer != null)
				{
					result = this.Index - this.lineContainer.StartIndex;
				}
				else
				{
					result = -1;
				}
				return result;
			}
		}
		internal virtual SizeF Size
		{
			get;
			set;
		}
		internal Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
			}
		}
		internal virtual bool Sized
		{
			get;
			set;
		}
		internal bool Settled
		{
			get;
			set;
		}
		internal virtual float BaseLine
		{
			get;
			set;
		}
		internal bool InBlank
		{
			get
			{
				bool result;
				if (this.document == null)
				{
					result = false;
				}
				else
				{
					foreach (Blank current in this.document.Blanks)
					{
						if (current.StartIndex <= this.Index && current.StartIndex + current.Count > this.Index)
						{
							result = true;
							return result;
						}
					}
					result = false;
				}
				return result;
			}
		}
		internal virtual void Draw(Graphics g)
		{
		}
		internal virtual void DrawHighLight(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = PixelOffsetMode.Half;
		}
		internal Element(Font f)
		{
			this.Font = f;
			this.region = new Region();
			this.lineContainer = null;
			this.document = null;
			this.Settled = false;
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.region != null)
				{
					this.region.Dispose();
				}
				if (this.font != null)
				{
					this.font.Dispose();
				}
			}
			this.region = null;
			this.font = null;
			this.lineContainer = null;
			this.document = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Element()
		{
			this.Dispose(false);
		}
	}
}
