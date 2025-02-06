using Meilisearch;
using Spectre.Console.Cli;

namespace MeilidexSharp.Commands;

class IndexListCommand : Command<IndexListCommand.Settings> {
    internal class Settings : MeilisearchBaseSettings;

    private async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
        var client = new MeilisearchClient(settings.MeilisearchUrl, settings.MasterKey);
        var indexes = await client.GetAllIndexesAsync();
        foreach (var index in indexes.Results) {
            Console.WriteLine(index.Uid);
        }
        return 0;
    }

    public override int Execute(CommandContext context, Settings settings) {
        return ExecuteAsync(context, settings).Result;
    }
}
