using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class lim : hanshuxiabiao
	{
		public lim(lineexpression parent, Color color) : base("lim", parent, color)
		{
			base.Type = FType.极限;
		}
	}
}
