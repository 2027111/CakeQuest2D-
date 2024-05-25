using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{




    public static IEnumerator SlowDown(float duration = .1f, float timeFactor = .5f)
    {

        float t = 0;
        Time.timeScale = timeFactor;
        while(t < duration)
        {
            t += Time.deltaTime / Time.timeScale;
            yield return null;
        }


        Time.timeScale = 1;
    }
}
