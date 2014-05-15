using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Qisi.General
{
	public class MemoryIniFile : IDisposable
	{
		private List<IniSection> List;
		private bool SectionExists(string SectionName)
		{
			bool result;
			foreach (IniSection current in this.List)
			{
				if (current.SectionName.ToLower() == SectionName.ToLower())
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public IniSection FindSection(string SectionName)
		{
			IniSection result;
			foreach (IniSection current in this.List)
			{
				if (current.SectionName.ToLower() == SectionName.ToLower())
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}
		public MemoryIniFile()
		{
			this.List = new List<IniSection>();
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}
		~MemoryIniFile()
		{
			this.Dispose(false);
		}
		public void LoadFromStream(StreamReader SR)
		{
			this.List.Clear();
			IniSection iniSection = null;
			while (true)
			{
				string text = SR.ReadLine();
				if (text == null)
				{
					break;
				}
				text = text.Trim();
				if (!(text == ""))
				{
					if (text != "" && text[0] == '[' && text[text.Length - 1] == ']')
					{
						text = text.Remove(0, 1);
						text = text.Remove(text.Length - 1, 1);
						iniSection = this.FindSection(text);
						if (iniSection == null)
						{
							iniSection = new IniSection(text);
							this.List.Add(iniSection);
						}
					}
					else
					{
						if (iniSection == null)
						{
							iniSection = this.FindSection("UnDefSection");
							if (iniSection == null)
							{
								iniSection = new IniSection("UnDefSection");
								this.List.Add(iniSection);
							}
						}
						int num = text.IndexOf('=');
						if (num != 0)
						{
							string key = text.Substring(0, num);
							string value = text.Substring(num + 1, text.Length - num - 1);
							iniSection.AddKeyValue(key, value);
						}
						else
						{
							iniSection.AddKeyValue(text, "");
						}
					}
				}
			}
			SR.Dispose();
		}
		public string SaveToString()
		{
			string text = "";
			foreach (IniSection current in this.List)
			{
				text += current.SaveToString();
			}
			return text;
		}
		public string ReadValue(string SectionName, string key, string defaultv)
		{
			IniSection iniSection = this.FindSection(SectionName);
			string result;
			if (iniSection != null)
			{
				result = iniSection.ReadValue(key, defaultv);
			}
			else
			{
				result = defaultv;
			}
			return result;
		}
		public bool ReadValue(string SectionName, string key, bool defaultv)
		{
			IniSection iniSection = this.FindSection(SectionName);
			bool result;
			if (iniSection != null)
			{
				result = iniSection.ReadValue(key, defaultv);
			}
			else
			{
				result = defaultv;
			}
			return result;
		}
		public int ReadValue(string SectionName, string key, int defaultv)
		{
			IniSection iniSection = this.FindSection(SectionName);
			int result;
			if (iniSection != null)
			{
				result = iniSection.ReadValue(key, defaultv);
			}
			else
			{
				result = defaultv;
			}
			return result;
		}
		public float ReadValue(string SectionName, string key, float defaultv)
		{
			IniSection iniSection = this.FindSection(SectionName);
			float result;
			if (iniSection != null)
			{
				result = iniSection.ReadValue(key, defaultv);
			}
			else
			{
				result = defaultv;
			}
			return result;
		}
		public DateTime ReadValue(string SectionName, string key, DateTime defaultv)
		{
			IniSection iniSection = this.FindSection(SectionName);
			DateTime result;
			if (iniSection != null)
			{
				result = iniSection.ReadValue(key, defaultv);
			}
			else
			{
				result = defaultv;
			}
			return result;
		}
		public IniSection WriteValue(string SectionName, string key, string value)
		{
			IniSection iniSection = this.FindSection(SectionName);
			if (iniSection == null)
			{
				iniSection = new IniSection(SectionName);
				this.List.Add(iniSection);
			}
			iniSection.WriteValue(key, value);
			return iniSection;
		}
		public IniSection WriteValue(string SectionName, string key, bool value)
		{
			IniSection iniSection = this.FindSection(SectionName);
			if (iniSection == null)
			{
				iniSection = new IniSection(SectionName);
				this.List.Add(iniSection);
			}
			iniSection.WriteValue(key, value);
			return iniSection;
		}
		public IniSection WriteValue(string SectionName, string key, int value)
		{
			IniSection iniSection = this.FindSection(SectionName);
			if (iniSection == null)
			{
				iniSection = new IniSection(SectionName);
				this.List.Add(iniSection);
			}
			iniSection.WriteValue(key, value);
			return iniSection;
		}
		public IniSection WriteValue(string SectionName, string key, float value)
		{
			IniSection iniSection = this.FindSection(SectionName);
			if (iniSection == null)
			{
				iniSection = new IniSection(SectionName);
				this.List.Add(iniSection);
			}
			iniSection.WriteValue(key, value);
			return iniSection;
		}
		public IniSection WriteValue(string SectionName, string key, DateTime value)
		{
			IniSection iniSection = this.FindSection(SectionName);
			if (iniSection == null)
			{
				iniSection = new IniSection(SectionName);
				this.List.Add(iniSection);
			}
			iniSection.WriteValue(key, value);
			return iniSection;
		}
		public void LoadFromFile(string FileName, Encoding encoding)
		{
			FileStream fileStream = new FileStream(Path.GetFullPath(FileName), FileMode.Open);
			StreamReader sR = new StreamReader(fileStream, encoding);
			this.LoadFromStream(sR);
			fileStream.Close();
			fileStream.Dispose();
		}
		public void LoadFromFile(string FileName)
		{
			FileStream fileStream = new FileStream(Path.GetFullPath(FileName), FileMode.Open);
			StreamReader sR = new StreamReader(fileStream, Encoding.Default);
			this.LoadFromStream(sR);
			fileStream.Close();
			fileStream.Dispose();
		}
		public void LoadFromByte(byte[] buffer, Encoding encoding)
		{
			MemoryStream memoryStream = new MemoryStream(buffer);
			StreamReader sR = new StreamReader(memoryStream, encoding);
			this.LoadFromStream(sR);
			memoryStream.Close();
			memoryStream.Dispose();
		}
		public void LoadFromString(string str, Encoding encoding)
		{
			byte[] bytes = encoding.GetBytes(str);
			this.LoadFromByte(bytes, encoding);
		}
		public void SaveToFile(string FileName)
		{
			string text = "";
			foreach (IniSection current in this.List)
			{
				text += current.SaveToString();
			}
			File.WriteAllText(text, FileName, Encoding.Default);
		}
		public void LoadFromEncodedFile(string FileName)
		{
			string data = File.ReadAllText(FileName);
			this.LoadFromStream(MemoryIniFile.Decrypt(data));
		}
		public void SaveToEncryptedFile(string FileName)
		{
			string text = this.SaveToString();
			text = MemoryIniFile.Encrypt(text);
			File.WriteAllText(FileName, text, Encoding.Default);
		}
		private static string Encrypt(string data)
		{
			byte[] bytes = Encoding.ASCII.GetBytes("KEYS1009");
			byte[] bytes2 = Encoding.ASCII.GetBytes("KEYS1009");
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			byte[] bytes3 = Encoding.UTF8.GetBytes(data);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write);
			cryptoStream.Write(bytes3, 0, bytes3.Length);
			cryptoStream.FlushFinalBlock();
			return Convert.ToBase64String(memoryStream.ToArray());
		}
		private static StreamReader Decrypt(string data)
		{
			byte[] bytes = Encoding.ASCII.GetBytes("KEYS1009");
			byte[] bytes2 = Encoding.ASCII.GetBytes("KEYS1009");
			byte[] buffer;
			StreamReader result;
			try
			{
				buffer = Convert.FromBase64String(data);
			}
			catch
			{
				result = null;
				return result;
			}
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream stream = new MemoryStream(buffer);
			CryptoStream stream2 = new CryptoStream(stream, dESCryptoServiceProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Read);
			StreamReader streamReader = new StreamReader(stream2);
			result = streamReader;
			return result;
		}
	}
}
