using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class coth : hanshu
	{
		public coth(lineexpression parent, Color color) : base("coth", parent, color)
		{
			base.Type = FType.双曲余切;
		}
	}
}
