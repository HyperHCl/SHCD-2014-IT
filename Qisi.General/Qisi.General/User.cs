using System;
using System.Net;
using System.Net.Sockets;
namespace Qisi.General
{
	internal class User
	{
		public UserSeesion commandSession
		{
			get;
			set;
		}
		public UserSeesion dataSession
		{
			get;
			set;
		}
		public TcpListener dataListener
		{
			get;
			set;
		}
		public IPEndPoint remoteEndPoint
		{
			get;
			set;
		}
		public string userName
		{
			get;
			set;
		}
		public string workDir
		{
			get;
			set;
		}
		public string currentDir
		{
			get;
			set;
		}
		public int loginOK
		{
			get;
			set;
		}
		public bool isBinary
		{
			get;
			set;
		}
		public bool isPassive
		{
			get;
			set;
		}
	}
}
