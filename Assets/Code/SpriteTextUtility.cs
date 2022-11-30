using System.Collections.Generic;
using System.Text;

public static class SpriteTextUtility
{
    static StringBuilder _stringBuilder = new StringBuilder(200);

    static Dictionary<char, string> _spriteStrings = new Dictionary<char, string>
    {
        {'0', "<sprite name=\"0\">"},
        {'1', "<sprite name=\"1\">"},
        {'2', "<sprite name=\"2\">"},
        {'3', "<sprite name=\"3\">"},
        {'4', "<sprite name=\"4\">"},
        {'5', "<sprite name=\"5\">"},
        {'6', "<sprite name=\"6\">"},
        {'7', "<sprite name=\"7\">"},
        {'8', "<sprite name=\"8\">"},
        {'9', "<sprite name=\"9\">"},
        {',', "<sprite name=\",\">"},
        {'.', "<sprite name=\".\">"},
        {'$', "<sprite name=\"$\">"},
        {'€', "<sprite name=\"€\">"},
        {'¢', "<sprite name=\"¢\">"},
        {'₱', "<sprite name=\"₱\">"},
        {'|', "<sprite name=\"|\">"}
    };

    public static string ConvertToTextImage(string textValue)
    {
        _stringBuilder.Remove(0, _stringBuilder.Length);
        for (int i = 0; i < textValue.Length; ++i)
        {
            char currentChar = textValue[i];
            if (_spriteStrings.ContainsKey(currentChar))
            {
                // No string format here to reduce string garbage creation.
                _stringBuilder.Append(_spriteStrings[currentChar]);
            }
            else
            {
                _stringBuilder.Append(currentChar);
            }
        }

        return _stringBuilder.ToString();
    }

    public static string ConvertToTextFromImage(string images)
    {
        _stringBuilder.Remove(0, _stringBuilder.Length);
        for (int i = 0; i < (images.Length / 17); i++)
        {
            _stringBuilder.Append(images[(i * 17) + 14]);
        }

        return _stringBuilder.ToString();
    }

    public static string ConvertToSpriteName(string textValue)
    {
        _stringBuilder.Remove(0, _stringBuilder.Length);
        string symbolName = "<sprite name=\"" + textValue + "\">";
        _stringBuilder.Append(symbolName);
        return _stringBuilder.ToString();
    }
}
