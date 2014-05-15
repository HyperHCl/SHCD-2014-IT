using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class log : hanshu
	{
		public log(lineexpression parent, Color color) : base("log", parent, color)
		{
			base.Type = FType.对数2;
		}
	}
}
