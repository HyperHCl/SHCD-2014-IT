using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	public class InflaterHuffmanTree
	{
		private const int MAX_BITLEN = 15;
		private short[] tree;
		public static InflaterHuffmanTree defLitLenTree;
		public static InflaterHuffmanTree defDistTree;
		static InflaterHuffmanTree()
		{
			try
			{
				byte[] array = new byte[288];
				int i = 0;
				while (i < 144)
				{
					array[i++] = 8;
				}
				while (i < 256)
				{
					array[i++] = 9;
				}
				while (i < 280)
				{
					array[i++] = 7;
				}
				while (i < 288)
				{
					array[i++] = 8;
				}
				InflaterHuffmanTree.defLitLenTree = new InflaterHuffmanTree(array);
				array = new byte[32];
				i = 0;
				while (i < 32)
				{
					array[i++] = 5;
				}
				InflaterHuffmanTree.defDistTree = new InflaterHuffmanTree(array);
			}
			catch (Exception)
			{
				throw new SharpZipBaseException("InflaterHuffmanTree: static tree length illegal");
			}
		}
		public InflaterHuffmanTree(byte[] codeLengths)
		{
			this.BuildTree(codeLengths);
		}
		private void BuildTree(byte[] codeLengths)
		{
			int[] array = new int[16];
			int[] array2 = new int[16];
			for (int i = 0; i < codeLengths.Length; i++)
			{
				int j = (int)codeLengths[i];
				if (j > 0)
				{
					array[j]++;
				}
			}
			int num = 0;
			int num2 = 512;
			for (int j = 1; j <= 15; j++)
			{
				array2[j] = num;
				num += array[j] << 16 - j;
				if (j >= 10)
				{
					int num3 = array2[j] & 130944;
					int num4 = num & 130944;
					num2 += num4 - num3 >> 16 - j;
				}
			}
			this.tree = new short[num2];
			int num5 = 512;
			for (int j = 15; j >= 10; j--)
			{
				int num4 = num & 130944;
				num -= array[j] << 16 - j;
				int num3 = num & 130944;
				for (int i = num3; i < num4; i += 128)
				{
					this.tree[(int)DeflaterHuffman.BitReverse(i)] = (short)(-num5 << 4 | j);
					num5 += 1 << j - 9;
				}
			}
			for (int i = 0; i < codeLengths.Length; i++)
			{
				int j = (int)codeLengths[i];
				if (j != 0)
				{
					num = array2[j];
					int num6 = (int)DeflaterHuffman.BitReverse(num);
					if (j <= 9)
					{
						do
						{
							this.tree[num6] = (short)(i << 4 | j);
							num6 += 1 << j;
						}
						while (num6 < 512);
					}
					else
					{
						int num7 = (int)this.tree[num6 & 511];
						int num8 = 1 << (num7 & 15);
						num7 = -(num7 >> 4);
						do
						{
							this.tree[num7 | num6 >> 9] = (short)(i << 4 | j);
							num6 += 1 << j;
						}
						while (num6 < num8);
					}
					array2[j] = num + (1 << 16 - j);
				}
			}
		}
		public int GetSymbol(StreamManipulator input)
		{
			int num;
			int result;
			if ((num = input.PeekBits(9)) >= 0)
			{
				int num2;
				if ((num2 = (int)this.tree[num]) >= 0)
				{
					input.DropBits(num2 & 15);
					result = num2 >> 4;
				}
				else
				{
					int num3 = -(num2 >> 4);
					int bitCount = num2 & 15;
					if ((num = input.PeekBits(bitCount)) >= 0)
					{
						num2 = (int)this.tree[num3 | num >> 9];
						input.DropBits(num2 & 15);
						result = num2 >> 4;
					}
					else
					{
						int availableBits = input.AvailableBits;
						num = input.PeekBits(availableBits);
						num2 = (int)this.tree[num3 | num >> 9];
						if ((num2 & 15) <= availableBits)
						{
							input.DropBits(num2 & 15);
							result = num2 >> 4;
						}
						else
						{
							result = -1;
						}
					}
				}
			}
			else
			{
				int availableBits = input.AvailableBits;
				num = input.PeekBits(availableBits);
				int num2 = (int)this.tree[num];
				if (num2 >= 0 && (num2 & 15) <= availableBits)
				{
					input.DropBits(num2 & 15);
					result = num2 >> 4;
				}
				else
				{
					result = -1;
				}
			}
			return result;
		}
	}
}
