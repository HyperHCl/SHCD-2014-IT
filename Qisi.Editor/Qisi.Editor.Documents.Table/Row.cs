using System;
using System.Collections.Generic;
namespace Qisi.Editor.Documents.Table
{
	internal class Row
	{
		private List<Cell> cells;
		private float height;
		public float Height
		{
			get
			{
				this.height = 0f;
				foreach (Cell current in this.Cells)
				{
					this.height = Math.Max(this.height, current.OutHeight);
				}
				return this.height;
			}
		}
		public List<Cell> Cells
		{
			get
			{
				return this.cells;
			}
			set
			{
				this.cells = value;
			}
		}
		public Row()
		{
			this.cells = new List<Cell>();
			this.height = 0f;
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			this.cells = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Row()
		{
			this.Dispose(false);
		}
	}
}
