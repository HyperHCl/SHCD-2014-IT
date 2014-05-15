using System;
using System.Drawing;
namespace Qisi.Editor.Controls
{
	public class FontEventArgs : EventArgs
	{
		public Font Font
		{
			get;
			set;
		}
		public FontEventArgs(Font font)
		{
			this.Font = font;
		}
	}
}
