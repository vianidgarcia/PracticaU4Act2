namespace PracticaU4Act2;

// Converts the text typed by the user into integer keys, with clear messages if something is not valid.
public static class KeyInput
{
    private static readonly char[] Separators = { ',', ';', ' ', '\t', '\r', '\n' };

    public static int ReadKey(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException(ErrorMessages.EmptyKey);

        text = text.Trim();
        if (!int.TryParse(text, out int key))
            throw new ArgumentException(ErrorMessages.InvalidKey(text));
        return key;
    }

    public static List<int> ReadSequence(string? text)
    {
        string[] parts = (text ?? "").Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            throw new ArgumentException(ErrorMessages.EmptySequence);

        var keys = new List<int>(parts.Length);
        foreach (string part in parts)
            keys.Add(ReadKey(part));
        return keys;
    }
}
