using Qisi.Editor.Properties;
using Qisi.General.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.Editor.Controls
{
	internal class FrmInsertTable : Form
	{
		private Point _tablesize = default(Point);
		private IContainer components = null;
		private Label label1;
		private Label label2;
		private NumericUpDown numericUpDown1;
		private NumericUpDown numericUpDown2;
		private CrystalButtonLeftRight crystalButtonLeftRight1;
		private CrystalButtonLeftRight crystalButtonLeftRight2;
		public Point TableSize
		{
			get
			{
				return this._tablesize;
			}
		}
		public FrmInsertTable()
		{
			this.InitializeComponent();
			base.Icon = SystemIcons.Information;
		}
		private void crystalButtonLeftRight2_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}
		private void crystalButtonLeftRight1_Click(object sender, EventArgs e)
		{
			this._tablesize = new Point(Convert.ToInt32(this.numericUpDown2.Value), Convert.ToInt32(this.numericUpDown1.Value));
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
			this.label2 = new Label();
			this.numericUpDown1 = new NumericUpDown();
			this.numericUpDown2 = new NumericUpDown();
			this.crystalButtonLeftRight2 = new CrystalButtonLeftRight();
			this.crystalButtonLeftRight1 = new CrystalButtonLeftRight();
			((ISupportInitialize)this.numericUpDown1).BeginInit();
			((ISupportInitialize)this.numericUpDown2).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label1.Location = new Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new Size(46, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "行数:";
			this.label2.AutoSize = true;
			this.label2.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label2.Location = new Point(12, 45);
			this.label2.Name = "label2";
			this.label2.Size = new Size(46, 21);
			this.label2.TabIndex = 1;
			this.label2.Text = "列数:";
			this.numericUpDown1.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.numericUpDown1.Location = new Point(151, 14);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new Size(64, 29);
			this.numericUpDown1.TabIndex = 2;
			this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
			this.numericUpDown1.ThousandsSeparator = true;
			NumericUpDown arg_210_0 = this.numericUpDown1;
			int[] array = new int[4];
			array[0] = 2;
			arg_210_0.Value = new decimal(array);
			this.numericUpDown2.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.numericUpDown2.Location = new Point(151, 43);
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new Size(64, 29);
			this.numericUpDown2.TabIndex = 3;
			this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
			this.numericUpDown2.ThousandsSeparator = true;
			NumericUpDown arg_2B4_0 = this.numericUpDown2;
			array = new int[4];
			array[0] = 5;
			arg_2B4_0.Value = new decimal(array);
			this.crystalButtonLeftRight2.ButtonText = "取消";
			this.crystalButtonLeftRight2.Cursor = Cursors.Hand;
			this.crystalButtonLeftRight2.Font = new Font("微软雅黑", 10f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.crystalButtonLeftRight2.Image = Resources.cancel;
			this.crystalButtonLeftRight2.Location = new Point(131, 86);
			this.crystalButtonLeftRight2.Margin = new Padding(4, 5, 4, 5);
			this.crystalButtonLeftRight2.Name = "crystalButtonLeftRight2";
			this.crystalButtonLeftRight2.Size = new Size(74, 25);
			this.crystalButtonLeftRight2.Speed = 10;
			this.crystalButtonLeftRight2.TabIndex = 5;
			this.crystalButtonLeftRight2.Click += new EventHandler(this.crystalButtonLeftRight2_Click);
			this.crystalButtonLeftRight1.ButtonText = "确定";
			this.crystalButtonLeftRight1.Cursor = Cursors.Hand;
			this.crystalButtonLeftRight1.Font = new Font("微软雅黑", 10f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.crystalButtonLeftRight1.Image = Resources.confirm;
			this.crystalButtonLeftRight1.Location = new Point(39, 86);
			this.crystalButtonLeftRight1.Margin = new Padding(4, 5, 4, 5);
			this.crystalButtonLeftRight1.Name = "crystalButtonLeftRight1";
			this.crystalButtonLeftRight1.Size = new Size(74, 25);
			this.crystalButtonLeftRight1.Speed = 10;
			this.crystalButtonLeftRight1.TabIndex = 4;
			this.crystalButtonLeftRight1.Click += new EventHandler(this.crystalButtonLeftRight1_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(248, 118);
			base.Controls.Add(this.crystalButtonLeftRight2);
			base.Controls.Add(this.crystalButtonLeftRight1);
			base.Controls.Add(this.numericUpDown2);
			base.Controls.Add(this.numericUpDown1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "FrmInsertTable";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "插入表格";
			((ISupportInitialize)this.numericUpDown1).EndInit();
			((ISupportInitialize)this.numericUpDown2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
