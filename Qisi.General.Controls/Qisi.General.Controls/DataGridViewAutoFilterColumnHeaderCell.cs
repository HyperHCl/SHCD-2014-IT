using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
namespace Qisi.General.Controls
{
	public class DataGridViewAutoFilterColumnHeaderCell : DataGridViewColumnHeaderCell
	{
		private class FilterListBox : ListBox
		{
			public FilterListBox()
			{
				base.Visible = false;
				base.IntegralHeight = true;
				base.BorderStyle = BorderStyle.FixedSingle;
				base.TabStop = false;
			}
			protected override bool IsInputKey(Keys keyData)
			{
				return true;
			}
			protected override bool ProcessKeyMessage(ref Message m)
			{
				return this.ProcessKeyEventArgs(ref m);
			}
		}
		private static DataGridViewAutoFilterColumnHeaderCell.FilterListBox dropDownListBox = new DataGridViewAutoFilterColumnHeaderCell.FilterListBox();
		private OrderedDictionary filters = new OrderedDictionary();
		private string selectedFilterValue = string.Empty;
		private string currentColumnFilter = string.Empty;
		private bool filtered;
		private bool dropDownListBoxShowing;
		private bool lostFocusOnDropDownButtonClick;
		private Rectangle dropDownButtonBoundsValue = Rectangle.Empty;
		private int currentDropDownButtonPaddingOffset;
		private bool filteringEnabledValue = true;
		private bool automaticSortingEnabledValue = true;
		private int dropDownListBoxMaxLinesValue = 20;
		protected int DropDownListBoxMaxHeightInternal
		{
			get
			{
				int num = base.DataGridView.Height - base.DataGridView.ColumnHeadersHeight - 1;
				if (base.DataGridView.DisplayedColumnCount(false) < base.DataGridView.ColumnCount)
				{
					num -= SystemInformation.HorizontalScrollBarHeight;
				}
				int num2 = this.dropDownListBoxMaxLinesValue * DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.ItemHeight + 2;
				if (num2 < num)
				{
					return num2;
				}
				return num;
			}
		}
		protected Rectangle DropDownButtonBounds
		{
			get
			{
				if (!this.FilteringEnabled)
				{
					return Rectangle.Empty;
				}
				if (this.dropDownButtonBoundsValue == Rectangle.Empty)
				{
					this.SetDropDownButtonBounds();
				}
				return this.dropDownButtonBoundsValue;
			}
		}
		[DefaultValue(true)]
		public bool FilteringEnabled
		{
			get
			{
				if (base.DataGridView == null || base.DataGridView.DataSource == null)
				{
					return this.filteringEnabledValue;
				}
				BindingSource bindingSource = base.DataGridView.DataSource as BindingSource;
				Debug.Assert(bindingSource != null);
				return this.filteringEnabledValue && bindingSource.SupportsFiltering;
			}
			set
			{
				if (!value)
				{
					this.AdjustPadding(0);
					this.InvalidateDropDownButtonBounds();
				}
				this.filteringEnabledValue = value;
			}
		}
		[DefaultValue(true)]
		public bool AutomaticSortingEnabled
		{
			get
			{
				return this.automaticSortingEnabledValue;
			}
			set
			{
				this.automaticSortingEnabledValue = value;
				if (base.OwningColumn != null)
				{
					if (value)
					{
						base.OwningColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
						return;
					}
					base.OwningColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
				}
			}
		}
		[DefaultValue(20)]
		public int DropDownListBoxMaxLines
		{
			get
			{
				return this.dropDownListBoxMaxLinesValue;
			}
			set
			{
				this.dropDownListBoxMaxLinesValue = value;
			}
		}
		public DataGridViewAutoFilterColumnHeaderCell(DataGridViewColumnHeaderCell oldHeaderCell)
		{
			this.ContextMenuStrip = oldHeaderCell.ContextMenuStrip;
			base.ErrorText = oldHeaderCell.ErrorText;
			base.Tag = oldHeaderCell.Tag;
			base.ToolTipText = oldHeaderCell.ToolTipText;
			base.Value = oldHeaderCell.Value;
			this.ValueType = oldHeaderCell.ValueType;
			if (oldHeaderCell.HasStyle)
			{
				base.Style = oldHeaderCell.Style;
			}
			DataGridViewAutoFilterColumnHeaderCell dataGridViewAutoFilterColumnHeaderCell = oldHeaderCell as DataGridViewAutoFilterColumnHeaderCell;
			if (dataGridViewAutoFilterColumnHeaderCell != null)
			{
				this.FilteringEnabled = dataGridViewAutoFilterColumnHeaderCell.FilteringEnabled;
				this.AutomaticSortingEnabled = dataGridViewAutoFilterColumnHeaderCell.AutomaticSortingEnabled;
				this.DropDownListBoxMaxLines = dataGridViewAutoFilterColumnHeaderCell.DropDownListBoxMaxLines;
				this.currentDropDownButtonPaddingOffset = dataGridViewAutoFilterColumnHeaderCell.currentDropDownButtonPaddingOffset;
			}
		}
		public DataGridViewAutoFilterColumnHeaderCell()
		{
		}
		public override object Clone()
		{
			return new DataGridViewAutoFilterColumnHeaderCell(this);
		}
		protected override void OnDataGridViewChanged()
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (base.OwningColumn != null)
			{
				if (base.OwningColumn is DataGridViewImageColumn || (base.OwningColumn is DataGridViewButtonColumn && ((DataGridViewButtonColumn)base.OwningColumn).UseColumnTextForButtonValue) || (base.OwningColumn is DataGridViewLinkColumn && ((DataGridViewLinkColumn)base.OwningColumn).UseColumnTextForLinkValue))
				{
					this.AutomaticSortingEnabled = false;
					this.FilteringEnabled = false;
				}
				if (base.OwningColumn.SortMode == DataGridViewColumnSortMode.Automatic)
				{
					base.OwningColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
				}
			}
			this.VerifyDataSource();
			this.HandleDataGridViewEvents();
			this.SetDropDownButtonBounds();
			base.OnDataGridViewChanged();
		}
		private void VerifyDataSource()
		{
			if (base.DataGridView == null || base.DataGridView.DataSource == null)
			{
				return;
			}
			if (!(base.DataGridView.DataSource is BindingSource))
			{
				throw new NotSupportedException("The DataSource property of the containing DataGridView control must be set to a BindingSource.");
			}
		}
		private void HandleDataGridViewEvents()
		{
			base.DataGridView.Scroll += new ScrollEventHandler(this.DataGridView_Scroll);
			base.DataGridView.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(this.DataGridView_ColumnDisplayIndexChanged);
			base.DataGridView.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.DataGridView_ColumnWidthChanged);
			base.DataGridView.ColumnHeadersHeightChanged += new EventHandler(this.DataGridView_ColumnHeadersHeightChanged);
			base.DataGridView.SizeChanged += new EventHandler(this.DataGridView_SizeChanged);
			base.DataGridView.DataSourceChanged += new EventHandler(this.DataGridView_DataSourceChanged);
			base.DataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.DataGridView_DataBindingComplete);
			base.DataGridView.ColumnSortModeChanged += new DataGridViewColumnEventHandler(this.DataGridView_ColumnSortModeChanged);
		}
		private void DataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
			{
				this.ResetDropDown();
			}
		}
		private void DataGridView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
		{
			this.ResetDropDown();
		}
		private void DataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			this.ResetDropDown();
		}
		private void DataGridView_ColumnHeadersHeightChanged(object sender, EventArgs e)
		{
			this.ResetDropDown();
		}
		private void DataGridView_SizeChanged(object sender, EventArgs e)
		{
			this.ResetDropDown();
		}
		private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			if (e.ListChangedType == ListChangedType.Reset)
			{
				this.ResetDropDown();
				this.ResetFilter();
			}
		}
		private void DataGridView_DataSourceChanged(object sender, EventArgs e)
		{
			this.VerifyDataSource();
			this.ResetDropDown();
			this.ResetFilter();
		}
		private void ResetDropDown()
		{
			this.InvalidateDropDownButtonBounds();
			if (this.dropDownListBoxShowing)
			{
				this.HideDropDownList();
			}
		}
		private void ResetFilter()
		{
			if (base.DataGridView == null)
			{
				return;
			}
			BindingSource bindingSource = base.DataGridView.DataSource as BindingSource;
			if (bindingSource == null || string.IsNullOrEmpty(bindingSource.Filter))
			{
				this.filtered = false;
				this.selectedFilterValue = "(全部)";
				this.currentColumnFilter = string.Empty;
			}
		}
		private void DataGridView_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (e.Column == base.OwningColumn && e.Column.SortMode == DataGridViewColumnSortMode.Automatic)
			{
				throw new InvalidOperationException("A SortMode value of Automatic is incompatible with the DataGridViewAutoFilterColumnHeaderCell type. Use the AutomaticSortingEnabled property instead.");
			}
		}
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
			if (!this.FilteringEnabled || (paintParts & DataGridViewPaintParts.ContentBackground) == DataGridViewPaintParts.None)
			{
				return;
			}
			Rectangle dropDownButtonBounds = this.DropDownButtonBounds;
			if (dropDownButtonBounds.Width < 1 || dropDownButtonBounds.Height < 1)
			{
				return;
			}
			if (Application.RenderWithVisualStyles)
			{
				ComboBoxState state = ComboBoxState.Normal;
				if (this.dropDownListBoxShowing)
				{
					state = ComboBoxState.Pressed;
				}
				else
				{
					if (this.filtered)
					{
						state = ComboBoxState.Hot;
					}
				}
				ComboBoxRenderer.DrawDropDownButton(graphics, dropDownButtonBounds, state);
				return;
			}
			int num = 0;
			PushButtonState state2 = PushButtonState.Normal;
			if (this.dropDownListBoxShowing)
			{
				state2 = PushButtonState.Pressed;
				num = 1;
			}
			ButtonRenderer.DrawButton(graphics, dropDownButtonBounds, state2);
			if (this.filtered)
			{
				graphics.DrawPolygon(SystemPens.ControlText, new Point[]
				{
					new Point(dropDownButtonBounds.Width / 2 + dropDownButtonBounds.Left - 1 + num, dropDownButtonBounds.Height * 3 / 4 + dropDownButtonBounds.Top - 1 + num),
					new Point(dropDownButtonBounds.Width / 4 + dropDownButtonBounds.Left + num, dropDownButtonBounds.Height / 2 + dropDownButtonBounds.Top - 1 + num),
					new Point(dropDownButtonBounds.Width * 3 / 4 + dropDownButtonBounds.Left - 1 + num, dropDownButtonBounds.Height / 2 + dropDownButtonBounds.Top - 1 + num)
				});
				return;
			}
			graphics.FillPolygon(SystemBrushes.ControlText, new Point[]
			{
				new Point(dropDownButtonBounds.Width / 2 + dropDownButtonBounds.Left - 1 + num, dropDownButtonBounds.Height * 3 / 4 + dropDownButtonBounds.Top - 1 + num),
				new Point(dropDownButtonBounds.Width / 4 + dropDownButtonBounds.Left + num, dropDownButtonBounds.Height / 2 + dropDownButtonBounds.Top - 1 + num),
				new Point(dropDownButtonBounds.Width * 3 / 4 + dropDownButtonBounds.Left - 1 + num, dropDownButtonBounds.Height / 2 + dropDownButtonBounds.Top - 1 + num)
			});
		}
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			Debug.Assert(base.DataGridView != null, "DataGridView is null");
			if (this.lostFocusOnDropDownButtonClick)
			{
				this.lostFocusOnDropDownButtonClick = false;
				return;
			}
			Rectangle cellDisplayRectangle = base.DataGridView.GetCellDisplayRectangle(e.ColumnIndex, -1, false);
			if (base.OwningColumn.Resizable == DataGridViewTriState.True && ((base.DataGridView.RightToLeft == RightToLeft.No && cellDisplayRectangle.Width - e.X < 6) || e.X < 6))
			{
				return;
			}
			int num = 0;
			if (base.DataGridView.RightToLeft == RightToLeft.No && base.DataGridView.FirstDisplayedScrollingColumnIndex == base.ColumnIndex)
			{
				num = base.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
			}
			if (this.FilteringEnabled && this.DropDownButtonBounds.Contains(e.X + cellDisplayRectangle.Left - num, e.Y + cellDisplayRectangle.Top))
			{
				if (base.DataGridView.IsCurrentCellInEditMode)
				{
					base.DataGridView.EndEdit();
					BindingSource bindingSource = base.DataGridView.DataSource as BindingSource;
					if (bindingSource != null)
					{
						bindingSource.EndEdit();
					}
				}
				this.ShowDropDownList();
			}
			else
			{
				if (this.AutomaticSortingEnabled && base.DataGridView.SelectionMode != DataGridViewSelectionMode.ColumnHeaderSelect)
				{
					this.SortByColumn();
				}
			}
			base.OnMouseDown(e);
		}
		private void SortByColumn()
		{
			Debug.Assert(base.DataGridView != null && base.OwningColumn != null, "DataGridView or OwningColumn is null");
			IBindingList bindingList = base.DataGridView.DataSource as IBindingList;
			if (bindingList == null || !bindingList.SupportsSorting || !this.AutomaticSortingEnabled)
			{
				return;
			}
			ListSortDirection direction = ListSortDirection.Ascending;
			if (base.DataGridView.SortedColumn == base.OwningColumn && base.DataGridView.SortOrder == SortOrder.Ascending)
			{
				direction = ListSortDirection.Descending;
			}
			base.DataGridView.Sort(base.OwningColumn, direction);
		}
		public void ShowDropDownList()
		{
			Debug.Assert(base.DataGridView != null, "DataGridView is null");
			if (base.DataGridView.CurrentRow != null && base.DataGridView.CurrentRow.IsNewRow)
			{
				base.DataGridView.CurrentCell = null;
			}
			this.PopulateFilters();
			string[] array = new string[this.filters.Count];
			this.filters.Keys.CopyTo(array, 0);
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Items.Clear();
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Items.AddRange(array);
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.SelectedItem = this.selectedFilterValue;
			this.HandleDropDownListBoxEvents();
			this.SetDropDownListBoxBounds();
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Visible = true;
			this.dropDownListBoxShowing = true;
			Debug.Assert(DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Parent == null, "ShowDropDownListBox has been called multiple times before HideDropDownListBox");
			base.DataGridView.Controls.Add(DataGridViewAutoFilterColumnHeaderCell.dropDownListBox);
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Focus();
			base.DataGridView.InvalidateCell(this);
		}
		public void HideDropDownList()
		{
			Debug.Assert(base.DataGridView != null, "DataGridView is null");
			this.dropDownListBoxShowing = false;
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Visible = false;
			this.UnhandleDropDownListBoxEvents();
			base.DataGridView.Controls.Remove(DataGridViewAutoFilterColumnHeaderCell.dropDownListBox);
			base.DataGridView.InvalidateCell(this);
		}
		private void SetDropDownListBoxBounds()
		{
			Debug.Assert(this.filters.Count > 0, "filters.Count <= 0");
			int num = 2;
			int num2 = 0;
			using (Graphics graphics = DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.CreateGraphics())
			{
				foreach (string text in this.filters.Keys)
				{
					SizeF sizeF = graphics.MeasureString(text, DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Font);
					num += (int)sizeF.Height;
					int num3 = (int)sizeF.Width;
					if (num2 < num3)
					{
						num2 = num3;
					}
				}
			}
			num2 += 6;
			if (num > this.DropDownListBoxMaxHeightInternal)
			{
				num = this.DropDownListBoxMaxHeightInternal;
				num2 += SystemInformation.VerticalScrollBarWidth;
			}
			int num4;
			if (base.DataGridView.RightToLeft == RightToLeft.No)
			{
				num4 = this.DropDownButtonBounds.Right - num2 + 1;
			}
			else
			{
				num4 = this.DropDownButtonBounds.Left - 1;
			}
			int num5 = 1;
			int num6 = base.DataGridView.ClientRectangle.Right;
			if (base.DataGridView.DisplayedRowCount(false) < base.DataGridView.RowCount)
			{
				if (base.DataGridView.RightToLeft == RightToLeft.Yes)
				{
					num5 += SystemInformation.VerticalScrollBarWidth;
				}
				else
				{
					num6 -= SystemInformation.VerticalScrollBarWidth;
				}
			}
			if (num4 < num5)
			{
				num4 = num5;
			}
			int num7 = num4 + num2 + 1;
			if (num7 > num6)
			{
				if (num4 == num5)
				{
					num2 -= num7 - num6;
				}
				else
				{
					num4 -= num7 - num6;
					if (num4 < num5)
					{
						num2 -= num5 - num4;
						num4 = num5;
					}
				}
			}
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.Bounds = new Rectangle(num4, this.DropDownButtonBounds.Bottom, num2, num);
		}
		private void HandleDropDownListBoxEvents()
		{
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.MouseClick += new MouseEventHandler(this.DropDownListBox_MouseClick);
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.LostFocus += new EventHandler(this.DropDownListBox_LostFocus);
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.KeyDown += new KeyEventHandler(this.DropDownListBox_KeyDown);
		}
		private void UnhandleDropDownListBoxEvents()
		{
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.MouseClick -= new MouseEventHandler(this.DropDownListBox_MouseClick);
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.LostFocus -= new EventHandler(this.DropDownListBox_LostFocus);
			DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.KeyDown -= new KeyEventHandler(this.DropDownListBox_KeyDown);
		}
		private void DropDownListBox_MouseClick(object sender, MouseEventArgs e)
		{
			Debug.Assert(base.DataGridView != null, "DataGridView is null");
			if (!DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.DisplayRectangle.Contains(e.X, e.Y))
			{
				return;
			}
			this.UpdateFilter();
			this.HideDropDownList();
		}
		private void DropDownListBox_LostFocus(object sender, EventArgs e)
		{
			if (this.DropDownButtonBounds.Contains(base.DataGridView.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y))))
			{
				this.lostFocusOnDropDownButtonClick = true;
			}
			this.HideDropDownList();
		}
		private void DropDownListBox_KeyDown(object sender, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode == Keys.Return)
			{
				this.UpdateFilter();
				this.HideDropDownList();
				return;
			}
			if (keyCode != Keys.Escape)
			{
				return;
			}
			this.HideDropDownList();
		}
		private void PopulateFilters()
		{
			if (base.DataGridView == null)
			{
				return;
			}
			BindingSource bindingSource = base.DataGridView.DataSource as BindingSource;
			Debug.Assert(bindingSource != null && bindingSource.SupportsFiltering && base.OwningColumn != null, "DataSource is not a BindingSource, or does not support filtering, or OwningColumn is null");
			bindingSource.RaiseListChangedEvents = false;
			string filter = bindingSource.Filter;
			bindingSource.Filter = this.FilterWithoutCurrentColumn(filter);
			this.filters.Clear();
			bool flag = false;
			bool flag2 = false;
			ArrayList arrayList = new ArrayList(bindingSource.Count);
			foreach (object current in bindingSource)
			{
				object obj = null;
				ICustomTypeDescriptor customTypeDescriptor = current as ICustomTypeDescriptor;
				if (customTypeDescriptor != null)
				{
					PropertyDescriptorCollection properties = customTypeDescriptor.GetProperties();
					IEnumerator enumerator2 = properties.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)enumerator2.Current;
							if (string.Compare(base.OwningColumn.DataPropertyName, propertyDescriptor.Name, true, CultureInfo.InvariantCulture) == 0)
							{
								obj = propertyDescriptor.GetValue(current);
								break;
							}
						}
						goto IL_164;
					}
					finally
					{
						IDisposable disposable = enumerator2 as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					goto IL_109;
				}
				goto IL_109;
				IL_164:
				if (obj == null || obj == DBNull.Value)
				{
					flag = true;
					continue;
				}
				if (!arrayList.Contains(obj))
				{
					arrayList.Add(obj);
					continue;
				}
				continue;
				IL_109:
				PropertyInfo[] properties2 = current.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
				PropertyInfo[] array = properties2;
				for (int i = 0; i < array.Length; i++)
				{
					PropertyInfo propertyInfo = array[i];
					if (string.Compare(base.OwningColumn.DataPropertyName, propertyInfo.Name, true, CultureInfo.InvariantCulture) == 0)
					{
						obj = propertyInfo.GetValue(current, null);
						break;
					}
				}
				goto IL_164;
			}
			arrayList.Sort();
			foreach (object current2 in arrayList)
			{
				DataGridViewCellStyle inheritedStyle = base.OwningColumn.InheritedStyle;
				string text = (string)this.GetFormattedValue(current2, -1, ref inheritedStyle, null, null, DataGridViewDataErrorContexts.Formatting);
				if (string.IsNullOrEmpty(text))
				{
					flag = true;
				}
				else
				{
					if (!this.filters.Contains(text))
					{
						flag2 = true;
						this.filters.Add(text, current2.ToString());
					}
				}
			}
			if (filter != null)
			{
				bindingSource.Filter = filter;
			}
			bindingSource.RaiseListChangedEvents = true;
			this.filters.Insert(0, "(全部)", null);
			if (flag && flag2)
			{
				this.filters.Add("(空白值)", null);
				this.filters.Add("(非空白值)", null);
			}
		}
		private string FilterWithoutCurrentColumn(string filter)
		{
			if (string.IsNullOrEmpty(filter))
			{
				return string.Empty;
			}
			if (!this.filtered)
			{
				return filter;
			}
			if (filter.IndexOf(this.currentColumnFilter) > 0)
			{
				return filter.Replace(" AND " + this.currentColumnFilter, string.Empty);
			}
			if (filter.Length > this.currentColumnFilter.Length)
			{
				return filter.Replace(this.currentColumnFilter + " AND ", string.Empty);
			}
			return string.Empty;
		}
		private void UpdateFilter()
		{
			if (DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.SelectedItem.ToString().Equals(this.selectedFilterValue))
			{
				return;
			}
			this.selectedFilterValue = DataGridViewAutoFilterColumnHeaderCell.dropDownListBox.SelectedItem.ToString();
			IBindingListView bindingListView = base.DataGridView.DataSource as IBindingListView;
			Debug.Assert(bindingListView != null && bindingListView.SupportsFiltering, "DataSource is not an IBindingListView or does not support filtering");
			if (this.selectedFilterValue.Equals("(全部)"))
			{
				bindingListView.Filter = this.FilterWithoutCurrentColumn(bindingListView.Filter);
				this.filtered = false;
				this.currentColumnFilter = string.Empty;
				return;
			}
			string text = null;
			string arg = base.OwningColumn.DataPropertyName.Replace("]", "\\]");
			string a;
			if ((a = this.selectedFilterValue) != null)
			{
				if (a == "(空白值)")
				{
					text = string.Format("LEN(ISNULL(CONVERT([{0}],'System.String'),''))=0", arg);
					goto IL_123;
				}
				if (a == "(非空白值)")
				{
					text = string.Format("LEN(ISNULL(CONVERT([{0}],'System.String'),''))>0", arg);
					goto IL_123;
				}
			}
			text = string.Format("[{0}]='{1}'", arg, ((string)this.filters[this.selectedFilterValue]).Replace("'", "''"));
			IL_123:
			string text2 = this.FilterWithoutCurrentColumn(bindingListView.Filter);
			if (string.IsNullOrEmpty(text2))
			{
				text2 += text;
			}
			else
			{
				text2 = text2 + " AND " + text;
			}
			try
			{
				bindingListView.Filter = text2;
			}
			catch (InvalidExpressionException innerException)
			{
				throw new NotSupportedException("Invalid expression: " + text2, innerException);
			}
			this.filtered = true;
			this.currentColumnFilter = text;
		}
		public static void RemoveFilter(DataGridView dataGridView)
		{
			if (dataGridView == null)
			{
				throw new ArgumentNullException("dataGridView");
			}
			BindingSource bindingSource = dataGridView.DataSource as BindingSource;
			if (bindingSource == null || bindingSource.DataSource == null || !bindingSource.SupportsFiltering)
			{
				throw new ArgumentException("The DataSource property of the specified DataGridView is not set to a BindingSource with a SupportsFiltering property value of true.");
			}
			if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.IsNewRow)
			{
				dataGridView.CurrentCell = null;
			}
			bindingSource.Filter = null;
		}
		public static string GetFilterStatus(DataGridView dataGridView)
		{
			if (dataGridView == null)
			{
				throw new ArgumentNullException("dataGridView");
			}
			BindingSource bindingSource = dataGridView.DataSource as BindingSource;
			if (string.IsNullOrEmpty(bindingSource.Filter) || bindingSource == null || bindingSource.DataSource == null || !bindingSource.SupportsFiltering)
			{
				return string.Empty;
			}
			int count = bindingSource.Count;
			bindingSource.RaiseListChangedEvents = false;
			string filter = bindingSource.Filter;
			bindingSource.Filter = null;
			int count2 = bindingSource.Count;
			bindingSource.Filter = filter;
			bindingSource.RaiseListChangedEvents = true;
			Debug.Assert(count <= count2, "current count is greater than unfiltered count");
			if (count == count2)
			{
				return string.Empty;
			}
			return string.Format("{0} of {1} records found", count, count2);
		}
		private void InvalidateDropDownButtonBounds()
		{
			if (!this.dropDownButtonBoundsValue.IsEmpty)
			{
				this.dropDownButtonBoundsValue = Rectangle.Empty;
			}
		}
		private void SetDropDownButtonBounds()
		{
			Rectangle cellDisplayRectangle = base.DataGridView.GetCellDisplayRectangle(base.ColumnIndex, -1, false);
			int num = base.InheritedStyle.Font.Height + 5;
			Rectangle rectangle = this.BorderWidths(base.DataGridView.AdjustColumnHeaderBorderStyle(base.DataGridView.AdvancedColumnHeadersBorderStyle, new DataGridViewAdvancedBorderStyle(), false, false));
			int num2 = 2 + rectangle.Top + rectangle.Height + base.InheritedStyle.Padding.Vertical;
			bool flag = Application.RenderWithVisualStyles && base.DataGridView.EnableHeadersVisualStyles;
			if (flag)
			{
				num2 += 3;
			}
			if (num > base.DataGridView.ColumnHeadersHeight - num2)
			{
				num = base.DataGridView.ColumnHeadersHeight - num2;
			}
			if (num > cellDisplayRectangle.Width - 3)
			{
				num = cellDisplayRectangle.Width - 3;
			}
			int num3 = flag ? 4 : 1;
			int y = cellDisplayRectangle.Bottom - num - num3;
			int num4 = flag ? 3 : 1;
			int x;
			if (base.DataGridView.RightToLeft == RightToLeft.No)
			{
				x = cellDisplayRectangle.Right - num - num4;
			}
			else
			{
				x = cellDisplayRectangle.Left + num4;
			}
			this.dropDownButtonBoundsValue = new Rectangle(x, y, num, num);
			this.AdjustPadding(num + num4);
		}
		private void AdjustPadding(int newDropDownButtonPaddingOffset)
		{
			int num = newDropDownButtonPaddingOffset - this.currentDropDownButtonPaddingOffset;
			if (num != 0)
			{
				this.currentDropDownButtonPaddingOffset = newDropDownButtonPaddingOffset;
				Padding p = new Padding(0, 0, num, 0);
				base.Style.Padding = Padding.Add(base.InheritedStyle.Padding, p);
			}
		}
	}
}
