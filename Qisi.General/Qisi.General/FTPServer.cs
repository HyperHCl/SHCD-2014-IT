using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace Qisi.General
{
	public class FTPServer
	{
		public delegate void TextEventHandler(object sender, MessageEventArgs e);
		private TcpListener myTcpListener = null;
		private Thread listenThread;
		private Dictionary<string, string> users;
		private string FTPRoot = "";
		private int port = 21;
		public event FTPServer.TextEventHandler Log;
		public FTPServer()
		{
			this.users = new Dictionary<string, string>();
			this.users.Add("KeysAdmin", "Keys1009");
			this.FTPRoot = Environment.SystemDirectory;
			ThreadPool.SetMaxThreads(1000, 1000);
			ThreadPool.SetMinThreads(1000, 1000);
		}
		public FTPServer(string root)
		{
			this.users = new Dictionary<string, string>();
			this.users.Add("KeysAdmin", "Keys1009");
			this.FTPRoot = root;
			ThreadPool.SetMaxThreads(1000, 1000);
			ThreadPool.SetMinThreads(1000, 1000);
		}
		public FTPServer(string username, string password, string root)
		{
			this.users = new Dictionary<string, string>();
			this.users.Add(username, password);
			this.FTPRoot = root;
			ThreadPool.SetMaxThreads(1000, 1000);
			ThreadPool.SetMinThreads(1000, 1000);
		}
		public void startFTPServer()
		{
			if (this.myTcpListener == null)
			{
				this.listenThread = new Thread(new ThreadStart(this.ListenClientConnect));
				this.listenThread.IsBackground = true;
				this.listenThread.Start();
			}
			else
			{
				this.myTcpListener.Stop();
				this.myTcpListener = null;
				this.listenThread.Abort();
			}
		}
		private void ListenClientConnect()
		{
			this.myTcpListener = new TcpListener(IPAddress.Any, this.port);
			try
			{
				this.myTcpListener.Start();
			}
			catch
			{
				this.AddInfo("21端口监听启动失败！");
				this.AddInfo("Ftp服务器启动失败");
				return;
			}
			this.AddInfo("启动FTP服务成功！");
			this.AddInfo("Ftp服务器运行中...");
			while (true)
			{
				try
				{
					TcpClient tcpClient = this.myTcpListener.AcceptTcpClient();
					this.AddInfo(string.Format("客户端（{0}）与本机（{1}）建立Ftp连接", tcpClient.Client.RemoteEndPoint, this.myTcpListener.LocalEndpoint));
					User user = new User();
					user.commandSession = new UserSeesion(tcpClient);
					user.workDir = this.FTPRoot;
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.UserProcessing), user);
				}
				catch (Exception ex)
				{
					this.AddInfo("客户端与本机建立Ftp连接时发生错误，具体原因是：" + ex.Message);
					break;
				}
				Thread.Sleep(1);
			}
		}
		private void UserProcessing(object obj)
		{
			User user = (User)obj;
			string str = "220 FTP Server v1.0";
			this.RepleyCommandToUser(user, str);
			while (true)
			{
				string text = null;
				try
				{
					text = user.commandSession.streamReader.ReadLine();
				}
				catch (Exception ex)
				{
					if (!user.commandSession.tcpClient.Connected)
					{
						this.AddInfo(string.Format("客户端({0}断开连接！)", user.commandSession.tcpClient.Client.RemoteEndPoint));
					}
					else
					{
						this.AddInfo("接收命令失败！" + ex.Message);
						text = null;
					}
					goto IL_3DD;
				}
				if (text == null)
				{
					break;
				}
				this.AddInfo(string.Format("来自{0}：[{1}]", user.commandSession.tcpClient.Client.RemoteEndPoint, text));
				string text2 = text;
				string text3 = string.Empty;
				int num = text.IndexOf(' ');
				if (num != -1)
				{
					text2 = text.Substring(0, num).ToUpper();
					text3 = text.Substring(text2.Length).Trim();
				}
				if (text2 == "QUIT")
				{
					goto Block_5;
				}
				switch (user.loginOK)
				{
				case 0:
					this.CommandUser(user, text2, text3);
					break;
				case 1:
					this.CommandPassword(user, text2, text3);
					break;
				case 2:
				{
					string text4 = text2;
					if (text4 == null)
					{
						goto IL_3B9;
					}
					if (<PrivateImplementationDetails>{544CA684-E25C-441E-8A25-D0A1091DD06C}.$$method0x600003e-1 == null)
					{
						<PrivateImplementationDetails>{544CA684-E25C-441E-8A25-D0A1091DD06C}.$$method0x600003e-1 = new Dictionary<string, int>(15)
						{

							{
								"CWD",
								0
							},

							{
								"PWD",
								1
							},

							{
								"PASV",
								2
							},

							{
								"PORT",
								3
							},

							{
								"LIST",
								4
							},

							{
								"NLST",
								5
							},

							{
								"RETR",
								6
							},

							{
								"STOR",
								7
							},

							{
								"DELE",
								8
							},

							{
								"TYPE",
								9
							},

							{
								"SYST",
								10
							},

							{
								"FEAT",
								11
							},

							{
								"MDTM",
								12
							},

							{
								"CDUP",
								13
							},

							{
								"SIZE",
								14
							}
						};
					}
					int num2;
					if (!<PrivateImplementationDetails>{544CA684-E25C-441E-8A25-D0A1091DD06C}.$$method0x600003e-1.TryGetValue(text4, out num2))
					{
						goto IL_3B9;
					}
					switch (num2)
					{
					case 0:
						this.CommandCWD(user, text3);
						break;
					case 1:
						this.CommandPWD(user);
						break;
					case 2:
						this.CommandPASV(user);
						break;
					case 3:
						this.CommandPORT(user, text3);
						break;
					case 4:
						this.CommandLIST(user, text3);
						break;
					case 5:
						this.CommandNLST(user, text3);
						break;
					case 6:
						this.CommandRETR(user, text3);
						break;
					case 7:
						this.CommandSTOR(user, text3);
						break;
					case 8:
						this.CommandDELE(user, text3);
						break;
					case 9:
						this.CommandTYPE(user, text3);
						break;
					case 10:
						str = "215 Windows_NT";
						this.RepleyCommandToUser(user, str);
						break;
					case 11:
						str = "211-Extended features supported:\r\n LANG EN*\r\n UTF8\r\n AUTH TLS;TLS-C;SSL;TLS-P;\r\n PBSZ\r\n PROT C;P;\r\n CCC\r\n HOST\r\n SIZE\r\n MDTM\r\n REST STREAM\r\n211 END";
						this.RepleyCommandToUser(user, str);
						break;
					case 12:
						this.CommandMDTM(user, text3);
						break;
					case 13:
						this.CommandCDUP(user, text3);
						break;
					case 14:
						this.CommandSIZE(user, text3);
						break;
					default:
						goto IL_3B9;
					}
					break;
					IL_3B9:
					str = "502 command is not implemented.";
					this.RepleyCommandToUser(user, str);
					break;
				}
				}
				Thread.Sleep(1);
			}
			this.AddInfo("接收字符串为null,结束线程！");
			try
			{
				user.commandSession.Close();
			}
			catch
			{
			}
			goto IL_3DD;
			Block_5:
			str = "221 Goodbye.";
			this.RepleyCommandToUser(user, str);
			user.commandSession.Close();
			return;
			IL_3DD:
			try
			{
				user.commandSession.Close();
			}
			catch
			{
			}
		}
		private void RepleyCommandToUser(User user, string str)
		{
			try
			{
				user.commandSession.streamWriter.WriteLine(str);
				this.AddInfo(string.Format("向客户端（{0}）发送[{1}]", user.commandSession.tcpClient.Client.RemoteEndPoint, str));
			}
			catch
			{
				this.AddInfo(string.Format("向客户端（{0}）发送信息失败", user.commandSession.tcpClient.Client.RemoteEndPoint));
			}
		}
		private void AddInfo(string str)
		{
			try
			{
				this.Log(this, new MessageEventArgs(str));
			}
			catch
			{
			}
		}
		private void CommandUser(User user, string command, string param)
		{
			string str = string.Empty;
			if (command == "USER")
			{
				str = "331 USER command OK, password required.";
				user.userName = param;
				user.loginOK = 1;
			}
			else
			{
				str = "501 USER command syntax error.";
			}
			this.RepleyCommandToUser(user, str);
		}
		private void CommandPassword(User user, string command, string param)
		{
			string str = string.Empty;
			if (command == "PASS")
			{
				string a = null;
				if (this.users.TryGetValue(user.userName, out a))
				{
					if (a == param)
					{
						str = "230 User logged in success";
						user.loginOK = 2;
					}
					else
					{
						str = "530 Password incorrect.";
					}
				}
				else
				{
					str = "530 User name or password incorrect.";
				}
			}
			else
			{
				str = "501 PASS command Syntax error.";
			}
			this.RepleyCommandToUser(user, str);
			user.currentDir = "/";
		}
		private void CommandCWD(User user, string temp)
		{
			string str = string.Empty;
			try
			{
				if (temp == "/")
				{
					user.currentDir = "/";
					user.workDir = this.FTPRoot;
					str = "250 CWD command successful";
				}
				else
				{
					if (temp.StartsWith("/"))
					{
						user.currentDir = temp;
						user.workDir = Path.Combine(this.FTPRoot, temp);
						str = "250 CWD command successful";
					}
					else
					{
						string text = Path.Combine(user.workDir, temp);
						if (Directory.Exists(text))
						{
							DirectoryInfo directoryInfo = new DirectoryInfo(text);
							try
							{
								directoryInfo.GetFiles();
								if (user.currentDir.EndsWith("/"))
								{
									user.currentDir += temp;
								}
								else
								{
									user.currentDir = user.currentDir + "/" + temp;
								}
								user.workDir = text;
								str = "250 CWD command successful";
							}
							catch
							{
								str = "550-Access is denied.\r\n Win32 error:   Access is denied.\r\n Error details: File system denied the access.\r\n550 End";
								this.RepleyCommandToUser(user, str);
								return;
							}
						}
						else
						{
							str = "550 Directory '" + user.currentDir + "' does not exist";
						}
					}
				}
			}
			catch
			{
				str = "502 Directory changed unsuccessfully";
			}
			this.RepleyCommandToUser(user, str);
		}
		private void CommandCDUP(User user, string temp)
		{
			string str = string.Empty;
			if (user.workDir == this.FTPRoot)
			{
				str = "502 Directory changed unsuccessfully";
			}
			else
			{
				try
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(user.workDir);
					string fullName = directoryInfo.Parent.FullName;
					if (Directory.Exists(fullName))
					{
						user.currentDir = user.currentDir.Substring(0, user.currentDir.LastIndexOf('/') + 1);
						user.workDir = fullName;
						str = "250 Directory changed to '" + user.currentDir + "' successfully";
					}
					else
					{
						str = "550 Directory '" + user.currentDir + "' does not exist";
					}
				}
				catch
				{
					str = "502 Directory changed unsuccessfully";
				}
			}
			this.RepleyCommandToUser(user, str);
		}
		private void CommandSIZE(User user, string temp)
		{
			string str = string.Empty;
			if (File.Exists(Path.Combine(user.workDir, temp)))
			{
				FileInfo fileInfo = new FileInfo(Path.Combine(user.workDir, temp));
				str = "213 " + fileInfo.Length;
			}
			else
			{
				str = "550-Access is denied.\r\n Win32 error:   Access is denied.\r\n Error details: File system denied the access.\r\n550 End";
			}
			this.RepleyCommandToUser(user, str);
		}
		private void CommandMDTM(User user, string temp)
		{
			string str = string.Empty;
			FileInfo fileInfo = new FileInfo(Path.Combine(this.FTPRoot, temp));
			str = "213 " + fileInfo.CreationTime.ToString("yyyyMMddHHmmss");
			this.RepleyCommandToUser(user, str);
		}
		private void CommandPWD(User user)
		{
			string str = string.Empty;
			str = "257 \"" + user.currentDir + "\" is current directory.";
			this.RepleyCommandToUser(user, str);
		}
		private void CommandLIST(User user, string parameter)
		{
			string text = string.Empty;
			DirectoryInfo directoryInfo = new DirectoryInfo(user.workDir);
			DirectoryInfo[] directories;
			try
			{
				directories = directoryInfo.GetDirectories();
			}
			catch
			{
				text = "550-Access is denied.\r\n Win32 error:   Access is denied.\r\n Error details: File system denied the access.\r\n550 End";
				this.RepleyCommandToUser(user, text);
				return;
			}
			if (!string.IsNullOrEmpty(parameter))
			{
				if (Directory.Exists(Path.Combine(user.workDir, parameter)))
				{
					directoryInfo = new DirectoryInfo(Path.Combine(user.workDir, parameter));
					directories = directoryInfo.GetDirectories();
				}
			}
			if (parameter == "-al")
			{
				for (int i = 0; i < directories.Length; i++)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						directories[i].CreationTime.ToString("MM-dd-yy  hh:mmtt", new CultureInfo("en-US")),
						"       <DIR>          ",
						directories[i].Name,
						"\r\n"
					});
				}
			}
			else
			{
				for (int i = 0; i < directories.Length; i++)
				{
					if (directories[i].Attributes == FileAttributes.Hidden)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							directories[i].CreationTime.ToString("MM-dd-yy  hh:mmtt", new CultureInfo("en-US")),
							"       <DIR>          ",
							directories[i].Name,
							"\r\n"
						});
					}
				}
			}
			FileInfo[] files = directoryInfo.GetFiles();
			if (parameter == "-al")
			{
				for (int i = 0; i < files.Length; i++)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						files[i].CreationTime.ToString("MM-dd-yy  hh:mmtt", new CultureInfo("en-US")),
						"                 ",
						files[i].Length,
						" ",
						files[i].Name,
						Environment.NewLine
					});
				}
			}
			else
			{
				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Attributes == FileAttributes.Normal)
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							files[i].CreationTime.ToString("MM-dd-yy  hh:mmtt", new CultureInfo("en-US")),
							"                 ",
							files[i].Length,
							" ",
							files[i].Name,
							Environment.NewLine
						});
					}
				}
			}
			this.InitDataSession(user);
			this.RepleyCommandToUser(user, "125 Data connection already open; Transfer starting.");
			this.SendByUserSession(user, text);
			this.RepleyCommandToUser(user, "226 Transfer complete");
		}
		private void CommandNLST(User user, string parameter)
		{
			string text = string.Empty;
			DirectoryInfo directoryInfo = new DirectoryInfo(user.workDir);
			DirectoryInfo[] directories;
			try
			{
				directories = directoryInfo.GetDirectories();
			}
			catch
			{
				text = "550-Access is denied.\r\n Win32 error:   Access is denied.\r\n Error details: File system denied the access.\r\n550 End";
				this.RepleyCommandToUser(user, text);
				return;
			}
			if (!string.IsNullOrEmpty(parameter))
			{
				if (Directory.Exists(Path.Combine(user.workDir, parameter)))
				{
					directoryInfo = new DirectoryInfo(Path.Combine(user.workDir, parameter));
					directories = directoryInfo.GetDirectories();
				}
			}
			if (parameter == "-al")
			{
				for (int i = 0; i < directories.Length; i++)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						directories[i].CreationTime.ToString("MM-dd-yy  hh:mmtt", new CultureInfo("en-US")),
						"       <DIR>          ",
						directories[i].Name,
						"\r\n"
					});
				}
			}
			else
			{
				for (int i = 0; i < directories.Length; i++)
				{
					if (directories[i].Attributes == FileAttributes.Hidden)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							directories[i].CreationTime.ToString("MM-dd-yy  hh:mmtt", new CultureInfo("en-US")),
							"       <DIR>          ",
							directories[i].Name,
							"\r\n"
						});
					}
				}
			}
			this.RepleyCommandToUser(user, "125 Data connection already open; Transfer starting.");
			this.InitDataSession(user);
			this.SendByUserSession(user, text);
			this.RepleyCommandToUser(user, "226 Transfer complete");
		}
		private void CommandRETR(User user, string filename)
		{
			string path;
			if (filename.StartsWith("/"))
			{
				path = this.FTPRoot + filename.Replace("/", "\\");
			}
			else
			{
				path = this.FTPRoot + user.currentDir + filename;
			}
			FileStream fs;
			string str;
			try
			{
				fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			}
			catch
			{
				str = "550-Access is denied.\r\n Win32 error:   Access is denied.\r\n Error details: File system denied the access.\r\n550 End";
				this.RepleyCommandToUser(user, str);
				return;
			}
			if (user.isBinary)
			{
				str = "150 Opening BINARY mode data connection for download";
			}
			else
			{
				str = "150 Opening ASCII mode data connection for download";
			}
			this.RepleyCommandToUser(user, str);
			this.InitDataSession(user);
			this.SendFileByUserSession(user, fs);
			this.RepleyCommandToUser(user, "226 Transfer complete");
		}
		private void CommandSTOR(User user, string filename)
		{
			string path;
			if (filename.StartsWith("/"))
			{
				path = this.FTPRoot + filename.Replace("/", "\\");
			}
			else
			{
				path = user.currentDir + filename;
			}
			FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
			string str;
			if (user.isBinary)
			{
				str = "150 Opening BINARY mode data connection for upload";
			}
			else
			{
				str = "150 Opeing ASCII mode data connection for upload";
			}
			this.RepleyCommandToUser(user, str);
			this.InitDataSession(user);
			this.ReadFileByUserSession(user, fs);
			this.RepleyCommandToUser(user, "226 Transfer complete");
		}
		private void CommandDELE(User user, string filename)
		{
			string path = user.currentDir + filename;
			this.AddInfo("正在删除文件" + filename + "...");
			File.Delete(path);
			this.AddInfo("删除成功");
			string str = "250 File " + filename + " has been deleted.";
			this.RepleyCommandToUser(user, str);
		}
		private void CommandPASV(User user)
		{
			string str = string.Empty;
			Random random = new Random();
			int num;
			int num2;
			while (true)
			{
				num = random.Next(5, 200);
				num2 = random.Next(0, 200);
				int num3 = num << 8 | num2;
				try
				{
					user.dataListener = new TcpListener(IPAddress.Any, num3);
					user.dataListener.Start();
					this.AddInfo("TCP 数据连接已打开（被动模式）--端口" + num3);
				}
				catch
				{
					continue;
				}
				break;
			}
			user.isPassive = true;
			string text = ((IPEndPoint)user.commandSession.tcpClient.Client.LocalEndPoint).Address.ToString().Replace('.', ',');
			str = string.Concat(new object[]
			{
				"227 Entering Passive Mode(",
				text,
				",",
				num,
				",",
				num2,
				")"
			});
			this.RepleyCommandToUser(user, str);
		}
		private void CommandPORT(User user, string portstring)
		{
			string str = string.Empty;
			string[] array = portstring.Split(new char[]
			{
				','
			});
			string ipString = string.Concat(new string[]
			{
				array[0],
				".",
				array[1],
				".",
				array[2],
				".",
				array[3]
			});
			int num = int.Parse(array[4]) << 8 | int.Parse(array[5]);
			user.remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipString), num);
			str = "200 PORT command successful.";
			this.RepleyCommandToUser(user, str);
		}
		private void CommandTYPE(User user, string param)
		{
			string str;
			if (param == "I")
			{
				user.isBinary = true;
				str = "200 Type set to I.";
			}
			else
			{
				user.isBinary = false;
				str = "200 Type set to A.";
			}
			this.RepleyCommandToUser(user, str);
		}
		private void InitDataSession(User user)
		{
			TcpClient tcpClient;
			if (user.isPassive)
			{
				this.AddInfo("采用被动模式返回LIST目录和文件列表");
				tcpClient = user.dataListener.AcceptTcpClient();
			}
			else
			{
				this.AddInfo("采用主动模式向用户发送LIST目录和文件列表");
				tcpClient = new TcpClient();
				tcpClient.Connect(user.remoteEndPoint);
			}
			user.dataSession = new UserSeesion(tcpClient);
		}
		private void SendByUserSession(User user, string sendString)
		{
			this.AddInfo("向用户发送(字符串信息)：[" + sendString + "]");
			try
			{
				user.dataSession.streamWriter.WriteLine(sendString);
				this.AddInfo("发送完毕");
			}
			finally
			{
				user.dataSession.Close();
			}
		}
		private void SendFileByUserSession(User user, FileStream fs)
		{
			this.AddInfo("向用户发送(文件流)：[...");
			try
			{
				if (user.isBinary)
				{
					byte[] array = new byte[1024];
					BinaryReader binaryReader = new BinaryReader(fs);
					for (int i = binaryReader.Read(array, 0, array.Length); i > 0; i = binaryReader.Read(array, 0, array.Length))
					{
						user.dataSession.binaryWriter.Write(array, 0, i);
						user.dataSession.binaryWriter.Flush();
					}
				}
				else
				{
					StreamReader streamReader = new StreamReader(fs);
					while (streamReader.Peek() > -1)
					{
						user.dataSession.streamWriter.WriteLine(streamReader.ReadLine());
					}
				}
				this.AddInfo("...]发送完毕！");
			}
			finally
			{
				user.dataSession.Close();
				fs.Close();
			}
		}
		private void ReadFileByUserSession(User user, FileStream fs)
		{
			this.AddInfo("接收用户上传数据（文件流）：[...");
			try
			{
				if (user.isBinary)
				{
					byte[] array = new byte[1024];
					BinaryWriter binaryWriter = new BinaryWriter(fs);
					for (int i = user.dataSession.binaryReader.Read(array, 0, array.Length); i > 0; i = user.dataSession.binaryReader.Read(array, 0, array.Length))
					{
						binaryWriter.Write(array, 0, i);
						binaryWriter.Flush();
					}
				}
				else
				{
					StreamWriter streamWriter = new StreamWriter(fs);
					while (user.dataSession.streamReader.Peek() > -1)
					{
						streamWriter.Write(user.dataSession.streamReader.ReadLine());
						streamWriter.Flush();
					}
				}
				this.AddInfo("...]接收完毕");
			}
			finally
			{
				user.dataSession.Close();
				fs.Close();
			}
		}
	}
}
