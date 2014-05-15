using System;
using System.Drawing;
namespace Qisi.General.Controls
{
	internal static class NativeMethods
	{
		internal struct MINMAXINFO
		{
			public Point reserved;
			public Size maxSize;
			public Point maxPosition;
			public Size minTrackSize;
			public Size maxTrackSize;
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
		internal const int WM_GETMINMAXINFO = 36;
		internal const int WM_NCHITTEST = 132;
		internal const int HTTRANSPARENT = -1;
		internal const int HTLEFT = 10;
		internal const int HTRIGHT = 11;
		internal const int HTTOP = 12;
		internal const int HTTOPLEFT = 13;
		internal const int HTTOPRIGHT = 14;
		internal const int HTBOTTOM = 15;
		internal const int HTBOTTOMLEFT = 16;
		internal const int HTBOTTOMRIGHT = 17;
		internal static int HIWORD(int n)
		{
			return n >> 16 & 65535;
		}
		internal static int HIWORD(IntPtr n)
		{
			return NativeMethods.HIWORD((int)((long)n));
		}
		internal static int LOWORD(int n)
		{
			return n & 65535;
		}
		internal static int LOWORD(IntPtr n)
		{
			return NativeMethods.LOWORD((int)((long)n));
		}
	}
}
