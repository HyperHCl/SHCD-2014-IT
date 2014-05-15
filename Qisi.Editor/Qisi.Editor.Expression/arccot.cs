using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class arccot : hanshu
	{
		public arccot(lineexpression parent, Color color) : base("arccot", parent, color)
		{
			base.Type = FType.反余切;
		}
	}
}
