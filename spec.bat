@echo off


echo.
echo MSpec Tests
echo ===========
echo.
echo *** Build:
c:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe .\MoneyOverview\MoneyOverview.sln /v:m
REM src\MoneyOverview\packages\Machine.Specifications.0.5.2.0\tools\mspec src\MoneyOverview\MoneyOverview.Core\bin\Debug\MoneyOverview.Core.dll 
echo.
echo *** Spec:
echo mspec-clr4.exe src\MoneyOverview\MoneyOverview.Core\bin\Debug\MoneyOverview.Core.dll %*
MoneyOverview\packages\Machine.Specifications.0.5.2.0\tools\mspec-clr4.exe %* MoneyOverview\MoneyOverview.Core.Specifications\bin\Debug\MoneyOverview.Core.Specifications.dll 
echo.

echo.
echo Consider html report:
echo --html \ego\@ferdig\mspec.html

:end
echo