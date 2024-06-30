# DDD Studio

## Project Description

**DDD Studio** is a tool designed to document artifacts identified in the Domain-Driven Design Starter Modelling
Process. It helps users document key elements such as Bounded Contexts and Aggregates. The tool automatically creates a
structured set of folders and YAML documents, making it easy to document and organize your DDD artifacts. YAML is used
for its simplicity and readability, allowing both humans and machines to easily understand and edit the documents. This
structured approach facilitates continuous collaboration and integration into repositories, supporting ongoing
development efforts.

## Installation

## Getting started

The DDD CLI tool helps you manage and document Domain-Driven Design (DDD) projects efficiently. This guide will walk you through the basic usage of the DDD CLI commands.

## Usage

```sh
ddd [command]
```

### Commands

- `init` : Initializes the DDD project.
- `generate-docs` : Generate SVG documentation from YAML files.
- `about` : Provides information about the DDD CLI tool.
- `list` : List all DDD artifacts.
- `add` : Adds a DDD item.

## Initializing a DDD Project

To initialize a new DDD project, use the following command:

```sh
ddd init
```

This will set up the necessary structure for your DDD project.

## Generating Documentation

You can generate SVG documentation from your YAML files using:

```sh
ddd generate-docs
```

Ensure that your YAML files are properly configured before running this command.

## Listing All DDD Artifacts

To list all the DDD artifacts in your project, use:

```sh
ddd list
```

This will display a comprehensive list of all the artifacts you have defined.

## Adding DDD Items

The `add` command allows you to add various DDD items to your project. The usage pattern is:

```sh
ddd add [command]
```

### Add Commands

- `subdomain` : Add a new subdomain and generate a base YAML file.
- `context` : Add a new bounded context to a subdomain.
- `aggregate` : Add a new aggregate to a bounded context of a subdomain.

#### Adding a Subdomain

To add a new subdomain and generate a base YAML file, use:

```sh
ddd add subdomain
```

This command will prompt you to enter the necessary details for the subdomain.

#### Adding a Bounded Context

To add a new bounded context to an existing subdomain, use:

```sh
ddd add context
```

You will be prompted to specify the subdomain to which the context will be added.

#### Adding an Aggregate

To add a new aggregate to a bounded context of a subdomain, use:

```sh
ddd add aggregate
```

This command requires specifying both the subdomain and the bounded context for the new aggregate.

## Getting Help

For more information about the DDD CLI tool and its commands, use the `about` command:

```sh
ddd about
```

This will provide detailed information about the tool and its capabilities.

With these commands, you can efficiently manage and document your DDD projects. Happy coding!
## DDD
- **Subdomains**
- **Bounded Contexts**
- **Aggregates**

## FAQ

## License

