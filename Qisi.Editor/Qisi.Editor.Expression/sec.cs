using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class sec : hanshu
	{
		public sec(lineexpression parent, Color color) : base("sec", parent, color)
		{
			base.Type = FType.正割;
		}
	}
}
