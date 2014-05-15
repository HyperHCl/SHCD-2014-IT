using System;
using System.Collections.Generic;
namespace Qisi.Editor.Documents.Table
{
	internal class Column : IDisposable
	{
		private List<Cell> cells;
		private float width;
		internal float Width
		{
			get
			{
				this.width = 0f;
				foreach (Cell current in this.Cells)
				{
					this.width = Math.Max(this.width, current.OutWidth);
				}
				return this.width;
			}
		}
		internal List<Cell> Cells
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
		internal Column()
		{
			this.cells = new List<Cell>();
			this.width = 0f;
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
		~Column()
		{
			this.Dispose(false);
		}
	}
}
