using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class arccos : hanshu
	{
		public arccos(lineexpression parent, Color color) : base("arccos", parent, color)
		{
			base.Type = FType.反余弦;
		}
	}
}
