using System;
namespace Qisi.General
{
	public class BoolEventArgs : EventArgs
	{
		private bool _message;
		public bool Message
		{
			get
			{
				return this._message;
			}
			set
			{
				this._message = value;
			}
		}
		public BoolEventArgs(bool message)
		{
			this._message = message;
		}
	}
}
