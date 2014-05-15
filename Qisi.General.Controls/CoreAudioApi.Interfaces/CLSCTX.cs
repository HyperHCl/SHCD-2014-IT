using System;
namespace CoreAudioApi.Interfaces
{
	[Flags]
	internal enum CLSCTX : uint
	{
		INPROC_SERVER = 1u,
		INPROC_HANDLER = 2u,
		LOCAL_SERVER = 4u,
		INPROC_SERVER16 = 8u,
		REMOTE_SERVER = 16u,
		INPROC_HANDLER16 = 32u,
		RESERVED1 = 64u,
		RESERVED2 = 128u,
		RESERVED3 = 256u,
		RESERVED4 = 512u,
		NO_CODE_DOWNLOAD = 1024u,
		RESERVED5 = 2048u,
		NO_CUSTOM_MARSHAL = 4096u,
		ENABLE_CODE_DOWNLOAD = 8192u,
		NO_FAILURE_LOG = 16384u,
		DISABLE_AAA = 32768u,
		ENABLE_AAA = 65536u,
		FROM_DEFAULT_CONTEXT = 131072u,
		INPROC = 3u,
		SERVER = 21u,
		ALL = 23u
	}
}
