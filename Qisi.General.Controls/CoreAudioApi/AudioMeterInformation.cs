using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioMeterInformation
	{
		private IAudioMeterInformation _AudioMeterInformation;
		private EEndpointHardwareSupport _HardwareSupport;
		private AudioMeterInformationChannels _Channels;
		public AudioMeterInformationChannels PeakValues
		{
			get
			{
				return this._Channels;
			}
		}
		public EEndpointHardwareSupport HardwareSupport
		{
			get
			{
				return this._HardwareSupport;
			}
		}
		public float MasterPeakValue
		{
			get
			{
				float result;
				Marshal.ThrowExceptionForHR(this._AudioMeterInformation.GetPeakValue(out result));
				return result;
			}
		}
		internal AudioMeterInformation(IAudioMeterInformation realInterface)
		{
			this._AudioMeterInformation = realInterface;
			int hardwareSupport;
			Marshal.ThrowExceptionForHR(this._AudioMeterInformation.QueryHardwareSupport(out hardwareSupport));
			this._HardwareSupport = (EEndpointHardwareSupport)hardwareSupport;
			this._Channels = new AudioMeterInformationChannels(this._AudioMeterInformation);
		}
	}
}
