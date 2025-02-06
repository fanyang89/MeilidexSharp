namespace MeilidexSharp;

record FileEntry(
    string Id,
    string FileName,
    string FilePath,
    string? Url,
    long FileBytes,
    string FileSize,
    DateTime LastAccess
);
