# Schema

Configuring a schema for DDDStudio YAML files in Visual Studio Code.

1. Install the [Yaml](https://marketplace.visualstudio.com/items?itemName=redhat.vscode-yaml) extension.
2. Open VS Code's settings (`CTRL+,`)
3. Add a mapping for our schemas.

```js
{
    "yaml.schemas": {
        "https://raw.githubusercontent.com/carlesdavila/DDDStudio/main/src/schema/ddd_schema.json": [
            "ddd.yaml"
        ],
            "https://raw.githubusercontent.com/carlesdavila/DDDStudio/main/src/schema/aggregate_schema.json": [
            "*Aggregate.yaml"
        ],
            "https://raw.githubusercontent.com/carlesdavila/DDDStudio/main/src/schema/bounded_context_schema.json": [
            "*Subdomain.yaml"
        ]
    }
}
```
