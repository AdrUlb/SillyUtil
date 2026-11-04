namespace SillyUtil;

public struct ProgressWriter() : IDisposable
{
	private int _top = Console.CursorTop;
	private int _length = 0;

	public void Update(string text)
	{
		if (_top >= Console.BufferHeight)
			_top = Console.BufferHeight - 1;

		if (text.Length < _length)
			text = text.PadRight(_length);
		else
			_length = text.Length;

		Console.SetCursorPosition(0, _top);
		Console.Write(text);

		var lines = _length == 0 ? 0 : (_length - 1) / Console.BufferWidth;

		var cursorOffset = (Console.CursorLeft == 0 && _length > 0) ? lines + 1 : lines;

		_top = Console.CursorTop - cursorOffset;

		if (_top < 0)
			_top = 0;

		Console.SetCursorPosition(0, _top);
	}

	public void Dispose()
	{
		var lines = _length == 0 ? 0 : (_length - 1) / Console.BufferWidth;
		var endTop = _top + lines;

		if (endTop >= Console.BufferHeight)
		{
			Console.SetCursorPosition(0, Console.BufferHeight - 1);
			Console.WriteLine();
		}
		else
			Console.SetCursorPosition(0, endTop + 1);
	}
}
