namespace SillyUtil;

public sealed class SubStream : Stream
{
	private readonly Stream _baseStream;
	private readonly long _offset;
	private long _maxLength;

	public override long Position
	{
		get;

		set
		{
			ArgumentOutOfRangeException.ThrowIfNegative(value);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Length);
			field = value;
		}
	} = 0;

	public override long Length => long.Min(_baseStream.Length - _offset, _maxLength);

	public override bool CanRead => _baseStream.CanRead;
	public override bool CanWrite => field && _baseStream.CanWrite;
	public override bool CanSeek => _baseStream.CanSeek;


	internal SubStream(Stream baseStream, long offset, long maxLength, bool canWrite)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(offset);
		ArgumentOutOfRangeException.ThrowIfNegative(maxLength);

		_baseStream = baseStream;
		_offset = offset;
		_maxLength = maxLength;
		CanWrite = canWrite;
	}

	public override void Flush()
	{
		_baseStream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		count = (int)long.Min(count, Length - Position);
		_baseStream.Position = _offset + Position;
		var actualCount = _baseStream.Read(buffer, offset, count);
		Position += actualCount;
		return actualCount;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		_baseStream.Position = _offset + Position;

		var newPosition = Position + count;

		if (newPosition > Length)
			_maxLength = newPosition;

		_baseStream.Write(buffer, offset, count);
		Position = newPosition;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		var newPosition = origin switch
		{
			SeekOrigin.Begin => offset,
			SeekOrigin.Current => Position + offset,
			SeekOrigin.End => Length - offset,
			_ => throw new ArgumentOutOfRangeException(nameof(origin))
		};

		return Position = newPosition;
	}

	public override void SetLength(long value)
	{
		_baseStream.SetLength(_offset + value);
		_maxLength = value;
	}
}
