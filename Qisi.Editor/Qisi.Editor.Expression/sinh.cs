using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class sinh : hanshu
	{
		public sinh(lineexpression parent, Color color) : base("sinh", parent, color)
		{
			base.Type = FType.双曲正弦;
		}
	}
}
