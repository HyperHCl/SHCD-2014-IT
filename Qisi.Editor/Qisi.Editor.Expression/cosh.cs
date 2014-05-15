using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class cosh : hanshu
	{
		public cosh(lineexpression parent, Color color) : base("cosh", parent, color)
		{
			base.Type = FType.双曲余弦;
		}
	}
}
