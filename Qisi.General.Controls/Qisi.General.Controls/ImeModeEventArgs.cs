using System;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class ImeModeEventArgs : EventArgs
	{
		private ImeMode _imemode;
		public ImeMode ImeMode
		{
			get
			{
				return this._imemode;
			}
		}
		public ImeModeEventArgs(ImeMode imemode)
		{
			this._imemode = imemode;
		}
	}
}
