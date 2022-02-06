using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SimpleMinecraftLauncher
{
    public static class AsyncWorker
    {
        private static int MAX_THREADS = 3;
        private static TaskPool taskPool = new TaskPool(MAX_THREADS);

        public static async void PollAsyncMethod(Func<string> Method, Action<string> Callback=null)
        {
            await taskPool.Enqueue(
                async () => {
                    if (Callback != null)
                        Callback(Method());
                    else
                        Method();
                }
            );
        }

        public static async void PollAsyncMethod(Action Method)
        {
            await taskPool.Enqueue(
                async () => {
                    Method();
                }
            );
        }

        public static async Task<string> MakeHTTPRequest(string URL)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            string result = string.Empty;

            Func<string> Request = () => {
                string __ret = "";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        __ret = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    __ret = "Ошибка выполнения запроса (" + ex.Message + ")";
                }
                return __ret;
            };

            result = await Task.Run(() =>
            {
                string __ret = string.Empty;
                PollAsyncMethod(Request, (response) => { __ret = response; });
                return __ret;
            });

            return result;

        }

        public static void Dispose()
        {
            taskPool.Dispose();
        }

    }
}
