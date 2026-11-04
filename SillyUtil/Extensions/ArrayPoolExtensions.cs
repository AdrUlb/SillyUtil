using System.Buffers;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;

namespace SillyUtil.Extensions;

public ref struct ArrayPoolLease<T> : IDisposable
{
	public Span<T> Span { get; private set; }
	private readonly ArrayPool<T> _pool;
	private T[]? _array;

	public bool ReturnClear { get; } = RuntimeHelpers.IsReferenceOrContainsReferences<T>();

	public ArrayPoolLease(ArrayPool<T> pool, int size, bool clear = true)
	{
		_pool = pool;

		_array = size != 0 ? pool.Rent(size) : null;

		Span = _array.AsSpan(0, size);

		if (clear)
			Span.Clear();
	}

	public void Resize(int size, bool clearNewSpace = true)
	{
		var oldSize = Span.Length;

		if (_array == null || _array.Length < size)
			_pool.Resize(ref _array, size, ReturnClear);

		Span = _array.AsSpan(0, size);

		if (clearNewSpace && size > oldSize)
			Span[oldSize..].Clear();
	}

	public void Dispose()
	{
		if (_pool == null || _array == null)
			return;

		var pool = _pool;
		var array = _array;
		_array = null;
		pool.Return(array, ReturnClear);
	}
}

public static class ArrayPoolExtensions
{
	extension<T>(ArrayPool<T> self)
	{
		public ArrayPoolLease<T> Lease(int size, bool clear = true) => new(self, size, clear);
	}
}
