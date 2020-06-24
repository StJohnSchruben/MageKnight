echo off

cd bin\Release

rmdir /s /q "de"
rmdir /s /q "en"
rmdir /s /q "es"
rmdir /s /q "fr"
rmdir /s /q "hu"
rmdir /s /q "it"
rmdir /s /q "ja"
rmdir /s /q "ko"
rmdir /s /q "Logs"
rmdir /s /q "pt-BR"
rmdir /s /q "ro"
rmdir /s /q "ru"
rmdir /s /q "sv"
rmdir /s /q "zh-Hans"
rmdir /s /q "zh-Hant"

del /q FabtServiceClient.*
del /q FabtServiceConsole.*
del /q *Dvs.Startup.*
del /q *Mrtcdl.Startup.*
del /q *Prototype*
del /q *.ConsoleServer.*
del /q *.DevLauncher.*
del /q Microsoft.*.xml
del /q log4net.xml
del /q ReDefNet*.xml
del /q NAudio.xml
del /q NSubstitute.*
del /q System*.xml
del /q *.vshost.*
del /q Xceed.Wpf.AvalonDock.*
del /q Xceed.Wpf.DataGrid.*
del /q *.svclog
del /q *.Test.*
del /q *.Tests.*
del /q *.UnitTestFramework.*
del /q *.manifest
del /q *.tmp