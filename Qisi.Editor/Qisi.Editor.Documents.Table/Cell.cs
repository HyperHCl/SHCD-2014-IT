using Qisi.Editor.Documents.Elements;
using System;
using System.Drawing;
namespace Qisi.Editor.Documents.Table
{
	internal class Cell : Document
	{
		public int hspan = 1;
		public int wspan = 1;
		public bool ismerged = false;
		private float _minheight;
		internal override float DocWidth
		{
			get
			{
				return base.DocWidth;
			}
			set
			{
				if (base.DocWidth != value)
				{
					base.DocWidth = value;
					TableInfo tableInfo = base.Parent as TableInfo;
					if (tableInfo != null)
					{
						float num = tableInfo.Location.Y + (float)tableInfo.Margin.Top + tableInfo.LineWidth;
						for (int i = 0; i < tableInfo.TableSize.X; i++)
						{
							float num2 = tableInfo.Location.X + (float)tableInfo.Margin.Left + tableInfo.LineWidth;
							for (int j = 0; j < tableInfo.TableSize.Y; j++)
							{
								if (tableInfo.Rows.Count > i && tableInfo.Rows[i].Cells.Count > j)
								{
									tableInfo.Rows[i].Cells[j].DocLocation = new PointF(num2, num);
									num2 += tableInfo.LineWidth + tableInfo.Columns[j].Width;
								}
							}
							num += tableInfo.Rows[i].Height + tableInfo.LineWidth;
						}
					}
				}
			}
		}
		internal override float DocHeight
		{
			get
			{
				return base.DocHeight;
			}
			set
			{
				if (base.DocHeight != value)
				{
					base.DocHeight = value;
					TableInfo tableInfo = base.Parent as TableInfo;
					if (tableInfo != null)
					{
						float num = tableInfo.Location.Y + (float)tableInfo.Margin.Top + tableInfo.LineWidth;
						for (int i = 0; i < tableInfo.TableSize.X; i++)
						{
							float num2 = tableInfo.Location.X + (float)tableInfo.Margin.Left + tableInfo.LineWidth;
							for (int j = 0; j < tableInfo.TableSize.Y; j++)
							{
								if (tableInfo.Rows.Count > i && tableInfo.Rows[i].Cells.Count > j)
								{
									tableInfo.Rows[i].Cells[j].DocLocation = new PointF(num2, num);
									num2 += tableInfo.LineWidth + tableInfo.Columns[j].Width;
								}
							}
							num += tableInfo.Rows[i].Height + tableInfo.LineWidth;
						}
					}
				}
			}
		}
		public float MinHeight
		{
			get
			{
				return this._minheight;
			}
			set
			{
				if (this._minheight != value)
				{
					this._minheight = value;
					float num = ((TableInfo)base.Parent).Location.Y + (float)((TableInfo)base.Parent).Margin.Top + ((TableInfo)base.Parent).LineWidth;
					for (int i = 0; i < ((TableInfo)base.Parent).TableSize.X; i++)
					{
						float num2 = ((TableInfo)base.Parent).Location.X + (float)((TableInfo)base.Parent).Margin.Left + ((TableInfo)base.Parent).LineWidth;
						for (int j = 0; j < ((TableInfo)base.Parent).TableSize.Y; j++)
						{
							if (((TableInfo)base.Parent).Rows.Count > i && ((TableInfo)base.Parent).Rows[i].Cells.Count > j)
							{
								((TableInfo)base.Parent).Rows[i].Cells[j].DocLocation = new PointF(num2, num);
								num2 += ((TableInfo)base.Parent).LineWidth + ((TableInfo)base.Parent).Columns[j].Width;
							}
						}
						num += ((TableInfo)base.Parent).Rows[i].Height + ((TableInfo)base.Parent).LineWidth;
					}
				}
			}
		}
		internal override float OutHeight
		{
			get
			{
				return Math.Max(this.MinHeight, this.DocHeight + (float)base.Margin.Vertical);
			}
		}
		public Cell(Font F, TableInfo con, float docwidth, PointF Location, Color backColor) : base(con.Margin, F, con, docwidth, Location, backColor)
		{
		}
	}
}
