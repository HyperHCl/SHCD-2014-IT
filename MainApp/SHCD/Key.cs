using SHCD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace SHCD
{
	public class Key : UserControl
	{
		private string text = "S";
		private Keys Value;
		private bool OneLine = false;
		private int keyType = 1;
		private IContainer components = null;
		public int myType
		{
			get
			{
				return this.keyType;
			}
			set
			{
				this.keyType = value;
			}
		}
		public bool one
		{
			get
			{
				return this.OneLine;
			}
			set
			{
				this.OneLine = value;
			}
		}
		public string KeyText
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}
		public Keys KeyValue
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = value;
			}
		}
		public Key()
		{
			this.InitializeComponent();
		}
		private void Key_Paint(object sender, PaintEventArgs e)
		{
			List<Point> list = new List<Point>();
			int width = base.Width;
			int height = base.Height;
			list.Add(new Point(0, 5));
			list.Add(new Point(1, 5));
			list.Add(new Point(1, 3));
			list.Add(new Point(2, 3));
			list.Add(new Point(2, 2));
			list.Add(new Point(3, 2));
			list.Add(new Point(3, 1));
			list.Add(new Point(5, 1));
			list.Add(new Point(5, 0));
			list.Add(new Point(width - 6, 0));
			list.Add(new Point(width - 6, 1));
			list.Add(new Point(width - 4, 1));
			list.Add(new Point(width - 4, 2));
			list.Add(new Point(width - 3, 2));
			list.Add(new Point(width - 3, 3));
			list.Add(new Point(width - 2, 3));
			list.Add(new Point(width - 2, 5));
			list.Add(new Point(width - 1, 5));
			list.Add(new Point(width - 1, height - 6));
			list.Add(new Point(width - 2, height - 6));
			list.Add(new Point(width - 2, height - 4));
			list.Add(new Point(width - 3, height - 4));
			list.Add(new Point(width - 3, height - 3));
			list.Add(new Point(width - 4, height - 3));
			list.Add(new Point(width - 4, height - 2));
			list.Add(new Point(width - 6, height - 2));
			list.Add(new Point(width - 6, height - 1));
			list.Add(new Point(5, height - 1));
			list.Add(new Point(5, height - 2));
			list.Add(new Point(3, height - 2));
			list.Add(new Point(3, height - 3));
			list.Add(new Point(2, height - 3));
			list.Add(new Point(2, height - 4));
			list.Add(new Point(1, height - 4));
			list.Add(new Point(1, height - 6));
			list.Add(new Point(0, height - 6));
			Point[] points = list.ToArray();
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddPolygon(points);
			base.Region = new Region(graphicsPath);
			e.Graphics.DrawPath(Pens.Gray, graphicsPath);
			List<Point> list2 = new List<Point>();
			int num = base.Width - 3;
			int num2 = base.Height - 4;
			int num3 = 2;
			int num4 = 2;
			list2.Add(new Point(num3, num4 + 5));
			list2.Add(new Point(num3 + 1, num4 + 5));
			list2.Add(new Point(num3 + 1, num4 + 3));
			list2.Add(new Point(num3 + 2, num4 + 3));
			list2.Add(new Point(num3 + 2, num4 + 2));
			list2.Add(new Point(num3 + 3, num4 + 2));
			list2.Add(new Point(num3 + 3, num4 + 1));
			list2.Add(new Point(num3 + 5, num4 + 1));
			list2.Add(new Point(num3 + 5, num4));
			list2.Add(new Point(num - 6, num4));
			list2.Add(new Point(num - 6, num4 + 1));
			list2.Add(new Point(num - 4, num4 + 1));
			list2.Add(new Point(num - 4, num4 + 2));
			list2.Add(new Point(num - 3, num4 + 2));
			list2.Add(new Point(num - 3, num4 + 3));
			list2.Add(new Point(num - 2, num4 + 3));
			list2.Add(new Point(num - 2, num4 + 5));
			list2.Add(new Point(num - 1, num4 + 5));
			list2.Add(new Point(num - 1, num2 - 6));
			list2.Add(new Point(num - 2, num2 - 6));
			list2.Add(new Point(num - 2, num2 - 4));
			list2.Add(new Point(num - 3, num2 - 4));
			list2.Add(new Point(num - 3, num2 - 3));
			list2.Add(new Point(num - 4, num2 - 3));
			list2.Add(new Point(num - 4, num2 - 2));
			list2.Add(new Point(num - 6, num2 - 2));
			list2.Add(new Point(num - 6, num2 - 1));
			list2.Add(new Point(num3 + 5, num2 - 1));
			list2.Add(new Point(num3 + 5, num2 - 2));
			list2.Add(new Point(num3 + 3, num2 - 2));
			list2.Add(new Point(num3 + 3, num2 - 3));
			list2.Add(new Point(num3 + 2, num2 - 3));
			list2.Add(new Point(num3 + 2, num2 - 4));
			list2.Add(new Point(num3 + 1, num2 - 4));
			list2.Add(new Point(num3 + 1, num2 - 6));
			list2.Add(new Point(num3, num2 - 6));
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			stringFormat.FormatFlags = StringFormatFlags.NoWrap;
			Point[] points2 = list2.ToArray();
			GraphicsPath graphicsPath2 = new GraphicsPath();
			graphicsPath2.AddPolygon(points2);
			e.Graphics.DrawPath(Pens.Gray, graphicsPath2);
			string text = this.text;
			if (!this.OneLine)
			{
				for (int i = this.text.Length - 1; i > 0; i--)
				{
					text = text.Insert(i, "\r\n");
				}
			}
			if (this.keyType == 1)
			{
				e.Graphics.DrawString(text, this.Font, Brushes.Black, new RectangleF(3f, 3f, (float)(base.ClientSize.Width - 3), (float)(base.ClientSize.Height - 3)), stringFormat);
			}
			else
			{
				e.Graphics.DrawImage(Resources.win, new Rectangle(10, 10, 20, 20));
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			this.DoubleBuffered = true;
			base.Margin = new Padding(0);
			base.Name = "Key";
			base.Size = new Size(40, 40);
			base.Paint += new PaintEventHandler(this.Key_Paint);
			base.ResumeLayout(false);
		}
	}
}
