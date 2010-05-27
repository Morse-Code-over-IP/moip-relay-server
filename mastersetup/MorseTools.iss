; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Morse Code Tools"
#define MyAppVerName "Morse Code Tools 1.5"
#define MyAppPublisher "Robert B. Denny"
#define MyAppURL "https://sourceforge.net/projects/morse-rss-news/"
#define MyAppExeName1 "RSSMorse.exe"
#define MyAppExeName2 "MorseKeyer.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{735ACD67-720C-4031-B534-BC81C87CB826}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=D:\dev\misc\MorseTools\mastersetup
OutputBaseFilename=MorseTools15Setup
SetupIconFile=D:\dev\misc\MorseTools\locrss\Resources\AppIcon.ico
WizardImageFile=SetupImage.bmp
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create desktop icons"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
; RSSMorse and common assemblies
Source: "D:\dev\misc\MorseTools\locrss\bin\Release\RSSMorse.exe"; DestDir: "{app}";
Source: "D:\dev\misc\MorseTools\locrss\bin\Release\RSSMorse.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\dev\misc\MorseTools\locrss\bin\Release\*.dll"; DestDir: "{app}";
Source: "D:\dev\misc\MorseTools\locrss\doc\*"; DestDir: "{app}\doc"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "D:\dev\misc\MorseTools\locrss\Setup\DirectX Assemblies\*"; DestDir: "{app}";
; Additional files for MorseKeyer
Source: "D:\dev\misc\MorseTools\keyer\bin\Release\MorseKeyer.exe"; DestDir: "{app}";
Source: "D:\dev\misc\MorseTools\keyer\bin\Release\MorseKeyer.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\dev\misc\MorseTools\keyer\bin\Release\DC3.Morse.IambicKeyer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\dev\misc\MorseTools\keyer\doc\*"; DestDir: "{app}\doc"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\RSS Morse"; Filename: "{app}\RSSMorse.exe"
Name: "{group}\RSS Morse Help"; Filename: "{app}\doc\rssmorse.html"; IconFilename: "{app}\doc\Help.ico"
Name: "{group}\Morse Keyer"; Filename: "{app}\MorseKeyer.exe"
Name: "{group}\Morse Keyer Help"; Filename: "{app}\doc\keyer.html"; IconFilename: "{app}\doc\Help.ico"
Name: "{commondesktop}\RSS Morse"; Filename: "{app}\RSSMorse.exe"; Tasks: desktopicon
Name: "{commondesktop}\Morse Keyer"; Filename: "{app}\MorseKeyer.exe"; Tasks: desktopicon
