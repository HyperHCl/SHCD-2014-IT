using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class max : hanshuxiabiao
	{
		public max(lineexpression parent, Color color) : base("max", parent, color)
		{
			base.Type = FType.最大值;
		}
	}
}
