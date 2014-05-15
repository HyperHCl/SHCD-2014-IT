using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class SessionCollection
	{
		private IAudioSessionEnumerator _AudioSessionEnumerator;
		public AudioSessionControl this[int index]
		{
			get
			{
				IAudioSessionControl2 realAudioSessionControl;
				Marshal.ThrowExceptionForHR(this._AudioSessionEnumerator.GetSession(index, out realAudioSessionControl));
				return new AudioSessionControl(realAudioSessionControl);
			}
		}
		public int Count
		{
			get
			{
				int result;
				Marshal.ThrowExceptionForHR(this._AudioSessionEnumerator.GetCount(out result));
				return result;
			}
		}
		internal SessionCollection(IAudioSessionEnumerator realEnumerator)
		{
			this._AudioSessionEnumerator = realEnumerator;
		}
	}
}
