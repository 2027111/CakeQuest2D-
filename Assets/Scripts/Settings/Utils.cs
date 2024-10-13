using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Utils
{



    public static IEnumerator SlowDown(float duration = .1f, float timeFactor = .5f)
    {

        float t = 0;
        Time.timeScale = timeFactor;
        while(t < duration)
        {

            if(Time.timeScale == 1)
            {
                t = duration;
            }
            t += Time.deltaTime / Time.timeScale;
            yield return null;
        }


        Time.timeScale = 1;
    }


    public static async Task<AudioClip> GetVoiceLine(string voiceClipId)
    {
        // Load the audio clip asynchronously
        var clipRequest = Resources.LoadAsync<AudioClip>($"VoiceLines/{LanguageData.GetLanguage()}/{voiceClipId}");
        await Task.Yield(); // Yield to allow the asynchronous operation to start

        // Wait for the audio clip to finish loading
        while (!clipRequest.isDone)
        {
            await Task.Yield();
        }

        AudioClip clip = clipRequest.asset as AudioClip;

        if (clip == null && LanguageData.GetLanguage() != LanguageData.defaultLanguage)
        {
            // Load the default language audio clip asynchronously
            var defaultClipRequest = Resources.LoadAsync<AudioClip>($"VoiceLines/{LanguageData.defaultLanguage}/{voiceClipId}");
            await Task.Yield(); // Yield to allow the asynchronous operation to start

            // Wait for the default audio clip to finish loading
            while (!defaultClipRequest.isDone)
            {
                await Task.Yield();
            }

            clip = defaultClipRequest.asset as AudioClip;
        }

        return clip;
    }

    public static void ResetTimeScale()
    {
        Time.timeScale = 1;

    }
}
