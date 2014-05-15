using System;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class DataGridViewAutoFilterTextAndPictureBoxColumn : DataGridViewAutoFilterTextBoxColumn
	{
		private TextAndImageCell TextAndImageCellTemplate
		{
			get
			{
				return (TextAndImageCell)this.CellTemplate;
			}
		}
		public DataGridViewAutoFilterTextAndPictureBoxColumn()
		{
			this.CellTemplate = new TextAndImageCell();
			this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
		}
		public override object Clone()
		{
			return (DataGridViewAutoFilterTextAndPictureBoxColumn)base.Clone();
		}
	}
}
