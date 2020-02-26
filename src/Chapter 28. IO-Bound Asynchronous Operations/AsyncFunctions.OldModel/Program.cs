using System;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace AsyncFunctions.OldModel
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer().GetAwaiter().GetResult();
        }

        private static async Task StartServer()
        {
            while(true)
            {
                var pipe = new NamedPipeServerStream("pipe", PipeDirection.InOut, -1, PipeTransmissionMode.Message, PipeOptions.Asynchronous | PipeOptions.WriteThrough);
                await Task.Factory.FromAsync(pipe.BeginWaitForConnection, pipe.EndWaitForConnection, null);

                await ServiceClientRequestAsync(pipe);
            }
        }

        private static Task ServiceClientRequestAsync(NamedPipeServerStream pipe) => throw new NotImplementedException();
    }
}
