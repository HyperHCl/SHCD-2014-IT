using System;
using System.IO;
namespace Qisi.Editor.Documents.Elements
{
	internal class MemoryFile
	{
		internal string FileName
		{
			get;
			set;
		}
		internal byte[] FileByte
		{
			get;
			set;
		}
		internal MemoryFile(string filepath)
		{
			FileStream fileStream = File.OpenRead(filepath);
			int num = (int)fileStream.Length;
			this.FileByte = new byte[num];
			fileStream.Read(this.FileByte, 0, num);
			fileStream.Close();
			this.FileName = Path.GetFileName(filepath);
		}
		internal MemoryFile(string filename, byte[] filebytes)
		{
			this.FileName = filename;
			this.FileByte = filebytes;
		}
		internal void ToFile(string dirPath)
		{
			if (this.FileByte != null && this.FileName != null)
			{
				try
				{
					File.WriteAllBytes(Path.Combine(dirPath, this.FileName), this.FileByte);
				}
				catch
				{
				}
			}
		}
	}
}
