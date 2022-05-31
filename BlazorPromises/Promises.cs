using Microsoft.JSInterop;
using System.Collections.Concurrent;

namespace BlazorPromises
{
    public class Promises
    {
        private readonly IJSRuntime? _jsRuntime;
        private static readonly ConcurrentDictionary<string, IPromiseCallbackHandler> CallbackHandlers =
            new();

        public Promises(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }

        [JSInvokable]
        public void PromiseCallback(string callbackId, string result)
        {
            if (CallbackHandlers.TryGetValue(callbackId, out IPromiseCallbackHandler? handler))
            {
                handler.SetResult(result);
                CallbackHandlers.TryRemove(callbackId, out IPromiseCallbackHandler _);
            }
        }

        [JSInvokable]
        public void PromiseError(string callbackId, string error)
        {
            if (CallbackHandlers.TryGetValue(callbackId, out IPromiseCallbackHandler? handler))
            {
                handler.SetError(error);
                CallbackHandlers.TryRemove(callbackId, out IPromiseCallbackHandler _);
            }
        }

        public Task<TResult> ExecuteAsync<TResult>(string fnName, object? data = null)
        {
            var tcs = new TaskCompletionSource<TResult>();

            string callbackId = Guid.NewGuid().ToString();
            DotNetObjectReference<Promises> callbackHandler = DotNetObjectReference.Create(this);
            if (CallbackHandlers.TryAdd(callbackId, new PromiseCallbackHandler<TResult>(tcs)))
            {
                if (data == null)
                {
                    _jsRuntime?.InvokeAsync<bool>("runFunction", callbackHandler, callbackId, fnName);
                }
                else
                {
                    _jsRuntime?.InvokeAsync<bool>("runFunction", callbackHandler, callbackId, fnName, data);
                }

                return tcs.Task;
            }

            throw new Exception("An entry with the same callback id already existed, really should never happen");
        }
    }
}
