using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace SillyUtil.Extensions;

public static class ValueTypeExtensions
{
	extension<T>(ref T self) where T : struct
	{
		[Pure]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> AsSpan() => new(ref self);
	}
}
