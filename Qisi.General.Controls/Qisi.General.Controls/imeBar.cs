using Qisi.General.Controls.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class imeBar : Control
	{
		public delegate bool EnumResNameProc(IntPtr hModule, IntPtr nType, StringBuilder sName, IntPtr lParam);
		private struct imeItem
		{
			public string LayoutName;
			public InputLanguage Tag;
			public Image image;
		}
		private const int DONT_RESOLVE_DLL_REFERENCES = 1;
		private const int LOAD_LIBRARY_AS_DATAFILE = 2;
		private const int LOAD_WITH_ALTERED_SEARCH_PATH = 8;
		private const int RT_ICON = 3;
		private const int RT_BITMAP = 2;
		private const int RT_GROUP_ICON = 14;
		private Form container;
		private string _CurrentImeHandleStr = "";
		private ComponentResourceManager resources = new ComponentResourceManager(typeof(Form));
		private static readonly int IMAGE_ICON = 1;
		private List<imeBar.imeItem> imeList;
		private IContainer components;
		private PictureBox pictureBox1;
		private Label label1;
		private ComboBox comboBox1;
		private PictureBox pictureBox2;
		private PictureBox pictureBox3;
		private Timer timer1;
		public imeBar()
		{
			this.InitializeComponent();
			this.InitMenus();
			this.timer1.Enabled = true;
		}
		[DllImport("Kernel32.dll")]
		public static extern bool FreeLibrary(IntPtr hModule);
		[DllImport("user32.dll")]
		public static extern IntPtr LoadIcon(IntPtr hInstance, string iID);
		[DllImport("Imm32.dll")]
		public static extern int ImmGetDescription(IntPtr Hkl, StringBuilder sName, int nBuffer);
		[DllImport("Imm32.dll")]
		public static extern int ImmGetIMEFileName(IntPtr Hkl, StringBuilder sFileName, int nBuffer);
		[DllImport("Kernel32.dll")]
		public static extern IntPtr LoadLibraryEx(string sFileName, IntPtr hFile, int dwFlags);
		[DllImport("Kernel32.dll")]
		public static extern bool EnumResourceNames(IntPtr hModule, IntPtr nType, imeBar.EnumResNameProc lpEnumFunc, int lParam);
		[DllImport("shell32.dll")]
		public static extern IntPtr ExtractIcon(IntPtr hInstance, string sExeFileName, int nIconIndex);
		[DllImport("user32.dll")]
		public static extern IntPtr LoadImage(IntPtr hInstance, string sID, int nType, int cx, int cy, int fuLoad);
		[DllImport("imm32.dll")]
		public static extern IntPtr ImmGetContext(IntPtr hWnd);
		[DllImport("imm32.dll")]
		public static extern bool ImmGetConversionStatus(IntPtr hIMC, ref int conversion, ref int sentence);
		[DllImport("imm32.dll")]
		public static extern bool ImmSetConversionStatus(IntPtr hIMC, int conversion, int sentence);
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr GetFocus();
		private void InitMenus()
		{
			this.imeList = new List<imeBar.imeItem>();
			this.comboBox1.Items.Clear();
			foreach (InputLanguage inputLanguage in InputLanguage.InstalledInputLanguages)
			{
				string immDescription = this.GetImmDescription(inputLanguage);
				if (!string.IsNullOrEmpty(immDescription))
				{
					imeBar.imeItem item = default(imeBar.imeItem);
					item.LayoutName = immDescription;
					item.Tag = inputLanguage;
					item.image = this.GetImeBitmap(inputLanguage);
					this.comboBox1.Items.Add(item.LayoutName);
					this.imeList.Add(item);
				}
			}
			this.InputLanauageChangedUI();
		}
		private string GetImmDescription(InputLanguage inpt)
		{
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			num = imeBar.ImmGetDescription(inpt.Handle, null, num);
			stringBuilder = new StringBuilder(num);
			imeBar.ImmGetDescription(inpt.Handle, stringBuilder, num);
			string text = stringBuilder.ToString();
			if (string.IsNullOrEmpty(text))
			{
				text = inpt.LayoutName;
			}
			return text;
		}
		private Image GetImeBitmap(InputLanguage ime)
		{
			int num = 0;
			Image image = null;
			num = imeBar.ImmGetIMEFileName(ime.Handle, null, num);
			StringBuilder stringBuilder = new StringBuilder(num);
			imeBar.ImmGetIMEFileName(ime.Handle, stringBuilder, num);
			if (string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				return Resources.input;
			}
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), stringBuilder.ToString());
			if (File.Exists(text))
			{
				image = this.GetBitmapFromResource(text, "");
			}
			if (image == null)
			{
				image = Resources.input;
			}
			return image;
		}
		private Image GetBitmapFromResource(string sFileName, string sBitmapFlag)
		{
			Bitmap result = null;
			IntPtr intPtr = imeBar.LoadLibraryEx(sFileName, IntPtr.Zero, 2);
			if (intPtr == IntPtr.Zero)
			{
				Debug.WriteLine("未能成功加载" + sFileName);
				return null;
			}
			IntPtr intPtr2 = IntPtr.Zero;
			Debug.WriteLine("正在获取" + sFileName + "中所有图标。");
			intPtr2 = imeBar.ExtractIcon(base.Handle, sFileName, 0);
			if (intPtr2 == IntPtr.Zero)
			{
				string sID = "#101";
				intPtr2 = imeBar.LoadImage(intPtr, sID, imeBar.IMAGE_ICON, 16, 16, 0);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Debug.WriteLine(string.Format("Hicon:{0}", intPtr2.ToString()));
				result = Icon.FromHandle(intPtr2).ToBitmap();
			}
			imeBar.EnumResourceNames(intPtr, this.MAKEINTRESOURCE(14), new imeBar.EnumResNameProc(this.EnumIconResourceProc), 0);
			imeBar.FreeLibrary(intPtr);
			return result;
		}
		private IntPtr MAKEINTRESOURCE(int nID)
		{
			return new IntPtr((long)((short)nID));
		}
		private bool EnumIconResourceProc(IntPtr hModule, IntPtr nType, StringBuilder sName, IntPtr lParam)
		{
			Debug.WriteLine(string.Format("得到的资源名称：{0}", sName));
			IntPtr handle = imeBar.LoadIcon(hModule, sName.ToString());
			Icon.FromHandle(handle);
			return true;
		}
		public void ChangeIme(IntPtr handle)
		{
			this._CurrentImeHandleStr = handle.ToString();
			foreach (imeBar.imeItem current in this.imeList)
			{
				if (current.Tag.Handle.ToString() == this._CurrentImeHandleStr)
				{
					this.comboBox1.SelectedIndex = this.imeList.IndexOf(current);
				}
			}
			this.InputLanauageChangedUI();
		}
		public void ChangeIme()
		{
			this._CurrentImeHandleStr = imeBar.ImmGetContext(imeBar.GetFocus()).ToString();
			foreach (imeBar.imeItem current in this.imeList)
			{
				if (current.Tag.Handle.ToString() == this._CurrentImeHandleStr)
				{
					this.comboBox1.SelectedIndex = this.imeList.IndexOf(current);
				}
			}
			this.InputLanauageChangedUI();
		}
		public void InputLanauageChangedUI()
		{
			foreach (imeBar.imeItem current in this.imeList)
			{
				if (current.Tag.Handle.ToString() == this._CurrentImeHandleStr)
				{
					this.pictureBox1.Image = current.image;
				}
			}
			this.changeLabel();
		}
		protected override void OnPrint(PaintEventArgs e)
		{
			base.OnPrint(e);
			if (this.container == null)
			{
				this.container = base.FindForm();
				if (this.container != null)
				{
					this.container.InputLanguageChanged += new InputLanguageChangedEventHandler(this.container_InputLanguageChanged);
					this.ChangeIme(Application.CurrentInputLanguage.Handle);
				}
			}
		}
		private void container_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
		{
			this.ChangeIme(Application.CurrentInputLanguage.Handle);
		}
		private void imeBar_SizeChanged(object sender, EventArgs e)
		{
			this.pictureBox1.Size = new Size(base.ClientSize.Height, base.ClientSize.Height);
			this.pictureBox1.Location = new Point(0, 0);
			Graphics graphics = this.label1.CreateGraphics();
			int num = 1;
			Font font = new Font("宋体", (float)num, FontStyle.Regular, GraphicsUnit.Pixel);
			while ((int)graphics.MeasureString("中", font).Height < base.Height)
			{
				num++;
				font = new Font("宋体", (float)num, FontStyle.Regular, GraphicsUnit.Pixel);
			}
			num--;
			font = new Font("宋体", (float)num, FontStyle.Regular, GraphicsUnit.Pixel);
			this.label1.Height = base.ClientSize.Height;
			this.label1.Width = (int)graphics.MeasureString("中", font).Width;
			this.label1.Top = 0;
			this.label1.Left = this.pictureBox1.Right;
			this.label1.Font = font;
			this.pictureBox2.Size = this.pictureBox1.Size;
			this.pictureBox3.Size = this.pictureBox2.Size;
			this.pictureBox2.Location = new Point(this.label1.Right, 0);
			this.pictureBox3.Location = new Point(this.pictureBox2.Right, 0);
			this.comboBox1.Top = 0;
			this.comboBox1.Left = this.pictureBox3.Right;
			this.comboBox1.Height = base.ClientSize.Height;
			this.comboBox1.Width = base.ClientSize.Width - this.comboBox1.Left;
			this.comboBox1.Font = font;
		}
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Application.CurrentInputLanguage = this.imeList[this.comboBox1.SelectedIndex].Tag;
		}
		private void changeLabel()
		{
			IntPtr hIMC;
			if (this.container != null && this.container.ActiveControl != null)
			{
				hIMC = imeBar.ImmGetContext(this.container.ActiveControl.Handle);
			}
			else
			{
				hIMC = imeBar.ImmGetContext(imeBar.GetFocus());
			}
			int iMode = 0;
			int num = 0;
			imeBar.ImmGetConversionStatus(hIMC, ref iMode, ref num);
			int num2 = this.Calc(iMode);
			if (num2 <= -2147482615)
			{
				if (num2 <= -2147483639)
				{
					switch (num2)
					{
					case -2147483648:
						goto IL_224;
					case -2147483647:
						goto IL_160;
					default:
						switch (num2)
						{
						case -2147483640:
							goto IL_255;
						case -2147483639:
							goto IL_191;
						default:
							return;
						}
						break;
					}
				}
				else
				{
					switch (num2)
					{
					case -2147482624:
						goto IL_1C2;
					case -2147482623:
						break;
					default:
						switch (num2)
						{
						case -2147482616:
							goto IL_1F3;
						case -2147482615:
							goto IL_12F;
						default:
							return;
						}
						break;
					}
				}
			}
			else
			{
				if (num2 <= 9)
				{
					switch (num2)
					{
					case 0:
						goto IL_224;
					case 1:
						goto IL_160;
					default:
						switch (num2)
						{
						case 8:
							goto IL_255;
						case 9:
							goto IL_191;
						default:
							return;
						}
						break;
					}
				}
				else
				{
					switch (num2)
					{
					case 1024:
						goto IL_1C2;
					case 1025:
						break;
					default:
						switch (num2)
						{
						case 1032:
							goto IL_1F3;
						case 1033:
							goto IL_12F;
						default:
							return;
						}
						break;
					}
				}
			}
			this.label1.Text = "中";
			this.pictureBox2.Image = Resources.Half;
			this.pictureBox3.Image = Resources.chs;
			return;
			IL_12F:
			this.label1.Text = "中";
			this.pictureBox2.Image = Resources.On;
			this.pictureBox3.Image = Resources.chs;
			return;
			IL_160:
			this.label1.Text = "中";
			this.pictureBox2.Image = Resources.Half;
			this.pictureBox3.Image = Resources.eng;
			return;
			IL_191:
			this.label1.Text = "中";
			this.pictureBox2.Image = Resources.On;
			this.pictureBox3.Image = Resources.eng;
			return;
			IL_1C2:
			this.label1.Text = "英";
			this.pictureBox2.Image = Resources.Half;
			this.pictureBox3.Image = Resources.chs;
			return;
			IL_1F3:
			this.label1.Text = "英";
			this.pictureBox2.Image = Resources.On;
			this.pictureBox3.Image = Resources.chs;
			return;
			IL_224:
			this.label1.Text = "英";
			this.pictureBox2.Image = Resources.Half;
			this.pictureBox3.Image = Resources.eng;
			return;
			IL_255:
			this.label1.Text = "英";
			this.pictureBox2.Image = Resources.On;
			this.pictureBox3.Image = Resources.eng;
		}
		private void label1_Click(object sender, EventArgs e)
		{
			IntPtr hIMC = imeBar.ImmGetContext(imeBar.GetFocus());
			int num = 1033;
			int sentence = 0;
			imeBar.ImmGetConversionStatus(hIMC, ref num, ref sentence);
			int num2 = this.Calc(num);
			if (num2 <= -2147482615)
			{
				if (num2 <= -2147483639)
				{
					if (num2 != -2147483647 && num2 != -2147483639)
					{
						goto IL_80;
					}
				}
				else
				{
					if (num2 != -2147482623 && num2 != -2147482615)
					{
						goto IL_80;
					}
				}
			}
			else
			{
				if (num2 <= 9)
				{
					if (num2 != 1 && num2 != 9)
					{
						goto IL_80;
					}
				}
				else
				{
					if (num2 != 1025 && num2 != 1033)
					{
						goto IL_80;
					}
				}
			}
			num--;
			goto IL_84;
			IL_80:
			num++;
			IL_84:
			hIMC = imeBar.ImmGetContext(imeBar.GetFocus());
			if (imeBar.ImmSetConversionStatus(hIMC, num, sentence))
			{
				this.changeLabel();
			}
		}
		private void pictureBox2_Click(object sender, EventArgs e)
		{
			IntPtr hIMC = imeBar.ImmGetContext(imeBar.GetFocus());
			int num = 1033;
			int sentence = 0;
			imeBar.ImmGetConversionStatus(hIMC, ref num, ref sentence);
			int num2 = this.Calc(num);
			if (num2 <= -2147482615)
			{
				switch (num2)
				{
				case -2147483640:
				case -2147483639:
					break;
				default:
					switch (num2)
					{
					case -2147482616:
					case -2147482615:
						break;
					default:
						goto IL_8A;
					}
					break;
				}
			}
			else
			{
				switch (num2)
				{
				case 8:
				case 9:
					break;
				default:
					switch (num2)
					{
					case 1032:
					case 1033:
						break;
					default:
						goto IL_8A;
					}
					break;
				}
			}
			num -= 8;
			Bitmap arg_87_0 = Resources.Half;
			goto IL_94;
			IL_8A:
			num += 8;
			Bitmap arg_93_0 = Resources.On;
			IL_94:
			hIMC = imeBar.ImmGetContext(imeBar.GetFocus());
			if (imeBar.ImmSetConversionStatus(hIMC, num, sentence))
			{
				this.changeLabel();
			}
		}
		private void pictureBox3_Click(object sender, EventArgs e)
		{
			IntPtr hIMC = imeBar.ImmGetContext(imeBar.GetFocus());
			int num = 1033;
			int sentence = 0;
			imeBar.ImmGetConversionStatus(hIMC, ref num, ref sentence);
			int num2 = this.Calc(num);
			if (num2 <= -2147482615)
			{
				switch (num2)
				{
				case -2147482624:
				case -2147482623:
					break;
				default:
					switch (num2)
					{
					case -2147482616:
					case -2147482615:
						break;
					default:
						goto IL_92;
					}
					break;
				}
			}
			else
			{
				switch (num2)
				{
				case 1024:
				case 1025:
					break;
				default:
					switch (num2)
					{
					case 1032:
					case 1033:
						break;
					default:
						goto IL_92;
					}
					break;
				}
			}
			num -= 1024;
			Bitmap arg_8F_0 = Resources.eng;
			goto IL_A0;
			IL_92:
			num += 1024;
			Bitmap arg_9F_0 = Resources.chs;
			IL_A0:
			hIMC = imeBar.ImmGetContext(imeBar.GetFocus());
			if (imeBar.ImmSetConversionStatus(hIMC, num, sentence))
			{
				this.changeLabel();
			}
		}
		private int Calc(int iMode)
		{
			if (iMode >= 0)
			{
				int num = 11;
				while (iMode - this.Pow(2, num) >= 0)
				{
					num++;
				}
				if (num != 11)
				{
					iMode -= this.Pow(2, num - 1);
				}
			}
			else
			{
				iMode = iMode + 2147483647 + 1;
				int num2 = 11;
				while (iMode - this.Pow(2, num2) >= 0)
				{
					num2++;
				}
				if (num2 != 11)
				{
					iMode -= this.Pow(2, num2 - 1);
				}
				iMode = iMode - 2147483647 - 1;
			}
			return iMode;
		}
		private int Pow(int x, int y)
		{
			if (y < 0 || y > 31)
			{
				return 2147483647;
			}
			switch (y)
			{
			case 0:
				return 1;
			case 1:
				return x;
			default:
				return x * this.Pow(x, y - 1);
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.InputLanauageChangedUI();
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
			this.components = new Container();
			this.pictureBox1 = new PictureBox();
			this.label1 = new Label();
			this.comboBox1 = new ComboBox();
			this.pictureBox2 = new PictureBox();
			this.pictureBox3 = new PictureBox();
			this.timer1 = new Timer(this.components);
			((ISupportInitialize)this.pictureBox1).BeginInit();
			((ISupportInitialize)this.pictureBox2).BeginInit();
			((ISupportInitialize)this.pictureBox3).BeginInit();
			base.SuspendLayout();
			this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
			this.pictureBox1.Location = new Point(16, 3);
			this.pictureBox1.Margin = new Padding(0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(100, 50);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.label1.BorderStyle = BorderStyle.FixedSingle;
			this.label1.Cursor = Cursors.Hand;
			this.label1.Location = new Point(122, 26);
			this.label1.Margin = new Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(100, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			this.label1.Click += new EventHandler(this.label1_Click);
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.FlatStyle = FlatStyle.Flat;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(189, 23);
			this.comboBox1.Margin = new Padding(0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new Size(121, 20);
			this.comboBox1.TabIndex = 4;
			this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
			this.pictureBox2.BorderStyle = BorderStyle.FixedSingle;
			this.pictureBox2.Cursor = Cursors.Hand;
			this.pictureBox2.Location = new Point(147, 49);
			this.pictureBox2.Margin = new Padding(0);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new Size(100, 50);
			this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 5;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
			this.pictureBox3.BorderStyle = BorderStyle.FixedSingle;
			this.pictureBox3.Cursor = Cursors.Hand;
			this.pictureBox3.Location = new Point(189, 26);
			this.pictureBox3.Margin = new Padding(0);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new Size(100, 50);
			this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox3.TabIndex = 6;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Click += new EventHandler(this.pictureBox3_Click);
			this.timer1.Interval = 1000;
			this.timer1.Tick += new EventHandler(this.timer1_Tick);
			base.Controls.Add(this.pictureBox3);
			base.Controls.Add(this.pictureBox2);
			base.Controls.Add(this.comboBox1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.pictureBox1);
			base.Margin = new Padding(0);
			base.Size = new Size(325, 65);
			base.SizeChanged += new EventHandler(this.imeBar_SizeChanged);
			((ISupportInitialize)this.pictureBox1).EndInit();
			((ISupportInitialize)this.pictureBox2).EndInit();
			((ISupportInitialize)this.pictureBox3).EndInit();
			base.ResumeLayout(false);
		}
	}
}
