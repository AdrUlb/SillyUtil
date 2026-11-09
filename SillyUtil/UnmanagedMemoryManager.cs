using System.Buffers;

namespace SillyUtil;

public unsafe class UnmanagedMemoryManager<T> : MemoryManager<T> where T : unmanaged
{
	private readonly T* _pointer;
	private readonly int _length;

	public UnmanagedMemoryManager(T* pointer, int length)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(length);
		_pointer = pointer;
		_length = length;
	}

	public override Span<T> GetSpan() => new(_pointer, _length);

	public override MemoryHandle Pin(int elementIndex = 0)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(elementIndex);
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(elementIndex, _length);

		return new(_pointer + elementIndex);
	}

	public override void Unpin() {}

	protected override void Dispose(bool disposing) {}
}