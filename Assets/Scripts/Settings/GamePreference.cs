using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GamePreferenceData
{
    public int SFXVolume;
    public int MusicVolume;
    public int VoiceVolume;
    public bool fullScreen;
    public int width;
    public int height;
    public Language Language;
}

public static class GamePreference
{
    private static int sfxVolume = 0;
    private static int musicVolume = 0;
    private static int voiceVolume = 0;
    private static bool fullScreen = true;
    private static int width = -1;
    private static int height = -1;
    private static Language language = LanguageData.defaultLanguage;
    public static string filePath = "";

    public static int SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            SaveToFile();
        }
    }

    public static int MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            SaveToFile();
        }
    }

    public static int VoiceVolume
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
