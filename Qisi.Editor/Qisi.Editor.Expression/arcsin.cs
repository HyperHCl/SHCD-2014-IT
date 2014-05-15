using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class arcsin : hanshu
	{
		public arcsin(lineexpression parent, Color color) : base("arcsin", parent, color)
		{
			base.Type = FType.反正弦;
		}
	}
}
