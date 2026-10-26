
namespace VotePreferentiel;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel)
    {
        return voteModel.Votes
            .SelectMany(vote => vote.Choices)
            .ToHashSet();
    }

    public string GetWinner(VoteModel voteModel)
    {
        HashSet<string> candidates = GetCandidates(voteModel);

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException(
                "Aucun candidat dans les votes.");
        }

        while (candidates.Count > 1)
        {
            List<string> firstChoices = voteModel.Votes
                .Where(vote => vote.Choices.Any(c => candidates.Contains(c)))
                .Select(vote => vote.Choices.First(c => candidates.Contains(c)))
                .ToList();

            IDictionary<string, int> counts = CountFirstChoices(firstChoices);

            foreach (string candidate in candidates)
            {
                counts.TryAdd(candidate, 0);
            }

            string[] eliminated = GetCandidatesWithLeastVotes(counts);

            foreach (string candidate in eliminated)
            {
                candidates.Remove(candidate);
            }
        }

        return candidates.Single();
    }

    public List<string> GetAllFirstChoices(VoteModel voteModel)
    {
        return voteModel.Votes
            .Where(vote => vote.Choices.Count > 0)
            .Select(vote => vote.Choices[0])
            .ToList();
    }

    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices)
    {
        return firstChoices
            .GroupBy(candidate => candidate)
            .ToDictionary(group => group.Key, group => group.Count());
    }

    public string[] GetCandidatesWithLeastVotes(
        IDictionary<string, int> firstChoiceCounts)
    {
        if (firstChoiceCounts.Count == 0)
        {
            throw new InvalidOperationException(
                "Aucun candidat à départager.");
        }

        int minVotes = firstChoiceCounts.Values.Min();

        return firstChoiceCounts
            .Where(pair => pair.Value == minVotes)
            .Select(pair => pair.Key)
            .ToArray();
    }
}
