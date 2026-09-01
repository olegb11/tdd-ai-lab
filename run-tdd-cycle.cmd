@echo off
setlocal enabledelayedexpansion

:: Force English localization for .NET CLI output
set DOTNET_CLI_UI_LANGUAGE=en-US
set VSLANG=1033

echo ==========================================
echo  Executing TDD Verification Cycle
echo ==========================================

:: Run unit tests
dotnet test --no-restore --verbosity quiet

if %ERRORLEVEL% EQU 0 (
    echo ------------------------------------------
    echo [SUCCESS] All tests passed! Committing Green state.
    echo ------------------------------------------
    git add src/ docs/specs/
    git commit -m "green: automated pass - implementation meets specifications"
    echo [GIT] Green state committed successfully.
    exit /b 0
) else (
    echo ------------------------------------------
    echo [FAILURE] Tests failed! Executing Rollback...
    echo ------------------------------------------
    git reset --hard HEAD
    git clean -fd src/
    echo [GIT] Rollback complete. Repository restored to previous green state.
    exit /b 1
)