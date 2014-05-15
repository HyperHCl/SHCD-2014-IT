using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Documents.Elements
{
	internal class PictureInfo : Pic_Tab
	{
		private Image image;
		private List<Document> documents;
		private SizeF imageShowSize;
		private SizeF size;
		internal SizeF OriginalSize
		{
			get
			{
				SizeF result;
				if (this.image != null)
				{
					result = this.Image.Size;
				}
				else
				{
					result = new SizeF(0f, 0f);
				}
				return result;
			}
		}
		internal Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
			}
		}
		internal SizeF ImageShowSize
		{
			get
			{
				return this.imageShowSize;
			}
			set
			{
				if (this.imageShowSize != value)
				{
					this.imageShowSize = value;
					this.size = new SizeF(this.imageShowSize.Width + (float)base.Margin.Horizontal, this.imageShowSize.Height + (float)base.Margin.Vertical);
				}
			}
		}
		internal List<Document> Documents
		{
			get
			{
				return this.documents;
			}
			set
			{
				this.documents = value;
			}
		}
		internal override SizeF Size
		{
			get
			{
				return this.size;
			}
		}
		internal PictureInfo(Image ima, Font font) : base(font)
		{
			this.image = ima;
			this.documents = new List<Document>();
			this.ImageShowSize = ima.Size;
		}
		internal PictureInfo(Image image, Font font, SizeF showSize) : base(font)
		{
			this.Image = image;
			this.documents = new List<Document>();
			this.ImageShowSize = showSize;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.image != null)
				{
					this.Image.Dispose();
				}
				if (this.documents != null)
				{
					foreach (Document current in this.documents)
					{
						current.Dispose();
					}
				}
			}
			this.image = null;
			if (this.documents != null)
			{
				for (int i = 0; i < this.documents.Count; i++)
				{
					this.documents[i] = null;
				}
			}
			this.documents = null;
			base.Dispose(disposing);
		}
		~PictureInfo()
		{
			this.Dispose(false);
		}
		internal override void Draw(Graphics g)
		{
			if (this.image != null)
			{
				base.Draw(g);
				float num = 6f;
				if (!base.AloneSelected)
				{
					g.DrawImage(this.Image, new RectangleF(this.Location.X + (float)base.Margin.Left, this.Location.Y + (float)base.Margin.Top, this.ImageShowSize.Width, this.ImageShowSize.Height), new RectangleF(new PointF(0f, 0f), this.Image.Size), GraphicsUnit.Pixel);
				}
				else
				{
					g.DrawImage(this.Image, new RectangleF(this.Location.X + (float)base.Margin.Left, this.Location.Y + (float)base.Margin.Top, this.ImageShowSize.Width, this.ImageShowSize.Height), new RectangleF(new PointF(0f, 0f), this.Image.Size), GraphicsUnit.Pixel);
					g.DrawRectangle(Pens.Black, this.Location.X + (float)base.Margin.Left, this.Location.Y + (float)base.Margin.Top, this.ImageShowSize.Width, this.ImageShowSize.Height);
					g.FillRectangle(Brushes.DarkBlue, this.Location.X + (float)base.Margin.Left - num / 2f, this.Location.Y + (float)base.Margin.Top + this.ImageShowSize.Height / 2f - num / 2f, num, num);
					g.FillRectangle(Brushes.DarkBlue, this.Location.X + (float)base.Margin.Left + this.ImageShowSize.Width / 2f - num / 2f, this.Location.Y + (float)base.Margin.Top - num / 2f, num, num);
					g.FillRectangle(Brushes.DarkBlue, this.Location.X + (float)base.Margin.Left + this.ImageShowSize.Width - num / 2f, this.Location.Y + (float)base.Margin.Top + this.ImageShowSize.Height / 2f - num / 2f, num, num);
					g.FillRectangle(Brushes.DarkBlue, this.Location.X + (float)base.Margin.Left + this.ImageShowSize.Width / 2f - num / 2f, this.Location.Y + (float)base.Margin.Top + this.ImageShowSize.Height - num / 2f, num, num);
				}
			}
		}
		internal override void DrawHighLight(Graphics g)
		{
			this.Draw(g);
		}
	}
}
