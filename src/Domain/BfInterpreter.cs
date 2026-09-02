namespace Domain;

public static class BfInterpreter
{
    public static byte[] Execute(string code, byte[] input, int maxSteps = 100_000)
    {
        var memory = new byte[30000];
        int ptr = 0;
        int codePtr = 0;
        int inputPtr = 0;
        var output = new List<byte>();
        int steps = 0;

        while (codePtr < code.Length)
        {
            if (++steps > maxSteps)
            {
                throw new InvalidOperationException("Infinite loop detected or step limit exceeded.");
            }

            char cmd = code[codePtr];
            switch (cmd)
            {
                case '>':
                    ptr = (ptr + 1) % memory.Length;
                    break;
                case '<':
                    ptr = (ptr - 1 + memory.Length) % memory.Length;
                    break;
                case '+':
                    memory[ptr]++;
                    break;
                case '-':
                    memory[ptr]--;
                    break;
                case '.':
                    output.Add(memory[ptr]);
                    break;
                case ',':
                    memory[ptr] = inputPtr < input.Length ? input[inputPtr++] : (byte)0;
                    break;
                case '[':
                    if (memory[ptr] == 0)
                    {
                        int open = 1;
                        while (open > 0)
                        {
                            codePtr++;
                            if (codePtr >= code.Length) break;
                            if (code[codePtr] == '[') open++;
                            else if (code[codePtr] == ']') open--;
                        }
                    }
                    break;
                case ']':
                    if (memory[ptr] != 0)
                    {
                        int close = 1;
                        while (close > 0)
                        {
                            codePtr--;
                            if (codePtr < 0) break;
                            if (code[codePtr] == ']') close++;
                            else if (code[codePtr] == '[') close--;
                        }
                    }
                    break;
            }
            codePtr++;
        }

        return output.ToArray();
    }
}