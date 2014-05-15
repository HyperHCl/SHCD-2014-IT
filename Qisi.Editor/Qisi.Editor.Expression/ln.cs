using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class ln : hanshu
	{
		public ln(lineexpression parent, Color color) : base("ln", parent, color)
		{
			base.Type = FType.对数e;
		}
	}
}
