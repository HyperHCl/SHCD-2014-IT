using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
namespace ExamClientControlsLibrary
{
	[SecurityCritical]
	internal static class NativeMethods
	{
		public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
		private const int WM_CLOSE = 16;
		private const int WM_DESTORY = 2;
		private const int WM_QUIT = 18;
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
		[DllImport("user32.dll")]
		public static extern bool IsWindow(IntPtr hWnd);
		[DllImport("User32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("User32.dll")]
		private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		internal static void ShutdownForms(string closeList)
		{
			Hashtable hashtable = new Hashtable();
			List<string> list = new List<string>();
			list.AddRange(closeList.Split(new char[]
			{
				'|'
			}));
			list.Add("DV2ControlHost");
			list.Add("BaseBar");
			foreach (string current in list)
			{
				IntPtr intPtr = NativeMethods.FindWindow(current, null);
				if (intPtr != IntPtr.Zero && NativeMethods.IsWindow(intPtr))
				{
					NativeMethods.PostMessage(intPtr, 16, 0, 0);
				}
				else
				{
					intPtr = IntPtr.Zero;
				}
			}
		}
		internal static void ShutdownForms(string closeList, string exceptClass)
		{
			Hashtable hashtable = new Hashtable();
			List<string> list = new List<string>();
			list.AddRange(closeList.Split(new char[]
			{
				'|'
			}));
			list.Add("DV2ControlHost");
			list.Add("BaseBar");
			foreach (string current in list)
			{
				IntPtr intPtr = NativeMethods.FindWindow(current, null);
				if (intPtr != IntPtr.Zero && NativeMethods.IsWindow(intPtr))
				{
					StringBuilder stringBuilder = new StringBuilder(1024);
					NativeMethods.GetWindowText(intPtr, stringBuilder, 255);
					if (!stringBuilder.ToString().Contains(exceptClass))
					{
						NativeMethods.PostMessage(intPtr, 16, 0, 0);
					}
				}
				else
				{
					intPtr = IntPtr.Zero;
				}
			}
		}
	}
}
