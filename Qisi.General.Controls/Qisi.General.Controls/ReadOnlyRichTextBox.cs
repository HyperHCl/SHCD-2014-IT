using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class ReadOnlyRichTextBox : RichTextBox
	{
		private const int WM_SETFOCUS = 7;
		private const int WM_LBUTTONDOWN = 513;
		private const int WM_LBUTTONUP = 514;
		private const int WM_LBUTTONDBLCLK = 515;
		private const int WM_RBUTTONDOWN = 516;
		private const int WM_RBUTTONUP = 517;
		private const int WM_RBUTTONDBLCLK = 518;
		private const int WM_KEYDOWN = 256;
		private const int WM_KEYUP = 257;
		private const int WM_MOUSEMOVE = 512;
		private const int WM_SETCURSOR = 32;
		private const long WS_OVERLAPPED = 0L;
		private const long WS_POPUP = 2147483648L;
		private const long WS_CHILD = 1073741824L;
		private const long WS_MINIMIZE = 536870912L;
		private const long WS_VISIBLE = 268435456L;
		private const long WS_DISABLED = 134217728L;
		private const long WS_CLIPSIBLINGS = 67108864L;
		private const long WS_CLIPCHILDREN = 33554432L;
		private const long WS_MAXIMIZE = 16777216L;
		private const long WS_CAPTION = 12582912L;
		private const int WS_BORDER = 8388608;
		private const long WS_DLGFRAME = 4194304L;
		private const int WS_VSCROLL = 2097152;
		private const int WS_HSCROLL = 1048576;
		private const long WS_SYSMENU = 524288L;
		private const long WS_THICKFRAME = 262144L;
		private const long WS_GROUP = 131072L;
		private const long WS_TABSTOP = 65536L;
		private const long WS_MINIMIZEBOX = 131072L;
		private const long WS_MAXIMIZEBOX = 65536L;
		private const int WS_EX_LEFT = 1073741824;
		private const int WS_EX_LTRREADING = 1073741824;
		private const int WS_EX_RIGHTSCROLLBAR = 1073741824;
		private const int WS_EX_ACCEPTFILES = 1073741824;
		private static IntPtr moduleHandle;
		protected override CreateParams CreateParams
		{
			get
			{
				if (ReadOnlyRichTextBox.moduleHandle == IntPtr.Zero)
				{
					ReadOnlyRichTextBox.moduleHandle = ReadOnlyRichTextBox.LoadLibrary("msftedit.dll");
					if ((long)ReadOnlyRichTextBox.moduleHandle < 32L)
					{
						throw new Win32Exception(Marshal.GetLastWin32Error(), "无法加载Msftedit.dll");
					}
				}
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "RICHEDIT50W";
				if (this.Multiline)
				{
					if ((base.ScrollBars & RichTextBoxScrollBars.Horizontal) != RichTextBoxScrollBars.None && !base.WordWrap)
					{
						createParams.Style |= 1048576;
						if ((base.ScrollBars & (RichTextBoxScrollBars)16) != RichTextBoxScrollBars.None)
						{
							createParams.Style |= 8192;
						}
					}
					if ((base.ScrollBars & RichTextBoxScrollBars.Vertical) != RichTextBoxScrollBars.None)
					{
						createParams.Style |= 2097152;
						if ((base.ScrollBars & (RichTextBoxScrollBars)16) != RichTextBoxScrollBars.None)
						{
							createParams.Style |= 8192;
						}
					}
				}
				if (BorderStyle.FixedSingle == base.BorderStyle && (createParams.Style & 8388608) != 0)
				{
					createParams.ExStyle |= 512;
				}
				if ((createParams.Style & 33220) != 33220)
				{
					createParams.Style &= 2147479807;
					createParams.Style |= 256;
				}
				return createParams;
			}
		}
		public ReadOnlyRichTextBox()
		{
			this.Cursor = Cursors.Arrow;
			base.TabStop = true;
			base.ReadOnly = true;
			this.BackColor = Color.White;
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 7 || m.Msg == 256 || m.Msg == 257 || m.Msg == 513 || m.Msg == 514 || m.Msg == 515 || m.Msg == 516 || m.Msg == 517 || m.Msg == 518 || m.Msg == 512 || m.Msg == 32)
			{
				return;
			}
			base.WndProc(ref m);
		}
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string path);
	}
}
