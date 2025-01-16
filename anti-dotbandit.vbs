Set fso = CreateObject("Scripting.FileSystemObject")

' Get the AppData folder path
appDataFolder = CreateObject("WScript.Shell").ExpandEnvironmentStrings("%APPDATA%")
dotBanditPath = appDataFolder & "\DotBandit"

' Check if the folder exists
If fso.FolderExists(dotBanditPath) Then
    MsgBox "DotBandit is present on your computer", vbInformation, "DotBandit Check"
Else
    MsgBox "We have not detected DotBandit on your computer", vbExclamation, "DotBandit Check"
End If
