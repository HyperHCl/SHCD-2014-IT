using System;
using System.Drawing;
namespace Qisi.Editor.Controls
{
	public class ColorEventArgs : EventArgs
	{
		public Color Color
		{
			get;
			set;
		}
		public ColorEventArgs(Color color)
		{
			this.Color = color;
		}
	}
}
