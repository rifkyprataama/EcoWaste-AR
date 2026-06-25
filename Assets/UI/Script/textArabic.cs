using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;

#pragma warning disable IDE1006, IDE0290
public class textArabic : MonoBehaviour
{
    [Header("TMP Input Field and TMP Text for Arabic Fixing")]
    public TMP_InputField inputField;
    public TMP_Text outputText;
    public bool fixAutomatically = true;
    public bool reverseArabicWords = true;

    private static readonly HashSet<char> NonConnectToNext = new HashSet<char>
    {
        'ا','أ','إ','آ','د','ذ','ر','ز','و','ؤ','ء','ى','ة'
    };

    private static readonly HashSet<char> NonConnectToPrev = new HashSet<char>
    {
        'ء'
    };

    private struct ArabicLetter
    {
        public char isolated;
        public char final;
        public char initial;
        public char medial;

        public ArabicLetter(char isolated, char final, char initial, char medial)
        {
            this.isolated = isolated;
            this.final = final;
            this.initial = initial;
            this.medial = medial;
        }
    }

    private static readonly Dictionary<char, ArabicLetter> ArabicForms = new Dictionary<char, ArabicLetter>
    {
        { 'ء', new ArabicLetter('\uFE80','\uFE80','\uFE80','\uFE80') },
        { 'آ', new ArabicLetter('\uFE81','\uFE82','\uFE81','\uFE82') },
        { 'أ', new ArabicLetter('\uFE83','\uFE84','\uFE83','\uFE84') },
        { 'ؤ', new ArabicLetter('\uFE85','\uFE86','\uFE85','\uFE86') },
        { 'إ', new ArabicLetter('\uFE87','\uFE88','\uFE87','\uFE88') },
        { 'ئ', new ArabicLetter('\uFE89','\uFE8A','\uFE8B','\uFE8C') },
        { 'ا', new ArabicLetter('\uFE8D','\uFE8E','\uFE8D','\uFE8E') },
        { 'ب', new ArabicLetter('\uFE8F','\uFE90','\uFE91','\uFE92') },
        { 'ة', new ArabicLetter('\uFE93','\uFE94','\uFE93','\uFE94') },
        { 'ت', new ArabicLetter('\uFE95','\uFE96','\uFE97','\uFE98') },
        { 'ث', new ArabicLetter('\uFE99','\uFE9A','\uFE9B','\uFE9C') },
        { 'ج', new ArabicLetter('\uFE9D','\uFE9E','\uFE9F','\uFEA0') },
        { 'ح', new ArabicLetter('\uFEA1','\uFEA2','\uFEA3','\uFEA4') },
        { 'خ', new ArabicLetter('\uFEA5','\uFEA6','\uFEA7','\uFEA8') },
        { 'د', new ArabicLetter('\uFEA9','\uFEAA','\uFEA9','\uFEAA') },
        { 'ذ', new ArabicLetter('\uFEAB','\uFEAC','\uFEAB','\uFEAC') },
        { 'ر', new ArabicLetter('\uFEAD','\uFEAE','\uFEAD','\uFEAE') },
        { 'ز', new ArabicLetter('\uFEAF','\uFEB0','\uFEAF','\uFEB0') },
        { 'س', new ArabicLetter('\uFEB1','\uFEB2','\uFEB3','\uFEB4') },
        { 'ش', new ArabicLetter('\uFEB5','\uFEB6','\uFEB7','\uFEB8') },
        { 'ص', new ArabicLetter('\uFEB9','\uFEBA','\uFEBB','\uFEBC') },
        { 'ض', new ArabicLetter('\uFEBD','\uFEBE','\uFEBF','\uFEC0') },
        { 'ط', new ArabicLetter('\uFEC1','\uFEC2','\uFEC3','\uFEC4') },
        { 'ظ', new ArabicLetter('\uFEC5','\uFEC6','\uFEC7','\uFEC8') },
        { 'ع', new ArabicLetter('\uFEC9','\uFECA','\uFECB','\uFECC') },
        { 'غ', new ArabicLetter('\uFECD','\uFECE','\uFECF','\uFED0') },
        { 'ف', new ArabicLetter('\uFED1','\uFED2','\uFED3','\uFED4') },
        { 'ق', new ArabicLetter('\uFED5','\uFED6','\uFED7','\uFED8') },
        { 'ك', new ArabicLetter('\uFED9','\uFEDA','\uFEDB','\uFEDC') },
        { 'ل', new ArabicLetter('\uFEDD','\uFEDE','\uFEDF','\uFEE0') },
        { 'م', new ArabicLetter('\uFEE1','\uFEE2','\uFEE3','\uFEE4') },
        { 'ن', new ArabicLetter('\uFEE5','\uFEE6','\uFEE7','\uFEE8') },
        { 'ه', new ArabicLetter('\uFEE9','\uFEEA','\uFEEB','\uFEEC') },
        { 'و', new ArabicLetter('\uFEED','\uFEEE','\uFEED','\uFEEE') },
        { 'ى', new ArabicLetter('\uFEEF','\uFEF0','\uFEEF','\uFEF0') },
        { 'ي', new ArabicLetter('\uFEF1','\uFEF2','\uFEF3','\uFEF4') }
    };

    private void Awake()
    {
        if (inputField != null)
        {
            inputField.interactable = true;
            inputField.readOnly = false;

            if (fixAutomatically)
                inputField.onValueChanged.AddListener(OnTextChanged);
        }
    }

    private void OnDestroy()
    {
        inputField?.onValueChanged.RemoveListener(OnTextChanged);
    }

    public void OnTextChanged(string value)
    {
        if (outputText == null)
            return;

        outputText.text = FixArabicText(value);
    }

    public void ApplyFix()
    {
        if (inputField == null || outputText == null)
            return;

        outputText.text = FixArabicText(inputField.text);
    }

    public string FixArabicText(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        if (reverseArabicWords)
            input = ReverseArabicWords(input);

        var result = new StringBuilder(input.Length);
        TMP_FontAsset fontAsset = outputText?.font;

        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];
            if (!ArabicForms.ContainsKey(current))
            {
                result.Append(current);
                continue;
            }

            char previous = i > 0 ? input[i - 1] : '\0';
            char next = i < input.Length - 1 ? input[i + 1] : '\0';
            bool joinPrevious = IsArabicLetter(previous) && ConnectsToNext(previous);
            bool joinNext = IsArabicLetter(next) && ConnectsToPrev(next);

            ArabicLetter form = ArabicForms[current];
            char shapedChar;
            if (joinPrevious && joinNext)
                shapedChar = form.medial;
            else if (joinPrevious)
                shapedChar = form.final;
            else if (joinNext)
                shapedChar = form.initial;
            else
                shapedChar = form.isolated;

            result.Append(GetSupportedForm(current, shapedChar, fontAsset));
        }

        return result.ToString();
    }

    private static char GetSupportedForm(char original, char shaped, TMP_FontAsset fontAsset)
    {
        if (fontAsset == null)
            return original;

        if (fontAsset.HasCharacter(shaped))
            return shaped;

        return original;
    }

    private static bool IsArabicLetter(char c)
    {
        return ArabicForms.ContainsKey(c);
    }

    private static bool ConnectsToNext(char c)
    {
        return IsArabicLetter(c) && !NonConnectToNext.Contains(c);
    }

    private static bool ConnectsToPrev(char c)
    {
        return IsArabicLetter(c) && !NonConnectToPrev.Contains(c);
    }

    private static bool IsArabicChar(char c)
    {
        return (c >= '\u0600' && c <= '\u06FF')
            || (c >= '\u0750' && c <= '\u077F')
            || (c >= '\u08A0' && c <= '\u08FF');
    }

    private static string ReverseArabicWords(string input)
    {
        var output = new StringBuilder(input.Length);
        var units = new List<string>();
        var currentUnit = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];
            if (IsArabicChar(current))
            {
                if (IsArabicDiacritic(current) && currentUnit.Length > 0)
                {
                    currentUnit.Append(current);
                }
                else
                {
                    if (currentUnit.Length > 0)
                    {
                        units.Add(currentUnit.ToString());
                        currentUnit.Clear();
                    }

                    currentUnit.Append(current);
                }
            }
            else
            {
                if (currentUnit.Length > 0)
                {
                    units.Add(currentUnit.ToString());
                    currentUnit.Clear();
                }

                AppendReversedUnits(output, units);
                units.Clear();

                output.Append(current);
            }
        }

        if (currentUnit.Length > 0)
        {
            units.Add(currentUnit.ToString());
        }

        AppendReversedUnits(output, units);
        return output.ToString();
    }

    private static void AppendReversedUnits(StringBuilder output, List<string> units)
    {
        for (int j = units.Count - 1; j >= 0; j--)
            output.Append(units[j]);
    }

    private static bool IsArabicDiacritic(char c)
    {
        UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
        return category == UnicodeCategory.NonSpacingMark
            || category == UnicodeCategory.SpacingCombiningMark
            || category == UnicodeCategory.EnclosingMark;
    }
}
