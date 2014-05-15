using Qisi.Editor.Properties;
using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class chengji3 : type3
	{
		public chengji3(lineexpression parent, Color color) : base(Resources.chengjihao, parent, color, false)
		{
			base.Type = FType.乘积3;
		}
	}
}
