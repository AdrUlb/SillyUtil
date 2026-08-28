using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SillyUtil;

public readonly struct Fixed32 : IBinaryNumber<Fixed32>, ISignedNumber<Fixed32>, IMinMaxValue<Fixed32>
{
	private const int _shiftCount = 16;
	private const int _scaleInt = 65536;
	private const double _scaleDouble = 65536.0;
	private const double _scaleDoubleInverse = 1.0 / _scaleDouble;
	private const decimal _scaleDecimal = 65536.0m;

	private readonly int _rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Fixed32(int rawValue) => _rawValue = rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(Fixed32 other) => _rawValue.CompareTo(other._rawValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(object? obj) => obj is Fixed32 other ? CompareTo(other) : throw new ArgumentException("Object must be of type Fixed32");

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Fixed32 other) => other._rawValue == _rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object? obj) => obj is Fixed32 other && Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => _rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() => ((decimal)this).ToString(CultureInfo.CurrentCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(IFormatProvider? formatProvider) => ((decimal)this).ToString(formatProvider);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string? format, IFormatProvider? formatProvider) => ((decimal)this).ToString(format, formatProvider);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
		((decimal)this).TryFormat(destination, out charsWritten, format, provider);

	public static Fixed32 Zero { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = FromRaw(0);
	public static Fixed32 One { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = FromRaw(1 << _shiftCount);
	public static Fixed32 NegativeOne { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = FromRaw(-1 << _shiftCount);

	public static Fixed32 AdditiveIdentity => Zero;

	public static Fixed32 MultiplicativeIdentity => One;

	public static int Radix { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = 2;

	public static Fixed32 MaxValue { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = FromRaw(int.MaxValue);

	public static Fixed32 MinValue { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = FromRaw(int.MinValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 FromRaw(int rawValue) => new(rawValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator Fixed32(int value) => FromRaw(value << _shiftCount);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator checked Fixed32(int value) => FromRaw(checked((short)value) << _shiftCount);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Fixed32(short value) => FromRaw(value << _shiftCount);

	public static explicit operator Fixed32(double value)
	{
		var scaled = double.Round(value * _scaleDouble, MidpointRounding.ToEven);
		return FromRaw(double.IsNaN(scaled) ? 0 : (int)scaled);
	}

	public static explicit operator checked Fixed32(double value)
	{
		var scaled = double.Round(value * _scaleDouble, MidpointRounding.ToEven);

		if (double.IsNaN(scaled) || scaled < int.MinValue || scaled > int.MaxValue)
			throw new OverflowException("Value is outside the range of representable values.");

		return FromRaw((int)scaled);
	}

	public static explicit operator Fixed32(decimal value)
	{
		var scaled = decimal.Round(value * _scaleDecimal, MidpointRounding.ToEven);
		return FromRaw((int)scaled);
	}

	public static explicit operator checked Fixed32(decimal value)
	{
		var scaled = decimal.Round(value * _scaleDecimal, MidpointRounding.ToEven);

		if (scaled is < int.MinValue or > int.MaxValue)
			throw new OverflowException("Value is outside the range of representable values.");

		return FromRaw((int)scaled);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator double(Fixed32 value) => value._rawValue * _scaleDoubleInverse;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator decimal(Fixed32 value) => value._rawValue / _scaleDecimal;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsZero(Fixed32 value) => value._rawValue == 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsFinite(Fixed32 value) => true;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInfinity(Fixed32 value) => false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsPositiveInfinity(Fixed32 value) => false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNegativeInfinity(Fixed32 value) => false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRealNumber(Fixed32 value) => true;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsImaginaryNumber(Fixed32 value) => false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsComplexNumber(Fixed32 value) => false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNaN(Fixed32 value) => false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNormal(Fixed32 value) => value._rawValue != 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsSubnormal(Fixed32 value) => false;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsCanonical(Fixed32 value) => true;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsInteger(Fixed32 value) => (value._rawValue & 0xFFFF) == 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsEvenInteger(Fixed32 value) => IsInteger(value) && (value._rawValue & 0x00010000) == 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsOddInteger(Fixed32 value) => IsInteger(value) && (value._rawValue & 0x00010000) != 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsPositive(Fixed32 value) => value._rawValue >= 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNegative(Fixed32 value) => value._rawValue < 0;

	// Basic arithmetic operations
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator +(Fixed32 left, Fixed32 right) => FromRaw(unchecked(left._rawValue + right._rawValue));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator -(Fixed32 left, Fixed32 right) => FromRaw(unchecked(left._rawValue - right._rawValue));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator *(Fixed32 left, Fixed32 right)
	{
		var product = (long)left._rawValue * right._rawValue;
		var adjust = 0x7FFF + ((product >> _shiftCount) & 1);
		return FromRaw(unchecked((int)((product + adjust) >> _shiftCount)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator /(Fixed32 left, Fixed32 right)
	{
		var dividend = (long)left._rawValue << _shiftCount;
		var divisor = (long)right._rawValue;

		var (quotient, remainder) = long.DivRem(dividend, divisor);

		var absDivisor = long.Abs(divisor);
		var doubleRemainder = long.Abs(remainder) << 1;

		if (doubleRemainder > absDivisor || (doubleRemainder == absDivisor && (quotient & 1) != 0))
			quotient += (dividend ^ divisor) < 0 ? -1 : 1;

		return FromRaw(unchecked((int)quotient));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator checked +(Fixed32 left, Fixed32 right) => FromRaw(checked(left._rawValue + right._rawValue));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator checked -(Fixed32 left, Fixed32 right) => FromRaw(checked(left._rawValue - right._rawValue));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator checked *(Fixed32 left, Fixed32 right)
	{
		var product = (long)left._rawValue * right._rawValue;
		var adjust = 0x7FFF + ((product >> _shiftCount) & 1);
		return FromRaw(checked((int)((product + adjust) >> _shiftCount)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator checked /(Fixed32 left, Fixed32 right)
	{
		var dividend = (long)left._rawValue << _shiftCount;
		var divisor = (long)right._rawValue;

		var (quotient, remainder) = long.DivRem(dividend, divisor);

		var absRemainder = long.Abs(remainder);
		var absDivisor = long.Abs(divisor);
		var doubleRemainder = absRemainder << 1;

		if (doubleRemainder > absDivisor || (doubleRemainder == absDivisor && (quotient & 1) != 0))
			quotient += (dividend ^ divisor) < 0 ? -1 : 1;

		return FromRaw(checked((int)quotient));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >(Fixed32 left, Fixed32 right) => left._rawValue > right._rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <(Fixed32 left, Fixed32 right) => left._rawValue < right._rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >=(Fixed32 left, Fixed32 right) => left._rawValue >= right._rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <=(Fixed32 left, Fixed32 right) => left._rawValue <= right._rawValue;

	// Increment/decrement
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator ++(Fixed32 value) => unchecked(value + One);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator checked ++(Fixed32 value) => checked(value + One);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator --(Fixed32 value) => unchecked(value - One);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator checked --(Fixed32 value) => checked(value - One);

	// Unary plus/minus
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator +(Fixed32 value) => value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator -(Fixed32 value) => FromRaw(unchecked(-value._rawValue));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator checked -(Fixed32 value) => FromRaw(checked(-value._rawValue));

	// Equality/inequality
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Fixed32 left, Fixed32 right) => left._rawValue == right._rawValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Fixed32 left, Fixed32 right) => left._rawValue != right._rawValue;

	// Binary value operations
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator &(Fixed32 left, Fixed32 right) => FromRaw(left._rawValue & right._rawValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator |(Fixed32 left, Fixed32 right) => FromRaw(left._rawValue | right._rawValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator ^(Fixed32 left, Fixed32 right) => FromRaw(left._rawValue ^ right._rawValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator %(Fixed32 left, Fixed32 right) => FromRaw(left._rawValue % right._rawValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator ~(Fixed32 value) => FromRaw(~value._rawValue);

	// Binary shift operations
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator <<(Fixed32 value, int amount) => FromRaw(value._rawValue << amount);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator >> (Fixed32 value, int amount) => FromRaw(value._rawValue >> amount);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 operator >>> (Fixed32 value, int amount) => FromRaw(value._rawValue >>> amount);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 Abs(Fixed32 value) => FromRaw(int.Abs(value._rawValue));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 MaxMagnitude(Fixed32 x, Fixed32 y) => Abs(x) > Abs(y) ? x : y;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 MinMagnitude(Fixed32 x, Fixed32 y) => Abs(x) < Abs(y) ? x : y;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 MaxMagnitudeNumber(Fixed32 x, Fixed32 y) => MaxMagnitude(x, y);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 MinMagnitudeNumber(Fixed32 x, Fixed32 y) => MinMagnitude(x, y);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => checked((Fixed32)decimal.Parse(s, style, provider));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 Parse(string s, NumberStyles style, IFormatProvider? provider) => checked((Fixed32)decimal.Parse(s, style, provider));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 Parse(string s, IFormatProvider? provider) => checked((Fixed32)decimal.Parse(s, provider));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => checked((Fixed32)decimal.Parse(s, provider));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Fixed32 result) =>
		TryParse(s, NumberStyles.Number, provider, out result);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Fixed32 result) => TryParse(s, NumberStyles.Number, provider, out result);

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Fixed32 result)
	{
		if (decimal.TryParse(s, style, provider, out var d))
		{
			var scaled = decimal.Round(d * _scaleDecimal, MidpointRounding.ToEven);

			if (scaled is < int.MinValue or > int.MaxValue)
				goto error;

			result = FromRaw((int)scaled);
			return true;
		}

	error:
		result = default;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Fixed32 result) =>
		TryParse(s.AsSpan(), style, provider, out result);

	public static bool TryConvertFromChecked<TOther>(TOther value, out Fixed32 result) where TOther : INumberBase<TOther>
	{
		if (TOther.TryConvertToChecked(value, out decimal d))
		{
			var scaled = decimal.Round(d * _scaleDecimal, MidpointRounding.ToEven);

			if (scaled is < int.MinValue or > int.MaxValue)
				throw new OverflowException("Value is outside the range of representable values.");

			result = FromRaw((int)scaled);
			return true;
		}

		result = default;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryConvertToChecked<TOther>(Fixed32 value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> =>
		TOther.TryConvertFromChecked((decimal)value, out result);

	public static bool TryConvertFromSaturating<TOther>(TOther value, out Fixed32 result) where TOther : INumberBase<TOther>
	{
		if (TOther.TryConvertToSaturating(value, out decimal d))
		{
			var scaled = decimal.Round(d * _scaleDecimal, MidpointRounding.AwayFromZero);
			result = FromRaw((int)decimal.Clamp(scaled, int.MinValue, int.MaxValue));
			return true;
		}

		result = default;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryConvertToSaturating<TOther>(Fixed32 value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> =>
		TOther.TryConvertFromSaturating((decimal)value, out result);

	public static bool TryConvertFromTruncating<TOther>(TOther value, out Fixed32 result) where TOther : INumberBase<TOther>
	{
		if (TOther.TryConvertToTruncating(value, out decimal d))
		{
			var scaled = decimal.Round(d * _scaleDecimal, MidpointRounding.ToZero);
			result = FromRaw(unchecked((int)scaled));
			return true;
		}

		result = default;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryConvertToTruncating<TOther>(Fixed32 value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> =>
		TOther.TryConvertFromTruncating((decimal)value, out result);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsPow2(Fixed32 value) => value._rawValue > 0 && (value._rawValue & (value._rawValue - 1)) == 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Fixed32 Log2(Fixed32 value)
	{
		if (value._rawValue <= 0)
			throw new ArgumentOutOfRangeException(nameof(value), "Log2 is undefined for numbers <= 0");

		// Find the integer part of the base2 logarithm
		var msb = 31 - BitOperations.LeadingZeroCount((uint)value._rawValue);
		var whole = (msb - 16) << 16;

		var x = (ulong)value._rawValue;

		// Normalize into [1.0, 2.0) space
		switch (msb)
		{
			case > 16:
				x >>= (msb - 16);
				break;
			case < 16:
				x <<= (16 - msb);
				break;
		}

		var frac = 0;

		// Keep squaring the remainder to get the fractional bits
		for (var i = 15; i >= 0; i--)
		{
			x = (x * x) >>> 16;

			if (x >= 0x20000) // 2^17 (2.0 in 16.16)
			{
				frac |= 1 << i;
				x >>= 1;
			}
		}

		return FromRaw(whole | frac);
	}
}
