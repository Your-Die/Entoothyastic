public static class Utility
{
    public static bool EqualsIgnoreCase(this char character, char other)
    {
        char upperCharacter = char.ToUpper(character);
        char upperOther = char.ToUpper(other);

        return upperCharacter == upperOther;
    }
}

