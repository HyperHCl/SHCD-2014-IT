using Qisi.General.Controls.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class FlatMessageBox : Form
	{
		public enum KeysIcon
		{
			Error,
			Explorer,
			Find,
			Information,
			Mail,
			Media,
			Print,
			Question,
			RecycleBinEmpty,
			RecycleBinFull,
			Stop,
			User,
			Warning
		}
		public enum KeysButtons
		{
			AbortRetryIgnore,
			OK,
			OKCancel,
			RetryCancel,
			YesNo,
			YesNoCancel
		}
		private static FlatMessageBox newMessageBox;
		private static Label frmMessage;
		private static PictureBox pIcon;
		private static FlowLayoutPanel flpButtons;
		private static Image frmIcon;
		private static Button btnOK;
		private static Button btnAbort;
		private static Button btnRetry;
		private static Button btnIgnore;
		private static Button btnCancel;
		private static Button btnYes;
		private static Button btnNo;
		private static DialogResult KeysReturnButton;
		private IContainer components;
		private static void BuildMessageBox(string title)
		{
			FlatMessageBox.newMessageBox = new FlatMessageBox();
			FlatMessageBox.newMessageBox.DoubleBuffered = true;
			FlatMessageBox.newMessageBox.Text = title;
			FlatMessageBox.newMessageBox.Size = new Size(400, 200);
			FlatMessageBox.newMessageBox.StartPosition = FormStartPosition.CenterScreen;
			FlatMessageBox.newMessageBox.FormBorderStyle = FormBorderStyle.FixedDialog;
			FlatMessageBox.newMessageBox.Visible = false;
			FlatMessageBox.newMessageBox.BackColor = Color.White;
			FlatMessageBox.newMessageBox.MaximizeBox = false;
			TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
			tableLayoutPanel.RowCount = 3;
			tableLayoutPanel.ColumnCount = 0;
			tableLayoutPanel.Dock = DockStyle.Fill;
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));
			tableLayoutPanel.BackColor = Color.Transparent;
			tableLayoutPanel.Padding = new Padding(2, 5, 2, 2);
			FlatMessageBox.frmMessage = new Label();
			FlatMessageBox.frmMessage.Dock = DockStyle.Fill;
			FlatMessageBox.frmMessage.BackColor = Color.White;
			FlatMessageBox.frmMessage.Font = new Font("Cambria", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.frmMessage.Text = "";
			FlatMessageBox.pIcon = new PictureBox();
			FlatMessageBox.pIcon.SizeMode = PictureBoxSizeMode.StretchImage;
			FlatMessageBox.flpButtons = new FlowLayoutPanel();
			FlatMessageBox.flpButtons.FlowDirection = FlowDirection.RightToLeft;
			FlatMessageBox.flpButtons.Padding = new Padding(0, 5, 5, 0);
			FlatMessageBox.flpButtons.Dock = DockStyle.Fill;
			FlatMessageBox.flpButtons.BackColor = Color.Transparent;
			TableLayoutPanel tableLayoutPanel2 = new TableLayoutPanel();
			tableLayoutPanel2.BackColor = Color.White;
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.ColumnCount = 2;
			tableLayoutPanel2.RowCount = 0;
			tableLayoutPanel2.Padding = new Padding(4, 5, 4, 4);
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 64f));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			tableLayoutPanel2.Controls.Add(FlatMessageBox.pIcon);
			tableLayoutPanel2.Controls.Add(FlatMessageBox.frmMessage);
			tableLayoutPanel.Controls.Add(tableLayoutPanel2);
			tableLayoutPanel.Controls.Add(FlatMessageBox.flpButtons);
			FlatMessageBox.newMessageBox.Controls.Add(tableLayoutPanel);
		}
		public static DialogResult Show(string Message)
		{
			FlatMessageBox.BuildMessageBox("");
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ShowOKButton();
			FlatMessageBox.newMessageBox.ShowDialog();
			return FlatMessageBox.KeysReturnButton;
		}
		public static DialogResult Show(IWin32Window win32, string Message)
		{
			FlatMessageBox.BuildMessageBox("");
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ShowOKButton();
			FlatMessageBox.newMessageBox.ShowDialog(win32);
			return FlatMessageBox.KeysReturnButton;
		}
		public static DialogResult Show(string Message, string Title)
		{
			FlatMessageBox.BuildMessageBox(Title);
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ShowOKButton();
			FlatMessageBox.newMessageBox.ShowDialog();
			return FlatMessageBox.KeysReturnButton;
		}
		public static DialogResult Show(IWin32Window win32, string Message, string Title)
		{
			FlatMessageBox.BuildMessageBox(Title);
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ShowOKButton();
			FlatMessageBox.newMessageBox.ShowDialog(win32);
			return FlatMessageBox.KeysReturnButton;
		}
		public static DialogResult Show(string Message, string Title, FlatMessageBox.KeysButtons MButtons)
		{
			FlatMessageBox.BuildMessageBox(Title);
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ButtonStatements(MButtons);
			FlatMessageBox.newMessageBox.ShowDialog();
			return FlatMessageBox.KeysReturnButton;
		}
		public static DialogResult Show(IWin32Window win32, string Message, string Title, FlatMessageBox.KeysButtons MButtons)
		{
			FlatMessageBox.BuildMessageBox(Title);
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ButtonStatements(MButtons);
			FlatMessageBox.newMessageBox.ShowDialog(win32);
			return FlatMessageBox.KeysReturnButton;
		}
		public static DialogResult Show(string Message, string Title, FlatMessageBox.KeysButtons MButtons, FlatMessageBox.KeysIcon MIcon)
		{
			FlatMessageBox.BuildMessageBox(Title);
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ButtonStatements(MButtons);
			FlatMessageBox.IconStatements(MIcon);
			FlatMessageBox.pIcon.Image = FlatMessageBox.frmIcon;
			FlatMessageBox.newMessageBox.ShowDialog();
			return FlatMessageBox.KeysReturnButton;
		}
		public static DialogResult Show(IWin32Window win32, string Message, string Title, FlatMessageBox.KeysButtons MButtons, FlatMessageBox.KeysIcon MIcon)
		{
			FlatMessageBox.BuildMessageBox(Title);
			FlatMessageBox.frmMessage.Text = Message;
			FlatMessageBox.ButtonStatements(MButtons);
			FlatMessageBox.IconStatements(MIcon);
			FlatMessageBox.pIcon.Image = FlatMessageBox.frmIcon;
			FlatMessageBox.newMessageBox.ShowDialog(win32);
			return FlatMessageBox.KeysReturnButton;
		}
		private static void btnOK_Click(object sender, EventArgs e)
		{
			FlatMessageBox.KeysReturnButton = DialogResult.OK;
			FlatMessageBox.newMessageBox.Close();
		}
		private static void btnAbort_Click(object sender, EventArgs e)
		{
			FlatMessageBox.KeysReturnButton = DialogResult.Abort;
			FlatMessageBox.newMessageBox.Close();
		}
		private static void btnRetry_Click(object sender, EventArgs e)
		{
			FlatMessageBox.KeysReturnButton = DialogResult.Retry;
			FlatMessageBox.newMessageBox.Close();
		}
		private static void btnIgnore_Click(object sender, EventArgs e)
		{
			FlatMessageBox.KeysReturnButton = DialogResult.Ignore;
			FlatMessageBox.newMessageBox.Close();
		}
		private static void btnCancel_Click(object sender, EventArgs e)
		{
			FlatMessageBox.KeysReturnButton = DialogResult.Cancel;
			FlatMessageBox.newMessageBox.Close();
		}
		private static void btnYes_Click(object sender, EventArgs e)
		{
			FlatMessageBox.KeysReturnButton = DialogResult.Yes;
			FlatMessageBox.newMessageBox.Close();
		}
		private static void btnNo_Click(object sender, EventArgs e)
		{
			FlatMessageBox.KeysReturnButton = DialogResult.No;
			FlatMessageBox.newMessageBox.Close();
		}
		private static void ShowOKButton()
		{
			FlatMessageBox.btnOK = new Button();
			FlatMessageBox.btnOK.Cursor = Cursors.Hand;
			FlatMessageBox.btnOK.Text = "确定";
			FlatMessageBox.btnOK.FlatStyle = FlatStyle.Flat;
			FlatMessageBox.btnOK.Size = new Size(80, 32);
			FlatMessageBox.btnOK.BackColor = Color.FromArgb(255, 255, 255);
			FlatMessageBox.btnOK.Font = new Font("Tahoma", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.btnOK.Click += new EventHandler(FlatMessageBox.btnOK_Click);
			FlatMessageBox.flpButtons.Controls.Add(FlatMessageBox.btnOK);
		}
		private static void ShowAbortButton()
		{
			FlatMessageBox.btnAbort = new Button();
			FlatMessageBox.btnAbort.Text = "终止";
			FlatMessageBox.btnAbort.Cursor = Cursors.Hand;
			FlatMessageBox.btnAbort.FlatStyle = FlatStyle.Flat;
			FlatMessageBox.btnAbort.Size = new Size(80, 32);
			FlatMessageBox.btnAbort.BackColor = Color.FromArgb(255, 255, 255);
			FlatMessageBox.btnAbort.Font = new Font("Tahoma", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.btnAbort.Click += new EventHandler(FlatMessageBox.btnAbort_Click);
			FlatMessageBox.flpButtons.Controls.Add(FlatMessageBox.btnAbort);
		}
		private static void ShowRetryButton()
		{
			FlatMessageBox.btnRetry = new Button();
			FlatMessageBox.btnRetry.Cursor = Cursors.Hand;
			FlatMessageBox.btnRetry.Text = "重试";
			FlatMessageBox.btnRetry.FlatStyle = FlatStyle.Flat;
			FlatMessageBox.btnRetry.Size = new Size(80, 32);
			FlatMessageBox.btnRetry.BackColor = Color.FromArgb(255, 255, 255);
			FlatMessageBox.btnRetry.Font = new Font("Tahoma", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.btnRetry.Click += new EventHandler(FlatMessageBox.btnRetry_Click);
			FlatMessageBox.flpButtons.Controls.Add(FlatMessageBox.btnRetry);
		}
		private static void ShowIgnoreButton()
		{
			FlatMessageBox.btnIgnore = new Button();
			FlatMessageBox.btnIgnore.Cursor = Cursors.Hand;
			FlatMessageBox.btnIgnore.Text = "忽略";
			FlatMessageBox.btnIgnore.FlatStyle = FlatStyle.Flat;
			FlatMessageBox.btnIgnore.Size = new Size(80, 32);
			FlatMessageBox.btnIgnore.BackColor = Color.FromArgb(255, 255, 255);
			FlatMessageBox.btnIgnore.Font = new Font("Tahoma", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.btnIgnore.Click += new EventHandler(FlatMessageBox.btnIgnore_Click);
			FlatMessageBox.flpButtons.Controls.Add(FlatMessageBox.btnIgnore);
		}
		private static void ShowCancelButton()
		{
			FlatMessageBox.btnCancel = new Button();
			FlatMessageBox.btnCancel.Cursor = Cursors.Hand;
			FlatMessageBox.btnCancel.Text = "取消";
			FlatMessageBox.btnCancel.FlatStyle = FlatStyle.Flat;
			FlatMessageBox.btnCancel.Size = new Size(80, 32);
			FlatMessageBox.btnCancel.BackColor = Color.FromArgb(255, 255, 255);
			FlatMessageBox.btnCancel.Font = new Font("Tahoma", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.btnCancel.Click += new EventHandler(FlatMessageBox.btnCancel_Click);
			FlatMessageBox.flpButtons.Controls.Add(FlatMessageBox.btnCancel);
		}
		private static void ShowYesButton()
		{
			FlatMessageBox.btnYes = new Button();
			FlatMessageBox.btnYes.Cursor = Cursors.Hand;
			FlatMessageBox.btnYes.Text = "是";
			FlatMessageBox.btnYes.FlatStyle = FlatStyle.Flat;
			FlatMessageBox.btnYes.Size = new Size(80, 32);
			FlatMessageBox.btnYes.BackColor = Color.FromArgb(255, 255, 255);
			FlatMessageBox.btnYes.Font = new Font("Tahoma", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.btnYes.Click += new EventHandler(FlatMessageBox.btnYes_Click);
			FlatMessageBox.flpButtons.Controls.Add(FlatMessageBox.btnYes);
		}
		private static void ShowNoButton()
		{
			FlatMessageBox.btnNo = new Button();
			FlatMessageBox.btnNo.Cursor = Cursors.Hand;
			FlatMessageBox.btnNo.Text = "否";
			FlatMessageBox.btnNo.FlatStyle = FlatStyle.Flat;
			FlatMessageBox.btnNo.Size = new Size(80, 32);
			FlatMessageBox.btnNo.BackColor = Color.FromArgb(255, 255, 255);
			FlatMessageBox.btnNo.Font = new Font("Tahoma", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
			FlatMessageBox.btnNo.Click += new EventHandler(FlatMessageBox.btnNo_Click);
			FlatMessageBox.flpButtons.Controls.Add(FlatMessageBox.btnNo);
		}
		private static void ButtonStatements(FlatMessageBox.KeysButtons MButtons)
		{
			if (MButtons == FlatMessageBox.KeysButtons.AbortRetryIgnore)
			{
				FlatMessageBox.ShowIgnoreButton();
				FlatMessageBox.ShowRetryButton();
				FlatMessageBox.ShowAbortButton();
			}
			if (MButtons == FlatMessageBox.KeysButtons.OK)
			{
				FlatMessageBox.ShowOKButton();
			}
			if (MButtons == FlatMessageBox.KeysButtons.OKCancel)
			{
				FlatMessageBox.ShowCancelButton();
				FlatMessageBox.ShowOKButton();
			}
			if (MButtons == FlatMessageBox.KeysButtons.RetryCancel)
			{
				FlatMessageBox.ShowCancelButton();
				FlatMessageBox.ShowRetryButton();
			}
			if (MButtons == FlatMessageBox.KeysButtons.YesNo)
			{
				FlatMessageBox.ShowNoButton();
				FlatMessageBox.ShowYesButton();
			}
			if (MButtons == FlatMessageBox.KeysButtons.YesNoCancel)
			{
				FlatMessageBox.ShowCancelButton();
				FlatMessageBox.ShowNoButton();
				FlatMessageBox.ShowYesButton();
			}
		}
		private static void IconStatements(FlatMessageBox.KeysIcon MIcon)
		{
			if (MIcon == FlatMessageBox.KeysIcon.Error)
			{
				FlatMessageBox.frmIcon = Resources.Error;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Explorer)
			{
				FlatMessageBox.frmIcon = Resources.Explorer;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Find)
			{
				FlatMessageBox.frmIcon = Resources.Find;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Information)
			{
				FlatMessageBox.frmIcon = Resources.Information;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Mail)
			{
				FlatMessageBox.frmIcon = Resources.Mail;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Media)
			{
				FlatMessageBox.frmIcon = Resources.Media;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Print)
			{
				FlatMessageBox.frmIcon = Resources.Print;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Question)
			{
				FlatMessageBox.frmIcon = Resources.Question;
			}
			if (MIcon == FlatMessageBox.KeysIcon.RecycleBinEmpty)
			{
				FlatMessageBox.frmIcon = Resources.ReEmpty;
			}
			if (MIcon == FlatMessageBox.KeysIcon.RecycleBinFull)
			{
				FlatMessageBox.frmIcon = Resources.ReFull;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Stop)
			{
				FlatMessageBox.frmIcon = Resources.Stop;
			}
			if (MIcon == FlatMessageBox.KeysIcon.User)
			{
				FlatMessageBox.frmIcon = Resources.User;
			}
			if (MIcon == FlatMessageBox.KeysIcon.Warning)
			{
				FlatMessageBox.frmIcon = Resources.Warning;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			base.AutoScaleMode = AutoScaleMode.Font;
		}
	}
}
