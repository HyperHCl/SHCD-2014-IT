using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class arctan : hanshu
	{
		public arctan(lineexpression parent, Color color) : base("arctan", parent, color)
		{
			base.Type = FType.反正切;
		}
	}
}
