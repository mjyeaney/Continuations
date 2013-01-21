using System;
using System.Diagnostics;

namespace PrototypeCode
{
	/// <summary>
	/// Defines a retry-workflow wrapper around a Promise specification.
	/// </summary>
	public class RetryOperation : IDisposable
	{
		private int _maxCount;
		private Exception _error;

		/// <summary>
		/// Creates a new RetryOperation instance.
		/// </summary>
		/// <param name="retryCount">The maximum number of times to retry.</param>
		public RetryOperation(Times retryCount)
		{
			_maxCount = retryCount.Value;
		}

		/// <summary>
		/// The number of times the action has been retried.
		/// </summary>
		public int RetryCount { get; private set; }

		/// <summary>
		/// Attempts the provided actions, retrying the specified number of times.
		/// </summary>
		/// <param name="action">The action to retry.</param>
		public RetryOperation Attempt(Action<RetryOperation> action)
		{
			while (RetryCount < _maxCount)
			{
				try
				{
					action(this);
				}
				catch (Exception e)
				{
					RetryCount++;
					_error = e;
					Trace.WriteLine("Retrying...");
				}
			}

			return this;
		}
		
		/// <summary>
        /// Signals the end of a SafeContext chain.
        /// </summary>
        /// <param name="a">The action to execute, which is passed any exception encountered.</param>
		public void EndWith(Action<Exception> action)
		{
			if (_error != null)
			{
				var ex = _error;
				_error = null;

				if (action != null)
				{
					action(ex);
				}
				else
				{
					throw ex;
				}
			}
			else
			{
				if (action != null)
				{
					action(null);
				}
			}
		}

		/// <summary>
		/// Releases any resources used by this instance.
		/// </summary>
		public void Dispose()
		{
			if (_error != null)
			{
				throw _error;
			}
		}
	}
}
