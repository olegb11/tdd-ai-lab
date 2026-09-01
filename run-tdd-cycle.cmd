@echo off
setlocal enabledelayedexpansion

:: ============================================================================
:: tdd-ai-lab: Arbiter Controller Script
:: Modes:
::   1. Fast Loop: run-tdd-cycle.cmd
::   2. Feature Finalization Gate: run-tdd-cycle.cmd --full
:: ============================================================================

set "MODE=FAST"
if /i "%~1"=="--full" set "MODE=FULL"

echo ============================================================================
echo [TDD-AI-LAB] Running Arbiter Check in %MODE% mode...
echo ============================================================================

:: 1. Run standard xUnit tests
echo [1/3] Executing dotnet test...
dotnet test --no-build --verbosity quiet
if %ERRORLEVEL% NEQ 0 goto :TEST_FAILED

echo [OK] [GREEN STATE] All xUnit tests passed!

:: 2. Feature Finalization Gate (Mutation Testing via Stryker.NET)
if /i "%MODE%"=="FULL" goto :RUN_MUTATION_GATE

goto :AUTO_COMMIT

:TEST_FAILED
echo.
echo [FAIL] [RED STATE] Tests failed! Triggering automatic rollback...
git reset --hard HEAD
git clean -fd src/
echo [ROLLBACK COMPLETE] Repository restored to last known green state.
exit /b 1

:RUN_MUTATION_GATE
echo.
echo [2/3] Running Mutation Testing Gate (Stryker.NET)...

where dotnet-stryker >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [WARN] dotnet-stryker is not installed.
    echo Installing dotnet-stryker globally...
    dotnet tool install --global dotnet-stryker
)

dotnet stryker --break-at 100
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [FAIL] [MUTATION GATE FAILED] One or more mutants SURVIVED!
    echo [INFO] Your domain implementation is correct, but your tests have blind spots.
    echo        No rollback performed. Write an additional RED test in Domain.Tests to kill the mutant,
    echo        then run Fast Loop again.
    exit /b 2
)
echo [OK] [MUTATION GATE PASSED] All mutants killed! Test suite coverage is 100 percent.

:AUTO_COMMIT
echo.
echo [3/3] Creating Auto-Commit...
git add .
git commit -m "chore(tdd): green state [%MODE% mode] - %date% %time%"
if %ERRORLEVEL% EQU 0 (
    echo [OK] [AUTO-COMMIT SUCCESSFUL] Changes committed to Git.
) else (
    echo [INFO] No changes to commit.
)

echo.
echo ============================================================================
echo [SUCCESS] Cycle complete in %MODE% mode.
echo ============================================================================
exit /b 0