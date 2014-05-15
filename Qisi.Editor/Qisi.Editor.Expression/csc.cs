using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class csc : hanshu
	{
		public csc(lineexpression parent, Color color) : base("csc", parent, color)
		{
			base.Type = FType.余割;
		}
	}
}
