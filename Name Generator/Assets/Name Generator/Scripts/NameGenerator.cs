using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class NameGenerator : MonoBehaviour
{
    public UnityEvent<List<string>> FinishedNameGeneration = new UnityEvent<List<string>>();
    private static readonly char[] _consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };
    private static readonly char[] _vowels = { 'a', 'e', 'i', 'o', 'u' };

    public int nameLengthMin = 4;
    public int nameLengthMax = 8;
    public int numberOfNames = 100;
    public bool logNames = true;
    public bool removeObscenities = true;

    private List<string> _names = new List<string>();

    public void SetAmountToGenerate(int numberToSpawn)
    {
        numberOfNames = numberToSpawn;
    }
    public void SetAmountToGenerate(string numberToSpawn)
    {
        if (numberToSpawn != "")
        {
            numberOfNames = int.Parse(numberToSpawn);
        }
        else
        {
            numberOfNames = 100;
        }
    }

    public void StartGeneration()
    {
        _names.Clear();
        FinishedNameGeneration.Invoke(_names);
        StartCoroutine(GenerateNames());
    }

    public IEnumerator GenerateNames()
    {
        while (_names.Count < numberOfNames)
        {
            var name = GenerateName(nameLengthMin, nameLengthMax);
            TestNames(name);
            yield return new WaitForSeconds(0.001f);
        }
        _names.Sort();
        FinishedNameGeneration.Invoke(_names);
        if (logNames)
        {
            string namesString = "";
            for (int i = 0; i < _names.Count; i++)
            {
                namesString += $"{_names[i]}, ";
            }
            Debug.Log($"Generated {_names.Count} unique names:");
            Debug.Log($"{namesString}");
        }
    }

    private void TestNames(string name)
    {
        if (name.Length < nameLengthMin || name.Length > nameLengthMax + 1)
        {
            Debug.LogError($"Invalid name '{name}': length must be between 4 and 8 characters");
            return;
        }
        if (name.Any(c => !char.IsLetter(c)))
        {
            Debug.LogError($"Invalid name '{name}': contains non-letter characters");
            return;
        }
        if (Obscenities.Obscenes.Contains(name.ToLower()) && removeObscenities)
        {
            Debug.LogError($"Invalid name '{name}': contains obscenity");
            return;
        }
        if (!_names.Contains(name))
        {
            _names.Add(name);
        }
    }

    private string GenerateName(int lengthMin, int lengthMax)
    {
        List<char> nameChar = new List<char>();
        var length = Random.Range(lengthMin, lengthMax + 1);
        char nextChar;

        for (var i = 0; i < length; i++)
        {
            var isVowel = Random.Range(0, 2) == 0;
            if (isVowel)
            {
                nextChar = _vowels[Random.Range(0, _vowels.Length)];
                if (nameChar.Count > 1)
                {
                    while (nameChar[nameChar.Count - 1] == nextChar || nameChar[nameChar.Count - 2] == nextChar)
                    {
                        nextChar = _vowels[Random.Range(0, _vowels.Length)];
                    }
                }

            }
            else
            {
                nextChar = _consonants[Random.Range(0, _consonants.Length)];
            }

            // Check if "u" should follow "q"
            if (nameChar.Count > 0 && nameChar[nameChar.Count - 1] == 'q' && nextChar != 'u')
            {
                nextChar = 'u';
            }

            // Check if "x" is last letter so shouldnt add "s"
            if (nameChar.Count > 0 && nameChar[nameChar.Count - 1] == 'x' && nextChar == 's')
            {
                nextChar = 'c';
            }

            // Check if last 2 letters are Consonants if so then the next one has to be a vowel
            if (nameChar.Count > 2 && (_consonants.Contains(nameChar[nameChar.Count - 1]) && _consonants.Contains(nameChar[nameChar.Count - 2]) && _consonants.Contains(nextChar)))
            {
                nextChar = _vowels[Random.Range(0, _vowels.Length)];
            }

            // Checks if the last char is a 'e'
            if (nameChar.Count > 1 && nameChar[nameChar.Count - 1] == 'e' && nextChar == 'i')
            {
                if (nameChar[nameChar.Count - 2] != 'c')
                {
                    nameChar[nameChar.Count - 1] = 'i';
                    nextChar = 'e';
                }
            }
            nameChar.Add(nextChar);
        }
        if (nameChar[nameChar.Count - 1] == 'v' || nameChar[nameChar.Count - 1] == 'j')
        {
            nameChar.Add('e');
        }
        string finalName = "";
        foreach (char item in nameChar)
        {
            finalName += item;
        }
        return char.ToUpper(finalName[0]) + finalName.Substring(1);
    }
}
