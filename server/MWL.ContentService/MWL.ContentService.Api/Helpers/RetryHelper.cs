namespace MWL.ContentService.Api.Helpers
{
    public static class RetryHelper
    {
        public static void Retry(Action action, int retryCount, TimeSpan retryDelay)
        {
            int retries = 0;
            while (retries < retryCount)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception)
                {
                    retries++;
                    Thread.Sleep(retryDelay);
                    Console.WriteLine($" [.] Retry {retries} of {retryCount}");
                }
            }

            if (retries == retryCount)
            {
                throw new Exception("Retry count exceeded");
            }
        }
    }
}
