using Microsoft.JSInterop;

namespace BlazorPromises
{
    public class PromisesService
    {
        private readonly Promises? promises;
        public string? MyPromiseFnResult { get; private set; }
        public string? ErrorPromiseFnResult { get; private set; }
        public string? ComplexPromiseFnResult { get; private set; }
        public string? ParamPromiseFnResult { get; private set; }
        public string? Status { get; private set; }
        public PromisesService? Instance { get; private set; }

        public event Action<PromisesService>? RunServicesEnded;

        public PromisesService(Promises promises)
        {
            this.promises = promises;
        }

        public void RunServices()
        {
            Task.Run(async () => await RunServicesAsync());
        }

        [JSInvokable]
        private async Task RunServicesAsync()
        {
            PromisesService promisesService = this;

            promisesService.MyPromiseFnResult = await promises!.ExecuteAsync<string>("myPromiseFn");

            try
            {
                await promises.ExecuteAsync<string>("errorPromiseFn");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                promisesService.ErrorPromiseFnResult = e.Message;
            }

            GreetingModel model = await promises.ExecuteAsync<GreetingModel>("complexPromiseFn");
            promisesService.ComplexPromiseFnResult = "Hello " + model.Name + "!";

            promisesService.ParamPromiseFnResult = await promises.ExecuteAsync<string>("paramPromiseFn", new GreetingModel { Name = "test" });

            promisesService.Status = "Done";

            Instance = promisesService;

            RunServicesEnded?.Invoke(Instance);
        }

        public void ClearResults()
        {
            MyPromiseFnResult = "";
            ErrorPromiseFnResult = "";
            ComplexPromiseFnResult = "";
            ParamPromiseFnResult = "";
            Status = "";
        }
    }
}
