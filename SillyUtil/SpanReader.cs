using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace SillyUtil;

public ref struct SpanReader(ReadOnlySpan<byte> span)
{
	public ReadOnlySpan<byte> Span { get; private set; } = span;
	public int Position { get; set; } = 0;

	public ReadOnlySpan<byte> ReadBytes(int count)
	{
		var ret = Span.Slice(Position, count);
		Position += count;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte ReadUInt8()
	{
		return Span[Position++];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public sbyte ReadInt8() => (sbyte)ReadUInt8();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort ReadUInt16LittleEndian()
	{
		var ret = BinaryPrimitives.ReadUInt16LittleEndian(Span[Position..]);
		Position += 2;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort ReadUInt16BigEndian()
	{
		var ret = BinaryPrimitives.ReadUInt16BigEndian(Span[Position..]);
		Position += 2;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint ReadUInt32LittleEndian()
	{
		var ret = BinaryPrimitives.ReadUInt32LittleEndian(Span[Position..]);
		Position += 4;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint ReadUInt32BigEndian()
	{
		var ret = BinaryPrimitives.ReadUInt32BigEndian(Span[Position..]);
		Position += 4;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong ReadUInt64LittleEndian()
	{
		var ret = BinaryPrimitives.ReadUInt64LittleEndian(Span[Position..]);
		Position += 8;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong ReadUInt64BigEndian()
	{
		var ret = BinaryPrimitives.ReadUInt64BigEndian(Span[Position..]);
		Position += 8;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public short ReadInt16LittleEndian() => (short)ReadUInt16LittleEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int ReadInt32LittleEndian() => (int)ReadUInt32LittleEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long ReadInt64LittleEndian() => (long)ReadUInt64LittleEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public short ReadInt16BigEndian() => (short)ReadUInt16BigEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int ReadInt32BigEndian() => (int)ReadUInt32BigEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long ReadInt64BigEndian() => (long)ReadUInt64BigEndian();
}
