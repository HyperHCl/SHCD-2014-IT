using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
namespace Qisi.Editor
{
	internal static class NativeMethods
	{
		[StructLayout(LayoutKind.Explicit)]
		internal struct Rect
		{
			[FieldOffset(0)]
			public int left;
			[FieldOffset(4)]
			public int top;
			[FieldOffset(8)]
			public int right;
			[FieldOffset(12)]
			public int bottom;
		}
		internal struct COMPOSITIONFORM
		{
			public int dwStyle;
			public Point ptCurrentPos;
			public NativeMethods.Rect rcArea;
		}
		[Flags]
		public enum KeyModifiers
		{
			None = 0,
			Alt = 1,
			Ctrl = 2,
			Shift = 4,
			WindowsKey = 8,
			CtrlAndShift = 6
		}
		internal const int DRIVERVERSION = 0;
		internal const int TECHNOLOGY = 2;
		internal const int HORZSIZE = 4;
		internal const int VERTSIZE = 6;
		internal const int HORZRES = 8;
		internal const int VERTRES = 10;
		internal const int BITSPIXEL = 12;
		internal const int PLANES = 14;
		internal const int NUMBRUSHES = 16;
		internal const int NUMPENS = 18;
		internal const int NUMMARKERS = 20;
		internal const int NUMFONTS = 22;
		internal const int NUMCOLORS = 24;
		internal const int PDEVICESIZE = 26;
		internal const int CURVECAPS = 28;
		internal const int LINECAPS = 30;
		internal const int POLYGONALCAPS = 32;
		internal const int TEXTCAPS = 34;
		internal const int CLIPCAPS = 36;
		internal const int RASTERCAPS = 38;
		internal const int ASPECTX = 40;
		internal const int ASPECTY = 42;
		internal const int ASPECTXY = 44;
		internal const int SHADEBLENDCAPS = 45;
		internal const int LOGPIXELSX = 88;
		internal const int LOGPIXELSY = 90;
		internal const int SIZEPALETTE = 104;
		internal const int NUMRESERVED = 106;
		internal const int COLORRES = 108;
		internal const int PHYSICALWIDTH = 110;
		internal const int PHYSICALHEIGHT = 111;
		internal const int PHYSICALOFFSETX = 112;
		internal const int PHYSICALOFFSETY = 113;
		internal const int SCALINGFACTORX = 114;
		internal const int SCALINGFACTORY = 115;
		internal const int VREFRESH = 116;
		internal const int DESKTOPVERTRES = 117;
		internal const int DESKTOPHORZRES = 118;
		internal const int BLTALIGNMENT = 119;
		internal const int CFS_POINT = 2;
		internal const int WM_IME_STARTCOMPOSITION = 269;
		[DllImport("user32.dll")]
		internal static extern IntPtr GetDC(IntPtr ptr);
		[DllImport("user32.dll")]
		internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);
		[DllImport("gdi32.dll")]
		internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
		[DllImport("user32.dll")]
		internal static extern bool SetProcessDPIAware();
		[DllImport("Imm32.dll")]
		internal static extern IntPtr ImmGetContext(IntPtr hWnd);
		[DllImport("imm32.dll")]
		internal static extern int ImmSetCompositionWindow(IntPtr hIMC, ref NativeMethods.COMPOSITIONFORM lpCompositionForm);
		[DllImport("Imm32.dll")]
		internal static extern bool ImmReleaseContext(IntPtr hWnd, int hIMC);
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern void CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern void ShowCaret(IntPtr hWnd);
		[DllImport("user32.dll")]
		internal static extern bool HideCaret(IntPtr hWnd);
		[DllImport("user32")]
		internal static extern int SetCaretPos(int x, int y);
		[DllImport("user32.dll")]
		internal static extern int DestroyCaret();
		[DllImport("User32.dll")]
		internal static extern bool GetCaretPos(out Point point);
		public static Color MixColor(Color c1, Color c2)
		{
			return Color.FromArgb((int)((c1.A + c2.A) / 2), (int)((c1.R + c2.R) / 2), (int)((c1.G + c2.G) / 2), (int)((c1.B + c2.B) / 2));
		}
		public static Color getRevColor(Color c)
		{
			return Color.FromArgb((int)(255 - c.R), (int)(255 - c.G), (int)(255 - c.B));
		}
		public static string GetMD5HashFromFile(string fileName)
		{
			string result;
			try
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Open);
				MD5 mD = new MD5CryptoServiceProvider();
				byte[] array = mD.ComputeHash(fileStream);
				fileStream.Close();
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				result = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
			}
			return result;
		}
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, NativeMethods.KeyModifiers fsModifiers, Keys vk);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("kernel32.dll")]
		public static extern int GlobalAddAtom(string lpString);
		[DllImport("kernel32.dll")]
		public static extern int GlobalDeleteAtom(uint nAtom);
	}
}
