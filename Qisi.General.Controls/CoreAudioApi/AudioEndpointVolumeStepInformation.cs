using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioEndpointVolumeStepInformation
	{
		private uint _Step;
		private uint _StepCount;
		public uint Step
		{
			get
			{
				return this._Step;
			}
		}
		public uint StepCount
		{
			get
			{
				return this._StepCount;
			}
		}
		internal AudioEndpointVolumeStepInformation(IAudioEndpointVolume parent)
		{
			Marshal.ThrowExceptionForHR(parent.GetVolumeStepInfo(out this._Step, out this._StepCount));
		}
	}
}
