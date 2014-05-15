using Microsoft.Win32;
using Qisi.Editor.Controls;
using Qisi.Editor.Properties;
using Qisi.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
namespace Qisi.Editor.Documents.Elements
{
	internal class OperationInfo : Element
	{
		internal enum OperationType
		{
			Flash,
			PhotoShop,
			VisualBasic,
			Access
		}
		private delegate void pexitcallback();
		public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
		private const int WM_CLOSE = 16;
		private const int WM_DESTORY = 2;
		private const int WM_QUIT = 18;
		private OperationInfo.OperationType operationType;
		private Image buttonImage;
		private List<MemoryFile> backupFileList;
		private List<MemoryFile> fileList;
		private static string doButtonText1 = "点击按钮答题(本题未做)";
		private static string doButtonText2 = "点击按钮答题(本题已做)";
		private static string redoButtonText = "点此重做";
		private static SizeF imageSize = new SizeF(50f, 50f);
		private Region doButtonRegion;
		private Region imageRegion;
		private Region redoButtonRegion;
		private PointF doButtonLocation;
		private PointF redoButtonLocation;
		private SizeF doButtonSize;
		private SizeF redoButtonSize;
		private string dataPath;
		private FormFlow formFlow;
		private string rtfpath;
		private string stdAnswer;
		private string gifpath;
		private Process openFileProcess;
		private List<string> processList;
		private bool formDisposed;
		private bool processExited;
		private Hashtable processWnd = null;
		internal OperationInfo.OperationType Operation
		{
			get
			{
				return this.operationType;
			}
			set
			{
				if (this.operationType != value)
				{
					this.operationType = value;
					if (this.operationType == OperationInfo.OperationType.Flash)
					{
						this.buttonImage = Resources.flash;
					}
					else
					{
						if (this.operationType == OperationInfo.OperationType.PhotoShop)
						{
							this.buttonImage = Resources.photoshop;
						}
						else
						{
							if (this.operationType == OperationInfo.OperationType.Access)
							{
								this.buttonImage = Resources.access;
							}
							else
							{
								if (this.operationType == OperationInfo.OperationType.VisualBasic)
								{
									this.buttonImage = Resources.vb;
								}
							}
						}
					}
				}
			}
		}
		internal bool Opened
		{
			get;
			set;
		}
		internal PointF DoButtonLocation
		{
			get
			{
				return this.doButtonLocation;
			}
		}
		internal PointF ReDoButtonLocation
		{
			get
			{
				return this.redoButtonLocation;
			}
		}
		internal Region ImageRegion
		{
			get
			{
				return this.imageRegion;
			}
		}
		internal Region DoButtonRegion
		{
			get
			{
				return this.doButtonRegion;
			}
		}
		internal Region ReDoButtonRegion
		{
			get
			{
				return this.redoButtonRegion;
			}
		}
		internal override PointF Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				if (base.Location != value)
				{
					base.Location = value;
					this.imageRegion = new Region(new RectangleF(value, OperationInfo.imageSize));
					this.doButtonLocation = new PointF(value.X, value.Y + OperationInfo.imageSize.Height);
					this.redoButtonLocation = new PointF(value.X + 20f + this.doButtonSize.Width, value.Y + OperationInfo.imageSize.Height);
					this.doButtonRegion = new Region(new RectangleF(this.doButtonLocation, this.doButtonSize));
					this.redoButtonRegion = new Region(new RectangleF(this.redoButtonLocation, this.redoButtonSize));
					this.Size = new SizeF(Math.Max(OperationInfo.imageSize.Width, this.doButtonSize.Width + this.redoButtonSize.Width + 20f), OperationInfo.imageSize.Height + Math.Max(this.doButtonSize.Height, this.redoButtonSize.Height));
				}
			}
		}
		internal string OperationID
		{
			get;
			set;
		}
		internal bool Review
		{
			get;
			set;
		}
		internal void LoadAnswer(List<byte[]> filebyets, List<string> filenames)
		{
			this.fileList = new List<MemoryFile>();
			this.backupFileList = new List<MemoryFile>();
			for (int i = 0; i < Math.Min(filebyets.Count, filenames.Count); i++)
			{
				this.fileList.Add(new MemoryFile(filenames[i], filebyets[i]));
				this.backupFileList.Add(new MemoryFile(filenames[i], filebyets[i]));
			}
		}
		internal override void Draw(Graphics g)
		{
			if (this.buttonImage != null)
			{
				base.Draw(g);
				g.DrawImage(this.buttonImage, new RectangleF(base.OutLocation, OperationInfo.imageSize));
				if (!this.Review)
				{
					StringFormat genericTypographic = StringFormat.GenericTypographic;
					genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
					if (this.Opened)
					{
						g.DrawString(OperationInfo.doButtonText2, new Font(base.Font.Name, base.Font.Size, base.Font.Style | FontStyle.Bold, base.Font.Unit), SystemBrushes.ControlText, this.DoButtonLocation, genericTypographic);
						g.DrawString(OperationInfo.redoButtonText, new Font(base.Font.Name, base.Font.Size, base.Font.Style | FontStyle.Bold, base.Font.Unit), SystemBrushes.ControlText, this.ReDoButtonLocation, genericTypographic);
					}
					else
					{
						g.DrawString(OperationInfo.doButtonText1, new Font(base.Font.Name, base.Font.Size, base.Font.Style | FontStyle.Bold, base.Font.Unit), SystemBrushes.ControlText, this.DoButtonLocation, genericTypographic);
						g.DrawString(OperationInfo.redoButtonText, new Font(base.Font.Name, base.Font.Size, base.Font.Style, base.Font.Unit), SystemBrushes.ControlText, this.ReDoButtonLocation, genericTypographic);
					}
					genericTypographic.Dispose();
				}
			}
		}
		internal override void DrawHighLight(Graphics g)
		{
			if (this.buttonImage != null)
			{
				base.Draw(g);
				g.DrawImage(this.buttonImage, new RectangleF(base.OutLocation, OperationInfo.imageSize));
				if (!this.Review)
				{
					StringFormat genericTypographic = StringFormat.GenericTypographic;
					genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
					if (this.Opened)
					{
						g.DrawString(OperationInfo.doButtonText2, base.Font, SystemBrushes.HighlightText, this.DoButtonLocation, genericTypographic);
						g.DrawString(OperationInfo.redoButtonText, base.Font, SystemBrushes.HighlightText, this.ReDoButtonLocation, genericTypographic);
					}
					else
					{
						g.DrawString(OperationInfo.doButtonText1, base.Font, SystemBrushes.HighlightText, this.DoButtonLocation, genericTypographic);
						g.DrawString(OperationInfo.redoButtonText, base.Font, SystemBrushes.HighlightText, this.ReDoButtonLocation, genericTypographic);
					}
					genericTypographic.Dispose();
				}
			}
		}
		internal void LayOut(Graphics g)
		{
			this.imageRegion = new Region(new RectangleF(this.Location, OperationInfo.imageSize));
			this.doButtonLocation = new PointF(this.Location.X, this.Location.Y + OperationInfo.imageSize.Height);
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			this.doButtonSize = g.MeasureString(OperationInfo.doButtonText1, base.Font, 0, genericTypographic);
			this.redoButtonLocation = new PointF(this.Location.X + 20f + this.doButtonSize.Width, this.Location.Y + OperationInfo.imageSize.Height);
			this.doButtonRegion = new Region(new RectangleF(this.doButtonLocation, this.doButtonSize));
			this.redoButtonSize = g.MeasureString(OperationInfo.redoButtonText, base.Font, 0, genericTypographic);
			this.redoButtonRegion = new Region(new RectangleF(this.redoButtonLocation, this.redoButtonSize));
			if (this.Review)
			{
				this.Size = OperationInfo.imageSize;
			}
			else
			{
				this.Size = new SizeF(Math.Max(OperationInfo.imageSize.Width, this.doButtonSize.Width + this.redoButtonSize.Width + 20f), OperationInfo.imageSize.Height + Math.Max(this.doButtonSize.Height, this.redoButtonSize.Height));
			}
			genericTypographic.Dispose();
		}
		internal void Do(Image stuImg, string stuInfo, string path, int examLeftTime, int tipTime, string stdanswer)
		{
			if (this.dataPath == "")
			{
				this.dataPath = path;
			}
			if (this.stdAnswer == "")
			{
				this.stdAnswer = stdanswer;
			}
			Qisi.General.CommonMethods.ClearDirectory(this.dataPath);
			string openFile = "";
			foreach (MemoryFile current in this.fileList)
			{
				string text = Path.Combine(path, current.FileName);
				string extension = Path.GetExtension(text);
				try
				{
					File.WriteAllBytes(text, current.FileByte);
				}
				catch
				{
				}
				if (this.operationType == OperationInfo.OperationType.Access && extension == ".mdb")
				{
					openFile = text;
				}
				else
				{
					if (this.operationType == OperationInfo.OperationType.Flash && extension == ".fla")
					{
						openFile = text;
					}
					else
					{
						if (this.operationType == OperationInfo.OperationType.PhotoShop && extension == ".psd")
						{
							openFile = text;
						}
						else
						{
							if (this.operationType == OperationInfo.OperationType.VisualBasic && extension == ".vbp")
							{
								openFile = text;
							}
						}
					}
				}
			}
			this.Open(stuImg, stuInfo, examLeftTime, tipTime, openFile);
		}
		private void Open(Image stuImg, string stuInfo, int examLeftTime, int tipTime, string openFile)
		{
			this.formDisposed = true;
			this.processExited = true;
			this.processList = new List<string>();
			Process[] processes = Process.GetProcesses();
			Process[] array = processes;
			for (int i = 0; i < array.Length; i++)
			{
				Process process = array[i];
				this.processList.Add(process.ProcessName);
			}
			if (tipTime < 0)
			{
				this.formFlow = new FormFlow(stuImg, stuInfo, this.rtfpath, this.dataPath, examLeftTime, tipTime, this.stdAnswer, this.gifpath, 1);
			}
			else
			{
				if (tipTime == 0)
				{
					this.formFlow = new FormFlow(stuImg, stuInfo, this.rtfpath, this.dataPath, examLeftTime, tipTime, this.stdAnswer, this.gifpath, 0);
				}
				else
				{
					this.formFlow = new FormFlow(stuImg, stuInfo, this.rtfpath, this.dataPath, examLeftTime, tipTime, this.stdAnswer, this.gifpath, -1);
				}
			}
			this.formFlow.Disposed += new EventHandler(this.formFlow_Disposed);
			Process process2 = new Process();
			process2.StartInfo.FileName = "cmd.exe";
			process2.StartInfo.UseShellExecute = false;
			process2.StartInfo.RedirectStandardInput = true;
			process2.StartInfo.RedirectStandardOutput = true;
			process2.StartInfo.CreateNoWindow = true;
			process2.Start();
			process2.StandardInput.WriteLine("start  " + this.dataPath);
			bool flag = false;
			string[] array2 = new string[0];
			string[] array3 = new string[0];
			string[] array4 = new string[0];
			string str = "";
			if (this.operationType == OperationInfo.OperationType.Access)
			{
				array2 = new string[]
				{
					"SOFTWARE\\Microsoft\\Office",
					"SOFTWARE\\Microsoft\\Office",
					"SOFTWARE\\Microsoft\\Office",
					"SOFTWARE\\Microsoft\\Office",
					"SOFTWARE\\Microsoft\\Office",
					"SOFTWARE\\Microsoft\\Office",
					"SOFTWARE\\Microsoft\\Office"
				};
				array3 = new string[]
				{
					"15.0\\Access\\InstallRoot",
					"14.0\\Access\\InstallRoot",
					"13.0\\Access\\InstallRoot",
					"12.0\\Access\\InstallRoot",
					"11.0\\Access\\InstallRoot",
					"10.0\\Access\\InstallRoot",
					"9.0\\Access\\InstallRoot"
				};
				array4 = new string[]
				{
					"Path",
					"Path",
					"Path",
					"Path",
					"Path",
					"Path",
					"Path"
				};
				str = "msaccess.exe";
			}
			else
			{
				if (this.operationType == OperationInfo.OperationType.Flash)
				{
					array2 = new string[]
					{
						"SOFTWARE\\Macromedia\\Flash",
						"SOFTWARE\\Macromedia\\Flash",
						"SOFTWARE\\Adobe\\Flash",
						"SOFTWARE\\Adobe\\Flash",
						"SOFTWARE\\Adobe\\Flash",
						"SOFTWARE\\Adobe\\Flash",
						"SOFTWARE\\Adobe\\Flash",
						"SOFTWARE\\Macromedia\\Flash"
					};
					array3 = new string[]
					{
						"7\\Installation",
						"8\\Installation",
						"9.0\\Installation",
						"10.0\\Installation",
						"11.0\\Installation",
						"11.5\\Installation",
						"12\\Installation",
						"6\\Installation"
					};
					array4 = new string[]
					{
						"InstallPath",
						"InstallPath",
						"InstallPath",
						"InstallPath",
						"InstallPath",
						"InstallPath",
						"InstallPath",
						"InstallPath"
					};
					str = "flash.exe";
				}
				else
				{
					if (this.operationType == OperationInfo.OperationType.PhotoShop)
					{
						array2 = new string[]
						{
							"SOFTWARE\\Adobe\\Photoshop",
							"SOFTWARE\\Adobe\\Photoshop",
							"SOFTWARE\\Adobe\\Photoshop",
							"SOFTWARE\\Adobe\\Photoshop",
							"SOFTWARE\\Adobe\\Photoshop",
							"SOFTWARE\\Adobe\\Photoshop",
							"SOFTWARE\\Adobe\\Photoshop"
						};
						array3 = new string[]
						{
							"8.0",
							"9.0",
							"10.0",
							"11.0",
							"12.0",
							"55.0",
							"60.0"
						};
						array4 = new string[]
						{
							"ApplicationPath",
							"ApplicationPath",
							"ApplicationPath",
							"ApplicationPath",
							"ApplicationPath",
							"ApplicationPath",
							"ApplicationPath"
						};
						str = "photoshop.exe";
					}
					else
					{
						if (this.operationType == OperationInfo.OperationType.VisualBasic)
						{
							array2 = new string[]
							{
								"SOFTWARE\\Microsoft\\VisualStudio"
							};
							array3 = new string[]
							{
								"6.0\\Setup\\Microsoft Visual Basic"
							};
							array4 = new string[]
							{
								"ProductDir"
							};
							str = "VB6.exe";
						}
					}
				}
			}
			for (int j = 0; j < array2.Length; j++)
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(array2[j], false);
				if (registryKey != null)
				{
					List<string> list = OperationInfo.FindRegistryValue(registryKey, array3[j], array4[j]);
					foreach (string current in list)
					{
						if (File.Exists((current.EndsWith("\\") ? current : (current + "\\")) + str))
						{
							this.openFileProcess = new Process();
							this.openFileProcess.StartInfo.Arguments = "\"" + openFile + "\"";
							this.openFileProcess.StartInfo.FileName = (current.EndsWith("\\") ? current : (current + "\\")) + str;
							this.openFileProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
							flag = this.openFileProcess.Start();
							Thread.Sleep(200);
							if (flag && !this.openFileProcess.HasExited)
							{
								this.processExited = false;
								OperationInfo.SetWindowPos(this.openFileProcess.Handle, -2, 0, 0, 0, 0, 3);
								this.openFileProcess.EnableRaisingEvents = true;
								this.openFileProcess.Exited += new EventHandler(this.openFileProcess_Exited);
								break;
							}
						}
					}
					if (flag)
					{
						this.formDisposed = false;
						this.formFlow.hasProcess = true;
						this.formFlow.Show();
						OperationInfo.SetWindowPos(this.formFlow.Handle, -1, 0, 0, 0, 0, 3);
						return;
					}
				}
			}
			this.formFlow.Show();
		}
		private void openFileProcess_Exited(object sender, EventArgs e)
		{
			this.processExited = true;
			if (!this.formDisposed)
			{
				try
				{
					this.pexit();
				}
				catch
				{
				}
			}
			else
			{
				this.Finish();
				if (base.DocumentContainer != null)
				{
					base.DocumentContainer.OperateFinished(this);
				}
			}
		}
		private void pexit()
		{
			if (this.formFlow != null && !this.formFlow.IsDisposed)
			{
				if (this.formFlow.InvokeRequired)
				{
					OperationInfo.pexitcallback method = new OperationInfo.pexitcallback(this.pexit);
					try
					{
						this.formFlow.Invoke(method);
					}
					catch
					{
					}
				}
				else
				{
					if (this.formFlow != null && !this.formFlow.IsDisposed)
					{
						this.formFlow.Dispose();
						this.formFlow = null;
					}
				}
			}
		}
		private void formFlow_Disposed(object sender, EventArgs e)
		{
			this.formDisposed = true;
			if (!this.processExited)
			{
				try
				{
					this.openFileProcess.Kill();
					this.openFileProcess.WaitForExit(3000);
				}
				catch
				{
				}
			}
			else
			{
				this.Finish();
				if (base.DocumentContainer != null)
				{
					base.DocumentContainer.OperateFinished(this);
				}
			}
		}
		private void Finish()
		{
			if (this.processList != null)
			{
				Process[] processes = Process.GetProcesses();
				Process[] array = processes;
				for (int i = 0; i < array.Length; i++)
				{
					Process process = array[i];
					if (!this.processList.Contains(process.ProcessName))
					{
						try
						{
							if (process.Responding)
							{
								process.Kill();
								process.WaitForExit();
								Thread.Sleep(200);
								int num = 0;
								while (!process.HasExited && num < 5)
								{
									Thread.Sleep(200);
									num++;
								}
								Thread.Sleep(200);
							}
						}
						catch
						{
						}
					}
				}
			}
			IntPtr intPtr = OperationInfo.FindWindow("CabinetWClass", null);
			if (intPtr != IntPtr.Zero && OperationInfo.IsWindow(intPtr))
			{
				StringBuilder stringBuilder = new StringBuilder();
				OperationInfo.GetWindowText(intPtr, stringBuilder, 255);
				if (stringBuilder.ToString() == "考生目录")
				{
					OperationInfo.PostMessage(intPtr, 16, 0, 0);
				}
			}
			else
			{
				intPtr = IntPtr.Zero;
			}
			if (!this.Review)
			{
				this.fileList = new List<MemoryFile>();
				string[] files = Directory.GetFiles(this.dataPath);
				for (int i = 0; i < files.Length; i++)
				{
					string filepath = files[i];
					this.fileList.Add(new MemoryFile(filepath));
				}
				this.Opened = true;
			}
		}
		internal void ReDo()
		{
			this.fileList = new List<MemoryFile>();
			foreach (MemoryFile current in this.backupFileList)
			{
				this.fileList.Add(new MemoryFile(current.FileName, current.FileByte));
			}
		}
		private static List<string> FindRegistryValue(RegistryKey myReg, string subkey, string item)
		{
			List<string> list = new List<string>();
			if (subkey == "")
			{
				if (myReg.GetValue(item) != null)
				{
					list.Add(myReg.GetValue(item).ToString());
				}
			}
			RegistryKey registryKey = myReg.OpenSubKey(subkey);
			if (registryKey == null || subkey == "")
			{
				string[] subKeyNames = myReg.GetSubKeyNames();
				string[] array = subKeyNames;
				for (int i = 0; i < array.Length; i++)
				{
					string name = array[i];
					try
					{
						list.AddRange(OperationInfo.FindRegistryValue(myReg.OpenSubKey(name), subkey, item));
					}
					catch
					{
					}
				}
			}
			else
			{
				if (registryKey.GetValue(item) == null)
				{
					list.AddRange(OperationInfo.FindRegistryValue(registryKey, "", item));
				}
				else
				{
					list.Add(registryKey.GetValue(item).ToString());
				}
			}
			return list;
		}
		internal OperationInfo(OperationInfo.OperationType opType, Font font, string dirpath, string rtf, string gif) : base(font)
		{
			this.OperationID = "1";
			this.dataPath = "";
			this.formFlow = null;
			this.openFileProcess = null;
			this.processList = null;
			this.rtfpath = rtf;
			this.gifpath = gif;
			this.operationType = opType;
			FontFamily fontFamily = font.FontFamily;
			int cellAscent = fontFamily.GetCellAscent(font.Style);
			this.BaseLine = font.Size * (float)cellAscent / (float)fontFamily.GetEmHeight(font.Style);
			this.Sized = false;
			this.backupFileList = new List<MemoryFile>();
			this.fileList = new List<MemoryFile>();
			string[] files = Directory.GetFiles(dirpath);
			for (int i = 0; i < files.Length; i++)
			{
				string filepath = files[i];
				this.backupFileList.Add(new MemoryFile(filepath));
				this.fileList.Add(new MemoryFile(filepath));
			}
			if (this.operationType == OperationInfo.OperationType.Flash)
			{
				this.buttonImage = Resources.flash;
			}
			else
			{
				if (this.operationType == OperationInfo.OperationType.PhotoShop)
				{
					this.buttonImage = Resources.photoshop;
				}
				else
				{
					if (this.operationType == OperationInfo.OperationType.Access)
					{
						this.buttonImage = Resources.access;
					}
					else
					{
						if (this.operationType == OperationInfo.OperationType.VisualBasic)
						{
							this.buttonImage = Resources.vb;
						}
					}
				}
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.buttonImage != null)
				{
					this.buttonImage.Dispose();
				}
				if (this.doButtonRegion != null)
				{
					this.doButtonRegion.Dispose();
				}
				if (this.imageRegion != null)
				{
					this.imageRegion.Dispose();
				}
				if (this.redoButtonRegion != null)
				{
					this.redoButtonRegion.Dispose();
				}
			}
			if (this.buttonImage != null)
			{
				this.buttonImage = null;
			}
			if (this.doButtonRegion != null)
			{
				this.doButtonRegion = null;
			}
			if (this.imageRegion != null)
			{
				this.imageRegion = null;
			}
			if (this.redoButtonRegion != null)
			{
				this.redoButtonRegion = null;
			}
			base.Dispose(disposing);
		}
		~OperationInfo()
		{
			this.Dispose(false);
		}
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool EnumWindows(OperationInfo.WNDENUMPROC lpEnumFunc, uint lParam);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetParent(IntPtr hWnd);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
		[DllImport("user32.dll")]
		public static extern bool IsWindow(IntPtr hWnd);
		[DllImport("kernel32.dll")]
		public static extern void SetLastError(uint dwErrCode);
		[DllImport("User32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("User32.dll")]
		private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
	}
}
