It's a .Net 7 project. To run it, you'll need:
1. .NET 7
2. Rider/Visual Studio (optional) 

Entrance to the implementation is in Transpiler.GenerateSql. Core ideas of implemetation are in WhereGenerator.GenerateClause and various PhraseGenerators.
Tests from the task are in Transpiler.Tests.TestsFromTask, additional tests end to end tests are in CustomEndToEndTests. 

To demonstrate extensiblity, I've added a new dialect (Redshoft), bool literal and like clause. You can check it in seprate commit in the repo. 
