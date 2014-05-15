using System;
using System.Windows.Forms;
namespace Update
{
	internal static class Program
	{
		internal static string URL;
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (args.Length > 0)
			{
				Program.URL = args[0];
				Application.Run(new Form1());
			}
			else
			{
				MessageBox.Show("获取更新失败！");
			}
		}
	}
}
