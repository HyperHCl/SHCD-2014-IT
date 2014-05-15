using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class tanh_2 : hanshushangbiao
	{
		public tanh_2(lineexpression parent, Color color) : base("tanh", parent, color)
		{
			base.Type = FType.双曲正切_2;
		}
	}
}
