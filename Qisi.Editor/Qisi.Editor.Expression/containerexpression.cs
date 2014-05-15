using Qisi.Editor.Documents.Elements;
using System;
using System.Drawing;
using System.Xml;
namespace Qisi.Editor.Expression
{
	internal class containerexpression : lineexpression
	{
		public ExpressionInfo Info
		{
			get;
			set;
		}
		public containerexpression(Font font) : base(font)
		{
		}
		public containerexpression(XmlNode node, Font font) : base(node, font)
		{
		}
	}
}
