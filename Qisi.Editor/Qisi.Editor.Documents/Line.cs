using Qisi.Editor.Documents.Elements;
using System;
using System.Drawing;
namespace Qisi.Editor.Documents
{
	internal class Line : IDisposable
	{
		private Document container;
		internal float Top
		{
			get;
			set;
		}
		internal float Height
		{
			get;
			set;
		}
		internal float BaseLine
		{
			get;
			set;
		}
		internal int StartIndex
		{
			get;
			set;
		}
		internal int ElementCount
		{
			get;
			set;
		}
		internal Document Container
		{
			get
			{
				return this.container;
			}
			set
			{
				this.container = value;
			}
		}
		internal float LineWidth
		{
			get;
			set;
		}
		internal float Left
		{
			get;
			set;
		}
		internal float Right
		{
			get;
			set;
		}
		internal Line(float top, Font font, int startIndex, Document document, float lineWidth, float left)
		{
			this.Top = top;
			this.Height = (float)font.Height;
			FontFamily fontFamily = font.FontFamily;
			int cellAscent = fontFamily.GetCellAscent(font.Style);
			this.BaseLine = font.Size * (float)cellAscent / (float)fontFamily.GetEmHeight(font.Style);
			this.StartIndex = startIndex;
			this.ElementCount = 0;
			this.LineWidth = lineWidth;
			this.Left = left;
			this.Right = left;
			this.container = document;
			this.container.Lines.Add(this);
			this.container.DocHeight = this.Top + this.Height - (float)document.Margin.Top - document.DocLocation.Y;
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			this.container = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Line()
		{
			this.Dispose(false);
		}
		internal void ResetVertical()
		{
			float height = this.Height;
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this.ElementCount; i++)
			{
				Element element = this.container.Elements[i + this.StartIndex];
				num = Math.Max(element.BaseLine, num);
				num2 = Math.Max(element.Size.Height - element.BaseLine, num2);
			}
			float num3 = num + num2;
			this.BaseLine = num;
			this.container.DocHeight += num3 - height;
			this.Height = num3;
			for (int i = 0; i < this.ElementCount; i++)
			{
				Element element2 = this.container.Elements[i + this.StartIndex];
				element2.Location = new PointF(element2.Location.X, this.Top + this.BaseLine - element2.BaseLine);
			}
		}
		internal void ResetVertical(Element elementToAdd)
		{
			float height = this.Height;
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this.ElementCount; i++)
			{
				Element element = this.container.Elements[i + this.StartIndex];
				num = Math.Max(element.BaseLine, num);
				num2 = Math.Max(element.Size.Height - element.BaseLine, num2);
			}
			num = Math.Max(elementToAdd.BaseLine, num);
			num2 = Math.Max(elementToAdd.Size.Height - elementToAdd.BaseLine, num2);
			float num3 = num + num2;
			this.BaseLine = num;
			this.container.DocHeight += num3 - height;
			this.Height = num3;
			for (int i = 0; i < this.ElementCount; i++)
			{
				Element element2 = this.container.Elements[i + this.StartIndex];
				element2.Location = new PointF(element2.Location.X, this.Top + this.BaseLine - element2.BaseLine);
			}
			elementToAdd.Location = new PointF(elementToAdd.Location.X, this.Top + this.BaseLine - elementToAdd.BaseLine);
		}
		internal void Separate()
		{
		}
	}
}
