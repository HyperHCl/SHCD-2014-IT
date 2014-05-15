using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioSessionManager
	{
		private IAudioSessionManager2 _AudioSessionManager;
		private SessionCollection _Sessions;
		public SessionCollection Sessions
		{
			get
			{
				return this._Sessions;
			}
		}
		internal AudioSessionManager(IAudioSessionManager2 realAudioSessionManager)
		{
			this._AudioSessionManager = realAudioSessionManager;
			IAudioSessionEnumerator realEnumerator;
			Marshal.ThrowExceptionForHR(this._AudioSessionManager.GetSessionEnumerator(out realEnumerator));
			this._Sessions = new SessionCollection(realEnumerator);
		}
	}
}
