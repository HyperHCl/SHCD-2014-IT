using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class lifanggen : genshi
	{
		public lifanggen(lineexpression parent, Color color) : base(parent, color)
		{
			base.Type = FType.立方根;
			base.Gen.Child = new List<structexpression>();
			base.Gen.Child.Add(new charexpression("3", base.Gen, color));
		}
	}
}
