using System;
namespace Qisi.General
{
	public class MessageEventArgs : EventArgs
	{
		private string _message;
		public string Message
		{
			get
			{
				return this._message;
			}
		}
		public MessageEventArgs(string Message)
		{
			this._message = Message;
		}
	}
}
