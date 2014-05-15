using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class TextAndImageCell : DataGridViewTextBoxCell
	{
		private Color ColorValue = Color.Transparent;
		public Color Color
		{
			get
			{
				return this.ColorValue;
			}
			set
			{
				this.ColorValue = value;
			}
		}
		private DataGridViewAutoFilterTextAndPictureBoxColumn OwningTextAndImageColumn
		{
			get
			{
				return (DataGridViewAutoFilterTextAndPictureBoxColumn)base.OwningColumn;
			}
		}
		public TextAndImageCell()
		{
			base.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
		}
		public override object Clone()
		{
			TextAndImageCell textAndImageCell = (TextAndImageCell)base.Clone();
			textAndImageCell.ColorValue = this.ColorValue;
			return textAndImageCell;
		}
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, "", formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
			Bitmap bitmap = new Bitmap((int)((double)cellBounds.Height * 1.2), cellBounds.Height);
			Graphics graphics2 = Graphics.FromImage(bitmap);
			graphics2.FillRectangle(new SolidBrush(this.ColorValue), 4, 4, bitmap.Width - 8, bitmap.Height - 8);
			GraphicsContainer container = graphics.BeginContainer();
			graphics.SetClip(cellBounds);
			graphics.DrawImageUnscaled(bitmap, cellBounds.Location);
			SolidBrush brush = new SolidBrush(cellStyle.ForeColor);
			SizeF sizeF = graphics.MeasureString(value.ToString(), cellStyle.Font);
			graphics.DrawString(value.ToString(), cellStyle.Font, brush, (float)(bitmap.Width - 8), ((float)cellBounds.Height - sizeF.Height) / 2f);
			graphics.EndContainer(container);
		}
	}
}
