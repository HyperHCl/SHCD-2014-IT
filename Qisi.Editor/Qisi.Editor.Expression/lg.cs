using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class lg : hanshu
	{
		public lg(lineexpression parent, Color color) : base("lg", parent, color)
		{
			base.Type = FType.对数10;
		}
	}
}
