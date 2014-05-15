using System;
namespace Qisi.Editor.Documents
{
	internal class UnderLine : IDisposable
	{
		private Line line;
		private int start;
		private int end;
		internal Line Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}
		internal int StartX
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value;
			}
		}
		internal int EndX
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value;
			}
		}
		internal UnderLine(Line ln, float num1, float num2)
		{
			this.Line = ln;
			this.StartX = (int)num1;
			this.EndX = (int)num2;
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			this.line = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~UnderLine()
		{
			this.Dispose(false);
		}
	}
}
