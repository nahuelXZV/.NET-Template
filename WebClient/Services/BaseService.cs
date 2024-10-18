using Application.Wrappers;
using Domain.Constants;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace WebClient.Services;

public class BaseService
{
    protected readonly string _serviceBaseUrl;
    private readonly HttpClient _httpClient;

    public BaseService(string serviceBaseUrl, IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
    {
        _serviceBaseUrl = "https://localhost:7037" + serviceBaseUrl;
        _httpClient = httpClientFactory.CreateClient(Constantes.HttpClientNames.ReportServer);
        _httpClient.Timeout = TimeSpan.FromSeconds(100);
    }

    protected async Task<TResult> GetAsync<TResult>(string uri = "") where TResult : class, new()
    {
        var response = await _httpClient.GetAsync($"{_serviceBaseUrl}/{uri}");
        Response<TResult> result = await ProcessResponse<Response<TResult>>(response);
        return result.Data;
    }

    protected TResult Post<TResult>(string uri = "", object content = default) where TResult : class, new()
    {
        try
        {
            var body = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var response = (_httpClient.PostAsync($"{_serviceBaseUrl}/{uri}", body)).Result;

            var result = (ProcessResponse<Response<TResult>>(response)).Result;
            return result.Data;
        }
        catch (HttpRequestException httpEx)
        {
            var errorStatusCodeStart = "Response status code does not indicate success: ";
            if (httpEx.Message.StartsWith(errorStatusCodeStart))
            {
                var statusCodeString = httpEx.Message.Substring(errorStatusCodeStart.Length, 3);
                var statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCodeString);
                throw new Exception($"Error status code: {(int)statusCode} {statusCode}");
            }
            throw;
        }
    }

    protected async Task<TResult> PostAsync<TResult>(string uri = "", object content = default) where TResult : class, new()
    {
        var body = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_serviceBaseUrl}/{uri}", body);

        Response<TResult> result = await ProcessResponse<Response<TResult>>(response);
        return result.Data;
    }

    protected async Task<TResult> PutAsync<TResult>(object content = default) where TResult : class, new()
    {
        return await PutAsync<TResult>("", content);
    }

    protected async Task<TResult> PutAsync<TResult>(string uri = "", object content = default) where TResult : class, new()
    {
        var body = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_serviceBaseUrl}/{uri}", body);

        Response<TResult> result = await ProcessResponse<Response<TResult>>(response);
        return result.Data;
    }

    protected async Task<TResult> DeleteAsync<TResult>(string uri = "") where TResult : class, new()
    {
        var response = await _httpClient.DeleteAsync($"{_serviceBaseUrl}/{uri}");

        Response<TResult> result = await ProcessResponse<Response<TResult>>(response);
        return result.Data;
    }

    private async Task<TResult> ProcessResponse<TResult>(HttpResponseMessage response) where TResult : class, new()
    {
        string jsonResponse = null;
        try
        {
            jsonResponse = await response.Content.ReadAsStringAsync();
            if (jsonResponse != "" && (int)response.StatusCode == StatusCodes.Status200OK || response.Content.Headers.ContentType.MediaType == "application/json")
                return JsonConvert.DeserializeObject<TResult>(jsonResponse);
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new Exception($"Status: {(int)response.StatusCode}, Status Text: {response.StatusCode}, Message: No se encontró el recurso para la url solicitada, {response.RequestMessage.RequestUri}");
                case HttpStatusCode.BadRequest:
                    throw new Exception($"Status: {(int)response.StatusCode}, Status Text: {response.StatusCode}, Message: Hay un Error en la Solicitud realizada");
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException($"Status: {(int)response.StatusCode}, Status Text: {response.StatusCode}, Message: La solicitud no tiene permiso al recurso solicitado");
                case HttpStatusCode.Forbidden:
                    throw new Exception($"Status: {(int)response.StatusCode}, Status Text: {response.StatusCode}, Message: La solicitud no tiene acceso al contenido");
                case HttpStatusCode.RequestTimeout:
                    throw new Exception($"Status: {(int)response.StatusCode}, Status Text: {response.StatusCode}, Message: Se expiró el tiempo de espera para la solicitud");
                case HttpStatusCode.ServiceUnavailable:
                    throw new Exception($"Status: {(int)response.StatusCode}, Status Text: {response.StatusCode}, Message: El servicio no esta disponible");
                default:
                    throw new Exception($"Ocurrió un error en la solicitud con el siguiente código: {response.StatusCode}");
            }

        }
        catch (Exception ex)
        {
            if (string.IsNullOrWhiteSpace(jsonResponse)) throw;
            var type = typeof(TResult);

            if (type.IsGenericType && type.GetGenericTypeDefinition() != typeof(Response<>)) return new TResult();

            Response<dynamic> result = JsonConvert.DeserializeObject<Response<dynamic>>(jsonResponse);
            dynamic responseResult = null;
            var typeResponseData = type.GetGenericArguments()[0];

            if (typeResponseData == typeof(bool))
            {
                responseResult = new Response<bool>
                {
                    Message = result.Message,
                    ClientMessage = result.ClientMessage,
                    Errors = result.Errors,
                    Succeded = result.Succeded,
                    Data = result.Succeded ? (bool)result.Data : false,
                };
            }

            if (typeResponseData == typeof(long))
            {
                responseResult = new Response<long>
                {
                    Message = result.Message,
                    ClientMessage = result.ClientMessage,
                    Errors = result.Errors,
                    Succeded = result.Succeded,
                    Data = result.Succeded ? (long)result.Data : default(long),
                };
            }

            if (typeResponseData == typeof(short))
            {
                responseResult = new Response<short>
                {
                    Message = result.Message,
                    ClientMessage = result.ClientMessage,
                    Errors = result.Errors,
                    Succeded = result.Succeded,
                    Data = result.Succeded ? (short)result.Data : default(short),
                };
            }

            if (typeResponseData == typeof(float))
            {
                responseResult = new Response<float>
                {
                    Message = result.Message,
                    ClientMessage = result.ClientMessage,
                    Errors = result.Errors,
                    Succeded = result.Succeded,
                    Data = result.Succeded ? (float)result.Data : default(float),
                };
            }

            if (typeResponseData == typeof(double))
            {
                responseResult = new Response<double>
                {
                    Message = result.Message,
                    ClientMessage = result.ClientMessage,
                    Errors = result.Errors,
                    Succeded = result.Succeded,
                    Data = result.Succeded ? (double)result.Data : default(double),
                };
            }

            if (typeResponseData == typeof(int))
            {
                responseResult = new Response<int>
                {
                    Message = result.Message,
                    ClientMessage = result.ClientMessage,
                    Errors = result.Errors,
                    Succeded = result.Succeded,
                    Data = result.Succeded ? (int)result.Data : default(int),
                };
            }
            responseResult.Message += ", El tipo de dato para el contenido Data de la respuesta no se pudo castear automaticamente";
            return (TResult)responseResult;
        }
    }
}
