using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class cos : hanshu
	{
		public cos(lineexpression parent, Color color) : base("cos", parent, color)
		{
			base.Type = FType.余弦;
		}
	}
}
