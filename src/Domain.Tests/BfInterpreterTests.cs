using Domain;
using Xunit;

namespace Domain.Tests;

public class BfInterpreterTests
{
    [Fact]
    public void Execute_MemoryPointerWrapLeft_WrapsToLastCell()
    {
        // Moves left from cell 0 to cell 29999 and increments it
        var result = BfInterpreter.Execute("<+.", Array.Empty<byte>());
        Assert.Single(result, (byte)1);
    }

    [Fact]
    public void Execute_MemoryPointerWrapRight_WrapsToZeroCell()
    {
        // Creates 30,000 '>' commands to force right-side wrap back to cell 0
        string code = new string('>', 30000) + "+.";
        var result = BfInterpreter.Execute(code, Array.Empty<byte>());
        Assert.Single(result, (byte)1);
    }

    [Fact]
    public void Execute_DecrementValue_DecrementsByte()
    {
        var result = BfInterpreter.Execute("++-.", Array.Empty<byte>());
        Assert.Single(result, (byte)1);
    }

    [Fact]
    public void Execute_InputExhausted_ReturnsZero()
    {
        // Reads input twice when only 1 byte is provided
        var result = BfInterpreter.Execute(",,.", new byte[] { 42 });
        Assert.Single(result, (byte)0);
    }

    [Fact]
    public void Execute_InfiniteLoop_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            BfInterpreter.Execute("+[]", Array.Empty<byte>(), maxSteps: 50));
    }

    [Fact]
    public void Execute_NestedLoops_ExecutesCorrectly()
    {
        // Verifies correct handling of nested loops: ++[>++[>++<-]<-]
        var result = BfInterpreter.Execute("++[>++[>++<-]<-]>>.", Array.Empty<byte>());
        Assert.Single(result, (byte)8);
    }

    [Fact]
    public void Execute_IgnoresNonBrainfuckCharacters()
    {
        var result = BfInterpreter.Execute("Hello + World .", Array.Empty<byte>());
        Assert.Single(result, (byte)1);
    }

    [Fact]
    public void Execute_SkipForwardLoop_WhenCurrentCellIsZero()
    {
        // Memory cell is 0, so [+++] must be completely skipped
        var result = BfInterpreter.Execute("[+++]+.", Array.Empty<byte>());
        Assert.Single(result, (byte)1);
    }

    [Fact]
    public void Execute_SkipBackwardLoop_WhenCurrentCellIsZero()
    {
        // +[-] increments cell to 1, loops, decrements to 0, then exits loop without repeating
        var result = BfInterpreter.Execute("+[-]+.", Array.Empty<byte>());
        Assert.Single(result, (byte)1);
    }

    [Fact]
    public void Execute_NestedSkippedLoops_SkipsEntireBlock()
    {
        // Outer loop cell is 0, skips nested loops entirely
        var result = BfInterpreter.Execute("[+[+]]+.", Array.Empty<byte>());
        Assert.Single(result, (byte)1);
    }

    [Fact]
    public void Execute_MultipleOutputs_AppendsAllBytes()
    {
        // Emit '1' and '2'
        var result = BfInterpreter.Execute("+.+.", Array.Empty<byte>());
        Assert.Equal(new byte[] { 1, 2 }, result);
    }

    [Fact]
    public void Execute_UnmatchedBrackets_TerminatesSafely()
    {
        // Unmatched opening bracket with 0 cell value
        var result = BfInterpreter.Execute("[", Array.Empty<byte>());
        Assert.Empty(result);
    }
}