using Newtonsoft.Json;

namespace MeilidexSharp.Outputs;

class ConsoleOutput : IFileEntryOutput {
    public Task UpdateEntry(FileEntry entry) {
        string json = JsonConvert.SerializeObject(entry);
        Console.WriteLine(json);
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync() {
        return ValueTask.CompletedTask;
    }
}
