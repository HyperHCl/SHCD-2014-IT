using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class tanh : hanshu
	{
		public tanh(lineexpression parent, Color color) : base("tanh", parent, color)
		{
			base.Type = FType.双曲正切;
		}
	}
}
