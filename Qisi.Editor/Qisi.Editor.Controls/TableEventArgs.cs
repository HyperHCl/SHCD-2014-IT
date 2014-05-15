using System;
using System.Drawing;
namespace Qisi.Editor.Controls
{
	public class TableEventArgs : EventArgs
	{
		public Point TableSize
		{
			get;
			set;
		}
		public TableEventArgs(Point size)
		{
			this.TableSize = size;
		}
	}
}
