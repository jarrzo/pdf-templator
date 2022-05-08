using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Fields;
using pdfTemplator.Shared.Wrapper;
using System.Text.Json;

namespace pdfTemplator.Server.Services
{
    public static class DataGetter
    {
        public static async Task<Shared.Wrapper.IResult<HttpResponseMessage>> GetData(DataSource dataSource)
        {
            HttpClient client = new HttpClient();

            var response = new HttpResponseMessage();
            try
            {
                SetupHeaders(client, dataSource);

                if (dataSource.Method == RequestMethod.GET)
                {
                    response = await client.GetAsync(dataSource.Url);
                }
                else if (dataSource.Method == RequestMethod.POST)
                {
                    response = await client.PostAsync(dataSource.Url, null);
                }
                else if (dataSource.Method == RequestMethod.PUT)
                {
                    response = await client.PutAsync(dataSource.Url, null);
                }
            }
            catch (Exception ex)
            {
                return await Result<HttpResponseMessage>.FailAsync(ex.Message);
            }

            return await Result<HttpResponseMessage>.SuccessAsync(response, "Success");
        }

        private static void SetupHeaders(HttpClient httpClient, DataSource dataSource)
        {
            httpClient.DefaultRequestHeaders.Clear();

            var headers = dataSource.HeadersJSON != null ? JsonSerializer.Deserialize<List<KeyValue>>(dataSource.HeadersJSON) : null;

            if (headers != null)
                foreach (var header in headers)
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }
}
