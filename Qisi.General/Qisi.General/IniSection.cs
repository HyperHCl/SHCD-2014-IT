using System;
using System.Collections.Generic;
namespace Qisi.General
{
	public class IniSection
	{
		private Dictionary<string, string> FDictionary;
		private string FSectionName;
		public string SectionName
		{
			get
			{
				return this.FSectionName;
			}
		}
		public int Count
		{
			get
			{
				return this.FDictionary.Count;
			}
		}
		public IniSection(string SName)
		{
			this.FSectionName = SName;
			this.FDictionary = new Dictionary<string, string>();
		}
		public void Clear()
		{
			this.FDictionary.Clear();
		}
		public void AddKeyValue(string key, string value)
		{
			if (this.FDictionary.ContainsKey(key))
			{
				this.FDictionary[key] = value;
			}
			else
			{
				this.FDictionary.Add(key, value);
			}
		}
		public void WriteValue(string key, string value)
		{
			this.AddKeyValue(key, value);
		}
		public void WriteValue(string key, bool value)
		{
			this.AddKeyValue(key, Convert.ToString(value));
		}
		public void WriteValue(string key, int value)
		{
			this.AddKeyValue(key, Convert.ToString(value));
		}
		public void WriteValue(string key, float value)
		{
			this.AddKeyValue(key, Convert.ToString(value));
		}
		public void WriteValue(string key, DateTime value)
		{
			this.AddKeyValue(key, Convert.ToString(value));
		}
		public string ReadValue(string key, string defaultv)
		{
			string result;
			if (this.FDictionary.ContainsKey(key))
			{
				result = this.FDictionary[key];
			}
			else
			{
				result = defaultv;
			}
			return result;
		}
		public bool ReadValue(string key, bool defaultv)
		{
			string value = this.ReadValue(key, Convert.ToString(defaultv));
			return Convert.ToBoolean(value);
		}
		public int ReadValue(string key, int defaultv)
		{
			string value = this.ReadValue(key, Convert.ToString(defaultv));
			return Convert.ToInt32(value);
		}
		public float ReadValue(string key, float defaultv)
		{
			string value = this.ReadValue(key, Convert.ToString(defaultv));
			return Convert.ToSingle(value);
		}
		public DateTime ReadValue(string key, DateTime defaultv)
		{
			string value = this.ReadValue(key, Convert.ToString(defaultv));
			return Convert.ToDateTime(value);
		}
		public string SaveToString()
		{
			string text = "";
			text = text + "[" + this.FSectionName + "]\r\n";
			foreach (KeyValuePair<string, string> current in this.FDictionary)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					current.Key,
					"=",
					current.Value,
					"\r\n"
				});
			}
			text += "\r\n";
			return text;
		}
	}
}
