using Qisi.Editor.Expression;
using Qisi.Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
namespace Qisi.Editor.Controls
{
	internal class FLabel : Control
	{
		internal delegate void AppendExpressionHandler(object sender, ExpressionEventArgs e);
		private FType _ftype;
		private Image image;
		private Rectangle recttodraw;
		private bool withhotkey;
		internal event FLabel.AppendExpressionHandler AppendExpression;
		[Browsable(true), Category("Text"), DefaultValue(""), Description("公式的类别"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DisplayName("FType"), Localizable(true)]
		public FType Ftype
		{
			get
			{
				return this._ftype;
			}
			set
			{
				if (this._ftype != value)
				{
					this._ftype = value;
					string specialChar = CommonMethods.GetSpecialChar(this._ftype.ToString());
					if (string.IsNullOrEmpty(specialChar))
					{
						ResourceManager resourceManager = Resources.ResourceManager;
						this.image = (Image)resourceManager.GetObject(this._ftype.ToString());
						if (this.image == null)
						{
							this.image = new Bitmap(CommonMethods.height, CommonMethods.height);
						}
						this.freshimage();
					}
					else
					{
						this.generateimage(specialChar);
					}
				}
			}
		}
		public Keys HotKey
		{
			get;
			set;
		}
		public int HotKeyId
		{
			get;
			set;
		}
		public FLabel() : this(FType.包含, false)
		{
		}
		internal FLabel(FType f, bool hotkey = false)
		{
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.recttodraw = new Rectangle(0, 0, CommonMethods.height, CommonMethods.height);
			base.Margin = new Padding(0, 0, 0, 0);
			base.Padding = new Padding(0, 0, 0, 0);
			this.Font = new Font("微软雅黑", 10f, FontStyle.Regular, GraphicsUnit.Pixel, 134);
			this.DoubleBuffered = true;
			this.AutoSize = false;
			this.Text = "";
			this.Cursor = Cursors.Hand;
			base.Click += new EventHandler(this.FLabel_Click);
			base.MouseEnter += new EventHandler(this.FLabel_MouseEnter);
			base.MouseLeave += new EventHandler(this.FLabel_MouseLeave);
			this.withhotkey = hotkey;
			base.Width = CommonMethods.height;
			if (this.withhotkey)
			{
				base.Height = CommonMethods.height + CommonMethods.height / 2;
			}
			else
			{
				base.Height = CommonMethods.height;
			}
			this.Ftype = f;
		}
		private void FLabel_MouseEnter(object sender, EventArgs e)
		{
			this.BackColor = Color.PaleTurquoise;
		}
		private void FLabel_MouseLeave(object sender, EventArgs e)
		{
			this.BackColor = Color.Transparent;
		}
		private void generateimage(string str)
		{
			this.image = new Bitmap(CommonMethods.height, CommonMethods.height);
			Graphics graphics = Graphics.FromImage(this.image);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			float num = (float)CommonMethods.height;
			Font cambriaFont = CommonMethods.GetCambriaFont(num, FontStyle.Regular);
			SizeF sizeF = graphics.MeasureString(str, cambriaFont, 0, genericTypographic);
			while ((double)sizeF.Width >= (double)CommonMethods.height * 0.8 || (double)sizeF.Height >= (double)CommonMethods.height * 0.8)
			{
				num -= 10f;
				cambriaFont = CommonMethods.GetCambriaFont(num, cambriaFont.Style);
				sizeF = graphics.MeasureString(str, cambriaFont, 0, genericTypographic);
			}
			graphics.DrawString(str, cambriaFont, Brushes.Black, (float)(CommonMethods.height / 2) - sizeF.Width / 2f, (float)(CommonMethods.height / 2) - sizeF.Height / 2f, genericTypographic);
			graphics.Dispose();
			cambriaFont.Dispose();
		}
		public void freshimage()
		{
			if (this.image.Width > this.image.Height)
			{
				this.recttodraw = new Rectangle(0, (CommonMethods.height - this.image.Height * CommonMethods.height / this.image.Width) / 2, CommonMethods.height, this.image.Height * CommonMethods.height / this.image.Width);
			}
			else
			{
				this.recttodraw = new Rectangle((CommonMethods.height - this.image.Width * CommonMethods.height / this.image.Height) / 2, 0, this.image.Width * CommonMethods.height / this.image.Height, CommonMethods.height);
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.image != null)
			{
				Rectangle destRect = this.recttodraw;
				if (this.withhotkey)
				{
					StringFormat genericTypographic = StringFormat.GenericTypographic;
					genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
					string s;
					if (this.HotKey != Keys.None)
					{
						s = "Alt+" + this.HotKey.ToString();
					}
					else
					{
						s = "无热键";
					}
					e.Graphics.DrawString(s, this.Font, Brushes.Black, new PointF(0f, 0f));
					destRect = new Rectangle(this.recttodraw.Left, this.recttodraw.Top + CommonMethods.height / 2, this.recttodraw.Width, this.recttodraw.Height);
				}
				e.Graphics.DrawImage(this.image, destRect, new Rectangle(0, 0, this.image.Width, this.image.Height), GraphicsUnit.Pixel);
			}
			e.Graphics.DrawRectangle(Pens.Black, 0, 0, base.Width - 1, base.Height - 1);
		}
		private void FLabel_Click(object sender, EventArgs e)
		{
			try
			{
				this.AppendExpression(this, new ExpressionEventArgs(this.Ftype));
			}
			catch
			{
			}
		}
	}
}
