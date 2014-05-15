using System;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal class specialchar : charexpression
	{
		private string specialcontent;
		private Font _Font;
		public specialchar(string str, FType f, string c2, lineexpression parent, Color color) : base(str, parent, color)
		{
			this.specialcontent = c2;
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			this._Font = CommonMethods.GetCambriaFont(12f, FontStyle.Regular);
			base.Type = f;
		}
		public override string ToString()
		{
			return this.specialcontent;
		}
	}
}
