namespace MeilidexSharp.Outputs;

interface IFileEntryOutput : IAsyncDisposable {
    Task UpdateEntry(FileEntry entry);
}
