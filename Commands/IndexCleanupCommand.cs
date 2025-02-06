using System.ComponentModel;
using Meilisearch;
using Meilisearch.QueryParameters;
using Serilog;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MeilidexSharp.Commands;

class IndexCleanupCommand : Command<IndexCleanupCommand.Settings> {
    internal class Settings : MeilisearchBaseSettings {
        [CommandArgument(0, "<INDEX>")]
        public required string IndexName { get; init; }

        [CommandOption("-b|--batch <BATCH>")]
        [DefaultValue(10000)]
        public required int Batch { get; init; }
    }

    private async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
        var client = new MeilisearchClient(settings.MeilisearchUrl, settings.MasterKey);
        var index = client.Index(settings.IndexName);

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .StartAsync("Cleaning up indexes", async ctx => {
                int lastCount;
                do {
                    var rsp = await index.GetDocumentsAsync<FileEntry>(new DocumentsQuery {
                        Limit = settings.Batch,
                        Offset = null
                    });
                    lastCount = 0;
                    foreach (var result in rsp.Results) {
                        if (result != null) {
                            if (!File.Exists(result.FilePath)) {
                                await index.DeleteOneDocumentAsync(result.Id);
                                Log.Information("Delete document {id}, file path: {file_path}", result.Id, result.FilePath);
                            }
                            lastCount++;
                        }
                    }
                } while (lastCount != 0);
            });

        return 0;
    }

    public override int Execute(CommandContext context, Settings settings) {
        return ExecuteAsync(context, settings).Result;
    }
}
