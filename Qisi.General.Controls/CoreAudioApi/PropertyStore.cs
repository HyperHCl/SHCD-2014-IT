using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class PropertyStore
	{
		private IPropertyStore _Store;
		public int Count
		{
			get
			{
				int result;
				Marshal.ThrowExceptionForHR(this._Store.GetCount(out result));
				return result;
			}
		}
		public PropertyStoreProperty this[int index]
		{
			get
			{
				PropertyKey key = this.Get(index);
				PropVariant value;
				Marshal.ThrowExceptionForHR(this._Store.GetValue(ref key, out value));
				return new PropertyStoreProperty(key, value);
			}
		}
		public PropertyStoreProperty this[Guid guid]
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					PropertyKey key = this.Get(i);
					if (key.fmtid == guid)
					{
						PropVariant value;
						Marshal.ThrowExceptionForHR(this._Store.GetValue(ref key, out value));
						return new PropertyStoreProperty(key, value);
					}
				}
				return null;
			}
		}
		public bool Contains(Guid guid)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.Get(i).fmtid == guid)
				{
					return true;
				}
			}
			return false;
		}
		public PropertyKey Get(int index)
		{
			PropertyKey result;
			Marshal.ThrowExceptionForHR(this._Store.GetAt(index, out result));
			return result;
		}
		public PropVariant GetValue(int index)
		{
			PropertyKey propertyKey = this.Get(index);
			PropVariant result;
			Marshal.ThrowExceptionForHR(this._Store.GetValue(ref propertyKey, out result));
			return result;
		}
		internal PropertyStore(IPropertyStore store)
		{
			this._Store = store;
		}
	}
}
