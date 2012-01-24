@echo off
echo.
echo MSpec Tests
echo ===========
echo.
REM src\MoneyOverview\packages\Machine.Specifications.0.5.2.0\tools\mspec src\MoneyOverview\MoneyOverview.Core\bin\Debug\MoneyOverview.Core.dll 
echo mspec-clr4.exe src\MoneyOverview\MoneyOverview.Core\bin\Debug\MoneyOverview.Core.dll %*
MoneyOverview\packages\Machine.Specifications.0.5.2.0\tools\mspec-clr4.exe %* MoneyOverview\MoneyOverview.Core.Specifications\bin\Debug\MoneyOverview.Core.Specifications.dll 

echo.
echo.
echo Consider html report:
echo --html \ego\@ferdig\mspec.html

:end
echo