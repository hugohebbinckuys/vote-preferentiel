
using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using VotePreferentiel;

namespace VotePreferentiel.Tests;

public class VoteCounterTests
{
    private readonly VoteCounter _counter = new();

    [Test]
    public async Task ExampleVotes_ReturnsChloe()
    {
        VoteModel model = new()
        {
            Votes =
            [
                new() { Choices = ["Alice", "Bilal", "Chloé"] },
                new() { Choices = ["Alice", "Chloé", "Bilal"] },
                new() { Choices = ["Bilal", "Chloé", "Alice"] },
                new() { Choices = ["Chloé", "Bilal", "Alice"] },
                new() { Choices = ["Chloé", "Alice", "Bilal"] }
            ]
        };

        await Assert.That(_counter.GetWinner(model))
            .IsEqualTo("Chloé");
    }

    [Test]
    public async Task OneCandidate_ReturnsThatCandidate()
    {
        VoteModel model = new()
        {
            Votes =
            [
                new() { Choices = ["Alice"] }
            ]
        };

        await Assert.That(_counter.GetWinner(model))
            .IsEqualTo("Alice");
    }

    [Test]
    public async Task TransfersVotes_ReturnsChloe()
    {
        VoteModel model = new()
        {
            Votes =
            [
                .. Enumerable.Repeat(
                    new Vote { Choices = ["Alice", "Bilal", "Chloé", "Diane"] }, 4),

                .. Enumerable.Repeat(
                    new Vote { Choices = ["Bilal", "Chloé", "Alice", "Diane"] }, 3),

                .. Enumerable.Repeat(
                    new Vote { Choices = ["Chloé", "Alice", "Bilal", "Diane"] }, 3),

                new() { Choices = ["Diane", "Chloé", "Bilal", "Alice"] }
            ]
        };

        await Assert.That(_counter.GetWinner(model))
            .IsEqualTo("Chloé");
    }

    [Test]
    public async Task TiedCandidatesAtLastPlace_AreEliminatedTogether()
    {
        VoteModel model = new()
        {
            Votes =
            [
                new() { Choices = ["Alice", "Chloé", "Bilal"] },
                new() { Choices = ["Bilal", "Chloé", "Alice"] },
                new() { Choices = ["Chloé", "Alice", "Bilal"] },
                new() { Choices = ["Chloé", "Bilal", "Alice"] }
            ]
        };

        await Assert.That(_counter.GetWinner(model))
            .IsEqualTo("Chloé");
    }

    [Test]
    public async Task EmptyBallot_ThrowsException()
    {
        VoteModel model = new()
        {
            Votes =
            [
                new() { Choices = [] }
            ]
        };

        await Assert.That(() => _counter.GetWinner(model))
            .Throws<InvalidOperationException>();
    }
}
