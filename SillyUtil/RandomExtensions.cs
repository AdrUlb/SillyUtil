using System.Numerics;

namespace SillyUtil;

public static class RandomExtensions
{
	extension(Random self)
	{
		public float NextSingle(float min, float max) => min + (max - min) * self.NextSingle();

		public Vector3 NextVector3() => new(self.NextSingle(), self.NextSingle(), self.NextSingle());

		public Vector3 NextVector3(float min, float max) => new(self.NextSingle(min, max), self.NextSingle(min, max), self.NextSingle(min, max));
	}
}
