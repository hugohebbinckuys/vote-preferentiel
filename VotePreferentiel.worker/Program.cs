
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VotePreferentiel;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<VoteProvider>();
builder.Services.AddSingleton<IVoteCounter, VoteCounter>();
builder.Services.AddHostedService<VotePreferentiel.Worker.Worker>();

var host = builder.Build();
host.Run();
