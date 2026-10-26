using System;
using System.Collections.Generic;
using System.Text;

namespace VotePreferentiel;

public class VoteProvider
{
    private readonly HttpClient _httpClient;

    public VoteProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VoteModel> GetVotesAsync()
    {
        const string url = "https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.example.json";
        string json = await _httpClient.GetStringAsync(url);
        VoteModel? voteModel = System.Text.Json.JsonSerializer.Deserialize<VoteModel>(json);
        return voteModel ?? throw new InvalidOperationException("Impossible de désérialiser les votes.");
    }
}