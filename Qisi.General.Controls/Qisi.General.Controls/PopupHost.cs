using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class PopupHost : ToolStripDropDown
	{
		private ToolStripControlHost _controlHost;
		private Control _popupControl;
		private bool _resizableTop;
		private bool _resizableLeft;
		private PopupHost _ownerPopup;
		private PopupHost _childPopup;
		public bool ChangeRegion
		{
			get;
			set;
		}
		public bool OpenFocused
		{
			get;
			set;
		}
		public bool CanResize
		{
			get;
			set;
		}
		public Color BorderColor
		{
			get;
			set;
		}
		public PopupHost(Control c)
		{
			this.DoubleBuffered = true;
			base.ResizeRedraw = true;
			this.AutoSize = false;
			this.CanResize = false;
			this.BorderColor = Color.Black;
			base.Padding = Padding.Empty;
			base.Margin = Padding.Empty;
			this.CreateHost(c);
		}
		protected override void OnOpening(CancelEventArgs e)
		{
			if (this._popupControl.IsDisposed || this._popupControl.Disposing)
			{
				e.Cancel = true;
			}
			else
			{
				this._popupControl.RegionChanged += new EventHandler(this.PopupControlRegionChanged);
				this.UpdateRegion();
			}
			base.OnOpening(e);
		}
		protected override void OnOpened(EventArgs e)
		{
			if (this.OpenFocused)
			{
				this._popupControl.Focus();
			}
			base.OnOpened(e);
		}
		protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
		{
			this._popupControl.RegionChanged -= new EventHandler(this.PopupControlRegionChanged);
			base.OnClosing(e);
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if (this._controlHost != null)
			{
				this._controlHost.Size = new Size(base.Width - base.Padding.Horizontal, base.Height - base.Padding.Vertical);
			}
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (!this.ProcessGrip(ref m))
			{
				base.WndProc(ref m);
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!this.ChangeRegion)
			{
				ControlPaint.DrawBorder(e.Graphics, base.ClientRectangle, this.BorderColor, ButtonBorderStyle.Solid);
			}
		}
		protected void UpdateRegion()
		{
			if (!this.ChangeRegion)
			{
				return;
			}
			if (base.Region != null)
			{
				base.Region.Dispose();
				base.Region = null;
			}
			if (this._popupControl.Region != null)
			{
				base.Region = this._popupControl.Region.Clone();
			}
		}
		public void Show(Control control)
		{
			this.Show(control, control.ClientRectangle);
		}
		public void Show(Control control, bool center)
		{
			this.Show(control, control.ClientRectangle, center);
		}
		public void Show(Control control, Rectangle rect)
		{
			this.Show(control, rect, false);
		}
		public void Show(Control control, Rectangle rect, bool center)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			this.SetOwnerItem(control);
			if (this.CanResize && !this.ChangeRegion)
			{
				base.Padding = new Padding(3);
			}
			else
			{
				if (!this.ChangeRegion)
				{
					base.Padding = new Padding(1);
				}
				else
				{
					base.Padding = Padding.Empty;
				}
			}
			int horizontal = base.Padding.Horizontal;
			int vertical = base.Padding.Vertical;
			base.Size = new Size(this._popupControl.Width + horizontal, this._popupControl.Height + vertical);
			this._resizableTop = false;
			this._resizableLeft = false;
			Point point = control.PointToScreen(new Point(rect.Left, rect.Bottom));
			Rectangle workingArea = Screen.FromControl(control).WorkingArea;
			if (center)
			{
				if (point.X + (rect.Width + base.Size.Width) / 2 > workingArea.Right)
				{
					point.X = workingArea.Right - base.Size.Width;
					this._resizableLeft = true;
				}
				else
				{
					point.X -= (base.Size.Width - rect.Width) / 2;
				}
			}
			else
			{
				if (point.X + base.Size.Width > workingArea.Left + workingArea.Width)
				{
					this._resizableLeft = true;
					point.X = workingArea.Left + workingArea.Width - base.Size.Width;
				}
			}
			if (point.Y + base.Size.Height > workingArea.Top + workingArea.Height)
			{
				this._resizableTop = true;
				point.Y -= base.Size.Height + rect.Height;
			}
			point = control.PointToClient(point);
			base.Show(control, point, ToolStripDropDownDirection.BelowRight);
		}
		private void SetOwnerItem(Control control)
		{
			if (control == null)
			{
				return;
			}
			if (control is PopupHost)
			{
				PopupHost popupHost = control as PopupHost;
				this._ownerPopup = popupHost;
				this._ownerPopup._childPopup = this;
				base.OwnerItem = popupHost.Items[0];
				return;
			}
			if (control.Parent != null)
			{
				this.SetOwnerItem(control.Parent);
			}
		}
		private void CreateHost(Control control)
		{
			if (control == null)
			{
				throw new ArgumentException("control");
			}
			this._popupControl = control;
			this._controlHost = new ToolStripControlHost(control, "PopupHost");
			this._controlHost.AutoSize = false;
			this._controlHost.Padding = Padding.Empty;
			this._controlHost.Margin = Padding.Empty;
			base.Size = new Size(control.Size.Width + base.Padding.Horizontal, control.Size.Height + base.Padding.Vertical);
			base.Items.Add(this._controlHost);
		}
		private void PopupControlRegionChanged(object sender, EventArgs e)
		{
			this.UpdateRegion();
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private bool ProcessGrip(ref Message m)
		{
			if (this.CanResize && !this.ChangeRegion)
			{
				int msg = m.Msg;
				if (msg == 36)
				{
					return this.OnGetMinMaxInfo(ref m);
				}
				if (msg == 132)
				{
					return this.OnNcHitTest(ref m);
				}
			}
			return false;
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private bool OnGetMinMaxInfo(ref Message m)
		{
			Control popupControl = this._popupControl;
			if (popupControl != null)
			{
				NativeMethods.MINMAXINFO mINMAXINFO = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
				if (popupControl.MaximumSize.Width != 0)
				{
					mINMAXINFO.maxTrackSize.Width = popupControl.MaximumSize.Width;
				}
				if (popupControl.MaximumSize.Height != 0)
				{
					mINMAXINFO.maxTrackSize.Height = popupControl.MaximumSize.Height;
				}
				mINMAXINFO.minTrackSize = new Size(100, 100);
				if (popupControl.MinimumSize.Width > mINMAXINFO.minTrackSize.Width)
				{
					mINMAXINFO.minTrackSize.Width = popupControl.MinimumSize.Width + base.Padding.Horizontal;
				}
				if (popupControl.MinimumSize.Height > mINMAXINFO.minTrackSize.Height)
				{
					mINMAXINFO.minTrackSize.Height = popupControl.MinimumSize.Height + base.Padding.Vertical;
				}
				Marshal.StructureToPtr(mINMAXINFO, m.LParam, false);
			}
			return true;
		}
		private bool OnNcHitTest(ref Message m)
		{
			Point pt = base.PointToClient(new Point(NativeMethods.LOWORD(m.LParam), NativeMethods.HIWORD(m.LParam)));
			Rectangle empty = Rectangle.Empty;
			if (this.CanResize && !this.ChangeRegion)
			{
				if (this._resizableLeft)
				{
					if (this._resizableTop)
					{
						empty = new Rectangle(0, 0, 6, 6);
					}
					else
					{
						empty = new Rectangle(0, base.Height - 6, 6, 6);
					}
				}
				else
				{
					if (this._resizableTop)
					{
						empty = new Rectangle(base.Width - 6, 0, 6, 6);
					}
					else
					{
						empty = new Rectangle(base.Width - 6, base.Height - 6, 6, 6);
					}
				}
			}
			if (empty.Contains(pt))
			{
				if (this._resizableLeft)
				{
					if (this._resizableTop)
					{
						m.Result = (IntPtr)13;
						return true;
					}
					m.Result = (IntPtr)16;
					return true;
				}
				else
				{
					if (this._resizableTop)
					{
						m.Result = (IntPtr)14;
						return true;
					}
					m.Result = (IntPtr)17;
					return true;
				}
			}
			else
			{
				Rectangle clientRectangle = base.ClientRectangle;
				if (pt.X > clientRectangle.Right - 3 && pt.X <= clientRectangle.Right && !this._resizableLeft)
				{
					m.Result = (IntPtr)11;
					return true;
				}
				if (pt.Y > clientRectangle.Bottom - 3 && pt.Y <= clientRectangle.Bottom && !this._resizableTop)
				{
					m.Result = (IntPtr)15;
					return true;
				}
				if (pt.X > -1 && pt.X < 3 && this._resizableLeft)
				{
					m.Result = (IntPtr)10;
					return true;
				}
				if (pt.Y > -1 && pt.Y < 3 && this._resizableTop)
				{
					m.Result = (IntPtr)12;
					return true;
				}
				return false;
			}
		}
	}
}
