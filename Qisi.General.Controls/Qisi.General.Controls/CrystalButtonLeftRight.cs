using System;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class CrystalButtonLeftRight : CrystalButton
	{
		public CrystalButtonLeftRight() : base(TextImageRelation.ImageBeforeText)
		{
			base.Size = new Size(60, 20);
		}
	}
}
