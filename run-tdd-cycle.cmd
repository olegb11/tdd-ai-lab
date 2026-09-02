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

:: Run Stryker directly - output streams straight to the console in real time
:: (dots / cleartext / progress reporters). No log redirection needed.
dotnet stryker --break-at 100
set "STRYKER_EXIT=%ERRORLEVEL%"

:: Find the newest Stryker report directory to read the Markdown summary.
for /f "delims=" %%D in ('dir /b /ad /o-d "StrykerOutput" 2^>nul') do (
    set "LATEST_STRYKER_DIR=%%D"
    goto :STRYKER_MD_FOUND
)
:STRYKER_MD_FOUND
set "STRYKER_MD="
if defined LATEST_STRYKER_DIR (
    if exist "StrykerOutput\%LATEST_STRYKER_DIR%\reports\mutation-report.md" (
        set "STRYKER_MD=StrykerOutput\%LATEST_STRYKER_DIR%\reports\mutation-report.md"
    )
)

:: Guard 1: no Markdown report means Stryker produced no score -> FAIL (not a pass).
if not defined STRYKER_MD (
    echo.
    echo [FAIL] [MUTATION GATE FAILED] Stryker did not produce a mutation score.
    echo [INFO] No rollback performed. Fix the Stryker configuration, then re-run the gate.
    exit /b 2
)

:: Guard 2: Markdown score column must not be N/A (no mutants tested -> FAIL).
findstr /C:"N/A" "%STRYKER_MD%" >nul 2>nul
if %ERRORLEVEL% EQU 0 (
    echo.
    echo [FAIL] [MUTATION GATE FAILED] Stryker tested no mutants (score is N/A).
    echo [INFO] No rollback performed. Fix the Stryker configuration, then re-run the gate.
    exit /b 2
)

:: Guard 3: nonzero Stryker exit code -> surviving mutants -> FAIL.
if %STRYKER_EXIT% NEQ 0 (
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