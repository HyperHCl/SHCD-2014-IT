using System;
using System.Drawing;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class QisiTreeView : TreeView
	{
		public QisiTreeView()
		{
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.DrawMode = TreeViewDrawMode.OwnerDrawText;
			base.DrawNode += new DrawTreeNodeEventHandler(this.PaperTree_DrawNode);
			base.HideSelection = false;
			base.HotTracking = true;
			this.BackColor = Color.White;
		}
		private void PaperTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			StringFormat genericTypographic = StringFormat.GenericTypographic;
			genericTypographic.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
			{
				Font font = e.Node.NodeFont;
				if (font == null)
				{
					font = this.Font;
				}
				e.Graphics.PageUnit = font.Unit;
				SizeF size = e.Graphics.MeasureString(e.Node.Text, font, 0, genericTypographic);
				RectangleF layoutRectangle = new RectangleF(e.Node.Bounds.Location, size);
				e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
				e.Graphics.DrawString(e.Node.Text, font, Brushes.White, layoutRectangle, genericTypographic);
			}
			else
			{
				if ((e.State & TreeNodeStates.Hot) == TreeNodeStates.Hot)
				{
					Font font2 = e.Node.NodeFont;
					if (font2 == null)
					{
						font2 = this.Font;
					}
					e.Graphics.PageUnit = font2.Unit;
					SizeF size2 = e.Graphics.MeasureString(e.Node.Text, font2, 0, genericTypographic);
					RectangleF layoutRectangle2 = new RectangleF(e.Node.Bounds.Location, size2);
					e.Graphics.FillRectangle(SystemBrushes.HotTrack, e.Bounds);
					e.Graphics.DrawString(e.Node.Text, font2, Brushes.White, layoutRectangle2, genericTypographic);
				}
				else
				{
					Font font3 = e.Node.NodeFont;
					if (font3 == null)
					{
						font3 = this.Font;
					}
					e.Graphics.PageUnit = font3.Unit;
					SizeF size3 = e.Graphics.MeasureString(e.Node.Text, font3, 0, genericTypographic);
					RectangleF layoutRectangle3 = new RectangleF(e.Node.Bounds.Location, size3);
					e.Graphics.FillRectangle(Brushes.White, e.Bounds);
					e.Graphics.DrawString(e.Node.Text, font3, Brushes.Black, layoutRectangle3, genericTypographic);
				}
			}
			genericTypographic.Dispose();
		}
	}
}
