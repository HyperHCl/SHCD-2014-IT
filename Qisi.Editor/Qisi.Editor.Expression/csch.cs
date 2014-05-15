using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class csch : hanshu
	{
		public csch(lineexpression parent, Color color) : base("csch", parent, color)
		{
			base.Type = FType.双曲余割;
		}
	}
}
