# RecipeBook
A simple application to create and view your recipes.

Add the following to your build events to avoid an exception at runtime:
```
XCOPY "$(ProjectDir)\resources" "$(TargetDir)"  /Y /S
```
