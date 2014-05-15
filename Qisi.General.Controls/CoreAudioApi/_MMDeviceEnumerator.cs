using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	[Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
	[ComImport]
	internal class _MMDeviceEnumerator
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		// Hey, What are you doing, ILSpy?
		public extern _MMDeviceEnumerator();
	}
}
