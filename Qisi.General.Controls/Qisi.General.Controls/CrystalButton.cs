using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class CrystalButton : UserControl
	{
		private const uint WM_MOUSEMOVE = 512u;
		private const uint WM_LBUTTONDOWN = 513u;
		private const uint WM_LBUTTONUP = 514u;
		private const uint WM_LBUTTONDBLCLK = 515u;
		private int _speed = 10;
		private string _text = "";
		private Image _image;
		private int splashlength = 20;
		private System.Timers.Timer timer1;
		private System.Timers.Timer timer2;
		private int i;
		private TextImageRelation textimagerelation;
		private IContainer components;
		[Browsable(true), Category("Image"), DefaultValue(null), Description("显示的图像"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Localizable(true)]
		public Image Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
				base.Invalidate();
			}
		}
		[Browsable(true), Category("Text"), DefaultValue(""), Description("显示的文字"), DisplayName("ButtonText"), Localizable(true)]
		public string ButtonText
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				base.Invalidate();
			}
		}
		[Browsable(true), Category("Text"), DefaultValue(""), Description("闪光的速度"), DisplayName("Speed"), Localizable(true)]
		public int Speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				this._speed = value;
			}
		}
		public CrystalButton()
		{
			this.InitializeComponent();
			this.timer1 = new System.Timers.Timer(40.0);
			this.timer2 = new System.Timers.Timer(40.0);
			this.timer1.AutoReset = true;
			this.timer2.AutoReset = true;
			this.timer1.Elapsed += new ElapsedEventHandler(this.timer1_Elapsed);
			this.timer2.Elapsed += new ElapsedEventHandler(this.timer2_Elapsed);
			base.CreateGraphics();
			new Pen(Brushes.Black, 4f);
			this.Cursor = Cursors.Hand;
		}
		public CrystalButton(TextImageRelation tir)
		{
			this.InitializeComponent();
			this.timer1 = new System.Timers.Timer(40.0);
			this.timer2 = new System.Timers.Timer(40.0);
			this.timer1.AutoReset = true;
			this.timer2.AutoReset = true;
			this.timer1.Elapsed += new ElapsedEventHandler(this.timer1_Elapsed);
			this.timer2.Elapsed += new ElapsedEventHandler(this.timer2_Elapsed);
			this.textimagerelation = tir;
			base.CreateGraphics();
			new Pen(Brushes.Black, 4f);
			this.Cursor = Cursors.Hand;
		}
		[DllImport("User32.Dll", EntryPoint = "PostMessageA")]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
		private static int MAKEPARAM(int l, int h)
		{
			return (l & 65535) | h << 16;
		}
		private static void myClick(IntPtr hWnd, Point point)
		{
			CrystalButton.PostMessage(hWnd, 512u, 0, CrystalButton.MAKEPARAM(point.X, point.Y));
			CrystalButton.PostMessage(hWnd, 513u, 0, CrystalButton.MAKEPARAM(point.X, point.Y));
			CrystalButton.PostMessage(hWnd, 514u, 0, CrystalButton.MAKEPARAM(point.X, point.Y));
		}
		public void PerformClick()
		{
			CrystalButton.myClick(base.Handle, new Point(2, 2));
		}
		private void CrystalButton_MouseEnter(object sender, EventArgs e)
		{
			if (base.Enabled)
			{
				this.timer1.Start();
			}
		}
		private void CrystalButton_Paint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			SolidBrush solidBrush = new SolidBrush(this.ForeColor);
			Pen pen = new Pen(Color.Black, 4f);
			graphics.DrawRectangle(pen, new Rectangle(new Point(0, 0), base.ClientSize));
			switch (this.textimagerelation)
			{
			case TextImageRelation.Overlay:
			{
				SizeF sizeF = graphics.MeasureString(this._text, this.Font);
				if (base.Enabled)
				{
					graphics.DrawString(this._text, this.Font, solidBrush, ((float)base.ClientSize.Width - sizeF.Width) / 2f, ((float)base.ClientSize.Height - sizeF.Height) / 2f);
					if (this._image != null)
					{
						graphics.DrawImage(this._image, new Rectangle(2, 2, base.ClientSize.Width - 4, base.ClientSize.Width - 4), new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
					}
				}
				else
				{
					if (this._image != null)
					{
						graphics.DrawString(this._text, this.Font, Brushes.DarkGray, ((float)base.ClientSize.Width - sizeF.Width) / 2f, ((float)base.ClientSize.Height - sizeF.Height) / 2f);
						Bitmap bitmap = new Bitmap(this._image.Width, this._image.Height);
						Graphics graphics2 = Graphics.FromImage(bitmap);
						ControlPaint.DrawImageDisabled(graphics2, this._image, 0, 0, Color.White);
						graphics.DrawImage(bitmap, new Rectangle(2, 2, base.ClientSize.Width - 4, base.ClientSize.Width - 4), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
					}
				}
				break;
			}
			case TextImageRelation.ImageAboveText:
			{
				SizeF sizeF = graphics.MeasureString(this._text, this.Font);
				if (base.Enabled)
				{
					graphics.DrawString(this._text, this.Font, solidBrush, ((float)base.ClientSize.Width - sizeF.Width) / 2f, (float)(base.ClientSize.Width * 2 / 3 + base.ClientSize.Height / 20));
					if (this._image != null)
					{
						graphics.DrawImage(this._image, new Rectangle(base.ClientSize.Width / 6, 2, base.ClientSize.Width * 2 / 3, base.ClientSize.Width * 2 / 3), new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
					}
				}
				else
				{
					graphics.DrawString(this._text, this.Font, Brushes.DarkGray, ((float)base.ClientSize.Width - sizeF.Width) / 2f, (float)(base.ClientSize.Width * 2 / 3 + base.ClientSize.Height / 20));
					if (this._image != null)
					{
						Bitmap bitmap2 = new Bitmap(this._image.Width, this._image.Height);
						Graphics graphics3 = Graphics.FromImage(bitmap2);
						ControlPaint.DrawImageDisabled(graphics3, this._image, 0, 0, Color.White);
						graphics.DrawImage(bitmap2, new Rectangle(base.ClientSize.Width / 6, 2, base.ClientSize.Width * 2 / 3, base.ClientSize.Width * 2 / 3), new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), GraphicsUnit.Pixel);
					}
				}
				break;
			}
			case TextImageRelation.TextAboveImage:
			{
				SizeF sizeF = graphics.MeasureString(this._text, this.Font);
				if (base.Enabled)
				{
					graphics.DrawString(this._text, this.Font, solidBrush, ((float)base.ClientSize.Width - sizeF.Width) / 2f, 2f);
					if (this._image != null)
					{
						graphics.DrawImage(this._image, new Rectangle(base.ClientSize.Width / 6, base.ClientSize.Width / 3 + 2 + base.ClientSize.Height / 10, base.ClientSize.Width * 2 / 3, base.ClientSize.Width * 2 / 3), new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
					}
				}
				else
				{
					if (this._image != null)
					{
						graphics.DrawString(this._text, this.Font, Brushes.DarkGray, ((float)base.ClientSize.Width - sizeF.Width) / 2f, 2f);
						Bitmap bitmap3 = new Bitmap(this._image.Width, this._image.Height);
						Graphics graphics4 = Graphics.FromImage(bitmap3);
						ControlPaint.DrawImageDisabled(graphics4, this._image, 0, 0, Color.White);
						graphics.DrawImage(bitmap3, new Rectangle(base.ClientSize.Width / 6, base.ClientSize.Width / 3 + 2 + base.ClientSize.Height / 10, base.ClientSize.Width * 2 / 3, base.ClientSize.Width * 2 / 3), new Rectangle(0, 0, bitmap3.Width, bitmap3.Height), GraphicsUnit.Pixel);
					}
				}
				break;
			}
			case TextImageRelation.ImageBeforeText:
			{
				SizeF sizeF = graphics.MeasureString(this._text, this.Font);
				if (base.Enabled)
				{
					graphics.DrawString(this._text, this.Font, solidBrush, (float)(base.ClientSize.Height + base.ClientSize.Width / 10), ((float)base.ClientSize.Height - sizeF.Height) / 2f);
					if (this._image != null)
					{
						graphics.DrawImage(this._image, new Rectangle(2, 2, base.ClientSize.Height - 4, base.ClientSize.Height - 4), new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
					}
				}
				else
				{
					graphics.DrawString(this._text, this.Font, Brushes.DarkGray, (float)(base.ClientSize.Height + base.ClientSize.Width / 10), ((float)base.ClientSize.Height - sizeF.Height) / 2f);
					if (this._image != null)
					{
						Bitmap bitmap4 = new Bitmap(this._image.Width, this._image.Height);
						Graphics graphics5 = Graphics.FromImage(bitmap4);
						ControlPaint.DrawImageDisabled(graphics5, this._image, 0, 0, Color.White);
						graphics.DrawImage(bitmap4, new Rectangle(2, 2, base.ClientSize.Height - 4, base.ClientSize.Height - 4), new Rectangle(0, 0, bitmap4.Width, bitmap4.Height), GraphicsUnit.Pixel);
					}
				}
				break;
			}
			case TextImageRelation.TextBeforeImage:
			{
				SizeF sizeF = graphics.MeasureString(this._text, this.Font);
				if (base.Enabled)
				{
					graphics.DrawString(this._text, this.Font, solidBrush, 2f, ((float)base.ClientSize.Height - sizeF.Height) / 2f);
					if (this._image != null)
					{
						graphics.DrawImage(this._image, new Rectangle((int)sizeF.Width + base.ClientSize.Width / 10, 2, base.ClientSize.Height - 4, base.ClientSize.Height - 4), new Rectangle(0, 0, this._image.Width, this._image.Height), GraphicsUnit.Pixel);
					}
				}
				else
				{
					if (this._image != null)
					{
						graphics.DrawString(this._text, this.Font, Brushes.DarkGray, 2f, ((float)base.ClientSize.Height - sizeF.Height) / 2f);
						Bitmap bitmap5 = new Bitmap(this._image.Width, this._image.Height);
						Graphics graphics6 = Graphics.FromImage(bitmap5);
						ControlPaint.DrawImageDisabled(graphics6, this._image, 0, 0, Color.White);
						graphics.DrawImage(bitmap5, new Rectangle((int)sizeF.Width + base.ClientSize.Width / 10, 2, base.ClientSize.Height - 4, base.ClientSize.Height - 4), new Rectangle(0, 0, bitmap5.Width, bitmap5.Height), GraphicsUnit.Pixel);
					}
				}
				break;
			}
			}
			pen.Dispose();
			solidBrush.Dispose();
		}
		private void timer1_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				if (!this.timer2.Enabled || this.i >= 0)
				{
					if (this.i < 0)
					{
						this.i = 0;
					}
					Graphics graphics = base.CreateGraphics();
					Pen pen = new Pen(Brushes.Black, 4f);
					graphics.DrawRectangle(pen, new Rectangle(new Point(0, 0), base.ClientSize));
					Color white = Color.White;
					Color black = Color.Black;
					Brush brush = new LinearGradientBrush(new Rectangle(0, this._speed * this.i, 2, this.splashlength), black, white, LinearGradientMode.Vertical);
					Brush brush2 = new LinearGradientBrush(new Rectangle(0, this._speed * this.i + this.splashlength, 2, this.splashlength), white, black, LinearGradientMode.Vertical);
					graphics.FillRectangle(brush, new Rectangle(0, this._speed * this.i, 2, this.splashlength));
					graphics.FillRectangle(brush2, new Rectangle(0, this._speed * this.i + this.splashlength, 2, this.splashlength));
					int y = base.ClientSize.Height - 2;
					int num = -base.ClientSize.Height;
					Brush brush3 = new LinearGradientBrush(new Rectangle(this._speed * this.i + num, y, this.splashlength, 2), black, white, LinearGradientMode.Horizontal);
					Brush brush4 = new LinearGradientBrush(new Rectangle(this._speed * this.i + num + this.splashlength, y, this.splashlength, 2), white, black, LinearGradientMode.Horizontal);
					graphics.FillRectangle(brush3, new Rectangle(this._speed * this.i + num, y, this.splashlength, 2));
					graphics.FillRectangle(brush4, new Rectangle(this._speed * this.i + this.splashlength + num, y, this.splashlength, 2));
					num = -base.ClientSize.Width;
					int x = base.ClientSize.Width - 2;
					Brush brush5 = new LinearGradientBrush(new Rectangle(x, this._speed * this.i + num, 2, this.splashlength), black, white, LinearGradientMode.Vertical);
					Brush brush6 = new LinearGradientBrush(new Rectangle(x, this._speed * this.i + this.splashlength + num, 2, this.splashlength), white, black, LinearGradientMode.Vertical);
					graphics.FillRectangle(brush5, new Rectangle(x, this._speed * this.i + num, 2, this.splashlength));
					graphics.FillRectangle(brush6, new Rectangle(x, this._speed * this.i + this.splashlength + num, 2, this.splashlength));
					Brush brush7 = new LinearGradientBrush(new Rectangle(this._speed * this.i, 0, this.splashlength, 2), black, white, LinearGradientMode.Horizontal);
					Brush brush8 = new LinearGradientBrush(new Rectangle(this._speed * this.i + this.splashlength, 0, this.splashlength, 2), white, black, LinearGradientMode.Horizontal);
					graphics.FillRectangle(brush7, new Rectangle(this._speed * this.i, 0, this.splashlength, 2));
					graphics.FillRectangle(brush8, new Rectangle(this._speed * this.i + this.splashlength, 0, this.splashlength, 2));
					if (this._speed * this.i + num > base.ClientSize.Height)
					{
						this.timer1.Stop();
					}
					this.i++;
				}
			}
			catch
			{
			}
		}
		private void timer2_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				if (!this.timer1.Enabled || this.i <= 0)
				{
					if (this.i > 0)
					{
						this.i = 0;
					}
					Graphics graphics = base.CreateGraphics();
					Pen pen = new Pen(Brushes.Black, 4f);
					graphics.DrawRectangle(pen, new Rectangle(new Point(0, 0), base.ClientSize));
					Color white = Color.White;
					Color black = Color.Black;
					int num = base.ClientSize.Height + base.ClientSize.Width - this.splashlength;
					Brush brush = new LinearGradientBrush(new Rectangle(0, this._speed * this.i + num, 2, this.splashlength), black, white, LinearGradientMode.Vertical);
					Brush brush2 = new LinearGradientBrush(new Rectangle(0, this._speed * this.i + this.splashlength + num, 2, this.splashlength), white, black, LinearGradientMode.Vertical);
					graphics.FillRectangle(brush, new Rectangle(0, this._speed * this.i + num, 2, this.splashlength));
					graphics.FillRectangle(brush2, new Rectangle(0, this._speed * this.i + this.splashlength + num, 2, this.splashlength));
					int y = base.ClientSize.Height - 2;
					num = base.ClientSize.Width - this.splashlength;
					Brush brush3 = new LinearGradientBrush(new Rectangle(this._speed * this.i + num, y, this.splashlength, 2), black, white, LinearGradientMode.Horizontal);
					Brush brush4 = new LinearGradientBrush(new Rectangle(this._speed * this.i + num + this.splashlength, y, this.splashlength, 2), white, black, LinearGradientMode.Horizontal);
					graphics.FillRectangle(brush3, new Rectangle(this._speed * this.i + num, y, this.splashlength, 2));
					graphics.FillRectangle(brush4, new Rectangle(this._speed * this.i + this.splashlength + num, y, this.splashlength, 2));
					num = base.ClientSize.Height - this.splashlength;
					int x = base.ClientSize.Width - 2;
					Brush brush5 = new LinearGradientBrush(new Rectangle(x, this._speed * this.i + num, 2, this.splashlength), black, white, LinearGradientMode.Vertical);
					Brush brush6 = new LinearGradientBrush(new Rectangle(x, this._speed * this.i + this.splashlength + num, 2, this.splashlength), white, black, LinearGradientMode.Vertical);
					graphics.FillRectangle(brush5, new Rectangle(x, this._speed * this.i + num, 2, this.splashlength));
					graphics.FillRectangle(brush6, new Rectangle(x, this._speed * this.i + this.splashlength + num, 2, this.splashlength));
					num = base.ClientSize.Height + base.ClientSize.Width - this.splashlength;
					Brush brush7 = new LinearGradientBrush(new Rectangle(this._speed * this.i + num, 0, this.splashlength, 2), black, white, LinearGradientMode.Horizontal);
					Brush brush8 = new LinearGradientBrush(new Rectangle(this._speed * this.i + this.splashlength + num, 0, this.splashlength, 2), white, black, LinearGradientMode.Horizontal);
					graphics.FillRectangle(brush7, new Rectangle(this._speed * this.i + num, 0, this.splashlength, 2));
					graphics.FillRectangle(brush8, new Rectangle(this._speed * this.i + this.splashlength + num, 0, this.splashlength, 2));
					if (this._speed * this.i + this.splashlength * 2 + num < 0)
					{
						this.timer2.Stop();
					}
					this.i--;
				}
			}
			catch
			{
			}
		}
		private void CrystalButton_MouseLeave(object sender, EventArgs e)
		{
			if (base.Enabled)
			{
				this.timer2.Start();
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "CrystalButton";
			base.Paint += new PaintEventHandler(this.CrystalButton_Paint);
			base.MouseEnter += new EventHandler(this.CrystalButton_MouseEnter);
			base.MouseLeave += new EventHandler(this.CrystalButton_MouseLeave);
			base.ResumeLayout(false);
		}
	}
}
