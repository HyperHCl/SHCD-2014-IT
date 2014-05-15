using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class MMDeviceCollection
	{
		private IMMDeviceCollection _MMDeviceCollection;
		public int Count
		{
			get
			{
				uint result;
				Marshal.ThrowExceptionForHR(this._MMDeviceCollection.GetCount(out result));
				return (int)result;
			}
		}
		public MMDevice this[int index]
		{
			get
			{
				IMMDevice realDevice;
				this._MMDeviceCollection.Item((uint)index, out realDevice);
				return new MMDevice(realDevice);
			}
		}
		internal MMDeviceCollection(IMMDeviceCollection parent)
		{
			this._MMDeviceCollection = parent;
		}
	}
}
