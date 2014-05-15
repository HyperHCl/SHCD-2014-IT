using Qisi.Editor.Expression;
using System;
namespace Qisi.Editor.Controls
{
	internal class ExpressionEventArgs : EventArgs
	{
		public FType Type
		{
			get;
			set;
		}
		public ExpressionEventArgs(FType ft)
		{
			this.Type = ft;
		}
	}
}
