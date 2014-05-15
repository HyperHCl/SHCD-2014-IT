using System;
using System.Drawing;
namespace Qisi.Editor.Controls
{
	public class ImageEventArgs : EventArgs
	{
		public Image Image
		{
			get;
			set;
		}
		public ImageEventArgs(Image image)
		{
			this.Image = image;
		}
	}
}
