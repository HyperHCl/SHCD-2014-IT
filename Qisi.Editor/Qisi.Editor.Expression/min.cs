using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class min : hanshuxiabiao
	{
		public min(lineexpression parent, Color color) : base("min", parent, color)
		{
			base.Type = FType.最小值;
		}
	}
}
