using Meilisearch;
using Spectre.Console.Cli;

namespace MeilidexSharp.Commands;

class IndexClearCommand : Command<IndexClearCommand.Settings> {
    internal class Settings : MeilisearchBaseSettings {
        [CommandArgument(0, "<INDEX>")]
        public required string IndexName { get; init; }
    }

    private async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
        var client = new MeilisearchClient(settings.MeilisearchUrl, settings.MasterKey);
        var index = client.Index(settings.IndexName);
        await index.DeleteAllDocumentsAsync();
        return 0;
    }

    public override int Execute(CommandContext context, Settings settings) {
        return ExecuteAsync(context, settings).Result;
    }
}
