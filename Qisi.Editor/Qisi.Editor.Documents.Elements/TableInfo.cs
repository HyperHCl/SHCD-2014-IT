using Qisi.Editor.Documents.Table;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Documents.Elements
{
	internal class TableInfo : Pic_Tab
	{
		private List<Cell> items;
		private List<Row> rows;
		private List<Column> columns;
		private float width;
		private float height;
		internal Point TableSize
		{
			get;
			set;
		}
		internal List<Cell> Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
			}
		}
		internal List<Row> Rows
		{
			get
			{
				return this.rows;
			}
			set
			{
				this.rows = value;
			}
		}
		internal List<Column> Columns
		{
			get
			{
				return this.columns;
			}
			set
			{
				this.columns = value;
			}
		}
		internal float LineWidth
		{
			get;
			set;
		}
		internal override SizeF Size
		{
			get
			{
				return new SizeF(this.Width, this.Height);
			}
		}
		internal override PointF Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				if (base.Location != value)
				{
					base.Location = value;
					float num = base.Location.Y + (float)base.Margin.Top + this.LineWidth;
					for (int i = 0; i < this.TableSize.X; i++)
					{
						float num2 = base.Location.X + (float)base.Margin.Left + this.LineWidth;
						for (int j = 0; j < this.TableSize.Y; j++)
						{
							this.rows[i].Cells[j].DocLocation = new PointF(num2, num);
							num2 += this.LineWidth + this.columns[j].Width;
						}
						num += this.rows[i].Height + this.LineWidth;
					}
				}
			}
		}
		internal float Width
		{
			get
			{
				this.width = 0f;
				foreach (Column current in this.columns)
				{
					this.width += current.Width + this.LineWidth;
				}
				this.width += (float)base.Margin.Horizontal + this.LineWidth;
				return this.width;
			}
		}
		internal float Height
		{
			get
			{
				this.height = 0f;
				foreach (Row current in this.Rows)
				{
					this.height += current.Height + this.LineWidth;
				}
				this.height += (float)base.Margin.Vertical + this.LineWidth;
				return this.height;
			}
		}
		internal override bool Sized
		{
			get
			{
				bool result;
				foreach (Cell current in this.items)
				{
					foreach (Element current2 in current.Elements)
					{
						if (!current2.Sized)
						{
							result = false;
							return result;
						}
					}
				}
				result = true;
				return result;
			}
		}
		internal TableInfo(Point tableSize, float totalWidth, Font font, float lineWidth = 1f) : base(font)
		{
			this.TableSize = tableSize;
			this.LineWidth = lineWidth;
			float num = (float)(font.Height * tableSize.Y) + ((float)base.Margin.Vertical + this.LineWidth) * (float)(tableSize.Y + 1);
			this.width = 0f;
			this.height = 0f;
			this.items = new List<Cell>();
			this.rows = new List<Row>();
			this.columns = new List<Column>();
			for (int i = 0; i < tableSize.X; i++)
			{
				this.rows.Add(new Row());
				this.rows[i].Cells = new List<Cell>();
			}
			for (int j = 0; j < tableSize.Y; j++)
			{
				this.columns.Add(new Column());
				this.columns[j].Cells = new List<Cell>();
			}
			float num2 = this.Location.Y + (float)base.Margin.Top + this.LineWidth;
			for (int i = 0; i < tableSize.X; i++)
			{
				float num3 = this.Location.X + (float)base.Margin.Left + this.LineWidth;
				for (int j = 0; j < tableSize.Y; j++)
				{
					Cell item = new Cell(font, this, totalWidth / (float)tableSize.Y, new PointF(num3, num2), Color.Transparent);
					this.rows[i].Cells.Add(item);
					this.columns[j].Cells.Add(item);
					this.items.Add(item);
					num3 += this.LineWidth + this.columns[j].Width;
				}
				num2 += this.Rows[i].Height + this.LineWidth;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.items != null)
				{
					foreach (Cell current in this.items)
					{
						current.Dispose();
					}
				}
				if (this.rows != null)
				{
					foreach (Row current2 in this.rows)
					{
						current2.Dispose();
					}
				}
				if (this.columns != null)
				{
					foreach (Column current3 in this.columns)
					{
						current3.Dispose();
					}
				}
			}
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					this.items[i] = null;
				}
			}
			if (this.rows != null)
			{
				for (int i = 0; i < this.rows.Count; i++)
				{
					this.rows[i] = null;
				}
			}
			if (this.columns != null)
			{
				for (int i = 0; i < this.columns.Count; i++)
				{
					this.columns[i] = null;
				}
			}
			this.items = null;
			this.rows = null;
			this.columns = null;
			base.Dispose(disposing);
		}
		~TableInfo()
		{
			this.Dispose(false);
		}
		public void LayOut(Graphics g)
		{
			foreach (Cell current in this.items)
			{
				current.PrepareToDraw(g);
			}
		}
		internal Cell GetItem(int rowID, int columnID)
		{
			Cell result;
			if (rowID < 0 || columnID < 0)
			{
				result = null;
			}
			else
			{
				if (this.rows == null || this.columns == null || this.items == null)
				{
					result = null;
				}
				else
				{
					Cell cell = this.rows[rowID].Cells[columnID];
					if (cell.ismerged)
					{
						if (rowID - 1 >= 0 && this.rows[rowID - 1].Cells[columnID].hspan > 1)
						{
							result = this.GetItem(rowID - 1, columnID);
						}
						else
						{
							if (columnID - 1 >= 0 && this.rows[rowID].Cells[columnID - 1].wspan > 1)
							{
								result = this.GetItem(rowID, columnID - 1);
							}
							else
							{
								result = null;
							}
						}
					}
					else
					{
						result = cell;
					}
				}
			}
			return result;
		}
		internal Point GetRowColumn(Cell cell)
		{
			Point result;
			if (this.rows == null || this.columns == null || this.items == null)
			{
				result = new Point(-1, -1);
			}
			else
			{
				int num = this.Items.IndexOf(cell);
				if (num < 0)
				{
					result = new Point(-1, -1);
				}
				else
				{
					result = new Point(num / this.TableSize.Y, num % this.TableSize.Y);
				}
			}
			return result;
		}
		internal override void Draw(Graphics g)
		{
			if (this.items != null && this.rows != null && this.columns != null)
			{
				base.Draw(g);
				float num = this.Location.Y + (float)base.Margin.Top;
				float num2 = num;
				float num3 = this.Location.X + (float)base.Margin.Left;
				float num4 = num3;
				float num5 = this.Size.Height - (float)base.Margin.Vertical;
				float num6 = this.Size.Width - (float)base.Margin.Horizontal;
				SolidBrush solidBrush = new SolidBrush(Color.Black);
				for (int i = 0; i < this.TableSize.X; i++)
				{
					g.FillRectangle(solidBrush, num3, num2, num6, this.LineWidth);
					num2 += this.rows[i].Height + this.LineWidth;
				}
				g.FillRectangle(solidBrush, num3, num2, num6, this.LineWidth);
				for (int j = 0; j < this.TableSize.Y; j++)
				{
					g.FillRectangle(solidBrush, num4, num, this.LineWidth, num5);
					num4 += this.columns[j].Width + this.LineWidth;
				}
				g.FillRectangle(solidBrush, num4, num, this.LineWidth, num5);
				foreach (Cell current in this.items)
				{
					current.Draw(g);
				}
				solidBrush.Dispose();
			}
		}
		internal override void DrawHighLight(Graphics g)
		{
			if (this.items != null && this.rows != null && this.columns != null)
			{
				base.Draw(g);
				float num = this.Location.Y + (float)base.Margin.Top;
				float num2 = num;
				float num3 = this.Location.X + (float)base.Margin.Left;
				float num4 = num3;
				float num5 = this.Size.Height - (float)base.Margin.Vertical;
				float num6 = this.Size.Width - (float)base.Margin.Horizontal;
				SolidBrush solidBrush = new SolidBrush(Color.Black);
				for (int i = 0; i < this.TableSize.X; i++)
				{
					g.FillRectangle(solidBrush, num3, num2, num6, this.LineWidth);
					num2 += this.rows[i].Height + this.LineWidth;
				}
				g.FillRectangle(solidBrush, num3, num2, num6, this.LineWidth);
				for (int j = 0; j < this.TableSize.Y; j++)
				{
					g.FillRectangle(solidBrush, num4, num, this.LineWidth, num5);
					num4 += this.columns[j].Width + this.LineWidth;
				}
				g.FillRectangle(solidBrush, num4, num, this.LineWidth, num5);
				foreach (Cell current in this.items)
				{
					current.DrawHighLight(g);
				}
				solidBrush.Dispose();
			}
		}
	}
}
