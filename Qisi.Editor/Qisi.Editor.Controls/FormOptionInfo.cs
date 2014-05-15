using Qisi.Editor.Documents;
using Qisi.Editor.Documents.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.Editor.Controls
{
	public class FormOptionInfo : Form
	{
		private List<SuperBox> superBoxList;
		private IContainer components = null;
		private Label label1;
		private NumericUpDown numericUpDown1;
		private FlowLayoutPanel flowLayoutPanel1;
		private Button button1;
		internal List<Element> Elements
		{
			get
			{
				List<Element> list = new List<Element>();
				for (int i = 0; i < this.superBoxList.Count; i++)
				{
					list.AddRange(this.superBoxList[i].Elements);
				}
				return list;
			}
		}
		internal Options Options
		{
			get
			{
				Options options = new Options(false, false);
				for (int i = 0; i < this.superBoxList.Count; i++)
				{
					Option item = new Option(0, this.superBoxList[i].Elements.Count, ((char)(65 + i)).ToString());
					options.OptionList.Add(item);
				}
				return options;
			}
		}
		public FormOptionInfo()
		{
			this.superBoxList = new List<SuperBox>();
			this.InitializeComponent();
		}
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (this.superBoxList.Count < this.numericUpDown1.Value)
			{
				int num = this.superBoxList.Count;
				while (num < this.numericUpDown1.Value)
				{
					SuperBox superBox = new SuperBox(this.flowLayoutPanel1.ClientSize.Width);
					superBox.Height = 100;
					superBox.BackColor = Color.Aqua;
					superBox.Visible = true;
					this.superBoxList.Add(superBox);
					this.flowLayoutPanel1.Controls.Add(superBox);
					this.flowLayoutPanel1.SetFlowBreak(superBox, true);
					num++;
				}
			}
			else
			{
				while (this.superBoxList.Count > this.numericUpDown1.Value)
				{
					this.superBoxList.RemoveAt(Convert.ToInt32(this.numericUpDown1.Value));
					this.flowLayoutPanel1.Controls.RemoveAt(Convert.ToInt32(this.numericUpDown1.Value));
				}
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
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
			this.label1 = new Label();
			this.numericUpDown1 = new NumericUpDown();
			this.flowLayoutPanel1 = new FlowLayoutPanel();
			this.button1 = new Button();
			((ISupportInitialize)this.numericUpDown1).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 19);
			this.label1.Name = "label1";
			this.label1.Size = new Size(47, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "选项数:";
			this.numericUpDown1.Location = new Point(65, 17);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new Size(51, 21);
			this.numericUpDown1.TabIndex = 1;
			this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
			NumericUpDown arg_11D_0 = this.numericUpDown1;
			int[] array = new int[4];
			array[0] = 4;
			arg_11D_0.Value = new decimal(array);
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Location = new Point(12, 44);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new Size(434, 206);
			this.flowLayoutPanel1.TabIndex = 2;
			this.button1.Location = new Point(357, 283);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "插入";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(458, 318);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.flowLayoutPanel1);
			base.Controls.Add(this.numericUpDown1);
			base.Controls.Add(this.label1);
			base.Name = "FormOptionInfo";
			this.Text = "选项信息";
			((ISupportInitialize)this.numericUpDown1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
