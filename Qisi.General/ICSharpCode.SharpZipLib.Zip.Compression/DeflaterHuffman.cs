using System;
namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	public class DeflaterHuffman
	{
		private class Tree
		{
			public short[] freqs;
			public byte[] length;
			public int minNumCodes;
			public int numCodes;
			private short[] codes;
			private int[] bl_counts;
			private int maxLength;
			private DeflaterHuffman dh;
			public Tree(DeflaterHuffman dh, int elems, int minCodes, int maxLength)
			{
				this.dh = dh;
				this.minNumCodes = minCodes;
				this.maxLength = maxLength;
				this.freqs = new short[elems];
				this.bl_counts = new int[maxLength];
			}
			public void Reset()
			{
				for (int i = 0; i < this.freqs.Length; i++)
				{
					this.freqs[i] = 0;
				}
				this.codes = null;
				this.length = null;
			}
			public void WriteSymbol(int code)
			{
				this.dh.pending.WriteBits((int)this.codes[code] & 65535, (int)this.length[code]);
			}
			public void CheckEmpty()
			{
				bool flag = true;
				for (int i = 0; i < this.freqs.Length; i++)
				{
					if (this.freqs[i] != 0)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					throw new SharpZipBaseException("!Empty");
				}
			}
			public void SetStaticCodes(short[] staticCodes, byte[] staticLengths)
			{
				this.codes = staticCodes;
				this.length = staticLengths;
			}
			public void BuildCodes()
			{
				int num = this.freqs.Length;
				int[] array = new int[this.maxLength];
				int num2 = 0;
				this.codes = new short[this.freqs.Length];
				for (int i = 0; i < this.maxLength; i++)
				{
					array[i] = num2;
					num2 += this.bl_counts[i] << 15 - i;
				}
				for (int j = 0; j < this.numCodes; j++)
				{
					int i = (int)this.length[j];
					if (i > 0)
					{
						this.codes[j] = DeflaterHuffman.BitReverse(array[i - 1]);
						array[i - 1] += 1 << 16 - i;
					}
				}
			}
			public void BuildTree()
			{
				int num = this.freqs.Length;
				int[] array = new int[num];
				int i = 0;
				int num2 = 0;
				for (int j = 0; j < num; j++)
				{
					int num3 = (int)this.freqs[j];
					if (num3 != 0)
					{
						int num4 = i++;
						int num5;
						while (num4 > 0 && (int)this.freqs[array[num5 = (num4 - 1) / 2]] > num3)
						{
							array[num4] = array[num5];
							num4 = num5;
						}
						array[num4] = j;
						num2 = j;
					}
				}
				while (i < 2)
				{
					int num6 = (num2 < 2) ? (++num2) : 0;
					array[i++] = num6;
				}
				this.numCodes = Math.Max(num2 + 1, this.minNumCodes);
				int num7 = i;
				int[] array2 = new int[4 * i - 2];
				int[] array3 = new int[2 * i - 1];
				int num8 = num7;
				for (int k = 0; k < i; k++)
				{
					int num6 = array[k];
					array2[2 * k] = num6;
					array2[2 * k + 1] = -1;
					array3[k] = (int)this.freqs[num6] << 8;
					array[k] = k;
				}
				do
				{
					int num9 = array[0];
					int num10 = array[--i];
					int num5 = 0;
					int l;
					for (l = 1; l < i; l = l * 2 + 1)
					{
						if (l + 1 < i && array3[array[l]] > array3[array[l + 1]])
						{
							l++;
						}
						array[num5] = array[l];
						num5 = l;
					}
					int num11 = array3[num10];
					while ((l = num5) > 0 && array3[array[num5 = (l - 1) / 2]] > num11)
					{
						array[l] = array[num5];
					}
					array[l] = num10;
					int num12 = array[0];
					num10 = num8++;
					array2[2 * num10] = num9;
					array2[2 * num10 + 1] = num12;
					int num13 = Math.Min(array3[num9] & 255, array3[num12] & 255);
					num11 = (array3[num10] = array3[num9] + array3[num12] - num13 + 1);
					num5 = 0;
					for (l = 1; l < i; l = num5 * 2 + 1)
					{
						if (l + 1 < i && array3[array[l]] > array3[array[l + 1]])
						{
							l++;
						}
						array[num5] = array[l];
						num5 = l;
					}
					while ((l = num5) > 0 && array3[array[num5 = (l - 1) / 2]] > num11)
					{
						array[l] = array[num5];
					}
					array[l] = num10;
				}
				while (i > 1);
				if (array[0] != array2.Length / 2 - 1)
				{
					throw new SharpZipBaseException("Heap invariant violated");
				}
				this.BuildLength(array2);
			}
			public int GetEncodedLength()
			{
				int num = 0;
				for (int i = 0; i < this.freqs.Length; i++)
				{
					num += (int)(this.freqs[i] * (short)this.length[i]);
				}
				return num;
			}
			public void CalcBLFreq(DeflaterHuffman.Tree blTree)
			{
				int num = -1;
				int i = 0;
				while (i < this.numCodes)
				{
					int num2 = 1;
					int num3 = (int)this.length[i];
					int num4;
					int num5;
					if (num3 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else
					{
						num4 = 6;
						num5 = 3;
						if (num != num3)
						{
							short[] expr_51_cp_0 = blTree.freqs;
							int expr_51_cp_1 = num3;
							expr_51_cp_0[expr_51_cp_1] += 1;
							num2 = 0;
						}
					}
					num = num3;
					i++;
					while (i < this.numCodes && num == (int)this.length[i])
					{
						i++;
						if (++num2 >= num4)
						{
							break;
						}
					}
					if (num2 < num5)
					{
						short[] expr_C1_cp_0 = blTree.freqs;
						int expr_C1_cp_1 = num;
						expr_C1_cp_0[expr_C1_cp_1] += (short)num2;
					}
					else
					{
						if (num != 0)
						{
							short[] expr_EB_cp_0 = blTree.freqs;
							int expr_EB_cp_1 = 16;
							expr_EB_cp_0[expr_EB_cp_1] += 1;
						}
						else
						{
							if (num2 <= 10)
							{
								short[] expr_115_cp_0 = blTree.freqs;
								int expr_115_cp_1 = 17;
								expr_115_cp_0[expr_115_cp_1] += 1;
							}
							else
							{
								short[] expr_134_cp_0 = blTree.freqs;
								int expr_134_cp_1 = 18;
								expr_134_cp_0[expr_134_cp_1] += 1;
							}
						}
					}
				}
			}
			public void WriteTree(DeflaterHuffman.Tree blTree)
			{
				int num = -1;
				int i = 0;
				while (i < this.numCodes)
				{
					int num2 = 1;
					int num3 = (int)this.length[i];
					int num4;
					int num5;
					if (num3 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else
					{
						num4 = 6;
						num5 = 3;
						if (num != num3)
						{
							blTree.WriteSymbol(num3);
							num2 = 0;
						}
					}
					num = num3;
					i++;
					while (i < this.numCodes && num == (int)this.length[i])
					{
						i++;
						if (++num2 >= num4)
						{
							break;
						}
					}
					if (num2 < num5)
					{
						while (num2-- > 0)
						{
							blTree.WriteSymbol(num);
						}
					}
					else
					{
						if (num != 0)
						{
							blTree.WriteSymbol(16);
							this.dh.pending.WriteBits(num2 - 3, 2);
						}
						else
						{
							if (num2 <= 10)
							{
								blTree.WriteSymbol(17);
								this.dh.pending.WriteBits(num2 - 3, 3);
							}
							else
							{
								blTree.WriteSymbol(18);
								this.dh.pending.WriteBits(num2 - 11, 7);
							}
						}
					}
				}
			}
			private void BuildLength(int[] childs)
			{
				this.length = new byte[this.freqs.Length];
				int num = childs.Length / 2;
				int num2 = (num + 1) / 2;
				int num3 = 0;
				for (int i = 0; i < this.maxLength; i++)
				{
					this.bl_counts[i] = 0;
				}
				int[] array = new int[num];
				array[num - 1] = 0;
				for (int i = num - 1; i >= 0; i--)
				{
					if (childs[2 * i + 1] != -1)
					{
						int num4 = array[i] + 1;
						if (num4 > this.maxLength)
						{
							num4 = this.maxLength;
							num3++;
						}
						array[childs[2 * i]] = (array[childs[2 * i + 1]] = num4);
					}
					else
					{
						int num4 = array[i];
						this.bl_counts[num4 - 1]++;
						this.length[childs[2 * i]] = (byte)array[i];
					}
				}
				if (num3 != 0)
				{
					int num5 = this.maxLength - 1;
					do
					{
						while (this.bl_counts[--num5] == 0)
						{
						}
						do
						{
							this.bl_counts[num5]--;
							this.bl_counts[++num5]++;
							num3 -= 1 << this.maxLength - 1 - num5;
						}
						while (num3 > 0 && num5 < this.maxLength - 1);
					}
					while (num3 > 0);
					this.bl_counts[this.maxLength - 1] += num3;
					this.bl_counts[this.maxLength - 2] -= num3;
					int num6 = 2 * num2;
					for (int num7 = this.maxLength; num7 != 0; num7--)
					{
						int j = this.bl_counts[num7 - 1];
						while (j > 0)
						{
							int num8 = 2 * childs[num6++];
							if (childs[num8 + 1] == -1)
							{
								this.length[childs[num8]] = (byte)num7;
								j--;
							}
						}
					}
				}
			}
		}
		private const int BUFSIZE = 16384;
		private const int LITERAL_NUM = 286;
		private const int DIST_NUM = 30;
		private const int BITLEN_NUM = 19;
		private const int REP_3_6 = 16;
		private const int REP_3_10 = 17;
		private const int REP_11_138 = 18;
		private const int EOF_SYMBOL = 256;
		private static readonly int[] BL_ORDER;
		private static readonly byte[] bit4Reverse;
		private static short[] staticLCodes;
		private static byte[] staticLLength;
		private static short[] staticDCodes;
		private static byte[] staticDLength;
		public DeflaterPending pending;
		private DeflaterHuffman.Tree literalTree;
		private DeflaterHuffman.Tree distTree;
		private DeflaterHuffman.Tree blTree;
		private short[] d_buf;
		private byte[] l_buf;
		private int last_lit;
		private int extra_bits;
		static DeflaterHuffman()
		{
			DeflaterHuffman.BL_ORDER = new int[]
			{
				16,
				17,
				18,
				0,
				8,
				7,
				9,
				6,
				10,
				5,
				11,
				4,
				12,
				3,
				13,
				2,
				14,
				1,
				15
			};
			DeflaterHuffman.bit4Reverse = new byte[]
			{
				0,
				8,
				4,
				12,
				2,
				10,
				6,
				14,
				1,
				9,
				5,
				13,
				3,
				11,
				7,
				15
			};
			DeflaterHuffman.staticLCodes = new short[286];
			DeflaterHuffman.staticLLength = new byte[286];
			int i = 0;
			while (i < 144)
			{
				DeflaterHuffman.staticLCodes[i] = DeflaterHuffman.BitReverse(48 + i << 8);
				DeflaterHuffman.staticLLength[i++] = 8;
			}
			while (i < 256)
			{
				DeflaterHuffman.staticLCodes[i] = DeflaterHuffman.BitReverse(256 + i << 7);
				DeflaterHuffman.staticLLength[i++] = 9;
			}
			while (i < 280)
			{
				DeflaterHuffman.staticLCodes[i] = DeflaterHuffman.BitReverse(-256 + i << 9);
				DeflaterHuffman.staticLLength[i++] = 7;
			}
			while (i < 286)
			{
				DeflaterHuffman.staticLCodes[i] = DeflaterHuffman.BitReverse(-88 + i << 8);
				DeflaterHuffman.staticLLength[i++] = 8;
			}
			DeflaterHuffman.staticDCodes = new short[30];
			DeflaterHuffman.staticDLength = new byte[30];
			for (i = 0; i < 30; i++)
			{
				DeflaterHuffman.staticDCodes[i] = DeflaterHuffman.BitReverse(i << 11);
				DeflaterHuffman.staticDLength[i] = 5;
			}
		}
		public DeflaterHuffman(DeflaterPending pending)
		{
			this.pending = pending;
			this.literalTree = new DeflaterHuffman.Tree(this, 286, 257, 15);
			this.distTree = new DeflaterHuffman.Tree(this, 30, 1, 15);
			this.blTree = new DeflaterHuffman.Tree(this, 19, 4, 7);
			this.d_buf = new short[16384];
			this.l_buf = new byte[16384];
		}
		public void Reset()
		{
			this.last_lit = 0;
			this.extra_bits = 0;
			this.literalTree.Reset();
			this.distTree.Reset();
			this.blTree.Reset();
		}
		public void SendAllTrees(int blTreeCodes)
		{
			this.blTree.BuildCodes();
			this.literalTree.BuildCodes();
			this.distTree.BuildCodes();
			this.pending.WriteBits(this.literalTree.numCodes - 257, 5);
			this.pending.WriteBits(this.distTree.numCodes - 1, 5);
			this.pending.WriteBits(blTreeCodes - 4, 4);
			for (int i = 0; i < blTreeCodes; i++)
			{
				this.pending.WriteBits((int)this.blTree.length[DeflaterHuffman.BL_ORDER[i]], 3);
			}
			this.literalTree.WriteTree(this.blTree);
			this.distTree.WriteTree(this.blTree);
		}
		public void CompressBlock()
		{
			for (int i = 0; i < this.last_lit; i++)
			{
				int num = (int)(this.l_buf[i] & 255);
				int num2 = (int)this.d_buf[i];
				if (num2-- != 0)
				{
					int num3 = DeflaterHuffman.Lcode(num);
					this.literalTree.WriteSymbol(num3);
					int num4 = (num3 - 261) / 4;
					if (num4 > 0 && num4 <= 5)
					{
						this.pending.WriteBits(num & (1 << num4) - 1, num4);
					}
					int num5 = DeflaterHuffman.Dcode(num2);
					this.distTree.WriteSymbol(num5);
					num4 = num5 / 2 - 1;
					if (num4 > 0)
					{
						this.pending.WriteBits(num2 & (1 << num4) - 1, num4);
					}
				}
				else
				{
					this.literalTree.WriteSymbol(num);
				}
			}
			this.literalTree.WriteSymbol(256);
		}
		public void FlushStoredBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
		{
			this.pending.WriteBits(lastBlock ? 1 : 0, 3);
			this.pending.AlignToByte();
			this.pending.WriteShort(storedLength);
			this.pending.WriteShort(~storedLength);
			this.pending.WriteBlock(stored, storedOffset, storedLength);
			this.Reset();
		}
		public void FlushBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
		{
			short[] expr_16_cp_0 = this.literalTree.freqs;
			int expr_16_cp_1 = 256;
			expr_16_cp_0[expr_16_cp_1] += 1;
			this.literalTree.BuildTree();
			this.distTree.BuildTree();
			this.literalTree.CalcBLFreq(this.blTree);
			this.distTree.CalcBLFreq(this.blTree);
			this.blTree.BuildTree();
			int num = 4;
			for (int i = 18; i > num; i--)
			{
				if (this.blTree.length[DeflaterHuffman.BL_ORDER[i]] > 0)
				{
					num = i + 1;
				}
			}
			int num2 = 14 + num * 3 + this.blTree.GetEncodedLength() + this.literalTree.GetEncodedLength() + this.distTree.GetEncodedLength() + this.extra_bits;
			int num3 = this.extra_bits;
			for (int i = 0; i < 286; i++)
			{
				num3 += (int)(this.literalTree.freqs[i] * (short)DeflaterHuffman.staticLLength[i]);
			}
			for (int i = 0; i < 30; i++)
			{
				num3 += (int)(this.distTree.freqs[i] * (short)DeflaterHuffman.staticDLength[i]);
			}
			if (num2 >= num3)
			{
				num2 = num3;
			}
			if (storedOffset >= 0 && storedLength + 4 < num2 >> 3)
			{
				this.FlushStoredBlock(stored, storedOffset, storedLength, lastBlock);
			}
			else
			{
				if (num2 == num3)
				{
					this.pending.WriteBits(2 + (lastBlock ? 1 : 0), 3);
					this.literalTree.SetStaticCodes(DeflaterHuffman.staticLCodes, DeflaterHuffman.staticLLength);
					this.distTree.SetStaticCodes(DeflaterHuffman.staticDCodes, DeflaterHuffman.staticDLength);
					this.CompressBlock();
					this.Reset();
				}
				else
				{
					this.pending.WriteBits(4 + (lastBlock ? 1 : 0), 3);
					this.SendAllTrees(num);
					this.CompressBlock();
					this.Reset();
				}
			}
		}
		public bool IsFull()
		{
			return this.last_lit >= 16384;
		}
		public bool TallyLit(int literal)
		{
			this.d_buf[this.last_lit] = 0;
			this.l_buf[this.last_lit++] = (byte)literal;
			short[] expr_3A_cp_0 = this.literalTree.freqs;
			expr_3A_cp_0[literal] += 1;
			return this.IsFull();
		}
		public bool TallyDist(int distance, int length)
		{
			this.d_buf[this.last_lit] = (short)distance;
			this.l_buf[this.last_lit++] = (byte)(length - 3);
			int num = DeflaterHuffman.Lcode(length - 3);
			short[] expr_46_cp_0 = this.literalTree.freqs;
			int expr_46_cp_1 = num;
			expr_46_cp_0[expr_46_cp_1] += 1;
			if (num >= 265 && num < 285)
			{
				this.extra_bits += (num - 261) / 4;
			}
			int num2 = DeflaterHuffman.Dcode(distance - 1);
			short[] expr_A3_cp_0 = this.distTree.freqs;
			int expr_A3_cp_1 = num2;
			expr_A3_cp_0[expr_A3_cp_1] += 1;
			if (num2 >= 4)
			{
				this.extra_bits += num2 / 2 - 1;
			}
			return this.IsFull();
		}
		public static short BitReverse(int toReverse)
		{
			return (short)((int)DeflaterHuffman.bit4Reverse[toReverse & 15] << 12 | (int)DeflaterHuffman.bit4Reverse[toReverse >> 4 & 15] << 8 | (int)DeflaterHuffman.bit4Reverse[toReverse >> 8 & 15] << 4 | (int)DeflaterHuffman.bit4Reverse[toReverse >> 12]);
		}
		private static int Lcode(int length)
		{
			int result;
			if (length == 255)
			{
				result = 285;
			}
			else
			{
				int num = 257;
				while (length >= 8)
				{
					num += 4;
					length >>= 1;
				}
				result = num + length;
			}
			return result;
		}
		private static int Dcode(int distance)
		{
			int num = 0;
			while (distance >= 4)
			{
				num += 2;
				distance >>= 1;
			}
			return num + distance;
		}
	}
}
