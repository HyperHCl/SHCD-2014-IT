using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
namespace Qisi.General
{
	public class FTPClient
	{
		public delegate void TextEventHandler(object sender, MessageEventArgs e);
		private const int ftpport = 21;
		private string ftpUristring = null;
		private NetworkCredential networkCredential;
		private string currentDir = "/";
		private List<string> DirList = null;
		private List<string> FileList = null;
		public event FTPClient.TextEventHandler Log;
		public FTPClient(string ip, string username, string password)
		{
			this.ftpUristring = "ftp://" + ip;
			this.networkCredential = new NetworkCredential(username, password);
			this.DirList = new List<string>();
			this.FileList = new List<string>();
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
		private FtpWebRequest CreateFtpWebRequest(string uri, string requestMethod)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(uri);
			ftpWebRequest.Credentials = this.networkCredential;
			ftpWebRequest.KeepAlive = true;
			ftpWebRequest.UseBinary = true;
			ftpWebRequest.Method = requestMethod;
			ftpWebRequest.Proxy = null;
			return ftpWebRequest;
		}
		private FtpWebResponse GetFtpResponse(FtpWebRequest request)
		{
			FtpWebResponse result;
			try
			{
				FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse();
				this.AddInfo("验证完毕，服务器回应信息：[" + ftpWebResponse.WelcomeMessage + "]");
				this.AddInfo("正在连接：[ " + ftpWebResponse.BannerMessage + "]");
				result = ftpWebResponse;
			}
			catch (WebException ex)
			{
				this.AddInfo("发送错误。返回信息为：" + ex.Status);
				result = null;
			}
			return result;
		}
		public bool login()
		{
			bool result;
			if (this.ShowFtpFileAndDirectory())
			{
				this.AddInfo("登录成功");
				result = true;
			}
			else
			{
				this.AddInfo("登录失败");
				result = false;
			}
			return result;
		}
		private bool ShowFtpFileAndDirectory()
		{
			string text = string.Empty;
			if (this.currentDir == "/")
			{
				text = this.ftpUristring;
			}
			else
			{
				text = this.ftpUristring + this.currentDir;
			}
			string[] array = text.Split(new char[]
			{
				' '
			});
			text = array[0];
			FtpWebRequest request = this.CreateFtpWebRequest(text, "LIST");
			FtpWebResponse ftpResponse = this.GetFtpResponse(request);
			bool result;
			if (ftpResponse == null)
			{
				result = false;
			}
			else
			{
				this.AddInfo(string.Concat(new object[]
				{
					"连接成功，服务器返回的是：",
					ftpResponse.StatusCode,
					" ",
					ftpResponse.StatusDescription
				}));
				Stream responseStream = ftpResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);
				this.AddInfo("获取响应流....");
				string text2 = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
				ftpResponse.Close();
				this.AddInfo("传输完成");
				this.DirList.Clear();
				this.FileList.Clear();
				string[] array2 = text2.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				int num = 0;
				int i;
				for (i = 0; i < array2.Length; i++)
				{
					if (array2[i].EndsWith("."))
					{
						num = array2[i].Length - 2;
						break;
					}
				}
				i = 0;
				while (i < array2.Length)
				{
					text2 = array2[i];
					int num2 = text2.LastIndexOf('\t');
					if (num2 != -1)
					{
						goto IL_1D4;
					}
					if (num < text2.Length)
					{
						num2 = num;
						goto IL_1D4;
					}
					IL_295:
					i++;
					continue;
					IL_1D4:
					string text3 = text2.Substring(num2 + 1);
					if (text3 == "." || text3 == "..")
					{
						goto IL_295;
					}
					if (text2[0] == 'd' || text2.ToLower().Contains("<dir>"))
					{
						string[] array3 = text3.Split(new char[]
						{
							' '
						});
						int num3 = array3.Length;
						string text4 = array3[num3 - 1];
						text4 = text4.PadRight(34, ' ');
						text3 = text4;
						this.DirList.Add(text3);
						this.AddInfo("[目录]" + text3);
					}
					goto IL_295;
				}
				i = 0;
				while (i < array2.Length)
				{
					text2 = array2[i];
					int num2 = text2.LastIndexOf('\t');
					if (num2 != -1)
					{
						goto IL_2F9;
					}
					if (num < text2.Length)
					{
						num2 = num;
						goto IL_2F9;
					}
					IL_3AA:
					i++;
					continue;
					IL_2F9:
					string text3 = text2.Substring(num2 + 1);
					if (text3 == "." || text3 == "..")
					{
						goto IL_3AA;
					}
					if (text2[0] != 'd' && !text2.ToLower().Contains("<dir>"))
					{
						string[] array3 = text3.Split(new char[]
						{
							' '
						});
						int num3 = array3.Length;
						string text5 = array3[num3 - 1];
						text5 = text5.PadRight(34, ' ');
						text3 = text5;
						this.FileList.Add(text3);
						this.AddInfo(text3);
					}
					goto IL_3AA;
				}
				result = true;
			}
			return result;
		}
		public void logout()
		{
			this.CreateFtpWebRequest(this.ftpUristring, "QUIT");
		}
		public bool Upload(string filepath)
		{
			bool result;
			if (!File.Exists(filepath))
			{
				result = false;
			}
			else
			{
				FileInfo fileInfo = new FileInfo(filepath);
				try
				{
					string uriString = this.GetUriString(fileInfo.Name);
					FtpWebRequest ftpWebRequest = this.CreateFtpWebRequest(uriString, "STOR");
					ftpWebRequest.ContentLength = fileInfo.Length;
					int num = 8196;
					byte[] buffer = new byte[num];
					FileStream fileStream = fileInfo.OpenRead();
					Stream requestStream = ftpWebRequest.GetRequestStream();
					this.AddInfo("打开上传流，文件上传中...");
					for (int num2 = fileStream.Read(buffer, 0, num); num2 != 0; num2 = fileStream.Read(buffer, 0, num))
					{
						requestStream.Write(buffer, 0, num2);
					}
					requestStream.Close();
					fileStream.Close();
					FtpWebResponse ftpResponse = this.GetFtpResponse(ftpWebRequest);
					if (ftpResponse == null)
					{
						this.AddInfo("服务器未响应...");
						result = false;
					}
					else
					{
						this.AddInfo(string.Concat(new object[]
						{
							"上传完毕，服务器返回：",
							ftpResponse.StatusCode,
							" ",
							ftpResponse.StatusDescription
						}));
						this.ShowFtpFileAndDirectory();
						result = true;
					}
				}
				catch (WebException ex)
				{
					this.AddInfo("上传发生错误，返回信息为：" + ex.Status);
					result = false;
				}
			}
			return result;
		}
		private string GetUriString(string filename)
		{
			string result = string.Empty;
			if (this.currentDir.EndsWith("/"))
			{
				result = this.ftpUristring + this.currentDir + filename;
			}
			else
			{
				result = this.ftpUristring + this.currentDir + "/" + filename;
			}
			return result;
		}
		public bool download(string fileName, string filePath)
		{
			bool result;
			if (!this.FileList.Contains(fileName.PadRight(34, ' ')))
			{
				this.AddInfo("要下载的文件不存在");
				result = false;
			}
			else
			{
				try
				{
					string uriString = this.GetUriString(fileName);
					FtpWebRequest request = this.CreateFtpWebRequest(uriString, "RETR");
					FtpWebResponse ftpResponse = this.GetFtpResponse(request);
					if (ftpResponse == null)
					{
						this.AddInfo("服务器未响应...");
						result = false;
					}
					else
					{
						Stream responseStream = ftpResponse.GetResponseStream();
						FileStream fileStream = File.Create(filePath);
						int num = 8196;
						byte[] buffer = new byte[num];
						int num2 = 1;
						this.AddInfo("打开下载通道，文件下载中...");
						while (num2 != 0)
						{
							num2 = responseStream.Read(buffer, 0, num);
							fileStream.Write(buffer, 0, num2);
						}
						responseStream.Close();
						fileStream.Close();
						this.AddInfo(string.Concat(new object[]
						{
							"下载完毕，服务器返回：",
							ftpResponse.StatusCode,
							" ",
							ftpResponse.StatusDescription
						}));
						result = true;
					}
				}
				catch (WebException ex)
				{
					this.AddInfo("发生错误，返回状态为：" + ex.Status);
					result = false;
				}
			}
			return result;
		}
		private bool Delete(string filename)
		{
			bool result;
			if (!this.FileList.Contains(filename.PadRight(34, ' ')))
			{
				this.AddInfo("要删除的文件不存在");
				result = false;
			}
			else
			{
				try
				{
					string uriString = this.GetUriString(filename);
					FtpWebRequest request = this.CreateFtpWebRequest(uriString, "DELE");
					FtpWebResponse ftpResponse = this.GetFtpResponse(request);
					if (ftpResponse == null)
					{
						result = false;
					}
					else
					{
						this.ShowFtpFileAndDirectory();
						result = true;
					}
				}
				catch (WebException ex)
				{
					this.AddInfo(ex.Message + " 删除失败");
					result = false;
				}
			}
			return result;
		}
		public void changeDir(string newDir)
		{
			if (newDir == "")
			{
				if (this.currentDir == "/")
				{
					this.AddInfo("当前目录已经是顶层目录");
				}
				else
				{
					int num = this.currentDir.LastIndexOf("/");
					if (num == 0)
					{
						this.currentDir = "/";
					}
					else
					{
						this.currentDir = this.currentDir.Substring(0, num);
					}
					this.ShowFtpFileAndDirectory();
				}
			}
			else
			{
				if (this.DirList.Contains(newDir.PadRight(34, ' ')))
				{
					if (this.currentDir == "/")
					{
						this.currentDir = "/" + newDir;
					}
					else
					{
						this.currentDir = this.currentDir + "/" + newDir;
					}
					string[] array = this.currentDir.Split(new char[]
					{
						' '
					});
					this.currentDir = array[0];
					this.ShowFtpFileAndDirectory();
				}
			}
		}
		public void makeDir(string newDir)
		{
			this.ShowFtpFileAndDirectory();
			if (newDir == "")
			{
				if (this.currentDir == "/")
				{
					this.AddInfo("当前目录已经是顶层目录");
				}
				else
				{
					int num = this.currentDir.LastIndexOf("/");
					if (num == 0)
					{
						this.currentDir = "/";
					}
					else
					{
						this.currentDir = this.currentDir.Substring(0, num);
					}
					this.ShowFtpFileAndDirectory();
				}
			}
			else
			{
				if (this.DirList.Contains(newDir.PadRight(34, ' ')))
				{
					if (this.currentDir == "/")
					{
						this.currentDir = "/" + newDir;
					}
					else
					{
						this.currentDir = this.currentDir + "/" + newDir;
					}
					string[] array = this.currentDir.Split(new char[]
					{
						' '
					});
					this.currentDir = array[0];
				}
				else
				{
					string uriString = this.GetUriString(newDir);
					FtpWebRequest request = this.CreateFtpWebRequest(uriString, "MKD");
					FtpWebResponse ftpResponse = this.GetFtpResponse(request);
					Stream responseStream = ftpResponse.GetResponseStream();
					this.ShowFtpFileAndDirectory();
					this.changeDir(newDir);
				}
			}
		}
	}
}
