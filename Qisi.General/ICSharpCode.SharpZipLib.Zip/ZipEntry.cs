using System;
using System.IO;
namespace ICSharpCode.SharpZipLib.Zip
{
	public class ZipEntry : ICloneable
	{
		[Flags]
		private enum Known : byte
		{
			None = 0,
			Size = 1,
			CompressedSize = 2,
			Crc = 4,
			Time = 8,
			ExternalAttributes = 16
		}
		private ZipEntry.Known known;
		private int externalFileAttributes;
		private ushort versionMadeBy;
		private string name;
		private ulong size;
		private ulong compressedSize;
		private ushort versionToExtract;
		private uint crc;
		private uint dosTime;
		private CompressionMethod method;
		private byte[] extra;
		private string comment;
		private int flags;
		private long zipFileIndex;
		private long offset;
		private bool forceZip64_;
		private byte cryptoCheckValue_;
		private int _aesVer;
		private int _aesEncryptionStrength;
		public bool HasCrc
		{
			get
			{
				return (byte)(this.known & ZipEntry.Known.Crc) != 0;
			}
		}
		public bool IsCrypted
		{
			get
			{
				return (this.flags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 1;
				}
				else
				{
					this.flags &= -2;
				}
			}
		}
		public bool IsUnicodeText
		{
			get
			{
				return (this.flags & 2048) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 2048;
				}
				else
				{
					this.flags &= -2049;
				}
			}
		}
		internal byte CryptoCheckValue
		{
			get
			{
				return this.cryptoCheckValue_;
			}
			set
			{
				this.cryptoCheckValue_ = value;
			}
		}
		public int Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}
		public long ZipFileIndex
		{
			get
			{
				return this.zipFileIndex;
			}
			set
			{
				this.zipFileIndex = value;
			}
		}
		public long Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}
		public int ExternalFileAttributes
		{
			get
			{
				int result;
				if ((byte)(this.known & ZipEntry.Known.ExternalAttributes) == 0)
				{
					result = -1;
				}
				else
				{
					result = this.externalFileAttributes;
				}
				return result;
			}
			set
			{
				this.externalFileAttributes = value;
				this.known |= ZipEntry.Known.ExternalAttributes;
			}
		}
		public int VersionMadeBy
		{
			get
			{
				return (int)(this.versionMadeBy & 255);
			}
		}
		public bool IsDOSEntry
		{
			get
			{
				return this.HostSystem == 0 || this.HostSystem == 10;
			}
		}
		public int HostSystem
		{
			get
			{
				return this.versionMadeBy >> 8 & 255;
			}
			set
			{
				this.versionMadeBy &= 255;
				this.versionMadeBy |= (ushort)((value & 255) << 8);
			}
		}
		public int Version
		{
			get
			{
				int result;
				if (this.versionToExtract != 0)
				{
					result = (int)this.versionToExtract;
				}
				else
				{
					int num = 10;
					if (this.AESKeySize > 0)
					{
						num = 51;
					}
					else
					{
						if (this.CentralHeaderRequiresZip64)
						{
							num = 45;
						}
						else
						{
							if (CompressionMethod.Deflated == this.method)
							{
								num = 20;
							}
							else
							{
								if (this.IsDirectory)
								{
									num = 20;
								}
								else
								{
									if (this.IsCrypted)
									{
										num = 20;
									}
									else
									{
										if (this.HasDosAttributes(8))
										{
											num = 11;
										}
									}
								}
							}
						}
					}
					result = num;
				}
				return result;
			}
		}
		public bool CanDecompress
		{
			get
			{
				return this.Version <= 51 && (this.Version == 10 || this.Version == 11 || this.Version == 20 || this.Version == 45 || this.Version == 51) && this.IsCompressionMethodSupported();
			}
		}
		public bool LocalHeaderRequiresZip64
		{
			get
			{
				bool flag = this.forceZip64_;
				if (!flag)
				{
					ulong num = this.compressedSize;
					if (this.versionToExtract == 0 && this.IsCrypted)
					{
						num += 12uL;
					}
					flag = ((this.size >= (ulong)-1 || num >= (ulong)-1) && (this.versionToExtract == 0 || this.versionToExtract >= 45));
				}
				return flag;
			}
		}
		public bool CentralHeaderRequiresZip64
		{
			get
			{
				return this.LocalHeaderRequiresZip64 || this.offset >= (long)((ulong)-1);
			}
		}
		public long DosTime
		{
			get
			{
				long result;
				if ((byte)(this.known & ZipEntry.Known.Time) == 0)
				{
					result = 0L;
				}
				else
				{
					result = (long)((ulong)this.dosTime);
				}
				return result;
			}
			set
			{
				this.dosTime = (uint)value;
				this.known |= ZipEntry.Known.Time;
			}
		}
		public DateTime DateTime
		{
			get
			{
				uint second = Math.Min(59u, 2u * (this.dosTime & 31u));
				uint minute = Math.Min(59u, this.dosTime >> 5 & 63u);
				uint hour = Math.Min(23u, this.dosTime >> 11 & 31u);
				uint month = Math.Max(1u, Math.Min(12u, this.dosTime >> 21 & 15u));
				uint year = (this.dosTime >> 25 & 127u) + 1980u;
				int day = Math.Max(1, Math.Min(DateTime.DaysInMonth((int)year, (int)month), (int)(this.dosTime >> 16 & 31u)));
				return new DateTime((int)year, (int)month, day, (int)hour, (int)minute, (int)second);
			}
			set
			{
				uint num = (uint)value.Year;
				uint num2 = (uint)value.Month;
				uint num3 = (uint)value.Day;
				uint num4 = (uint)value.Hour;
				uint num5 = (uint)value.Minute;
				uint num6 = (uint)value.Second;
				if (num < 1980u)
				{
					num = 1980u;
					num2 = 1u;
					num3 = 1u;
					num4 = 0u;
					num5 = 0u;
					num6 = 0u;
				}
				else
				{
					if (num > 2107u)
					{
						num = 2107u;
						num2 = 12u;
						num3 = 31u;
						num4 = 23u;
						num5 = 59u;
						num6 = 59u;
					}
				}
				this.DosTime = (long)((ulong)((num - 1980u & 127u) << 25 | num2 << 21 | num3 << 16 | num4 << 11 | num5 << 5 | num6 >> 1));
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public long Size
		{
			get
			{
				return (long)(((byte)(this.known & ZipEntry.Known.Size) != 0) ? this.size : 18446744073709551615uL);
			}
			set
			{
				this.size = (ulong)value;
				this.known |= ZipEntry.Known.Size;
			}
		}
		public long CompressedSize
		{
			get
			{
				return (long)(((byte)(this.known & ZipEntry.Known.CompressedSize) != 0) ? this.compressedSize : 18446744073709551615uL);
			}
			set
			{
				this.compressedSize = (ulong)value;
				this.known |= ZipEntry.Known.CompressedSize;
			}
		}
		public long Crc
		{
			get
			{
				return (long)(((byte)(this.known & ZipEntry.Known.Crc) != 0) ? ((ulong)this.crc & (ulong)-1) : 18446744073709551615uL);
			}
			set
			{
				if (((ulong)this.crc & 18446744069414584320uL) != 0uL)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.crc = (uint)value;
				this.known |= ZipEntry.Known.Crc;
			}
		}
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this.method;
			}
			set
			{
				if (!ZipEntry.IsCompressionMethodSupported(value))
				{
					throw new NotSupportedException("Compression method not supported");
				}
				this.method = value;
			}
		}
		internal CompressionMethod CompressionMethodForHeader
		{
			get
			{
				return (this.AESKeySize > 0) ? CompressionMethod.WinZipAES : this.method;
			}
		}
		public byte[] ExtraData
		{
			get
			{
				return this.extra;
			}
			set
			{
				if (value == null)
				{
					this.extra = null;
				}
				else
				{
					if (value.Length > 65535)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					this.extra = new byte[value.Length];
					Array.Copy(value, 0, this.extra, 0, value.Length);
				}
			}
		}
		public int AESKeySize
		{
			get
			{
				int result;
				switch (this._aesEncryptionStrength)
				{
				case 0:
					result = 0;
					break;
				case 1:
					result = 128;
					break;
				case 2:
					result = 192;
					break;
				case 3:
					result = 256;
					break;
				default:
					throw new ZipException("Invalid AESEncryptionStrength " + this._aesEncryptionStrength);
				}
				return result;
			}
			set
			{
				if (value != 0)
				{
					if (value != 128)
					{
						if (value != 256)
						{
							throw new ZipException("AESKeySize must be 0, 128 or 256: " + value);
						}
						this._aesEncryptionStrength = 3;
					}
					else
					{
						this._aesEncryptionStrength = 1;
					}
				}
				else
				{
					this._aesEncryptionStrength = 0;
				}
			}
		}
		internal byte AESEncryptionStrength
		{
			get
			{
				return (byte)this._aesEncryptionStrength;
			}
		}
		internal int AESSaltLen
		{
			get
			{
				return this.AESKeySize / 16;
			}
		}
		internal int AESOverheadSize
		{
			get
			{
				return 12 + this.AESSaltLen;
			}
		}
		public string Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				if (value != null && value.Length > 65535)
				{
					throw new ArgumentOutOfRangeException("value", "cannot exceed 65535");
				}
				this.comment = value;
			}
		}
		public bool IsDirectory
		{
			get
			{
				int length = this.name.Length;
				return (length > 0 && (this.name[length - 1] == '/' || this.name[length - 1] == '\\')) || this.HasDosAttributes(16);
			}
		}
		public bool IsFile
		{
			get
			{
				return !this.IsDirectory && !this.HasDosAttributes(8);
			}
		}
		public ZipEntry(string name) : this(name, 0, 51, CompressionMethod.Deflated)
		{
		}
		internal ZipEntry(string name, int versionRequiredToExtract) : this(name, versionRequiredToExtract, 51, CompressionMethod.Deflated)
		{
		}
		internal ZipEntry(string name, int versionRequiredToExtract, int madeByInfo, CompressionMethod method)
		{
			this.externalFileAttributes = -1;
			this.method = CompressionMethod.Deflated;
			this.zipFileIndex = -1L;
			// public ctor() : base(); // base..ctor() is referencing the .ctor() method in the base class. However this doesn't compile.
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length > 65535)
			{
				throw new ArgumentException("Name is too long", "name");
			}
			if (versionRequiredToExtract != 0 && versionRequiredToExtract < 10)
			{
				throw new ArgumentOutOfRangeException("versionRequiredToExtract");
			}
			this.DateTime = DateTime.Now;
			this.name = name;
			this.versionMadeBy = (ushort)madeByInfo;
			this.versionToExtract = (ushort)versionRequiredToExtract;
			this.method = method;
		}
		[Obsolete("Use Clone instead")]
		public ZipEntry(ZipEntry entry)
		{
			this.externalFileAttributes = -1;
			this.method = CompressionMethod.Deflated;
			this.zipFileIndex = -1L;
			// public ctor() : base();
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.known = entry.known;
			this.name = entry.name;
			this.size = entry.size;
			this.compressedSize = entry.compressedSize;
			this.crc = entry.crc;
			this.dosTime = entry.dosTime;
			this.method = entry.method;
			this.comment = entry.comment;
			this.versionToExtract = entry.versionToExtract;
			this.versionMadeBy = entry.versionMadeBy;
			this.externalFileAttributes = entry.externalFileAttributes;
			this.flags = entry.flags;
			this.zipFileIndex = entry.zipFileIndex;
			this.offset = entry.offset;
			this.forceZip64_ = entry.forceZip64_;
			if (entry.extra != null)
			{
				this.extra = new byte[entry.extra.Length];
				Array.Copy(entry.extra, 0, this.extra, 0, entry.extra.Length);
			}
		}
		private bool HasDosAttributes(int attributes)
		{
			bool result = false;
			if ((byte)(this.known & ZipEntry.Known.ExternalAttributes) != 0)
			{
				if ((this.HostSystem == 0 || this.HostSystem == 10) && (this.ExternalFileAttributes & attributes) == attributes)
				{
					result = true;
				}
			}
			return result;
		}
		public void ForceZip64()
		{
			this.forceZip64_ = true;
		}
		public bool IsZip64Forced()
		{
			return this.forceZip64_;
		}
		internal void ProcessExtraData(bool localHeader)
		{
			ZipExtraData zipExtraData = new ZipExtraData(this.extra);
			if (zipExtraData.Find(1))
			{
				this.forceZip64_ = true;
				if (zipExtraData.ValueLength < 4)
				{
					throw new ZipException("Extra data extended Zip64 information length is invalid");
				}
				if (localHeader || this.size == (ulong)-1)
				{
					this.size = (ulong)zipExtraData.ReadLong();
				}
				if (localHeader || this.compressedSize == (ulong)-1)
				{
					this.compressedSize = (ulong)zipExtraData.ReadLong();
				}
				if (!localHeader && this.offset == (long)((ulong)-1))
				{
					this.offset = zipExtraData.ReadLong();
				}
			}
			else
			{
				if ((this.versionToExtract & 255) >= 45 && (this.size == (ulong)-1 || this.compressedSize == (ulong)-1))
				{
					throw new ZipException("Zip64 Extended information required but is missing.");
				}
			}
			if (zipExtraData.Find(10))
			{
				if (zipExtraData.ValueLength < 4)
				{
					throw new ZipException("NTFS Extra data invalid");
				}
				zipExtraData.ReadInt();
				while (zipExtraData.UnreadCount >= 4)
				{
					int num = zipExtraData.ReadShort();
					int num2 = zipExtraData.ReadShort();
					if (num == 1)
					{
						if (num2 >= 24)
						{
							long fileTime = zipExtraData.ReadLong();
							long num3 = zipExtraData.ReadLong();
							long num4 = zipExtraData.ReadLong();
							this.DateTime = DateTime.FromFileTime(fileTime);
						}
						break;
					}
					zipExtraData.Skip(num2);
				}
			}
			else
			{
				if (zipExtraData.Find(21589))
				{
					int valueLength = zipExtraData.ValueLength;
					int num5 = zipExtraData.ReadByte();
					if ((num5 & 1) != 0 && valueLength >= 5)
					{
						int seconds = zipExtraData.ReadInt();
						this.DateTime = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
					}
				}
			}
			if (this.method == CompressionMethod.WinZipAES)
			{
				this.ProcessAESExtraData(zipExtraData);
			}
		}
		private void ProcessAESExtraData(ZipExtraData extraData)
		{
			if (!extraData.Find(39169))
			{
				throw new ZipException("AES Extra Data missing");
			}
			this.versionToExtract = 51;
			this.Flags |= 64;
			int valueLength = extraData.ValueLength;
			if (valueLength < 7)
			{
				throw new ZipException("AES Extra Data Length " + valueLength + " invalid.");
			}
			int aesVer = extraData.ReadShort();
			int num = extraData.ReadShort();
			int aesEncryptionStrength = extraData.ReadByte();
			int num2 = extraData.ReadShort();
			this._aesVer = aesVer;
			this._aesEncryptionStrength = aesEncryptionStrength;
			this.method = (CompressionMethod)num2;
		}
		public bool IsCompressionMethodSupported()
		{
			return ZipEntry.IsCompressionMethodSupported(this.CompressionMethod);
		}
		public object Clone()
		{
			ZipEntry zipEntry = (ZipEntry)base.MemberwiseClone();
			if (this.extra != null)
			{
				zipEntry.extra = new byte[this.extra.Length];
				Array.Copy(this.extra, 0, zipEntry.extra, 0, this.extra.Length);
			}
			return zipEntry;
		}
		public override string ToString()
		{
			return this.name;
		}
		public static bool IsCompressionMethodSupported(CompressionMethod method)
		{
			return method == CompressionMethod.Deflated || method == CompressionMethod.Stored;
		}
		public static string CleanName(string name)
		{
			string result;
			if (name == null)
			{
				result = string.Empty;
			}
			else
			{
				if (Path.IsPathRooted(name))
				{
					name = name.Substring(Path.GetPathRoot(name).Length);
				}
				name = name.Replace("\\", "/");
				while (name.Length > 0 && name[0] == '/')
				{
					name = name.Remove(0, 1);
				}
				result = name;
			}
			return result;
		}
	}
}
