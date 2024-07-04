using UnityEngine;

[System.Serializable]
public class GamePreferenceData
{
    public float SFXVolume;
    public float MusicVolume;
    public float VoiceVolume;
    public Language Language;
}


public static class GamePreference
{
    private static float sfxVolume = 0;
    private static float musicVolume = 0;
    private static float voiceVolume = 0;
    private static Language language = LanguageData.defaultLanguage;
    public static string filePath = "";

    public static float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            SaveToFile();
        }
    }

    public static float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            SaveToFile();
        }
    }

    public static float VoiceVolume
    {
        get => voiceVolume;
        set
        {
            voiceVolume = value;
            SaveToFile();
        }
    }

    public static Language Language
    {
        get => language;
        set
        {
            language = value;
            SaveToFile();
        }
    }

    public static void SetPath(string path)
    {
        filePath = path;
    }

    public static void SaveToFile()
    {
        GamePreferenceData data = new GamePreferenceData
        {
            SFXVolume = SFXVolume,
            MusicVolume = MusicVolume,
            VoiceVolume = VoiceVolume,
            Language = Language
        };

        string jsonString = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(filePath, jsonString);
    }

    public static void LoadFromFile()
    {
        if (System.IO.File.Exists(filePath))
        {
            string jsonString = System.IO.File.ReadAllText(filePath);
            GamePreferenceData data = JsonUtility.FromJson<GamePreferenceData>(jsonString);

            sfxVolume = data.SFXVolume;
            musicVolume = data.MusicVolume;
            voiceVolume = data.VoiceVolume;
            language = data.Language;
        }
    }
}
