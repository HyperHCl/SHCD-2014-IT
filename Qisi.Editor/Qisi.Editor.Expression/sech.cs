using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class sech : hanshu
	{
		public sech(lineexpression parent, Color color) : base("sech", parent, color)
		{
			base.Type = FType.双曲正割;
		}
	}
}
