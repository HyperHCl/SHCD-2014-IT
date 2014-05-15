using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Qisi.Editor.Documents
{
	internal class Blank : IDisposable
	{
		private GraphicsPath graphicspath;
		private Region region;
		private List<UnderLine> underlines;
		private Pen pen;
		internal int StartIndex
		{
			get;
			set;
		}
		internal int Count
		{
			get;
			set;
		}
		internal int MaxCharsCount
		{
			get;
			set;
		}
		internal float MinLength
		{
			get;
			set;
		}
		internal List<UnderLine> UnderLines
		{
			get
			{
				return this.underlines;
			}
			set
			{
				this.underlines = value;
			}
		}
		internal GraphicsPath Path
		{
			get
			{
				this.graphicspath = new GraphicsPath();
				foreach (UnderLine current in this.UnderLines)
				{
					this.graphicspath.AddLine((float)current.StartX, current.Line.Top + current.Line.Height, (float)current.EndX, current.Line.Top + current.Line.Height);
					this.graphicspath.CloseFigure();
				}
				return this.graphicspath;
			}
		}
		internal Region Region
		{
			get
			{
				this.region = new Region();
				this.region.MakeEmpty();
				foreach (UnderLine current in this.UnderLines)
				{
					this.region.Union(new RectangleF((float)current.StartX, current.Line.Top, (float)(current.EndX - current.StartX), current.Line.Height));
				}
				return this.region;
			}
		}
		internal bool Refreshed
		{
			get;
			set;
		}
		internal bool AllowCR
		{
			get;
			set;
		}
		internal void Draw(Graphics g)
		{
			if (this.pen == null)
			{
				this.pen = new Pen(Color.Black, 2f);
				this.pen.StartCap = LineCap.Flat;
				this.pen.EndCap = LineCap.Flat;
			}
			g.DrawPath(this.pen, this.Path);
		}
		internal Blank(int startIndex, int count, int maxCharsCount, float minLength, bool cr = false)
		{
			this.graphicspath = new GraphicsPath();
			this.region = new Region();
			this.pen = new Pen(Color.Black, 2f);
			this.underlines = new List<UnderLine>();
			this.StartIndex = startIndex;
			this.Count = count;
			this.MaxCharsCount = maxCharsCount;
			this.MinLength = minLength;
			this.Refreshed = false;
			this.AllowCR = cr;
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.graphicspath != null)
				{
					this.graphicspath.Dispose();
				}
				if (this.region != null)
				{
					this.region.Dispose();
				}
				if (this.pen != null)
				{
					this.pen.Dispose();
				}
				if (this.underlines != null)
				{
					foreach (UnderLine current in this.underlines)
					{
						current.Dispose();
					}
				}
			}
			if (this.graphicspath != null)
			{
				this.graphicspath = null;
			}
			if (this.region != null)
			{
				this.region = null;
			}
			if (this.pen != null)
			{
				this.pen = null;
			}
			if (this.underlines != null)
			{
				for (int i = 0; i < this.underlines.Count; i++)
				{
					this.underlines[i] = null;
				}
			}
			this.underlines = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Blank()
		{
			this.Dispose(false);
		}
	}
}
