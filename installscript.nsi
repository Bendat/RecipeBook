OutFile "setup.exe"
InstallDir $APPDATA\RecipeBook
RequestExecutionLevel user
Section
SetOutPath $INSTDIR
File /r "C:\path\to\release\folder\*"
WriteUninstaller $INSTDIR\uninstaller.exe
createShortCut "$SMPROGRAMS\RecipeBook.lnk" "$INSTDIR\RecipeBook.exe"
createShortCut "$DESKTOP\RecipeBook.lnk" "$INSTDIR\RecipeBook.exe"
SectionEnd
Section "Uninstall"
Delete $INSTDIR\uninstaller.exe
RMDir /r $INSTDIR
SectionEnd