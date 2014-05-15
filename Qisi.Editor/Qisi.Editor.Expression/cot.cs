using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class cot : hanshu
	{
		public cot(lineexpression parent, Color color) : base("cot", parent, color)
		{
			base.Type = FType.余切;
		}
	}
}
