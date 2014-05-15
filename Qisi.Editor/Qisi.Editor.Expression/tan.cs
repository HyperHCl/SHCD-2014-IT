using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class tan : hanshu
	{
		public tan(lineexpression parent, Color color) : base("tan", parent, color)
		{
			base.Type = FType.正切;
		}
	}
}
