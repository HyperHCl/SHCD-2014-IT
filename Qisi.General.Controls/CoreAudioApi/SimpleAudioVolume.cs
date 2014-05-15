using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class SimpleAudioVolume
	{
		private ISimpleAudioVolume _SimpleAudioVolume;
		public float MasterVolume
		{
			get
			{
				float result;
				Marshal.ThrowExceptionForHR(this._SimpleAudioVolume.GetMasterVolume(out result));
				return result;
			}
			set
			{
				Guid empty = Guid.Empty;
				Marshal.ThrowExceptionForHR(this._SimpleAudioVolume.SetMasterVolume(value, ref empty));
			}
		}
		public bool Mute
		{
			get
			{
				bool result;
				Marshal.ThrowExceptionForHR(this._SimpleAudioVolume.GetMute(out result));
				return result;
			}
			set
			{
				Guid empty = Guid.Empty;
				Marshal.ThrowExceptionForHR(this._SimpleAudioVolume.SetMute(value, ref empty));
			}
		}
		internal SimpleAudioVolume(ISimpleAudioVolume realSimpleVolume)
		{
			this._SimpleAudioVolume = realSimpleVolume;
		}
	}
}
