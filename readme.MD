# Project Overview

This is a .NET 7 project designed to showcase a SQL transpiler implementation.

## Requirements

To run this project, ensure you have the following:

1. .NET 7 SDK
2. IDE (Optional) - Rider or Visual Studio can be used for easier navigation and debugging.

## Getting Started

The main entry point of the implementation is located in `Transpiler.Transpiler.GenerateSql`. Core functionalities, including SQL clause generation, are placed in `WhereGenerator.GenerateClause` and several PhraseGenerator classes in `Transpiler.Where.PhraseGenerators` namespace.

### Tests
All End-to-End tests are places in `Transpiler.Tests` project.

* Tests from the task description: Located in Transpiler.Tests.TestsFromTask
* Additional End-to-End Tests: Located in CustomEndToEndTests

## Extensibility Demonstration

To showcase the extensibility of the transpiler, a new SQL dialect for Redshift (`Transpiler.Redshift` namespace), support for boolean literals (`Transpiler.Where.PhraseGenerators.Bool` namespace), and a "LIKE" clause (`Transpiler.Where.PhraseGenerators.Like` namespace) were added. Tests for extended part are placed in dedicated tests file `Transpiler.Tests.ExtensionTests`
