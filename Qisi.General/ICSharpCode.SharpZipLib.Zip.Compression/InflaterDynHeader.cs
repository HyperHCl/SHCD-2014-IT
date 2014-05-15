using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	internal class InflaterDynHeader
	{
		private const int LNUM = 0;
		private const int DNUM = 1;
		private const int BLNUM = 2;
		private const int BLLENS = 3;
		private const int LENS = 4;
		private const int REPS = 5;
		private static readonly int[] repMin = new int[]
		{
			3,
			3,
			11
		};
		private static readonly int[] repBits = new int[]
		{
			2,
			3,
			7
		};
		private static readonly int[] BL_ORDER = new int[]
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
		private byte[] blLens;
		private byte[] litdistLens;
		private InflaterHuffmanTree blTree;
		private int mode;
		private int lnum;
		private int dnum;
		private int blnum;
		private int num;
		private int repSymbol;
		private byte lastLen;
		private int ptr;
		public bool Decode(StreamManipulator input)
		{
			while (true)
			{
				while (true)
				{
					switch (this.mode)
					{
					case 0:
						goto IL_33;
					case 1:
						goto IL_7E;
					case 2:
						goto IL_E9;
					case 3:
						goto IL_144;
					case 4:
						goto IL_1CD;
					case 5:
						goto IL_28E;
					}
				}
				IL_28E:
				int bitCount = InflaterDynHeader.repBits[this.repSymbol];
				int num = input.PeekBits(bitCount);
				if (num < 0)
				{
					goto Block_11;
				}
				input.DropBits(bitCount);
				num += InflaterDynHeader.repMin[this.repSymbol];
				if (this.ptr + num > this.num)
				{
					goto Block_12;
				}
				while (num-- > 0)
				{
					this.litdistLens[this.ptr++] = this.lastLen;
				}
				if (this.ptr == this.num)
				{
					goto Block_14;
				}
				this.mode = 4;
				continue;
				IL_1CD:
				int symbol;
				while (((symbol = this.blTree.GetSymbol(input)) & -16) == 0)
				{
					this.litdistLens[this.ptr++] = (this.lastLen = (byte)symbol);
					if (this.ptr == this.num)
					{
						goto Block_6;
					}
				}
				if (symbol < 0)
				{
					goto Block_8;
				}
				if (symbol >= 17)
				{
					this.lastLen = 0;
				}
				else
				{
					if (this.ptr == 0)
					{
						goto Block_10;
					}
				}
				this.repSymbol = symbol - 16;
				this.mode = 5;
				goto IL_28E;
				IL_144:
				while (this.ptr < this.blnum)
				{
					int num2 = input.PeekBits(3);
					if (num2 < 0)
					{
						goto Block_4;
					}
					input.DropBits(3);
					this.blLens[InflaterDynHeader.BL_ORDER[this.ptr]] = (byte)num2;
					this.ptr++;
				}
				this.blTree = new InflaterHuffmanTree(this.blLens);
				this.blLens = null;
				this.ptr = 0;
				this.mode = 4;
				goto IL_1CD;
				IL_E9:
				this.blnum = input.PeekBits(4);
				if (this.blnum < 0)
				{
					goto Block_3;
				}
				this.blnum += 4;
				input.DropBits(4);
				this.blLens = new byte[19];
				this.ptr = 0;
				this.mode = 3;
				goto IL_144;
				IL_7E:
				this.dnum = input.PeekBits(5);
				if (this.dnum < 0)
				{
					goto Block_2;
				}
				this.dnum++;
				input.DropBits(5);
				this.num = this.lnum + this.dnum;
				this.litdistLens = new byte[this.num];
				this.mode = 2;
				goto IL_E9;
				IL_33:
				this.lnum = input.PeekBits(5);
				if (this.lnum < 0)
				{
					break;
				}
				this.lnum += 257;
				input.DropBits(5);
				this.mode = 1;
				goto IL_7E;
			}
			bool result = false;
			return result;
			Block_2:
			result = false;
			return result;
			Block_3:
			result = false;
			return result;
			Block_4:
			result = false;
			return result;
			Block_6:
			result = true;
			return result;
			Block_8:
			result = false;
			return result;
			Block_10:
			throw new SharpZipBaseException();
			Block_11:
			result = false;
			return result;
			Block_12:
			throw new SharpZipBaseException();
			Block_14:
			result = true;
			return result;
		}
		public InflaterHuffmanTree BuildLitLenTree()
		{
			byte[] array = new byte[this.lnum];
			Array.Copy(this.litdistLens, 0, array, 0, this.lnum);
			return new InflaterHuffmanTree(array);
		}
		public InflaterHuffmanTree BuildDistTree()
		{
			byte[] array = new byte[this.dnum];
			Array.Copy(this.litdistLens, this.lnum, array, 0, this.dnum);
			return new InflaterHuffmanTree(array);
		}
	}
}
