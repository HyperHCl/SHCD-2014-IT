using Qisi.Editor.Properties;
using Qisi.General.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.Editor.Controls
{
	public class FormBlankInfo : Form
	{
		private IContainer components = null;
		private Label label1;
		private NumericUpDown numericUpDown1;
		private NumericUpDown numericUpDown2;
		private Label label2;
		private CrystalButtonLeftRight crystalButtonLeftRight1;
		private Label label3;
		public int MaxCount
		{
			get
			{
				return (int)this.numericUpDown1.Value;
			}
		}
		public float MinWidth
		{
			get
			{
				return (float)this.numericUpDown2.Value;
			}
		}
		public FormBlankInfo()
		{
			this.InitializeComponent();
		}
		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
		}
		private void FormBlankInfo_Load(object sender, EventArgs e)
		{
			this.label3.Text = "供参考，本窗口的宽度为" + base.Width.ToString() + "像素。";
		}
		private void crystalButtonLeftRight1_Click(object sender, EventArgs e)
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
			this.numericUpDown2 = new NumericUpDown();
			this.label2 = new Label();
			this.crystalButtonLeftRight1 = new CrystalButtonLeftRight();
			this.label3 = new Label();
			((ISupportInitialize)this.numericUpDown1).BeginInit();
			((ISupportInitialize)this.numericUpDown2).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label1.Location = new Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new Size(92, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "最大输入字符数";
			this.numericUpDown1.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.numericUpDown1.Location = new Point(142, 12);
			NumericUpDown arg_13F_0 = this.numericUpDown1;
			int[] array = new int[4];
			array[0] = 10000;
			arg_13F_0.Maximum = new decimal(array);
			NumericUpDown arg_15C_0 = this.numericUpDown1;
			array = new int[4];
			array[0] = 1;
			arg_15C_0.Minimum = new decimal(array);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new Size(120, 23);
			this.numericUpDown1.TabIndex = 1;
			NumericUpDown arg_1AC_0 = this.numericUpDown1;
			array = new int[4];
			array[0] = 1;
			arg_1AC_0.Value = new decimal(array);
			this.numericUpDown2.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.numericUpDown2.Location = new Point(142, 53);
			NumericUpDown arg_207_0 = this.numericUpDown2;
			array = new int[4];
			array[0] = 10000;
			arg_207_0.Maximum = new decimal(array);
			NumericUpDown arg_225_0 = this.numericUpDown2;
			array = new int[4];
			array[0] = 50;
			arg_225_0.Minimum = new decimal(array);
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new Size(120, 23);
			this.numericUpDown2.TabIndex = 3;
			NumericUpDown arg_276_0 = this.numericUpDown2;
			array = new int[4];
			array[0] = 50;
			arg_276_0.Value = new decimal(array);
			this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
			this.label2.AutoSize = true;
			this.label2.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label2.Location = new Point(12, 55);
			this.label2.Name = "label2";
			this.label2.Size = new Size(124, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "最小长度(单位：像素)";
			this.crystalButtonLeftRight1.ButtonText = "确定";
			this.crystalButtonLeftRight1.Cursor = Cursors.Hand;
			this.crystalButtonLeftRight1.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.crystalButtonLeftRight1.Image = Resources.confirm;
			this.crystalButtonLeftRight1.Location = new Point(110, 119);
			this.crystalButtonLeftRight1.Margin = new Padding(3, 4, 3, 4);
			this.crystalButtonLeftRight1.Name = "crystalButtonLeftRight1";
			this.crystalButtonLeftRight1.Size = new Size(70, 28);
			this.crystalButtonLeftRight1.Speed = 10;
			this.crystalButtonLeftRight1.TabIndex = 4;
			this.crystalButtonLeftRight1.Click += new EventHandler(this.crystalButtonLeftRight1_Click);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(15, 90);
			this.label3.Name = "label3";
			this.label3.Size = new Size(41, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "label3";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(284, 160);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.crystalButtonLeftRight1);
			base.Controls.Add(this.numericUpDown2);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.numericUpDown1);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "FormBlankInfo";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "填空格信息";
			base.Load += new EventHandler(this.FormBlankInfo_Load);
			((ISupportInitialize)this.numericUpDown1).EndInit();
			((ISupportInitialize)this.numericUpDown2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
