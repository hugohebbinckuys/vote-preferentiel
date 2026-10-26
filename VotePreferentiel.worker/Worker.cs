
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VotePreferentiel;

namespace VotePreferentiel.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly VoteProvider _voteProvider;
    private readonly IVoteCounter _voteCounter;

    public Worker(
        ILogger<Worker> logger,
        VoteProvider voteProvider,
        IVoteCounter voteCounter)
    {
        _logger = logger;
        _voteProvider = voteProvider;
        _voteCounter = voteCounter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            VoteModel voteModel = await _voteProvider.GetVotesAsync();

            _logger.LogInformation(
                "Nombre de bulletins récupérés : {Count}",
                voteModel.Votes.Count);

            string winner = _voteCounter.GetWinner(voteModel);

            _logger.LogInformation("Gagnant : {Winner}", winner);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erreur lors du traitement des votes.");
        }
    }
}
