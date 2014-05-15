using System;
using System.Collections.Generic;
using System.Drawing;
namespace Qisi.Editor.Expression
{
	internal abstract class imageexpression : structexpression
	{
		protected Image fuhao;
		protected SizeF GSize
		{
			get
			{
				return new SizeF((float)this.fuhao.Width * this.Font.Size / 70f, (float)this.fuhao.Height * this.Font.Size / 70f);
			}
		}
		protected float FuhaoBaseLine
		{
			get;
			set;
		}
		protected PointF FuhaoLoc
		{
			get;
			set;
		}
		protected lineexpression Benti
		{
			get
			{
				return base.Child[base.Child.Count - 1];
			}
		}
		protected imageexpression(Image f, lineexpression parent, Color color) : base(parent, color, true)
		{
			this.fuhao = f;
			base.Child = new List<lineexpression>();
			base.Child.Add(new lineexpression(this.Font));
			this.Benti.ParentExpression = this;
		}
	}
}
