// Program.cs: The SHCD Core
using Microsoft.Win32;
using Qisi.General.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
namespace SHCD
{
	internal static class Program
	{
		[Flags]
		private enum SendMessageTimeoutFlags : uint
		{
			SMTO_NORMAL = 0u,
			SMTO_BLOCK = 1u,
			SMTO_ABORTIFHUNG = 2u,
			SMTO_NOTIMEOUTIFNOTHUNG = 8u
		}
		private static string defaultID = "19";
		internal static string paperDir;
		internal static string dataDir;
		internal static string answerDir;
		internal static string tempAnswerDir;
		private static readonly IntPtr HWND_BROADCAST = new IntPtr(65535);
		private static uint WM_SETTINGCHANGE = 26u;
		[STAThread]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			bool flag;
			Mutex mutex = new Mutex(false, "Global\\SHCD", ref flag);
			if (!flag)
			{
				FlatMessageBox.Show("程序已经在运行！", "错误", FlatMessageBox.KeysButtons.OK, FlatMessageBox.KeysIcon.Error);
			}
			else
			{ // UAC Checking Deleted.
				new StartForm
				{
					OpacityIncreaseMilliseconds = 500,
					OpacityDecreaseMilliseconds = 500,
					KeepOpacityMilliseconds = 1000
				}.ShowDialog();
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load("http://www.keys-edu.com/update/update.xml");
					Version value = new Version(Application.ProductVersion); // Check for updates
					string text = "";
					foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
					{
						if (xmlNode.Attributes["name"] != null && xmlNode.Attributes["version"] != null && xmlNode.Attributes["url"] != null)
						{
							if (xmlNode.Attributes["name"].Value == "上海光盘2014")
							{
								Version version = new Version(xmlNode.Attributes["version"].Value);
								if (version.CompareTo(value) > 0)
								{
									value = version;
									text = xmlNode.Attributes["url"].Value;
								}
							}
						}
					}
					if (text != "")
					{
						if (FlatMessageBox.Show("练习光盘已发布最新版本，是否进行更新？", "提示", FlatMessageBox.KeysButtons.YesNo, FlatMessageBox.KeysIcon.Information) == DialogResult.Yes)
						{
							new Process
							{
								StartInfo = 
								{
									FileName = Path.Combine(Application.StartupPath, "Update.exe"),
									Arguments = text,
									WindowStyle = ProcessWindowStyle.Normal
								}
							}.Start();
							return;
						}
					}
				}
				catch
				{
				}
				string tempPath;
				try
				{
					tempPath = Path.GetTempPath(); // In Windows, this returns %windir%\temp so it would cause permission issues, as described in the following error meaasge.
					// However, in Linux this returns /tmp and in OS X this returns /private/var/folders/something-here. There shouldn't be permission issues then.
					// So, TODO: hack for Windows so this returns the user temp dir. 
				}
				catch (SecurityException) // Again. You ARE requiring the user to RUN as admin!
				{
					FlatMessageBox.Show("当前用户没有权限读取临时文件夹！\r\n请切换到管理员账户或者尝试右击程序图标选择“以管理员身份运行”。", "错误", FlatMessageBox.KeysButtons.OK, FlatMessageBox.KeysIcon.Error);
					return;
				}
				Program.paperDir = Path.Combine(tempPath, "Qisi");
				if (!Directory.Exists(Program.paperDir))
				{
					Directory.CreateDirectory(Program.paperDir);
				}
				Program.paperDir = Path.Combine(Program.paperDir, "Paper");
				if (!Directory.Exists(Program.paperDir))
				{
					Directory.CreateDirectory(Program.paperDir);
				}
				Program.dataDir = Path.Combine(tempPath, "Qisi");
				if (!Directory.Exists(Program.dataDir))
				{
					Directory.CreateDirectory(Program.dataDir);
				}
				Program.dataDir = Path.Combine(Program.dataDir, "VirtualDir");
				if (!Directory.Exists(Program.dataDir))
				{
					Directory.CreateDirectory(Program.dataDir);
				}
				Program.tempAnswerDir = Path.Combine(tempPath, "Qisi");
				if (!Directory.Exists(Program.tempAnswerDir))
				{
					Directory.CreateDirectory(Program.tempAnswerDir);
				}
				Program.tempAnswerDir = Path.Combine(Program.tempAnswerDir, "Answer");
				if (!Directory.Exists(Program.tempAnswerDir))
				{
					Directory.CreateDirectory(Program.tempAnswerDir);
				}
				Program.answerDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				if (!Directory.Exists(Program.answerDir))
				{
					string message; // Well, how about using just $HOME/SHCD-2014-IT?
					if (Environment.OSVersion.Version.Major >= 0)
					{
						message = "没有找到当前用户的“文档”文件夹。";
					}
					else
					{
						message = "没有找到当前用户的“我的文档”文件夹。";
					}
					FlatMessageBox.Show(message, "错误", FlatMessageBox.KeysButtons.OK, FlatMessageBox.KeysIcon.Error);
				}
				else
				{
					Program.answerDir = Path.Combine(Program.answerDir, "Qisi");
					if (!Directory.Exists(Program.answerDir))
					{
						Directory.CreateDirectory(Program.answerDir);
					}
					if (!File.Exists(Environment.GetEnvironmentVariable("USERPROFILE") + "\\SHCD.ini"))
					{
						FormReg formReg = new FormReg();
						if (formReg.ShowDialog() != DialogResult.OK)
						{
							return;
						}
					}
					else
					{
						string text2 = Environment.GetEnvironmentVariable("USERPROFILE") + "\\SHCD.ini";
						string text3 = "";
						bool unregistered = false;
						try
						{
							// Generate the user hash-code for the key checking.
							text3 = File.ReadAllText(Environment.GetEnvironmentVariable("USERPROFILE") + "\\SHCD.ini");
						}
						catch
						{
							text3 = "";
						}
						finally
						{
							byte[] bytes = Convert.FromBase64String(text3);
							text3 = Encoding.ASCII.GetString(bytes); // Finally here comes the pseudo-hash
							if (text3.Length < 34)
							{
								unregistered = true;
							}
							else
							{
								string text4 = text3.Substring(0, 18);
								string text5 = text3.Substring(18, 8);
								string a = text3.Substring(26, 8);
								unregistered = !Program.CheckListCode(text4);
								if (text5 != Program.doString(Program.getCpuId()) + Program.doString(Program.getBaseBoardId()) + Program.doString(Program.getBIOSId()) + Program.doString(Program.getPhysicalMediaId()))
								{
									unregistered = true;
								}
								int[] array = new int[8];
								for (int i = 0; i < text5.Length; i++)
								{
									array[i] = Convert.ToInt32(text5[i].ToString());
								}
								Array.Sort<int>(array);
								int num = 0;
								for (int i = 0; i < array.Length; i++)
								{
									num += array[i] * (int)Math.Pow(10.0, (double)(array.Length - 1 - i));
								}
								num = 100000000 - num;
								if (a != (Convert.ToInt64(text4.Substring(2)) % (long)num).ToString().PadLeft(8, '0'))
								{
									unregistered = true;
								}
							}
						}
						if (unregistered)
						{
							FormReg formReg = new FormReg();
							if (formReg.ShowDialog() != DialogResult.OK)
							{
								return;
							}
						}
					}
					string text6 = "";
					DriveInfo[] drives = DriveInfo.GetDrives();
					DriveInfo driveInfo = new DriveInfo("C"); // Ouch....
					DriveInfo[] array2 = drives;
					for (int j = 0; j < array2.Length; j++)
					{
						DriveInfo driveInfo2 = array2[j];
						if (driveInfo2.RootDirectory.FullName == Path.GetPathRoot(Program.dataDir))
						{
							driveInfo = driveInfo2;
							break;
						}
					}
					string volumeLabel = driveInfo.VolumeLabel;
					for (char c = 'Z'; c >= 'A'; c -= '\u0001')
					{
						bool flag3 = false;
						array2 = drives;
						for (int j = 0; j < array2.Length; j++)
						{
							DriveInfo driveInfo2 = array2[j];
							if (driveInfo2.Name.Contains(c.ToString()))
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							text6 = c.ToString();
							break;
						}
					}
					int num2 = 0;
					try
					{
						while (!Directory.Exists(text6 + ":\\"))
						{
							Thread.Sleep(500);
							Process process = new Process();
							process.StartInfo.FileName = "cmd.exe";
							process.StartInfo.LoadUserProfile = true;
							process.StartInfo.UseShellExecute = false;
							process.StartInfo.RedirectStandardInput = true;
							process.StartInfo.RedirectStandardOutput = true;
							process.StartInfo.CreateNoWindow = true;
							process.Start();
							process.StandardInput.WriteLine(string.Concat(new string[]
							{
								"subst.exe ",
								text6,
								": \"",
								Program.dataDir,
								"\""
							}));
							num2++;
							if (num2 == 10)
							{
								break;
							}
						}
						if (num2 != 10)
						{
							driveInfo.VolumeLabel = "考生目录";
							Program.dataDir = text6 + ":\\";
						}
						else
						{
							DirectoryInfo parent = Directory.GetParent(Program.dataDir);
							string destDirName = Path.Combine(parent.FullName, "考生目录");
							Directory.Move(Program.dataDir, destDirName);
							Program.dataDir = destDirName;
						}
						if (!Directory.Exists(Program.dataDir))
						{
							Directory.CreateDirectory(Program.dataDir);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
					byte[] array3 = new byte[4];
					array2 = drives;
					for (int j = 0; j < array2.Length; j++)
					{
						DriveInfo driveInfo2 = array2[j];
						byte[] expr_94C_cp_0 = array3;
						char expr_94C_cp_1 = (driveInfo2.Name.ToCharArray()[0] - 'A') / '\b';
						expr_94C_cp_0[(int)expr_94C_cp_1] = expr_94C_cp_0[(int)expr_94C_cp_1] + (byte)Math.Pow(2.0, (double)((driveInfo2.Name.ToCharArray()[0] - 'A') % '\b'));
					}
					RegistryKey registryKey2;
					try
					{
						registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
						if (registryKey2 == null)
						{
							Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
							registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
						}
						registryKey2.SetValue("NoDrives", array3, RegistryValueKind.Binary);
					}
					catch
					{
					}
					IntPtr intPtr = 0;
					Program.SendMessageTimeout(Program.HWND_BROADCAST, Program.WM_SETTINGCHANGE, IntPtr.Zero, IntPtr.Zero, Program.SendMessageTimeoutFlags.SMTO_BLOCK | Program.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 3000u, out intPtr);
					Application.Run(new FormSel());
					for (char c2 = text6[0]; c2 <= 'Z'; c2 += '\u0001')
					{
						int num3 = 0;
						while (Directory.Exists(c2.ToString() + ":\\"))
						{
							Thread.Sleep(500);
							Process process = new Process();
							process.StartInfo.FileName = "cmd.exe";
							process.StartInfo.UseShellExecute = false;
							process.StartInfo.RedirectStandardInput = true;
							process.StartInfo.RedirectStandardOutput = true;
							process.StartInfo.CreateNoWindow = true;
							process.Start();
							process.StandardInput.WriteLine("subst.exe  " + c2.ToString() + ": /D");
							num3++;
							if (num3 == 10)
							{
								break;
							}
						}
					}
					driveInfo.VolumeLabel = volumeLabel;
					registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
					if (registryKey2 == null)
					{
						Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", RegistryKeyPermissionCheck.ReadWriteSubTree);
						registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
					}
					RegistryKey arg_B77_0 = registryKey2;
					string arg_B77_1 = "NoDrives";
					byte[] value2 = new byte[4];
					arg_B77_0.SetValue(arg_B77_1, value2, RegistryValueKind.Binary);
					Program.SendMessageTimeout(Program.HWND_BROADCAST, Program.WM_SETTINGCHANGE, IntPtr.Zero, IntPtr.Zero, Program.SendMessageTimeoutFlags.SMTO_BLOCK | Program.SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 3000u, out intPtr);
				}
			}
		}
		internal static bool CheckListCode(string mylistcode)
		{
			bool result;
			if (mylistcode.Length != 18)
			{
				result = false;
			}
			else
			{
				if (!mylistcode.StartsWith("14"))
				{
					result = false;
				}
				else
				{
					int[] array = new int[18];
					for (int i = 0; i < mylistcode.Length; i++)
					{
						array[i] = Convert.ToInt32(mylistcode[i].ToString());
					}
					int num = Convert.ToInt32((double)array[2] * Math.Pow(17.0, (double)array[2]) % 10.0);
					if (array[3] == num % 10)
					{
						num = 0;
						for (int i = 2; i <= 4; i++)
						{
							num += Convert.ToInt32((double)array[i] * Math.Pow(17.0, (double)array[i]) % 10.0);
						}
						if (array[5] == num % 10)
						{
							num = 0;
							for (int i = 2; i <= 8; i++)
							{
								num += Convert.ToInt32((double)array[i] * Math.Pow(17.0, (double)array[i]) % 10.0);
							}
							if (array[9] == num % 10)
							{
								num = 0;
								for (int i = 2; i <= 16; i++)
								{
									num += Convert.ToInt32((double)array[i] * Math.Pow(17.0, (double)array[i]) % 10.0);
								}
								result = (array[17] == num % 10);
							}
							else
							{
								result = false;
							}
						}
						else
						{
							result = false;
						}
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}
		internal static string getCpuId() // Well..
		{
			string result;
			try
			{
				ManagementClass managementClass = new ManagementClass("win32_processor");
				ManagementObjectCollection instances = managementClass.GetInstances();
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						result = managementObject.Properties["processorid"].Value.ToString();
						return result;
					}
				}
				result = Program.defaultID;
			}
			catch
			{
				result = Program.defaultID;
			}
			return result;
		}
		internal static string getBaseBoardId()
		{
			string result;
			try
			{
				ManagementClass managementClass = new ManagementClass("Win32_BaseBoard");
				ManagementObjectCollection instances = managementClass.GetInstances();
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						result = managementObject.Properties["SerialNumber"].Value.ToString();
						return result;
					}
				}
				result = Program.defaultID;
			}
			catch
			{
				result = Program.defaultID;
			}
			return result;
		}
		internal static string getPhysicalMediaId()
		{
			string result;
			try
			{
				ManagementClass managementClass = new ManagementClass("Win32_PhysicalMedia");
				ManagementObjectCollection instances = managementClass.GetInstances();
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						result = managementObject.Properties["SerialNumber"].Value.ToString();
						return result;
					}
				}
				result = Program.defaultID;
			}
			catch
			{
				result = Program.defaultID;
			}
			return result;
		}
		internal static string getBIOSId()
		{
			string result;
			try
			{
				ManagementClass managementClass = new ManagementClass("Win32_BIOS");
				ManagementObjectCollection instances = managementClass.GetInstances();
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						result = managementObject.Properties["SerialNumber"].Value.ToString();
						return result;
					}
				}
				result = Program.defaultID;
			}
			catch
			{
				result = Program.defaultID;
			}
			return result;
		}
		internal static string doString(string str)
		{
			string text = "";
			char[] array = str.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
				{
					text += c.ToString();
				}
			}
			text = text.ToUpper();
			int num = 0;
			for (int j = 0; j < text.Length; j++)
			{
				int num2;
				if (text[j] > 'A')
				{
					num2 = (int)(text[j] - 'A' + '\n');
				}
				else
				{
					num2 = (int)(text[j] - '0');
				}
				num += num2 * Program.pow(36, j);
				num %= 100;
			}
			return num.ToString().PadLeft(2, '0');
		}
		internal static int pow(int a, int e)
		{
			int result;
			if (e > 1)
			{
				result = Program.pow(a, e - 1) % 100;
			}
			else
			{
				if (e == 1)
				{
					result = a % 100;
				}
				else
				{
					result = 1;
				}
			}
			return result;
		}
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, Program.SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
		}
		private static void UIThreadException(object sender, ThreadExceptionEventArgs e)
		{
		}
	}
}
