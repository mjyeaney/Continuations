using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using NUnit.Framework;
using PrototypeCode;

namespace Tests
{
	[TestFixture]
    public class ContinuationTests
    {
		[TestFixtureSetUp]
		public void SetupTests()
		{
			Trace.Listeners.Add(new ConsoleTraceListener());
		}

		[Test]
		public void CanChainTasks()
		{
			using (var c = new ChainedOperation())
            {
                c.BeginWith(First) // first action
                    .Then(Second) // second action
                    .Then(Third) // third action
                    .Then(Fourth) // fourth action
                    .EndWith(e => 
					{
						if (e != null)
						{
							Trace.TraceError("An exception ocurred!");
						}
					});
            }
		}

		[Test]
        public void CanChainAsyncTasks()
        {
			using (var c = new ChainedOperation())
            {
                c.BeginWith(FirstAsync) // first action
                    .Then(SecondAsync) // second action
                    .Then(ThirdAsync) // third action
                    .Then(FourthAsync) // fourth action
                    .EndWith(e => 
					{
						if (e != null)
						{
							Trace.TraceError("An exception ocurred!");
						}
					});
            }
		}

		[Test]
		public void CanChainTasksWithoutEndWith()
		{
			using (var c = new ChainedOperation())
            {
                c.BeginWith(First) // first action
                    .Then(Second) // second action
                    .Then(Third) // third action
                    .Then(Fourth); // fourth action
            }
		}

		[Test]
		public void CanChainTasksWithoutEndWithAndThrowException()
		{
			using (var c = new ChainedOperation())
            {
                c.BeginWith(First) // first action
                    .Then(Second) // second action
                    .Then(Third) // third action
                    .Then(Fourth); // fourth action
            }
		}

		[Test]
		public void CanRetryOperationSpecifiedNumberOfTimes()
		{
	    	using (var r = new RetryOperation(Times.Count(5)))
	    	{
				r.Attempt(c =>
				{
					/*
					if (!BlobStore.SaveFile())
					{
						c.Fail();
					}
					*/
				
					// This simulates an action that is slow, and fails
					// (uh, SQL Azure anyone? **laugh** )
					Thread.Sleep(TimeSpan.FromSeconds(1));
					throw new Exception("Yargh!!!");
					//c.Fail("Waited too long...");
					//c.Pass();
				})
				.EndWith(e =>
				{
					if (e != null)
					{
						Trace.TraceError(String.Format("An exception went boom and said {0}", e.Message));
					}
				});
	    	}
        }

        static void First(ChainedOperation c)
        {
            Trace.WriteLine("First");
            c.Pass();
        }

        static void Second(ChainedOperation c)
        {
            Trace.WriteLine("Second");
            c.Pass();
        }

        static void Third(ChainedOperation c)
        {
            // throw new Exception("Wakka");
            Trace.WriteLine("Third");
            c.Pass();
        }

        static void Fourth(ChainedOperation c)
        {
            Trace.WriteLine("Fourth");
            c.Pass();
        }

		static void CauseException(ChainedOperation c)
		{
			throw new Exception("Boom!!!!");
		}

        static void FirstAsync(ChainedOperation c)
        {
            Task.Factory.StartNew(() =>
            {
                Trace.TraceInformation("First (TID: {0})", Thread.CurrentThread.ManagedThreadId);
                c.Pass();
            });
        }

        static void SecondAsync(ChainedOperation c)
        {
            Task.Factory.StartNew(() =>
            {
                Trace.TraceInformation("Second (TID: {0})", Thread.CurrentThread.ManagedThreadId);
                c.Pass();
            });
        }

        static void ThirdAsync(ChainedOperation c)
        {
            Task.Factory.StartNew(() =>
            {
                //throw new Exception("Wakka");
                Trace.TraceInformation("Third (TID: {0})", Thread.CurrentThread.ManagedThreadId);
                c.Pass();
				//c.Fail();
            });
        }

        static void FourthAsync(ChainedOperation c)
        {
            Task.Factory.StartNew(() =>
            {
                Trace.TraceInformation("Fourth (TID: {0})", Thread.CurrentThread.ManagedThreadId);
                c.Pass();
            });
        }

		static void CauseExceptionAsync(ChainedOperation c)
		{
			Task.Factory.StartNew(() =>
			{
				throw new Exception("Boom!!!!");
			});
		}    
	}
}
