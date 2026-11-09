using System.Diagnostics;

namespace SillyUtil;

public struct BitReader()
{
	private ushort _bits = 0;
	private int _count = 0;

	public bool TryReadBits(ref SpanReader reader, int count, out byte value)
	{
		Debug.Assert(count <= 8);

		while (_count < count)
		{
			if (reader.Position >= reader.Span.Length)
			{
				value = 0;
				return false;
			}

			_bits |= (ushort)(reader.ReadUInt8() << _count);
			_count += 8;
		}

		value = (byte)(_bits & ((1 << count) - 1));

		_count -= count;
		_bits >>= count;

		return true;
	}
}
