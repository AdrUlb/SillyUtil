using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace SillyUtil.Extensions;

public static class StreamExtensions
{
	extension(Stream self)
	{
		public Stream SubStream(long offset, long length, bool canWrite = true) => new SubStream(self, offset, length, canWrite);

		[SkipLocalsInit]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte ReadUInt8()
		{
			Span<byte> buf = stackalloc byte[1];
			self.ReadExactly(buf);
			return buf[0];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sbyte ReadInt8() => (sbyte)self.ReadUInt8();

		[SkipLocalsInit]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort ReadUInt16LittleEndian()
		{
			Span<byte> buf = stackalloc byte[2];
			self.ReadExactly(buf);
			return BinaryPrimitives.ReadUInt16LittleEndian(buf);
		}

		[SkipLocalsInit]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint ReadUInt32LittleEndian()
		{
			Span<byte> buf = stackalloc byte[4];
			self.ReadExactly(buf);
			return BinaryPrimitives.ReadUInt32LittleEndian(buf);
		}

		[SkipLocalsInit]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong ReadUInt64LittleEndian()
		{
			Span<byte> buf = stackalloc byte[8];
			self.ReadExactly(buf);
			return BinaryPrimitives.ReadUInt64LittleEndian(buf);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public short ReadInt16LittleEndian() => (short)self.ReadUInt16LittleEndian();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReadInt32LittleEndian() => (int)self.ReadUInt32LittleEndian();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long ReadInt64LittleEndian() => (long)self.ReadUInt64LittleEndian();

		[SkipLocalsInit]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort ReadUInt16BigEndian()
		{
			Span<byte> buf = stackalloc byte[2];
			self.ReadExactly(buf);
			return BinaryPrimitives.ReadUInt16BigEndian(buf);
		}

		[SkipLocalsInit]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint ReadUInt32BigEndian()
		{
			Span<byte> buf = stackalloc byte[4];
			self.ReadExactly(buf);
			return BinaryPrimitives.ReadUInt32BigEndian(buf);
		}

		[SkipLocalsInit]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong ReadUInt64BigEndian()
		{
			Span<byte> buf = stackalloc byte[8];
			self.ReadExactly(buf);
			return BinaryPrimitives.ReadUInt64BigEndian(buf);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public short ReadInt16BigEndian() => (short)self.ReadUInt16BigEndian();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReadInt32BigEndian() => (int)self.ReadUInt32BigEndian();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long ReadInt64BigEndian() => (long)self.ReadUInt64BigEndian();
	}
}
