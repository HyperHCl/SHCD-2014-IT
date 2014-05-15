using Qisi.Editor.Documents;
using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class charexpression : structexpression
	{
		protected string content = "";
		public charexpression(string str, lineexpression parent, Color color) : base(parent, color, false)
		{
			this.content = Document.FromEscape(str);
			base.Type = FType.字符;
		}
		public override string ToString()
		{
			return this.content;
		}
		public override void RefreshInputLocation()
		{
		}
		public override void DrawExpression(Graphics g)
		{
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			SolidBrush solidBrush = new SolidBrush(this.Color);
			g.DrawString(this.content, base.ParentExpression.Font, solidBrush, base.InputLocation, genericTypographic);
			solidBrush.Dispose();
		}
		public override void RefreshRegion(Graphics g)
		{
			g.PageUnit = GraphicsUnit.Pixel;
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			base.Region = g.MeasureString(this.content, this.Font, 0, genericTypographic);
			base.BaseLine = CommonMethods.CalcAscentPixel(base.ParentExpression.Font);
		}
		public override string ToXml()
		{
			string text = string.Concat(new string[]
			{
				"<",
				base.Type.ToString(),
				" Color=\"",
				this.Color.ToArgb().ToString("x8"),
				"\">"
			});
			string text2 = text;
			return string.Concat(new string[]
			{
				text2,
				this.content,
				"</",
				base.Type.ToString(),
				">"
			});
		}
	}
}
