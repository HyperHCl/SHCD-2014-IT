using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class animateImage : UserControl
	{
		private Bitmap animatedImage;
		private bool currentlyAnimating;
		private string path;
		private IContainer components;
		[Browsable(true), Category("外观"), DefaultValue(null), Description("gif图像的路径"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Localizable(true)]
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
				this.DoubleBuffered = true;
				this.animatedImage = new Bitmap(this.Path);
				base.Size = this.animatedImage.Size;
			}
		}
		public animateImage(string Path)
		{
			this.path = Path;
			this.DoubleBuffered = true;
			this.animatedImage = new Bitmap(Path);
			base.Size = this.animatedImage.Size;
		}
		public animateImage()
		{
			this.DoubleBuffered = true;
		}
		public void distory()
		{
			this.Stop();
			if (this.animatedImage == null)
			{
				return;
			}
			this.animatedImage.Dispose();
			this.animatedImage = null;
		}
		public void AnimateImage()
		{
			if (!this.currentlyAnimating)
			{
				ImageAnimator.Animate(this.animatedImage, new EventHandler(this.OnFrameChanged));
				this.currentlyAnimating = true;
			}
		}
		private void OnFrameChanged(object o, EventArgs e)
		{
			base.Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.animatedImage != null)
			{
				if (ImageAnimator.CanAnimate(this.animatedImage))
				{
					ImageAnimator.UpdateFrames(this.animatedImage);
				}
				e.Graphics.DrawImage(this.animatedImage, new Point(0, 0));
			}
		}
		public void Stop()
		{
			ImageAnimator.StopAnimate(this.animatedImage, new EventHandler(this.OnFrameChanged));
		}
		public void Play()
		{
			if (ImageAnimator.CanAnimate(this.animatedImage))
			{
				ImageAnimator.Animate(this.animatedImage, new EventHandler(this.OnFrameChanged));
			}
		}
		protected override void Dispose(bool disposing)
		{
			this.distory();
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			base.AutoScaleMode = AutoScaleMode.Font;
		}
	}
}
