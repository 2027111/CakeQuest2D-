using UnityEngine;

[System.Serializable]
public class GamePreferenceData
{
    public float SFXVolume;
    public float MusicVolume;
    public float VoiceVolume;
    public bool fullScreen;
    public int width;
    public int height;
    public Language Language;
}

public static class GamePreference
{
    private static float sfxVolume = 0;
    private static float musicVolume = 0;
    private static float voiceVolume = 0;
    private static bool fullScreen = true;
    private static int width = -1;
    private static int height = -1;
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

    public static bool FullScreen
    {
        get => fullScreen;
        set
        {
            fullScreen = value;
            SaveToFile();
        }
    }

    public static int Width
    {
        get => width;
        set
        {
            width = value;
            SaveToFile();
        }
    }

    public static int Height
    {
        get => height;
        set
        {
            height = value;
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
            fullScreen = FullScreen,
            width = Width,
            height = Height,
            Language = Language
        };

        string jsonString = JsonUtility.ToJson(data, true);
        if (!string.IsNullOrEmpty(filePath))
        {
            System.IO.File.WriteAllText(filePath, jsonString);
        }
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
            fullScreen = data.fullScreen;
            width = data.width;
            height = data.height;
            language = data.Language;
        }
    }
}
