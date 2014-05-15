using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace SHCD
{
	public class ChooseSubject : MdiBase
	{
		private IContainer components = null;
		private ListBox listBox1;
		private Label label1;
		private Button button1;
		public event EventHandler SubjectSelected;
		public string SelectSubject
		{
			get
			{
				return this.listBox1.SelectedItem.ToString();
			}
		}
		public ChooseSubject()
		{
			this.InitializeComponent();
			this.button1.Enabled = false;
		}
		public void AddItem(string text)
		{
			this.listBox1.Items.Clear();
			string[] array = text.Split(new char[]
			{
				'_'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string item = array[i];
				this.listBox1.Items.Add(item);
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this.SubjectSelected(this, new EventArgs());
		}
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listBox1.SelectedItem != null)
			{
				this.button1.Enabled = true;
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
			this.listBox1 = new ListBox();
			this.label1 = new Label();
			this.button1 = new Button();
			base.SuspendLayout();
			this.listBox1.Font = new Font("黑体", 15f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 20;
			this.listBox1.Location = new Point(55, 51);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new Size(345, 144);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.BackColor = Color.Transparent;
			this.label1.Font = new Font("黑体", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label1.Location = new Point(51, 28);
			this.label1.Name = "label1";
			this.label1.Size = new Size(177, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "请选择选考模块：";
			this.button1.AutoSize = true;
			this.button1.Cursor = Cursors.Hand;
			this.button1.FlatStyle = FlatStyle.Flat;
			this.button1.Font = new Font("黑体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.button1.Location = new Point(178, 210);
			this.button1.Name = "button1";
			this.button1.Size = new Size(89, 31);
			this.button1.TabIndex = 5;
			this.button1.Text = "确定";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.listBox1);
			this.DoubleBuffered = true;
			base.Name = "ChooseSubject";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
