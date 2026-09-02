namespace Domain;

/// <summary>
/// Contains Brainfuck program routines stored as constant C# strings.
/// </summary>
public static class BfPrograms
{
    /// <summary>
    /// Addition routine for two input bytes.
    /// Expects 2 bytes from standard input and outputs a single byte result.
    /// Algorithm: ,>,[<+>-]<.
    /// </summary>/// 
    public const string Addition = ",>,[<+>-]<.";
}