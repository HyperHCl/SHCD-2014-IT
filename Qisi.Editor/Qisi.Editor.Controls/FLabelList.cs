using Qisi.Editor.Expression;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.Editor.Controls
{
	public class FLabelList : Control
	{
		private const int head = 20;
		private const int initlines = 3;
		private List<List<FLabel>> mylist;
		private List<bool> rightout;
		private int moveleft;
		private int moveright;
		private int flabelheight;
		private Size initsize;
		private int clickregion;
		private int clickregionfoot;
		private Keys[] keyslist = new Keys[]
		{
			Keys.A,
			Keys.B,
			Keys.C,
			Keys.D,
			Keys.E,
			Keys.F,
			Keys.G,
			Keys.H,
			Keys.I,
			Keys.J,
			Keys.K,
			Keys.L,
			Keys.M,
			Keys.N,
			Keys.O,
			Keys.P,
			Keys.Q,
			Keys.R,
			Keys.S,
			Keys.T,
			Keys.U,
			Keys.V,
			Keys.W,
			Keys.X,
			Keys.Y,
			Keys.Z
		};
		private bool isexpanded = false;
		public FLabelList() : this("其他", 500, true)
		{
		}
		public FLabelList(string subject, int width, bool hashotkey = true)
		{
			base.TabStop = true;
			this.Font = new Font("微软雅黑", 10f, FontStyle.Regular, GraphicsUnit.Pixel, 134);
			this.DoubleBuffered = true;
			this.mylist = new List<List<FLabel>>();
			this.rightout = new List<bool>();
			string[] array = CommonMethods.Groups(subject).Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string key = array2[i];
				this.rightout.Add(false);
				List<FLabel> list = new List<FLabel>();
				string[] array3 = CommonMethods.Exprs(key).Split(new char[]
				{
					','
				});
				for (int j = 0; j < array3.Length; j++)
				{
					list.Add(new FLabel((FType)Enum.Parse(typeof(FType), array3[j], true), hashotkey));
				}
				this.mylist.Add(list);
			}
			if (hashotkey)
			{
				this.flabelheight = CommonMethods.height + CommonMethods.height / 2;
			}
			else
			{
				this.flabelheight = CommonMethods.height;
			}
			for (int j = 0; j < this.mylist.Count; j++)
			{
				int k = j;
				for (int l = 0; l < this.mylist[j].Count; l++)
				{
					base.Controls.Add(this.mylist[j][l]);
					this.mylist[j][l].Location = new Point(CommonMethods.height * l, this.flabelheight * j + 20);
					if (CommonMethods.height * l > width)
					{
						this.rightout[j] = true;
					}
					this.mylist[j][l].AppendExpression += new FLabel.AppendExpressionHandler(this.FLabelList_AppenExpression);
					if (j >= 3)
					{
						this.mylist[j][l].Visible = false;
					}
					if (hashotkey && k < this.keyslist.Length)
					{
						int num = NativeMethods.GlobalAddAtom(Guid.NewGuid().ToString());
						while (k < this.keyslist.Length)
						{
							if (NativeMethods.RegisterHotKey(base.Handle, num, NativeMethods.KeyModifiers.Alt, this.keyslist[k]))
							{
								this.mylist[j][l].HotKey = this.keyslist[k];
								this.mylist[j][l].HotKeyId = num;
								k += this.mylist.Count;
								break;
							}
							num = NativeMethods.GlobalAddAtom(Guid.NewGuid().ToString());
							k += this.mylist.Count;
						}
					}
				}
			}
			this.moveleft = NativeMethods.GlobalAddAtom(Guid.NewGuid().ToString());
			NativeMethods.RegisterHotKey(base.Handle, this.moveleft, NativeMethods.KeyModifiers.Alt, Keys.Left);
			this.moveright = NativeMethods.GlobalAddAtom(Guid.NewGuid().ToString());
			NativeMethods.RegisterHotKey(base.Handle, this.moveright, NativeMethods.KeyModifiers.Alt, Keys.Right);
			this.initsize = new Size(width, Math.Min(this.mylist.Count, 3) * this.flabelheight + 40);
			base.Size = new Size(width, Math.Min(this.mylist.Count, 3) * this.flabelheight + 40);
			base.Click += new EventHandler(this.FLabelList_Click);
			base.MouseMove += new MouseEventHandler(this.FLabelList_MouseMove);
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			if (base.Size != this.initsize)
			{
				base.Size = this.initsize;
			}
			base.OnSizeChanged(e);
		}
		private void FLabelList_AppenExpression(object sender, ExpressionEventArgs e)
		{
			Form form = base.FindForm();
			if (form != null && form.ActiveControl != null)
			{
				if (form.ActiveControl.GetType().ToString() == "Qisi.Editor.Controls.SuperBox")
				{
					((SuperBox)form.ActiveControl).AppendF(e.Type);
				}
				else
				{
					Control control = this.ActiveSuperBox(form.ActiveControl);
					if (control != null)
					{
						((SuperBox)control).AppendF(e.Type);
					}
				}
			}
		}
		private void MoveLeft()
		{
			for (int i = 0; i < this.mylist.Count; i++)
			{
				if (this.rightout[i])
				{
					for (int j = 0; j < this.mylist[i].Count; j++)
					{
						this.mylist[i][j].Left = this.mylist[i][j].Left - CommonMethods.height;
						if (this.mylist[i][j].Left + this.mylist[i][j].Width < 0)
						{
							if (j == 0)
							{
								this.mylist[i][j].Left = this.mylist[i][this.mylist[i].Count - 1].Right - CommonMethods.height;
							}
							else
							{
								this.mylist[i][j].Left = this.mylist[i][j - 1].Right;
							}
						}
					}
				}
			}
		}
		private void MoveRight()
		{
			for (int i = 0; i < this.mylist.Count; i++)
			{
				if (this.rightout[i])
				{
					for (int j = this.mylist[i].Count - 1; j >= 0; j--)
					{
						this.mylist[i][j].Left = this.mylist[i][j].Left + CommonMethods.height;
						if (this.mylist[i][j].Left > base.Width)
						{
							if (j == this.mylist[i].Count - 1)
							{
								this.mylist[i][j].Left = this.mylist[i][0].Left - this.mylist[i][j].Width + CommonMethods.height;
							}
							else
							{
								this.mylist[i][j].Left = this.mylist[i][j + 1].Left - this.mylist[i][j].Width;
							}
						}
					}
				}
			}
		}
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 786)
			{
				if (this.moveleft == (int)m.WParam)
				{
					this.MoveLeft();
				}
				else
				{
					if (this.moveright == (int)m.WParam)
					{
						this.MoveRight();
					}
					else
					{
						foreach (List<FLabel> current in this.mylist)
						{
							foreach (FLabel current2 in current)
							{
								if (current2.HotKeyId == (int)m.WParam)
								{
									Form form = base.FindForm();
									if (form != null && form.ActiveControl != null)
									{
										if (form.ActiveControl.GetType().ToString() == "Qisi.Editor.Controls.SuperBox")
										{
											((SuperBox)form.ActiveControl).AppendF(current2.Ftype);
										}
										else
										{
											Control control = this.ActiveSuperBox(form.ActiveControl);
											if (control != null)
											{
												((SuperBox)control).AppendF(current2.Ftype);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			base.WndProc(ref m);
		}
		private Control ActiveSuperBox(Control c)
		{
			Control result;
			if (c is ContainerControl)
			{
				if (((ContainerControl)c).ActiveControl != null && ((ContainerControl)c).ActiveControl.GetType().ToString() == "Qisi.Editor.Controls.SuperBox")
				{
					result = ((ContainerControl)c).ActiveControl;
				}
				else
				{
					result = this.ActiveSuperBox(((ContainerControl)c).ActiveControl);
				}
			}
			else
			{
				result = null;
			}
			return result;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			this.clickregion = 0;
			foreach (bool current in this.rightout)
			{
				if (current)
				{
					SizeF sizeF = e.Graphics.MeasureString("点击此处或者按Alt+→向右滚动查看更多", this.Font, 0);
					if (sizeF.Width * 2f < (float)base.Width)
					{
						this.clickregion = (int)sizeF.Width;
						e.Graphics.DrawString("点击此处或者按Alt+←向左滚动查看更多", this.Font, Brushes.Black, new Point(0, 0));
						e.Graphics.DrawString("点击此处或者按Alt+→向右滚动查看更多", this.Font, Brushes.Black, new Point(base.Width - (int)sizeF.Width, 0));
					}
					else
					{
						this.clickregion = base.Width / 2;
					}
					break;
				}
			}
			if (this.mylist.Count > 3)
			{
				this.clickregionfoot = (int)e.Graphics.MeasureString("点此展开", this.Font, 0).Width;
				if (this.isexpanded)
				{
					e.Graphics.DrawString("点此收起", this.Font, Brushes.Black, new Point(0, base.Height - 20));
				}
				else
				{
					e.Graphics.DrawString("点此展开", this.Font, Brushes.Black, new Point(0, base.Height - 20));
				}
			}
			else
			{
				this.clickregionfoot = 0;
			}
		}
		private void FLabelList_Click(object sender, EventArgs e)
		{
			Point point = base.PointToClient(Control.MousePosition);
			if (point.Y < 20)
			{
				if (point.X < this.clickregion)
				{
					this.MoveLeft();
				}
				else
				{
					if (point.X > base.Width - this.clickregion)
					{
						this.MoveRight();
					}
				}
			}
			if (point.Y > base.Height - 20 && point.X < this.clickregionfoot)
			{
				for (int i = 3; i < this.mylist.Count; i++)
				{
					for (int j = 0; j < this.mylist[i].Count; j++)
					{
						this.mylist[i][j].Visible = !this.isexpanded;
					}
				}
				if (this.isexpanded)
				{
					this.initsize = new Size(base.Width, 3 * this.flabelheight + 40);
					base.Size = new Size(base.Width, 3 * this.flabelheight + 40);
				}
				else
				{
					this.initsize = new Size(base.Width, this.mylist.Count * this.flabelheight + 40);
					base.Size = new Size(base.Width, this.mylist.Count * this.flabelheight + 40);
				}
				this.isexpanded = !this.isexpanded;
				base.Invalidate();
			}
		}
		private void FLabelList_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Y < 20 && (e.X < this.clickregion || e.X > base.Width - this.clickregion))
			{
				this.Cursor = Cursors.Hand;
			}
			else
			{
				if (e.Y > base.Height - 20 && e.X < this.clickregionfoot)
				{
					this.Cursor = Cursors.Hand;
				}
				else
				{
					this.Cursor = Cursors.Arrow;
				}
			}
		}
	}
}
