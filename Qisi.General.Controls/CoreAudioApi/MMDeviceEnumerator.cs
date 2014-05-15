using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class MMDeviceEnumerator
	{
		private IMMDeviceEnumerator _realEnumerator = new _MMDeviceEnumerator() as IMMDeviceEnumerator;
		public MMDeviceCollection EnumerateAudioEndPoints(EDataFlow dataFlow, EDeviceState dwStateMask)
		{
			IMMDeviceCollection parent;
			Marshal.ThrowExceptionForHR(this._realEnumerator.EnumAudioEndpoints(dataFlow, dwStateMask, out parent));
			return new MMDeviceCollection(parent);
		}
		public MMDevice GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role)
		{
			IMMDevice realDevice = null;
			Marshal.ThrowExceptionForHR(this._realEnumerator.GetDefaultAudioEndpoint(dataFlow, role, out realDevice));
			return new MMDevice(realDevice);
		}
		public MMDevice GetDevice(string ID)
		{
			IMMDevice realDevice = null;
			Marshal.ThrowExceptionForHR(this._realEnumerator.GetDevice(ID, out realDevice));
			return new MMDevice(realDevice);
		}
		public MMDeviceEnumerator()
		{
			if (Environment.OSVersion.Version.Major < 6)
			{
				throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
			}
		}
	}
}
