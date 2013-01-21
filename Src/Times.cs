using System;

namespace PrototypeCode
{
	/// <summary>
	/// Creates a wrapper around a iteration counter.
	/// </summary>
	public class Times
	{
		/// <summary>
		/// Creates a new Times instance.
		/// </summary>
		/// <param name="count">The number of times to iterate.</param>
		private Times(int count)
		{
			Value = count;
		}

		/// <summary>
		/// The value of the current instance.
		/// </summary>
		public int Value { get; private set; }

		/// <summary>
		/// Creates a new Times instance using the provided count.
		/// </summary>
		/// <param name="count">The number of times the instance represents.</param>
		public static Times Count(int count)
		{
			return new Times(count);
		}
	}
}
