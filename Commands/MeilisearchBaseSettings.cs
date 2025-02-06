using System.ComponentModel;
using Spectre.Console.Cli;

namespace MeilidexSharp.Commands;

class MeilisearchBaseSettings : CommandSettings {
    [CommandOption("-u|--meilisearch-url <URL>")]
    [DefaultValue("http://localhost:7700")]
    public required string? MeilisearchUrl { get; init; }

    [CommandOption("-m|--master-key")]
    public required string MasterKey { get; init; }
}
