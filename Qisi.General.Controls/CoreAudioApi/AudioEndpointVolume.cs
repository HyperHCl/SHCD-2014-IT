using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioEndpointVolume : IDisposable
	{
		private IAudioEndpointVolume _AudioEndPointVolume;
		private AudioEndpointVolumeChannels _Channels;
		private AudioEndpointVolumeStepInformation _StepInformation;
		private AudioEndPointVolumeVolumeRange _VolumeRange;
		private EEndpointHardwareSupport _HardwareSupport;
		private AudioEndpointVolumeCallback _CallBack;
		public event AudioEndpointVolumeNotificationDelegate OnVolumeNotification;
		public AudioEndPointVolumeVolumeRange VolumeRange
		{
			get
			{
				return this._VolumeRange;
			}
		}
		public EEndpointHardwareSupport HardwareSupport
		{
			get
			{
				return this._HardwareSupport;
			}
		}
		public AudioEndpointVolumeStepInformation StepInformation
		{
			get
			{
				return this._StepInformation;
			}
		}
		public AudioEndpointVolumeChannels Channels
		{
			get
			{
				return this._Channels;
			}
		}
		public float MasterVolumeLevel
		{
			get
			{
				float result;
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.GetMasterVolumeLevel(out result));
				return result;
			}
			set
			{
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.SetMasterVolumeLevel(value, Guid.Empty));
			}
		}
		public float MasterVolumeLevelScalar
		{
			get
			{
				float result;
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.GetMasterVolumeLevelScalar(out result));
				return result;
			}
			set
			{
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.SetMasterVolumeLevelScalar(value, Guid.Empty));
			}
		}
		public bool Mute
		{
			get
			{
				bool result;
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.GetMute(out result));
				return result;
			}
			set
			{
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.SetMute(value, Guid.Empty));
			}
		}
		public void VolumeStepUp()
		{
			Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.VolumeStepUp(Guid.Empty));
		}
		public void VolumeStepDown()
		{
			Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.VolumeStepDown(Guid.Empty));
		}
		internal AudioEndpointVolume(IAudioEndpointVolume realEndpointVolume)
		{
			this._AudioEndPointVolume = realEndpointVolume;
			this._Channels = new AudioEndpointVolumeChannels(this._AudioEndPointVolume);
			this._StepInformation = new AudioEndpointVolumeStepInformation(this._AudioEndPointVolume);
			uint hardwareSupport;
			Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.QueryHardwareSupport(out hardwareSupport));
			this._HardwareSupport = (EEndpointHardwareSupport)hardwareSupport;
			this._VolumeRange = new AudioEndPointVolumeVolumeRange(this._AudioEndPointVolume);
			this._CallBack = new AudioEndpointVolumeCallback(this);
			Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.RegisterControlChangeNotify(this._CallBack));
		}
		internal void FireNotification(AudioVolumeNotificationData NotificationData)
		{
			AudioEndpointVolumeNotificationDelegate onVolumeNotification = this.OnVolumeNotification;
			if (onVolumeNotification != null)
			{
				onVolumeNotification(NotificationData);
			}
		}
		public void Dispose()
		{
			if (this._CallBack != null)
			{
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.UnregisterControlChangeNotify(this._CallBack));
				this._CallBack = null;
			}
		}
		~AudioEndpointVolume()
		{
			this.Dispose();
		}
	}
}
