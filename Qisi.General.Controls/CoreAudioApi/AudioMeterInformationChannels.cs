using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioMeterInformationChannels
	{
		private IAudioMeterInformation _AudioMeterInformation;
		public int Count
		{
			get
			{
				int result;
				Marshal.ThrowExceptionForHR(this._AudioMeterInformation.GetMeteringChannelCount(out result));
				return result;
			}
		}
		public float this[int index]
		{
			get
			{
				float[] array = new float[this.Count];
				GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				Marshal.ThrowExceptionForHR(this._AudioMeterInformation.GetChannelsPeakValues(array.Length, gCHandle.AddrOfPinnedObject()));
				gCHandle.Free();
				return array[index];
			}
		}
		internal AudioMeterInformationChannels(IAudioMeterInformation parent)
		{
			this._AudioMeterInformation = parent;
		}
	}
}
