using Meilisearch;
using Serilog;
using Index = Meilisearch.Index;

namespace MeilidexSharp.Outputs;

class MeilisearchOutput : IFileEntryOutput {
    private List<FileEntry> _files = [];
    private readonly Index _index;
    private readonly int _batch;

    public MeilisearchOutput(string url, string? masterKey, string? indexName, int batch) {
        if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(indexName)) {
            throw new ArgumentException("invalid arguments for initializing meilisearch client");
        }
        masterKey ??= "";
        var client = new MeilisearchClient(url, masterKey);
        _index = client.Index(indexName);
        _batch = batch;
    }

    public async Task UpdateEntry(FileEntry entry) {
        _files.Add(entry);
        if (_files.Count >= _batch) {
            var files = _files;
            _files = [];
            await _index.AddDocumentsAsync(files);
            Log.Information("{file_count} files added", files.Count);
        }
    }

    public async ValueTask DisposeAsync() {
        if (_files.Count > 0) {
            await _index.AddDocumentsAsync(_files);
            Log.Information("{file_count} files added", _files.Count);
        }
    }
}
