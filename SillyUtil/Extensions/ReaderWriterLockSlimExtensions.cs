namespace SillyUtil.Extensions;

public static class ReaderWriterLockSlimExtensions
{
	public readonly struct WriteLockGuard(ReaderWriterLockSlim self): IDisposable
	{
		public void Dispose() => self.ExitWriteLock();
	}

	public readonly struct ReadLockGuard(ReaderWriterLockSlim self): IDisposable
	{
		public void Dispose() => self.ExitReadLock();
	}

	public readonly struct UpgradeableReadLockGuard(ReaderWriterLockSlim self): IDisposable
	{
		public void Dispose() => self.ExitUpgradeableReadLock();
	}

	extension(ReaderWriterLockSlim self)
	{
		public WriteLockGuard HoldWriteLock()
		{
			self.EnterWriteLock();
			return new(self);
		}
		
		public ReadLockGuard HoldReadLock()
		{
			self.EnterReadLock();
			return new(self);
		}
		
		public UpgradeableReadLockGuard HoldUpgradeableReadLock()
		{
			self.EnterUpgradeableReadLock();
			return new(self);
		}
	}
}
