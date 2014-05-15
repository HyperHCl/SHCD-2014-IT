using Qisi.Editor.Properties;
using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class mianjifen2 : type2
	{
		public mianjifen2(lineexpression parent, Color color) : base(Resources.mjhao, parent, color, true)
		{
			base.Type = FType.面积分2;
		}
	}
}
