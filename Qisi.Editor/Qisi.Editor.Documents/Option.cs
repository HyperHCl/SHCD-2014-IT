using System;
using System.Drawing;
namespace Qisi.Editor.Documents
{
	internal class Option : IDisposable
	{
		private int optionFontSize;
		private Font font;
		private Line line;
		private Region region;
		internal int StartIndex
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
		}
		internal int Count
		{
			get;
			set;
		}
		internal float Width
		{
			get;
			set;
		}
		internal float Left
		{
			get;
			set;
		}
		internal bool Checked
		{
			get;
			set;
		}
		internal string Value
		{
			get;
			set;
		}
		public Line Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}
		public Region Region
		{
			get
			{
				return this.region;
			}
			set
			{
				this.region = value;
			}
		}
		internal bool Answer
		{
			get;
			set;
		}
		public Option(int start, int count, string value)
		{
			this.optionFontSize = 20;
			this.font = new Font("微软雅黑", (float)this.optionFontSize, FontStyle.Bold, GraphicsUnit.Pixel);
			this.region = new Region();
			this.line = null;
			this.StartIndex = start;
			this.Count = count;
			this.Checked = false;
			this.Region = new Region();
			this.Value = value;
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.font != null)
				{
					this.font.Dispose();
				}
				if (this.region != null)
				{
					this.region.Dispose();
				}
			}
			this.region = null;
			this.font = null;
			this.line = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Option()
		{
			this.Dispose(false);
		}
		public void Draw(Graphics g, string s)
		{
			FontFamily fontFamily = this.Font.FontFamily;
			int cellAscent = fontFamily.GetCellAscent(this.Font.Style);
			float num = this.Font.Size * (float)cellAscent / (float)fontFamily.GetEmHeight(this.Font.Style);
			PointF pointF = new PointF(this.Left, this.Line.Top + this.Line.BaseLine - num);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			if (this.Answer)
			{
				g.DrawString(s + ".", this.Font, Brushes.Red, pointF, genericTypographic);
			}
			else
			{
				g.DrawString(s + ".", this.Font, Brushes.Black, pointF, genericTypographic);
			}
			SizeF size = g.MeasureString(s + ".", this.Font, 0, genericTypographic);
			pointF.X -= size.Width * 0.3f;
			size.Width *= 1.6f;
			if (this.Checked)
			{
				Pen pen = new Pen(Color.Blue, 3f);
				g.DrawEllipse(pen, new RectangleF(pointF, size));
				pen.Dispose();
			}
		}
	}
}
