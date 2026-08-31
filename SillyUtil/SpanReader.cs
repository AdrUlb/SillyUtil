using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace SillyUtil;

public ref struct SpanReader(Span<byte> span)
{
	public Span<byte> Span { get; private set; } = span;

	public Span<byte> ReadBytes(int count)
	{
		var ret = Span[..count];
		Span = Span[count..];
		return ret;
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte ReadUInt8()
	{
		var ret = Span[0];
		Span = Span[1..];
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public sbyte ReadInt8() => (sbyte)ReadUInt8();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort ReadUInt16LittleEndian()
	{
		var ret = BinaryPrimitives.ReadUInt16LittleEndian(Span);
		Span = Span[2..];
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint ReadUInt32LittleEndian()
	{
		var ret = BinaryPrimitives.ReadUInt32LittleEndian(Span);
		Span = Span[4..];
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong ReadUInt64LittleEndian()
	{
		var ret = BinaryPrimitives.ReadUInt64LittleEndian(Span);
		Span = Span[8..];
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public short ReadInt16LittleEndian() => (short)ReadUInt16LittleEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int ReadInt32LittleEndian() => (int)ReadUInt32LittleEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long ReadInt64LittleEndian() => (long)ReadUInt64LittleEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ushort ReadUInt16BigEndian()
	{
		var ret = BinaryPrimitives.ReadUInt16BigEndian(Span);
		Span = Span[2..];
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint ReadUInt32BigEndian()
	{
		var ret = BinaryPrimitives.ReadUInt32BigEndian(Span);
		Span = Span[4..];
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong ReadUInt64BigEndian()
	{
		var ret = BinaryPrimitives.ReadUInt64BigEndian(Span);
		Span = Span[8..];
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public short ReadInt16BigEndian() => (short)ReadUInt16BigEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int ReadInt32BigEndian() => (int)ReadUInt32BigEndian();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long ReadInt64BigEndian() => (long)ReadUInt64BigEndian();
}
