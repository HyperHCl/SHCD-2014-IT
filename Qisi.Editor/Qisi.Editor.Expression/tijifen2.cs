using Qisi.Editor.Properties;
using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class tijifen2 : type2
	{
		public tijifen2(lineexpression parent, Color color) : base(Resources.tijifenhao, parent, color, true)
		{
			base.Type = FType.体积分2;
		}
	}
}
