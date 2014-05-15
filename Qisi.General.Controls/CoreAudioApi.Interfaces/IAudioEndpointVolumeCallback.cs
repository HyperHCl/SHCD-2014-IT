using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi.Interfaces
{
	[Guid("657804FA-D6AD-4496-8A60-352752AF4F89"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAudioEndpointVolumeCallback
	{
		[PreserveSig]
		int OnNotify(IntPtr pNotifyData);
	}
}
