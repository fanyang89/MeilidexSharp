using MeilidexSharp.Commands;
using Serilog;
using Spectre.Console.Cli;

namespace MeilidexSharp;

class Program {
    private static int Main(string[] args) {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        var app = new CommandApp();
        app.Configure(configurator => {
            configurator.AddBranch("index", c => {
                c.AddCommand<IndexClearCommand>("clear");
                c.AddCommand<IndexListCommand>("list");
                c.AddCommand<IndexCreateCommand>("create");
                c.AddCommand<IndexCleanupCommand>("cleanup");
            });
        });
        int rc = app.Run(args);
        Log.CloseAndFlush();
        return rc;
    }
}
