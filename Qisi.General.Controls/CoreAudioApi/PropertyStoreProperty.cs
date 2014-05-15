using System;
namespace CoreAudioApi
{
	public class PropertyStoreProperty
	{
		private PropertyKey _PropertyKey;
		private PropVariant _PropValue;
		public PropertyKey Key
		{
			get
			{
				return this._PropertyKey;
			}
		}
		public object Value
		{
			get
			{
				return this._PropValue.Value;
			}
		}
		internal PropertyStoreProperty(PropertyKey key, PropVariant value)
		{
			this._PropertyKey = key;
			this._PropValue = value;
		}
	}
}
