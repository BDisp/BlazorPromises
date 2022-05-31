using System.Text.Json;

namespace BlazorPromises
{
    public class PromiseCallbackHandler<TResult> : IPromiseCallbackHandler
    {
        private readonly TaskCompletionSource<TResult> _tcs;

        public PromiseCallbackHandler(TaskCompletionSource<TResult> tcs)
        {
            _tcs = tcs;
        }

        public void SetResult(string json)
        {
            try
            {
                TResult result = JsonSerializer.Deserialize<TResult>(json)!;
                _tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SetError(string error)
        {
            var exception = new Exception(error);
            _tcs.SetException(exception);
        }
    }
}
