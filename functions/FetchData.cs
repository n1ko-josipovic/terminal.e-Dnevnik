namespace FunctionCode;
class FetchData
{
    private readonly HttpClient _client;

    public FetchData(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<string?> GetHTML(string dataUrl)
    {
        HttpResponseMessage dataResponse = await _client.GetAsync(dataUrl);

        string dataHtml = await dataResponse.Content.ReadAsStringAsync();
        return dataHtml;
    }
}