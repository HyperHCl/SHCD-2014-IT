using System;
using System.Drawing;
namespace Qisi.Editor.Documents.Elements
{
	internal class CharInfo : Element
	{
		public char chr;
		internal char Char
		{
			get
			{
				return this.chr;
			}
		}
		internal Color Color
		{
			get;
			set;
		}
		internal static bool IsPunctuationLegalTOL(char c)
		{
			return !"!),.:;?]}¨·ˇˉ―‖’”…∶、。〃々〉》」』】〕〗！＂＇），．：；？］｀｜｝～￠".Contains(c.ToString());
		}
		internal static bool IsPunctuationLegalEOL(char c)
		{
			return !"([{·‘“〈《「『【〔〖（．［｛￡￥".Contains(c.ToString());
		}
		internal static SizeF CalCharSize(Graphics g, CharInfo charInfo)
		{
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			SizeF result = g.MeasureString(charInfo.chr.ToString(), charInfo.Font, 0, genericTypographic);
			genericTypographic.Dispose();
			return result;
		}
		internal override void Draw(Graphics g)
		{
			if (base.Font != null)
			{
				base.Draw(g);
				StringFormat genericTypographic = StringFormat.GenericTypographic;
				genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
				SolidBrush solidBrush = new SolidBrush(this.Color);
				g.DrawString(this.chr.ToString(), base.Font, solidBrush, this.Location, genericTypographic);
				solidBrush.Dispose();
				genericTypographic.Dispose();
			}
		}
		internal override void DrawHighLight(Graphics g)
		{
			if (base.Font != null)
			{
				base.Draw(g);
				StringFormat genericTypographic = StringFormat.GenericTypographic;
				genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
				g.DrawString(this.chr.ToString(), base.Font, SystemBrushes.HighlightText, this.Location, genericTypographic);
				genericTypographic.Dispose();
			}
		}
		internal CharInfo(char ch, Font font) : base(font)
		{
			this.chr = ch;
			this.Color = Color.Black;
			FontFamily fontFamily = font.FontFamily;
			int cellAscent = fontFamily.GetCellAscent(font.Style);
			this.BaseLine = font.Size * (float)cellAscent / (float)fontFamily.GetEmHeight(font.Style);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}
		~CharInfo()
		{
			this.Dispose(false);
		}
	}
}
