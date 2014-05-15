using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace SHCD
{
	public class Test : MdiBase
	{
		private Key currentKey;
		private Keys[] Klist = new Keys[]
		{
			Keys.D1,
			Keys.D2,
			Keys.D3,
			Keys.D4,
			Keys.D5,
			Keys.D6,
			Keys.D7,
			Keys.D8,
			Keys.D9,
			Keys.D0,
			Keys.Q,
			Keys.W,
			Keys.E,
			Keys.R,
			Keys.T,
			Keys.Y,
			Keys.U,
			Keys.I,
			Keys.O,
			Keys.P,
			Keys.A,
			Keys.S,
			Keys.D,
			Keys.F,
			Keys.G,
			Keys.H,
			Keys.J,
			Keys.K,
			Keys.L,
			Keys.Z,
			Keys.X,
			Keys.C,
			Keys.V,
			Keys.B,
			Keys.N,
			Keys.M
		};
		private List<Keys> myList = new List<Keys>();
		private IContainer components = null;
		private Label label5;
		private Label label4;
		private Label label3;
		private Label label2;
		private Key key1;
		private Key key2;
		private Key key3;
		private Key key4;
		private Key key5;
		private Key key6;
		private Key key7;
		private Key key8;
		private Key key9;
		private Key key10;
		private Key key11;
		private Key key12;
		private Key key13;
		private Key key14;
		private Key key15;
		private Key key16;
		private Key key17;
		private Key key18;
		private Key key19;
		private Key key20;
		private Key key21;
		private Key key22;
		private Key key23;
		private Key key24;
		private Key key25;
		private Key key26;
		private Key key27;
		private Key key28;
		private Key key29;
		private Key key30;
		private Key key31;
		private Key key32;
		private Key key33;
		private Key key34;
		private Key key35;
		private Key key36;
		private Key key37;
		private Key key38;
		private Key key39;
		private Key key40;
		private Key key41;
		private Key key42;
		private Key key43;
		private Key key44;
		private Key key45;
		private Key key46;
		private Key key47;
		private Key key48;
		private Key key49;
		private Key key52;
		private Key key53;
		private Key key51;
		private Key key54;
		private Key key55;
		private Key key56;
		private Key key58;
		private Key key59;
		private Key key60;
		private Key key61;
		private Key key62;
		private Key key50;
		private Label label1;
		private PictureBox pictureBox1;
		private TableLayoutPanel tableLayoutPanel1;
		private Button button1;
		private Timer timer1;
		public event EventHandler Exit;
		public event EventHandler Complete;
		public string stuID
		{
			get
			{
				return this.label3.Text;
			}
			set
			{
				this.label3.Text = value;
			}
		}
		public string stuName
		{
			get
			{
				return this.label5.Text;
			}
			set
			{
				this.label5.Text = value;
			}
		}
		public Test()
		{
			this.InitializeComponent();
			this.currentKey = new Key();
			this.currentKey = this.key2;
			Keys[] klist = this.Klist;
			for (int i = 0; i < klist.Length; i++)
			{
				Keys item = klist[i];
				this.myList.Add(item);
			}
			this.key50.KeyText = this.currentKey.KeyText;
			this.key50.KeyValue = this.currentKey.KeyValue;
			this.currentKey.BackColor = this.key50.BackColor;
			foreach (Control control in base.Controls)
			{
				control.KeyDown += new KeyEventHandler(this.c_KeyDown);
				control.TabStop = true;
			}
			foreach (Control control in this.tableLayoutPanel1.Controls)
			{
				control.KeyDown += new KeyEventHandler(this.c_KeyDown);
				control.TabStop = true;
			}
			if (base.Parent != null)
			{
				base.Parent.KeyDown += new KeyEventHandler(this.c_KeyDown);
			}
			base.KeyDown += new KeyEventHandler(this.c_KeyDown);
			base.LostFocus += new EventHandler(this.Test_LostFocus);
		}
		private void Test_LostFocus(object sender, EventArgs e)
		{
			base.Focus();
		}
		private void key1_Load(object sender, EventArgs e)
		{
		}
		private void c_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			foreach (Control control in base.Controls)
			{
				if (control.GetType().ToString() == "SHCD.Key" && ((Key)control).KeyValue != this.currentKey.KeyValue && ((Key)control).BackColor != Color.White)
				{
					((Key)control).BackColor = Color.White;
				}
			}
			if (e.KeyData == this.currentKey.KeyValue)
			{
				this.NextKey();
			}
			else
			{
				foreach (Control control in base.Controls)
				{
					if (control.GetType().ToString() == "SHCD.Key" && ((Key)control).KeyValue == e.KeyData)
					{
						((Key)control).BackColor = Color.Pink;
					}
				}
			}
		}
		private void NextKey()
		{
			int num = this.myList.IndexOf(this.currentKey.KeyValue);
			if (num == this.myList.Count - 1)
			{
				base.Visible = false;
				this.Complete(this, new EventArgs());
			}
			if (num < this.myList.Count - 1)
			{
				foreach (Control control in base.Controls)
				{
					if (control.GetType().ToString() == "SHCD.Key" && ((Key)control).KeyValue == this.myList[num + 1])
					{
						this.currentKey.BackColor = Color.White;
						this.currentKey = (Key)control;
						this.key50.KeyText = this.currentKey.KeyText;
						this.key50.KeyValue = this.currentKey.KeyValue;
						this.key50.Refresh();
						this.currentKey.BackColor = this.key50.BackColor;
						break;
					}
				}
			}
		}
		private void Test_Load(object sender, EventArgs e)
		{
			if (base.Parent != null)
			{
				base.Parent.KeyDown += new KeyEventHandler(this.c_KeyDown);
			}
		}
		private void Label2_Click(object sender, EventArgs e)
		{
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this.Exit(this, new EventArgs());
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (!base.Parent.Focused)
				{
					base.Parent.BringToFront();
				}
				if (!this.Focused)
				{
					base.Focus();
				}
			}
			catch
			{
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
			this.components = new Container();
			this.key62 = new Key();
			this.key61 = new Key();
			this.key59 = new Key();
			this.key56 = new Key();
			this.key55 = new Key();
			this.key54 = new Key();
			this.key51 = new Key();
			this.key53 = new Key();
			this.key52 = new Key();
			this.key49 = new Key();
			this.key48 = new Key();
			this.key47 = new Key();
			this.key46 = new Key();
			this.key45 = new Key();
			this.key44 = new Key();
			this.key43 = new Key();
			this.key42 = new Key();
			this.key41 = new Key();
			this.key40 = new Key();
			this.key39 = new Key();
			this.key38 = new Key();
			this.key37 = new Key();
			this.key36 = new Key();
			this.key35 = new Key();
			this.key34 = new Key();
			this.key33 = new Key();
			this.key32 = new Key();
			this.key31 = new Key();
			this.key30 = new Key();
			this.key29 = new Key();
			this.key28 = new Key();
			this.key22 = new Key();
			this.key23 = new Key();
			this.key24 = new Key();
			this.key25 = new Key();
			this.key26 = new Key();
			this.key27 = new Key();
			this.key21 = new Key();
			this.key20 = new Key();
			this.key19 = new Key();
			this.key18 = new Key();
			this.key17 = new Key();
			this.key16 = new Key();
			this.key15 = new Key();
			this.key14 = new Key();
			this.key13 = new Key();
			this.key12 = new Key();
			this.key11 = new Key();
			this.key10 = new Key();
			this.key9 = new Key();
			this.key8 = new Key();
			this.key7 = new Key();
			this.key6 = new Key();
			this.key5 = new Key();
			this.key4 = new Key();
			this.key3 = new Key();
			this.key2 = new Key();
			this.key1 = new Key();
			this.key60 = new Key();
			this.key58 = new Key();
			this.key50 = new Key();
			this.label1 = new Label();
			this.pictureBox1 = new PictureBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.label2 = new Label();
			this.label3 = new Label();
			this.label4 = new Label();
			this.label5 = new Label();
			this.button1 = new Button();
			this.timer1 = new Timer(this.components);
			((ISupportInitialize)this.pictureBox1).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.key62.BackColor = Color.White;
			this.key62.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key62.KeyText = "";
			this.key62.KeyValue = Keys.Space;
			this.key62.Location = new Point(157, 366);
			this.key62.Margin = new Padding(0);
			this.key62.myType = 1;
			this.key62.Name = "key62";
			this.key62.one = true;
			this.key62.Size = new Size(320, 40);
			this.key62.TabIndex = 62;
			this.key61.BackColor = Color.White;
			this.key61.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key61.KeyText = "Alt";
			this.key61.KeyValue = (Keys.RButton | Keys.ShiftKey | Keys.Alt);
			this.key61.Location = new Point(477, 366);
			this.key61.Margin = new Padding(0);
			this.key61.myType = 1;
			this.key61.Name = "key61";
			this.key61.one = true;
			this.key61.Size = new Size(40, 40);
			this.key61.TabIndex = 61;
			this.key59.BackColor = Color.White;
			this.key59.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key59.KeyText = "Alt";
			this.key59.KeyValue = (Keys.RButton | Keys.ShiftKey | Keys.Alt);
			this.key59.Location = new Point(117, 366);
			this.key59.Margin = new Padding(0);
			this.key59.myType = 1;
			this.key59.Name = "key59";
			this.key59.one = true;
			this.key59.Size = new Size(40, 40);
			this.key59.TabIndex = 59;
			this.key56.BackColor = Color.White;
			this.key56.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key56.KeyText = "Ctrl";
			this.key56.KeyValue = (Keys.LButton | Keys.ShiftKey | Keys.Control);
			this.key56.Location = new Point(559, 366);
			this.key56.Margin = new Padding(0);
			this.key56.myType = 1;
			this.key56.Name = "key56";
			this.key56.one = true;
			this.key56.Size = new Size(60, 40);
			this.key56.TabIndex = 56;
			this.key55.BackColor = Color.White;
			this.key55.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key55.KeyText = "Ctrl";
			this.key55.KeyValue = (Keys.LButton | Keys.ShiftKey | Keys.Control);
			this.key55.Location = new Point(17, 366);
			this.key55.Margin = new Padding(0);
			this.key55.myType = 1;
			this.key55.Name = "key55";
			this.key55.one = true;
			this.key55.Size = new Size(60, 40);
			this.key55.TabIndex = 55;
			this.key54.BackColor = Color.White;
			this.key54.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key54.KeyText = "↑Shift";
			this.key54.KeyValue = (Keys.ShiftKey | Keys.Shift);
			this.key54.Location = new Point(509, 326);
			this.key54.Margin = new Padding(0);
			this.key54.myType = 1;
			this.key54.Name = "key54";
			this.key54.one = true;
			this.key54.Size = new Size(110, 40);
			this.key54.TabIndex = 54;
			this.key51.BackColor = Color.White;
			this.key51.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key51.KeyText = "?/";
			this.key51.KeyValue = Keys.OemQuestion;
			this.key51.Location = new Point(469, 326);
			this.key51.Margin = new Padding(0);
			this.key51.myType = 1;
			this.key51.Name = "key51";
			this.key51.one = false;
			this.key51.Size = new Size(40, 40);
			this.key51.TabIndex = 53;
			this.key53.BackColor = Color.White;
			this.key53.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key53.KeyText = ">.";
			this.key53.KeyValue = Keys.OemPeriod;
			this.key53.Location = new Point(429, 326);
			this.key53.Margin = new Padding(0);
			this.key53.myType = 1;
			this.key53.Name = "key53";
			this.key53.one = false;
			this.key53.Size = new Size(40, 40);
			this.key53.TabIndex = 52;
			this.key52.BackColor = Color.White;
			this.key52.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key52.KeyText = "<,";
			this.key52.KeyValue = Keys.Oemcomma;
			this.key52.Location = new Point(389, 326);
			this.key52.Margin = new Padding(0);
			this.key52.myType = 1;
			this.key52.Name = "key52";
			this.key52.one = false;
			this.key52.Size = new Size(40, 40);
			this.key52.TabIndex = 51;
			this.key49.BackColor = Color.White;
			this.key49.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key49.KeyText = "M";
			this.key49.KeyValue = Keys.M;
			this.key49.Location = new Point(349, 326);
			this.key49.Margin = new Padding(0);
			this.key49.myType = 1;
			this.key49.Name = "key49";
			this.key49.one = false;
			this.key49.Size = new Size(40, 40);
			this.key49.TabIndex = 48;
			this.key48.BackColor = Color.White;
			this.key48.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key48.KeyText = "N";
			this.key48.KeyValue = Keys.N;
			this.key48.Location = new Point(309, 326);
			this.key48.Margin = new Padding(0);
			this.key48.myType = 1;
			this.key48.Name = "key48";
			this.key48.one = false;
			this.key48.Size = new Size(40, 40);
			this.key48.TabIndex = 47;
			this.key47.BackColor = Color.White;
			this.key47.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key47.KeyText = "B";
			this.key47.KeyValue = Keys.B;
			this.key47.Location = new Point(269, 326);
			this.key47.Margin = new Padding(0);
			this.key47.myType = 1;
			this.key47.Name = "key47";
			this.key47.one = false;
			this.key47.Size = new Size(40, 40);
			this.key47.TabIndex = 46;
			this.key46.BackColor = Color.White;
			this.key46.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key46.KeyText = "V";
			this.key46.KeyValue = Keys.V;
			this.key46.Location = new Point(229, 326);
			this.key46.Margin = new Padding(0);
			this.key46.myType = 1;
			this.key46.Name = "key46";
			this.key46.one = false;
			this.key46.Size = new Size(40, 40);
			this.key46.TabIndex = 45;
			this.key45.BackColor = Color.White;
			this.key45.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key45.KeyText = "C";
			this.key45.KeyValue = Keys.C;
			this.key45.Location = new Point(189, 326);
			this.key45.Margin = new Padding(0);
			this.key45.myType = 1;
			this.key45.Name = "key45";
			this.key45.one = false;
			this.key45.Size = new Size(40, 40);
			this.key45.TabIndex = 44;
			this.key44.BackColor = Color.White;
			this.key44.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key44.KeyText = "↑Shift";
			this.key44.KeyValue = (Keys.ShiftKey | Keys.Shift);
			this.key44.Location = new Point(17, 326);
			this.key44.Margin = new Padding(0);
			this.key44.myType = 1;
			this.key44.Name = "key44";
			this.key44.one = true;
			this.key44.Size = new Size(92, 40);
			this.key44.TabIndex = 43;
			this.key43.BackColor = Color.White;
			this.key43.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key43.KeyText = "←┘Enter";
			this.key43.KeyValue = Keys.Return;
			this.key43.Location = new Point(532, 286);
			this.key43.Margin = new Padding(0);
			this.key43.myType = 1;
			this.key43.Name = "key43";
			this.key43.one = true;
			this.key43.Size = new Size(87, 40);
			this.key43.TabIndex = 42;
			this.key42.BackColor = Color.White;
			this.key42.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key42.KeyText = "\"'";
			this.key42.KeyValue = Keys.OemQuotes;
			this.key42.Location = new Point(492, 286);
			this.key42.Margin = new Padding(0);
			this.key42.myType = 1;
			this.key42.Name = "key42";
			this.key42.one = false;
			this.key42.Size = new Size(40, 40);
			this.key42.TabIndex = 41;
			this.key41.BackColor = Color.White;
			this.key41.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key41.KeyText = ":;";
			this.key41.KeyValue = Keys.OemSemicolon;
			this.key41.Location = new Point(452, 286);
			this.key41.Margin = new Padding(0);
			this.key41.myType = 1;
			this.key41.Name = "key41";
			this.key41.one = false;
			this.key41.Size = new Size(40, 40);
			this.key41.TabIndex = 40;
			this.key40.BackColor = Color.White;
			this.key40.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key40.KeyText = "X";
			this.key40.KeyValue = Keys.X;
			this.key40.Location = new Point(149, 326);
			this.key40.Margin = new Padding(0);
			this.key40.myType = 1;
			this.key40.Name = "key40";
			this.key40.one = false;
			this.key40.Size = new Size(40, 40);
			this.key40.TabIndex = 39;
			this.key39.BackColor = Color.White;
			this.key39.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key39.KeyText = "Z";
			this.key39.KeyValue = Keys.Z;
			this.key39.Location = new Point(109, 326);
			this.key39.Margin = new Padding(0);
			this.key39.myType = 1;
			this.key39.Name = "key39";
			this.key39.one = false;
			this.key39.Size = new Size(40, 40);
			this.key39.TabIndex = 38;
			this.key38.BackColor = Color.White;
			this.key38.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key38.KeyText = "L";
			this.key38.KeyValue = Keys.L;
			this.key38.Location = new Point(412, 286);
			this.key38.Margin = new Padding(0);
			this.key38.myType = 1;
			this.key38.Name = "key38";
			this.key38.one = false;
			this.key38.Size = new Size(40, 40);
			this.key38.TabIndex = 37;
			this.key37.BackColor = Color.White;
			this.key37.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key37.KeyText = "K";
			this.key37.KeyValue = Keys.K;
			this.key37.Location = new Point(372, 286);
			this.key37.Margin = new Padding(0);
			this.key37.myType = 1;
			this.key37.Name = "key37";
			this.key37.one = false;
			this.key37.Size = new Size(40, 40);
			this.key37.TabIndex = 36;
			this.key36.BackColor = Color.White;
			this.key36.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key36.KeyText = "J";
			this.key36.KeyValue = Keys.J;
			this.key36.Location = new Point(332, 286);
			this.key36.Margin = new Padding(0);
			this.key36.myType = 1;
			this.key36.Name = "key36";
			this.key36.one = false;
			this.key36.Size = new Size(40, 40);
			this.key36.TabIndex = 35;
			this.key35.BackColor = Color.White;
			this.key35.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key35.KeyText = "H";
			this.key35.KeyValue = Keys.H;
			this.key35.Location = new Point(292, 286);
			this.key35.Margin = new Padding(0);
			this.key35.myType = 1;
			this.key35.Name = "key35";
			this.key35.one = false;
			this.key35.Size = new Size(40, 40);
			this.key35.TabIndex = 34;
			this.key34.BackColor = Color.White;
			this.key34.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key34.KeyText = "G";
			this.key34.KeyValue = Keys.G;
			this.key34.Location = new Point(252, 286);
			this.key34.Margin = new Padding(0);
			this.key34.myType = 1;
			this.key34.Name = "key34";
			this.key34.one = false;
			this.key34.Size = new Size(40, 40);
			this.key34.TabIndex = 33;
			this.key33.BackColor = Color.White;
			this.key33.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key33.KeyText = "F";
			this.key33.KeyValue = Keys.F;
			this.key33.Location = new Point(212, 286);
			this.key33.Margin = new Padding(0);
			this.key33.myType = 1;
			this.key33.Name = "key33";
			this.key33.one = false;
			this.key33.Size = new Size(40, 40);
			this.key33.TabIndex = 32;
			this.key32.BackColor = Color.White;
			this.key32.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key32.KeyText = "D";
			this.key32.KeyValue = Keys.D;
			this.key32.Location = new Point(172, 286);
			this.key32.Margin = new Padding(0);
			this.key32.myType = 1;
			this.key32.Name = "key32";
			this.key32.one = false;
			this.key32.Size = new Size(40, 40);
			this.key32.TabIndex = 31;
			this.key31.BackColor = Color.White;
			this.key31.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key31.KeyText = "CapsLock";
			this.key31.KeyValue = Keys.Capital;
			this.key31.Location = new Point(17, 286);
			this.key31.Margin = new Padding(0);
			this.key31.myType = 1;
			this.key31.Name = "key31";
			this.key31.one = true;
			this.key31.Size = new Size(75, 40);
			this.key31.TabIndex = 30;
			this.key30.BackColor = Color.White;
			this.key30.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key30.KeyText = "|\\";
			this.key30.KeyValue = Keys.OemPipe;
			this.key30.Location = new Point(557, 246);
			this.key30.Margin = new Padding(0);
			this.key30.myType = 1;
			this.key30.Name = "key30";
			this.key30.one = false;
			this.key30.Size = new Size(62, 40);
			this.key30.TabIndex = 29;
			this.key29.BackColor = Color.White;
			this.key29.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key29.KeyText = "}]";
			this.key29.KeyValue = Keys.OemCloseBrackets;
			this.key29.Location = new Point(517, 246);
			this.key29.Margin = new Padding(0);
			this.key29.myType = 1;
			this.key29.Name = "key29";
			this.key29.one = false;
			this.key29.Size = new Size(40, 40);
			this.key29.TabIndex = 28;
			this.key28.BackColor = Color.White;
			this.key28.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key28.KeyText = "{[";
			this.key28.KeyValue = Keys.OemOpenBrackets;
			this.key28.Location = new Point(477, 246);
			this.key28.Margin = new Padding(0);
			this.key28.myType = 1;
			this.key28.Name = "key28";
			this.key28.one = false;
			this.key28.Size = new Size(40, 40);
			this.key28.TabIndex = 27;
			this.key22.BackColor = Color.White;
			this.key22.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key22.KeyText = "S";
			this.key22.KeyValue = Keys.S;
			this.key22.Location = new Point(132, 286);
			this.key22.Margin = new Padding(0);
			this.key22.myType = 1;
			this.key22.Name = "key22";
			this.key22.one = false;
			this.key22.Size = new Size(40, 40);
			this.key22.TabIndex = 26;
			this.key23.BackColor = Color.White;
			this.key23.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key23.KeyText = "A";
			this.key23.KeyValue = Keys.A;
			this.key23.Location = new Point(92, 286);
			this.key23.Margin = new Padding(0);
			this.key23.myType = 1;
			this.key23.Name = "key23";
			this.key23.one = false;
			this.key23.Size = new Size(40, 40);
			this.key23.TabIndex = 25;
			this.key24.BackColor = Color.White;
			this.key24.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key24.KeyText = "P";
			this.key24.KeyValue = Keys.P;
			this.key24.Location = new Point(437, 246);
			this.key24.Margin = new Padding(0);
			this.key24.myType = 1;
			this.key24.Name = "key24";
			this.key24.one = false;
			this.key24.Size = new Size(40, 40);
			this.key24.TabIndex = 24;
			this.key25.BackColor = Color.White;
			this.key25.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key25.KeyText = "O";
			this.key25.KeyValue = Keys.O;
			this.key25.Location = new Point(397, 246);
			this.key25.Margin = new Padding(0);
			this.key25.myType = 1;
			this.key25.Name = "key25";
			this.key25.one = false;
			this.key25.Size = new Size(40, 40);
			this.key25.TabIndex = 23;
			this.key26.BackColor = Color.White;
			this.key26.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key26.KeyText = "I";
			this.key26.KeyValue = Keys.I;
			this.key26.Location = new Point(357, 246);
			this.key26.Margin = new Padding(0);
			this.key26.myType = 1;
			this.key26.Name = "key26";
			this.key26.one = false;
			this.key26.Size = new Size(40, 40);
			this.key26.TabIndex = 22;
			this.key27.BackColor = Color.White;
			this.key27.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key27.KeyText = "U";
			this.key27.KeyValue = Keys.U;
			this.key27.Location = new Point(317, 246);
			this.key27.Margin = new Padding(0);
			this.key27.myType = 1;
			this.key27.Name = "key27";
			this.key27.one = false;
			this.key27.Size = new Size(40, 40);
			this.key27.TabIndex = 21;
			this.key21.BackColor = Color.White;
			this.key21.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key21.KeyText = "Y";
			this.key21.KeyValue = Keys.Y;
			this.key21.Location = new Point(277, 246);
			this.key21.Margin = new Padding(0);
			this.key21.myType = 1;
			this.key21.Name = "key21";
			this.key21.one = false;
			this.key21.Size = new Size(40, 40);
			this.key21.TabIndex = 20;
			this.key20.BackColor = Color.White;
			this.key20.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key20.KeyText = "T";
			this.key20.KeyValue = Keys.T;
			this.key20.Location = new Point(237, 246);
			this.key20.Margin = new Padding(0);
			this.key20.myType = 1;
			this.key20.Name = "key20";
			this.key20.one = false;
			this.key20.Size = new Size(40, 40);
			this.key20.TabIndex = 19;
			this.key19.BackColor = Color.White;
			this.key19.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key19.KeyText = "R";
			this.key19.KeyValue = Keys.R;
			this.key19.Location = new Point(197, 246);
			this.key19.Margin = new Padding(0);
			this.key19.myType = 1;
			this.key19.Name = "key19";
			this.key19.one = false;
			this.key19.Size = new Size(40, 40);
			this.key19.TabIndex = 18;
			this.key18.BackColor = Color.White;
			this.key18.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key18.KeyText = "E";
			this.key18.KeyValue = Keys.E;
			this.key18.Location = new Point(157, 246);
			this.key18.Margin = new Padding(0);
			this.key18.myType = 1;
			this.key18.Name = "key18";
			this.key18.one = false;
			this.key18.Size = new Size(40, 40);
			this.key18.TabIndex = 17;
			this.key17.BackColor = Color.White;
			this.key17.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key17.KeyText = "W";
			this.key17.KeyValue = Keys.W;
			this.key17.Location = new Point(117, 246);
			this.key17.Margin = new Padding(0);
			this.key17.myType = 1;
			this.key17.Name = "key17";
			this.key17.one = false;
			this.key17.Size = new Size(40, 40);
			this.key17.TabIndex = 16;
			this.key16.BackColor = Color.White;
			this.key16.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key16.KeyText = "Q";
			this.key16.KeyValue = Keys.Q;
			this.key16.Location = new Point(77, 246);
			this.key16.Margin = new Padding(0);
			this.key16.myType = 1;
			this.key16.Name = "key16";
			this.key16.one = false;
			this.key16.Size = new Size(40, 40);
			this.key16.TabIndex = 15;
			this.key15.BackColor = Color.White;
			this.key15.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key15.KeyText = "Tab";
			this.key15.KeyValue = Keys.Tab;
			this.key15.Location = new Point(17, 246);
			this.key15.Margin = new Padding(0);
			this.key15.myType = 1;
			this.key15.Name = "key15";
			this.key15.one = true;
			this.key15.Size = new Size(60, 40);
			this.key15.TabIndex = 14;
			this.key14.BackColor = Color.White;
			this.key14.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key14.KeyText = "Backspace";
			this.key14.KeyValue = Keys.Back;
			this.key14.Location = new Point(537, 206);
			this.key14.Margin = new Padding(0);
			this.key14.myType = 1;
			this.key14.Name = "key14";
			this.key14.one = true;
			this.key14.Size = new Size(82, 40);
			this.key14.TabIndex = 13;
			this.key13.BackColor = Color.White;
			this.key13.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key13.KeyText = "+=";
			this.key13.KeyValue = Keys.Oemplus;
			this.key13.Location = new Point(497, 206);
			this.key13.Margin = new Padding(0);
			this.key13.myType = 1;
			this.key13.Name = "key13";
			this.key13.one = false;
			this.key13.Size = new Size(40, 40);
			this.key13.TabIndex = 12;
			this.key12.BackColor = Color.White;
			this.key12.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key12.KeyText = "-_";
			this.key12.KeyValue = Keys.OemMinus;
			this.key12.Location = new Point(457, 206);
			this.key12.Margin = new Padding(0);
			this.key12.myType = 1;
			this.key12.Name = "key12";
			this.key12.one = false;
			this.key12.Size = new Size(40, 40);
			this.key12.TabIndex = 11;
			this.key11.BackColor = Color.White;
			this.key11.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key11.KeyText = ")0";
			this.key11.KeyValue = Keys.D0;
			this.key11.Location = new Point(417, 206);
			this.key11.Margin = new Padding(0);
			this.key11.myType = 1;
			this.key11.Name = "key11";
			this.key11.one = false;
			this.key11.Size = new Size(40, 40);
			this.key11.TabIndex = 10;
			this.key10.BackColor = Color.White;
			this.key10.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key10.KeyText = "(9";
			this.key10.KeyValue = Keys.D9;
			this.key10.Location = new Point(377, 206);
			this.key10.Margin = new Padding(0);
			this.key10.myType = 1;
			this.key10.Name = "key10";
			this.key10.one = false;
			this.key10.Size = new Size(40, 40);
			this.key10.TabIndex = 9;
			this.key9.BackColor = Color.White;
			this.key9.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key9.KeyText = "*8";
			this.key9.KeyValue = Keys.D8;
			this.key9.Location = new Point(337, 206);
			this.key9.Margin = new Padding(0);
			this.key9.myType = 1;
			this.key9.Name = "key9";
			this.key9.one = false;
			this.key9.Size = new Size(40, 40);
			this.key9.TabIndex = 8;
			this.key8.BackColor = Color.White;
			this.key8.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key8.KeyText = "&7";
			this.key8.KeyValue = Keys.D7;
			this.key8.Location = new Point(297, 206);
			this.key8.Margin = new Padding(0);
			this.key8.myType = 1;
			this.key8.Name = "key8";
			this.key8.one = false;
			this.key8.Size = new Size(40, 40);
			this.key8.TabIndex = 7;
			this.key7.BackColor = Color.White;
			this.key7.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key7.KeyText = "^6";
			this.key7.KeyValue = Keys.D6;
			this.key7.Location = new Point(257, 206);
			this.key7.Margin = new Padding(0);
			this.key7.myType = 1;
			this.key7.Name = "key7";
			this.key7.one = false;
			this.key7.Size = new Size(40, 40);
			this.key7.TabIndex = 6;
			this.key6.BackColor = Color.White;
			this.key6.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key6.KeyText = "%5";
			this.key6.KeyValue = Keys.D5;
			this.key6.Location = new Point(217, 206);
			this.key6.Margin = new Padding(0);
			this.key6.myType = 1;
			this.key6.Name = "key6";
			this.key6.one = false;
			this.key6.Size = new Size(40, 40);
			this.key6.TabIndex = 5;
			this.key5.BackColor = Color.White;
			this.key5.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key5.KeyText = "$4";
			this.key5.KeyValue = Keys.D4;
			this.key5.Location = new Point(177, 206);
			this.key5.Margin = new Padding(0);
			this.key5.myType = 1;
			this.key5.Name = "key5";
			this.key5.one = false;
			this.key5.Size = new Size(40, 40);
			this.key5.TabIndex = 4;
			this.key4.BackColor = Color.White;
			this.key4.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key4.KeyText = "#3";
			this.key4.KeyValue = Keys.D3;
			this.key4.Location = new Point(137, 206);
			this.key4.Margin = new Padding(0);
			this.key4.myType = 1;
			this.key4.Name = "key4";
			this.key4.one = false;
			this.key4.Size = new Size(40, 40);
			this.key4.TabIndex = 3;
			this.key3.BackColor = Color.White;
			this.key3.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key3.KeyText = "@2";
			this.key3.KeyValue = Keys.D2;
			this.key3.Location = new Point(97, 206);
			this.key3.Margin = new Padding(0);
			this.key3.myType = 1;
			this.key3.Name = "key3";
			this.key3.one = false;
			this.key3.Size = new Size(40, 40);
			this.key3.TabIndex = 2;
			this.key2.BackColor = Color.White;
			this.key2.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key2.KeyText = "!1";
			this.key2.KeyValue = Keys.D1;
			this.key2.Location = new Point(57, 206);
			this.key2.Margin = new Padding(0);
			this.key2.myType = 1;
			this.key2.Name = "key2";
			this.key2.one = false;
			this.key2.Size = new Size(40, 40);
			this.key2.TabIndex = 1;
			this.key1.BackColor = Color.White;
			this.key1.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key1.KeyText = "~`";
			this.key1.KeyValue = Keys.Oemtilde;
			this.key1.Location = new Point(17, 206);
			this.key1.Margin = new Padding(0);
			this.key1.myType = 1;
			this.key1.Name = "key1";
			this.key1.one = false;
			this.key1.Size = new Size(40, 40);
			this.key1.TabIndex = 0;
			this.key60.BackColor = Color.White;
			this.key60.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key60.KeyText = "Ctrl";
			this.key60.KeyValue = Keys.RWin;
			this.key60.Location = new Point(517, 366);
			this.key60.Margin = new Padding(0);
			this.key60.myType = 2;
			this.key60.Name = "key60";
			this.key60.one = true;
			this.key60.Size = new Size(42, 40);
			this.key60.TabIndex = 60;
			this.key58.BackColor = Color.White;
			this.key58.BackgroundImageLayout = ImageLayout.Zoom;
			this.key58.Font = new Font("宋体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key58.KeyText = "";
			this.key58.KeyValue = Keys.LWin;
			this.key58.Location = new Point(77, 366);
			this.key58.Margin = new Padding(0);
			this.key58.myType = 2;
			this.key58.Name = "key58";
			this.key58.one = false;
			this.key58.Size = new Size(40, 40);
			this.key58.TabIndex = 58;
			this.key50.BackColor = Color.DarkSeaGreen;
			this.key50.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.key50.KeyText = "S";
			this.key50.KeyValue = Keys.None;
			this.key50.Location = new Point(297, 148);
			this.key50.Margin = new Padding(0);
			this.key50.myType = 1;
			this.key50.Name = "key50";
			this.key50.one = false;
			this.key50.Size = new Size(40, 40);
			this.key50.TabIndex = 63;
			this.label1.AutoSize = true;
			this.label1.BackColor = Color.Transparent;
			this.label1.FlatStyle = FlatStyle.Flat;
			this.label1.Font = new Font("黑体", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.label1.Location = new Point(305, 45);
			this.label1.Name = "label1";
			this.label1.Size = new Size(368, 76);
			this.label1.TabIndex = 64;
			this.label1.Text = "    现在开始键盘测试。请根据屏幕上\r\n的提示按键，输入对应按键。测试完毕\r\n之后请按下一步。如果键盘某个按键有\r\n问题，请与监考老师联系。";
			this.pictureBox1.BackColor = Color.Transparent;
			this.pictureBox1.Location = new Point(19, 45);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(70, 76);
			this.pictureBox1.TabIndex = 65;
			this.pictureBox1.TabStop = false;
			this.tableLayoutPanel1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel1.BackColor = Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 81.13207f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.86792f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148f));
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label5, 2, 1);
			this.tableLayoutPanel1.Location = new Point(97, 45);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
			this.tableLayoutPanel1.Size = new Size(186, 76);
			this.tableLayoutPanel1.TabIndex = 67;
			this.label2.Dock = DockStyle.Fill;
			this.label2.Location = new Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new Size(24, 30);
			this.label2.TabIndex = 0;
			this.label2.Text = "报名号";
			this.label2.Click += new EventHandler(this.Label2_Click);
			this.label3.Dock = DockStyle.Fill;
			this.label3.Location = new Point(40, 0);
			this.label3.Name = "label3";
			this.label3.Size = new Size(143, 30);
			this.label3.TabIndex = 1;
			this.label4.Dock = DockStyle.Fill;
			this.label4.Location = new Point(3, 30);
			this.label4.Name = "label4";
			this.label4.Size = new Size(24, 15);
			this.label4.TabIndex = 2;
			this.label4.Text = "姓名";
			this.label5.Dock = DockStyle.Fill;
			this.label5.Location = new Point(40, 30);
			this.label5.Name = "label5";
			this.label5.Size = new Size(143, 15);
			this.label5.TabIndex = 3;
			this.button1.AutoSize = true;
			this.button1.Cursor = Cursors.Hand;
			this.button1.FlatStyle = FlatStyle.Flat;
			this.button1.Font = new Font("黑体", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.button1.Location = new Point(571, 409);
			this.button1.Name = "button1";
			this.button1.Size = new Size(89, 31);
			this.button1.TabIndex = 68;
			this.button1.Text = "退出";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.timer1.Enabled = true;
			this.timer1.Interval = 500;
			this.timer1.Tick += new EventHandler(this.timer1_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.button1);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.key50);
			base.Controls.Add(this.key62);
			base.Controls.Add(this.key61);
			base.Controls.Add(this.key60);
			base.Controls.Add(this.key59);
			base.Controls.Add(this.key58);
			base.Controls.Add(this.key56);
			base.Controls.Add(this.key55);
			base.Controls.Add(this.key54);
			base.Controls.Add(this.key51);
			base.Controls.Add(this.key53);
			base.Controls.Add(this.key52);
			base.Controls.Add(this.key49);
			base.Controls.Add(this.key48);
			base.Controls.Add(this.key47);
			base.Controls.Add(this.key46);
			base.Controls.Add(this.key45);
			base.Controls.Add(this.key44);
			base.Controls.Add(this.key43);
			base.Controls.Add(this.key42);
			base.Controls.Add(this.key41);
			base.Controls.Add(this.key40);
			base.Controls.Add(this.key39);
			base.Controls.Add(this.key38);
			base.Controls.Add(this.key37);
			base.Controls.Add(this.key36);
			base.Controls.Add(this.key35);
			base.Controls.Add(this.key34);
			base.Controls.Add(this.key33);
			base.Controls.Add(this.key32);
			base.Controls.Add(this.key31);
			base.Controls.Add(this.key30);
			base.Controls.Add(this.key29);
			base.Controls.Add(this.key28);
			base.Controls.Add(this.key22);
			base.Controls.Add(this.key23);
			base.Controls.Add(this.key24);
			base.Controls.Add(this.key25);
			base.Controls.Add(this.key26);
			base.Controls.Add(this.key27);
			base.Controls.Add(this.key21);
			base.Controls.Add(this.key20);
			base.Controls.Add(this.key19);
			base.Controls.Add(this.key18);
			base.Controls.Add(this.key17);
			base.Controls.Add(this.key16);
			base.Controls.Add(this.key15);
			base.Controls.Add(this.key14);
			base.Controls.Add(this.key13);
			base.Controls.Add(this.key12);
			base.Controls.Add(this.key11);
			base.Controls.Add(this.key10);
			base.Controls.Add(this.key9);
			base.Controls.Add(this.key8);
			base.Controls.Add(this.key7);
			base.Controls.Add(this.key6);
			base.Controls.Add(this.key5);
			base.Controls.Add(this.key4);
			base.Controls.Add(this.key3);
			base.Controls.Add(this.key2);
			base.Controls.Add(this.key1);
			base.Name = "Test";
			base.Size = new Size(686, 451);
			base.Load += new EventHandler(this.Test_Load);
			((ISupportInitialize)this.pictureBox1).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
