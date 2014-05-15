using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class MMDevice
	{
		private IMMDevice _RealDevice;
		private PropertyStore _PropertyStore;
		private AudioMeterInformation _AudioMeterInformation;
		private AudioEndpointVolume _AudioEndpointVolume;
		private AudioSessionManager _AudioSessionManager;
		private static Guid IID_IAudioMeterInformation = typeof(IAudioMeterInformation).GUID;
		private static Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
		private static Guid IID_IAudioSessionManager = typeof(IAudioSessionManager2).GUID;
		public AudioSessionManager AudioSessionManager
		{
			get
			{
				if (this._AudioSessionManager == null)
				{
					this.GetAudioSessionManager();
				}
				return this._AudioSessionManager;
			}
		}
		public AudioMeterInformation AudioMeterInformation
		{
			get
			{
				if (this._AudioMeterInformation == null)
				{
					this.GetAudioMeterInformation();
				}
				return this._AudioMeterInformation;
			}
		}
		public AudioEndpointVolume AudioEndpointVolume
		{
			get
			{
				if (this._AudioEndpointVolume == null)
				{
					this.GetAudioEndpointVolume();
				}
				return this._AudioEndpointVolume;
			}
		}
		public PropertyStore Properties
		{
			get
			{
				if (this._PropertyStore == null)
				{
					this.GetPropertyInformation();
				}
				return this._PropertyStore;
			}
		}
		public string FriendlyName
		{
			get
			{
				if (this._PropertyStore == null)
				{
					this.GetPropertyInformation();
				}
				if (this._PropertyStore.Contains(PKEY.PKEY_DeviceInterface_FriendlyName))
				{
					return (string)this._PropertyStore[PKEY.PKEY_DeviceInterface_FriendlyName].Value;
				}
				return "Unknown";
			}
		}
		public string ID
		{
			get
			{
				string result;
				Marshal.ThrowExceptionForHR(this._RealDevice.GetId(out result));
				return result;
			}
		}
		public EDataFlow DataFlow
		{
			get
			{
				IMMEndpoint iMMEndpoint = this._RealDevice as IMMEndpoint;
				EDataFlow result;
				iMMEndpoint.GetDataFlow(out result);
				return result;
			}
		}
		public EDeviceState State
		{
			get
			{
				EDeviceState result;
				Marshal.ThrowExceptionForHR(this._RealDevice.GetState(out result));
				return result;
			}
		}
		private void GetPropertyInformation()
		{
			IPropertyStore store;
			Marshal.ThrowExceptionForHR(this._RealDevice.OpenPropertyStore(EStgmAccess.STGM_READ, out store));
			this._PropertyStore = new PropertyStore(store);
		}
		private void GetAudioSessionManager()
		{
			object obj;
			Marshal.ThrowExceptionForHR(this._RealDevice.Activate(ref MMDevice.IID_IAudioSessionManager, CLSCTX.ALL, IntPtr.Zero, out obj));
			this._AudioSessionManager = new AudioSessionManager(obj as IAudioSessionManager2);
		}
		private void GetAudioMeterInformation()
		{
			object obj;
			Marshal.ThrowExceptionForHR(this._RealDevice.Activate(ref MMDevice.IID_IAudioMeterInformation, CLSCTX.ALL, IntPtr.Zero, out obj));
			this._AudioMeterInformation = new AudioMeterInformation(obj as IAudioMeterInformation);
		}
		private void GetAudioEndpointVolume()
		{
			object obj;
			Marshal.ThrowExceptionForHR(this._RealDevice.Activate(ref MMDevice.IID_IAudioEndpointVolume, CLSCTX.ALL, IntPtr.Zero, out obj));
			this._AudioEndpointVolume = new AudioEndpointVolume(obj as IAudioEndpointVolume);
		}
		internal MMDevice(IMMDevice realDevice)
		{
			this._RealDevice = realDevice;
		}
	}
}
