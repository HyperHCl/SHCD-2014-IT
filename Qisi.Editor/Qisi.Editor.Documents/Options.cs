using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Documents
{
	internal class Options : IDisposable
	{
		private List<Option> optionList;
		private int startIndex;
		private int count;
		private Region region;
		internal List<Option> OptionList
		{
			get
			{
				return this.optionList;
			}
			set
			{
				this.optionList = value;
			}
		}
		internal bool Multiple
		{
			get;
			set;
		}
		internal bool Randomized
		{
			get;
			set;
		}
		internal bool Handled
		{
			get;
			set;
		}
		internal int StartIndex
		{
			get
			{
				this.startIndex = 2147483647;
				foreach (Option current in this.OptionList)
				{
					this.startIndex = Math.Min(current.StartIndex, this.startIndex);
				}
				return this.startIndex;
			}
		}
		internal int Count
		{
			get
			{
				this.count = 0;
				foreach (Option current in this.OptionList)
				{
					this.count += current.Count;
				}
				return this.count;
			}
		}
		internal Region Region
		{
			get
			{
				this.region = new Region();
				this.region.MakeEmpty();
				foreach (Option current in this.OptionList)
				{
					this.region.Union(current.Region);
				}
				return this.region;
			}
		}
		internal string RandOrder
		{
			get;
			set;
		}
		public Options(bool multiple, bool randomized)
		{
			this.Multiple = multiple;
			this.Randomized = randomized;
			this.optionList = new List<Option>();
			this.startIndex = -1;
			this.count = 0;
			this.region = new Region();
			this.Handled = false;
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.optionList != null)
				{
					foreach (Option current in this.optionList)
					{
						current.Dispose();
					}
				}
				if (this.region != null)
				{
					this.region.Dispose();
				}
			}
			this.region = null;
			if (this.optionList != null)
			{
				for (int i = 0; i < this.optionList.Count; i++)
				{
					this.optionList[i] = null;
				}
			}
			this.optionList = null;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		~Options()
		{
			this.Dispose(false);
		}
		internal virtual void Draw(Graphics g)
		{
			for (int i = 0; i < this.OptionList.Count; i++)
			{
				this.OptionList[i].Draw(g, ((char)(65 + i)).ToString());
			}
		}
	}
}
