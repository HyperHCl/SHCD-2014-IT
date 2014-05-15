using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.Editor.Controls
{
	public class TextBox : UserControl
	{
		private IContainer components = null;
		private TextBar textBar1;
		private FlowLayoutPanel flowLayoutPanel1;
		private SuperBox superBox1;
		public TextBox()
		{
			this.InitializeComponent();
			this.textBar1.ColorChanged += new TextBar.ColorChangedEventHandler(this.textBar1_ColorChanged);
			this.textBar1.FontChanged += new TextBar.FontChangedEventHandler(this.textBar1_FontChanged);
			this.textBar1.InsertTable += new TextBar.InsertTableEventHandler(this.textBar1_InsertTable);
			this.textBar1.InsertImage += new TextBar.InsertImageEventHandler(this.textBar1_InsertImage);
		}
		private void textBar1_ColorChanged(object sender, ColorEventArgs e)
		{
			this.superBox1.ForeColor = e.Color;
		}
		private void textBar1_FontChanged(object sender, FontEventArgs e)
		{
			this.superBox1.setFont(e.Font);
			this.superBox1.Focus();
		}
		private void textBar1_InsertTable(object sender, TableEventArgs e)
		{
			this.superBox1.AppendT(e.TableSize);
		}
		private void textBar1_InsertImage(object sender, ImageEventArgs e)
		{
			this.superBox1.AppendImage(e.Image);
		}
		public void Save()
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.superBox1.Save(folderBrowserDialog.SelectedPath);
			}
		}
		public void LoadFromXml()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Random random = new Random(DateTime.Now.Millisecond);
				this.superBox1.LoadFromXml(openFileDialog.FileName);
			}
		}
		public void LoadFromXml(string file)
		{
			Random random = new Random(DateTime.Now.Millisecond);
			this.superBox1.LoadFromXml(file);
		}
		public void Clear()
		{
			this.superBox1.Clear();
		}
		public void Content()
		{
			this.superBox1.getContent();
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
			this.flowLayoutPanel1 = new FlowLayoutPanel();
			this.superBox1 = new SuperBox();
			this.textBar1 = new TextBar();
			this.flowLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Controls.Add(this.superBox1);
			this.flowLayoutPanel1.Location = new Point(3, 29);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new Size(714, 277);
			this.flowLayoutPanel1.TabIndex = 1;
			this.superBox1.InputLimited = false;
			this.superBox1.Location = new Point(3, 3);
			this.superBox1.Name = "superBox1";
			this.superBox1.Size = new Size(682, 32);
			this.superBox1.TabIndex = 0;
			this.superBox1.TabStop = false;
			this.superBox1.Text = "superBox1";
			this.textBar1.Color = Color.Black;
			this.textBar1.Dock = DockStyle.Top;
			this.textBar1.Location = new Point(0, 0);
			this.textBar1.Margin = new Padding(4);
			this.textBar1.Name = "textBar1";
			this.textBar1.Size = new Size(840, 22);
			this.textBar1.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.flowLayoutPanel1);
			base.Controls.Add(this.textBar1);
			base.Name = "TextBox";
			base.Size = new Size(840, 536);
			this.flowLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
