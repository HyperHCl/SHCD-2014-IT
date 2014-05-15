using System;
namespace Qisi.General
{
	public class BoolIndexEventArgs : BoolEventArgs
	{
		public int Index
		{
			get;
			set;
		}
		public BoolIndexEventArgs(int index, bool n) : base(n)
		{
			this.Index = index;
		}
	}
}
