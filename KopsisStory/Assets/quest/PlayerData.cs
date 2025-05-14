using UnityEngine;

public static class CharacterData
{
    private const string NAMA_KEY = "Character_NAMA";
    private const string HO1_KEY = "Character_HO1";
    private const string HO2_KEY = "Character_HO2";

    public static string NAMA
    {
        get => PlayerPrefs.GetString(NAMA_KEY, "Gilang");
        set => PlayerPrefs.SetString(NAMA_KEY, value);
    }

    public static string HO1
    {
        get => PlayerPrefs.GetString(HO1_KEY, "Bang");
        set => PlayerPrefs.SetString(HO1_KEY, value);
    }

    public static string HO2
    {
        get => PlayerPrefs.GetString(HO2_KEY, "Abang");
        set => PlayerPrefs.SetString(HO2_KEY, value);
    }

    public static void ResetToDefault()
    {
        NAMA = "Gilang";
        HO1 = "Bang";
        HO2 = "Abang";
    }
}