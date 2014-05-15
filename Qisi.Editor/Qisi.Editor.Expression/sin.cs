using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class sin : hanshu
	{
		public sin(lineexpression parent, Color color) : base("sin", parent, color)
		{
			base.Type = FType.正弦;
		}
	}
}
