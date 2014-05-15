using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioSessionControl
	{
		internal IAudioSessionControl2 _AudioSessionControl;
		internal AudioMeterInformation _AudioMeterInformation;
		internal SimpleAudioVolume _SimpleAudioVolume;
		public AudioMeterInformation AudioMeterInformation
		{
			get
			{
				return this._AudioMeterInformation;
			}
		}
		public SimpleAudioVolume SimpleAudioVolume
		{
			get
			{
				return this._SimpleAudioVolume;
			}
		}
		public AudioSessionState State
		{
			get
			{
				AudioSessionState result;
				Marshal.ThrowExceptionForHR(this._AudioSessionControl.GetState(out result));
				return result;
			}
		}
		public string DisplayName
		{
			get
			{
				IntPtr ptr;
				Marshal.ThrowExceptionForHR(this._AudioSessionControl.GetDisplayName(out ptr));
				string result = Marshal.PtrToStringAuto(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}
		public string IconPath
		{
			get
			{
				IntPtr ptr;
				Marshal.ThrowExceptionForHR(this._AudioSessionControl.GetIconPath(out ptr));
				string result = Marshal.PtrToStringAuto(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}
		public string SessionIdentifier
		{
			get
			{
				IntPtr ptr;
				Marshal.ThrowExceptionForHR(this._AudioSessionControl.GetSessionIdentifier(out ptr));
				string result = Marshal.PtrToStringAuto(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}
		public string SessionInstanceIdentifier
		{
			get
			{
				IntPtr ptr;
				Marshal.ThrowExceptionForHR(this._AudioSessionControl.GetSessionInstanceIdentifier(out ptr));
				string result = Marshal.PtrToStringAuto(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
		}
		public uint ProcessID
		{
			get
			{
				uint result;
				Marshal.ThrowExceptionForHR(this._AudioSessionControl.GetProcessId(out result));
				return result;
			}
		}
		public bool IsSystemIsSystemSoundsSession
		{
			get
			{
				return this._AudioSessionControl.IsSystemSoundsSession() == 0;
			}
		}
		internal AudioSessionControl(IAudioSessionControl2 realAudioSessionControl)
		{
			IAudioMeterInformation audioMeterInformation = realAudioSessionControl as IAudioMeterInformation;
			ISimpleAudioVolume simpleAudioVolume = realAudioSessionControl as ISimpleAudioVolume;
			if (audioMeterInformation != null)
			{
				this._AudioMeterInformation = new AudioMeterInformation(audioMeterInformation);
			}
			if (simpleAudioVolume != null)
			{
				this._SimpleAudioVolume = new SimpleAudioVolume(simpleAudioVolume);
			}
			this._AudioSessionControl = realAudioSessionControl;
		}
		public void RegisterAudioSessionNotification(IAudioSessionEvents eventConsumer)
		{
			Marshal.ThrowExceptionForHR(this._AudioSessionControl.RegisterAudioSessionNotification(eventConsumer));
		}
		public void UnregisterAudioSessionNotification(IAudioSessionEvents eventConsumer)
		{
			Marshal.ThrowExceptionForHR(this._AudioSessionControl.UnregisterAudioSessionNotification(eventConsumer));
		}
	}
}
