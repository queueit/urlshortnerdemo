using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UrlShortnerDemo
{
    public static class ConcurrencyHelper
    {
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(10, 10);

        public static async Task ExecuteAsync(Func<Task> method)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                await method.Invoke();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> method)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                return await method.Invoke();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
