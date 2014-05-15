using System;
namespace Qisi.General
{
	public class IndexEventArgs : EventArgs
	{
		private int index;
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}
		public IndexEventArgs(int i)
		{
			this.index = i;
		}
	}
}
