# File indexer for Meilisearch

## Usage

### Index local files

```bash
MeilidexSharp index create <PATH>
```

### Index local files and write documents to Meilisearch

```bash
MeilidexSharp index create <PATH> -l <BASE_URL> -u <MEILISEARCH-API> -i <INDEX-NAME>
```

### Remove all documents from Meilisearch

```bash
MeilidexSharp index clear -u <MEILISEARCH-API> <INDEX-NAME>
```

### Remove documents not exists

```bash
MeilidexSharp index cleanup -u <MEILISEARCH-API> <INDEX-NAME>
```

### List all indexes

```bash
MeilidexSharp index list -u <MEILISEARCH-API>
```
