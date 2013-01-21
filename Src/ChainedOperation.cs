using System;
using System.Threading;

namespace PrototypeCode
{
	/// <summary>
    /// Implements a 'safe' chain of method calls.
    /// </summary>
    public class ChainedOperation : IDisposable
    {
        // The current error.
        private Exception error { get; set; }
        private AutoResetEvent _waitHandle { get; set; }

        /// <summary>
        /// Creates a new instance of the SafeContext class.
        /// </summary>
        public ChainedOperation()
        {
            _waitHandle = new AutoResetEvent(false);
        }

        /// <summary>
        /// Starts a safe call chain.
        /// </summary>
        /// <param name="a">The method to invoke.</param>
        /// <returns>A new instance of the Safe class.</returns>
        public ChainedOperation BeginWith(Action<ChainedOperation> a)
        {
        	return Then(a); 
        }

        /// <summary>
        /// Executes the specified action, if and only if the previous
        /// method in the current chain was successful.
        /// </summary>
        /// <param name="a">The method to invoke.</param>
        /// <returns>The current Safe instance.</returns>
        public ChainedOperation Then(Action<ChainedOperation> a)
        {
            if (this.error == null)
            {
                try
                {
                    a(this);
                    _waitHandle.WaitOne();
                }
                catch (Exception e)
                {
                    this.error = e;
                }
            }
            return this;
        }

        /// <summary>
        /// Marks the current context as passing.
        /// </summary>
        public void Pass()
        {
            _waitHandle.Set();
        }

        /// <summary>
        /// To be invoked when the current call chain encounters an exception.
        /// </summary>
        public void Fail()
        {
            this.error = new Exception();
            _waitHandle.Set();
        }

        /// <summary>
        /// To be invoked when the current call chain encounters an exception.
        /// </summary>
        /// <param name="message">The message describing the current exception.</param>
        public void Fail(string message)
        {
            this.error = new Exception(message);
            _waitHandle.Set();
        }

        /// <summary>
        /// To be invoked when the current call chain encounters an exception.
        /// </summary>
        /// <param name="error">The current System.Exception.</param>
        public void Fail(Exception error)
        {
            this.error = error;
            _waitHandle.Set();
        }

        /// <summary>
        /// Signals the end of a SafeContext chain.
        /// </summary>
        public void EndWith()
        {
            EndWith(null);
        }

        /// <summary>
        /// Signals the end of a SafeContext chain.
        /// </summary>
        /// <param name="a">The action to execute, which is passed any exception encountered.</param>
        public void EndWith(Action<Exception> a)
        {
            if (this.error != null)
            {
                if (a != null)
                {
                    a(this.error);
                }
                else
                {
                    throw error;
                }

				this.error = null;
            } 
			else 
			{
				if (a != null)
				{
					a(null);
				}
			}
        }

        /// <summary>
        /// Releases any unmanaged resources held by this instance.
        /// </summary>
        public void Dispose()
        {
            if (_waitHandle != null)
            {
                _waitHandle.Dispose();
            }

			if (this.error != null)
			{
				throw this.error;
			}
        }
    }
}
