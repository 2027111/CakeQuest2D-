using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    private static CamManager _instance;
    private Transform _cameraTransform;
    private Vector3 _originalPos;
    private Vector3 _originalCamPos;
    [SerializeField] private float zoomDistance = -5f;

    void Awake()
    {
        // Ensure there's only one instance of the CamManager
        if (_instance == null)
        {
            _instance = this;
            _cameraTransform = Camera.main.transform;
            _originalCamPos = _cameraTransform.localPosition;
            _originalPos = transform.localPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Shake(float duration, float magnitude)
    {
        if (_instance != null)
        {
            _instance.ShakeCam(duration, magnitude);
        }
    }
    public void ShakeCam(float duration, float magnitude)
    {
        StartCoroutine(_instance.DoShake(duration, magnitude));
    }
    public void ShakeCam()
    {
        ShakeCam(-1, .15f);
    }

    public void StopShake()
    {
        _instance.StopCoroutine("DoShake");
    }
    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        bool timed  = duration > 0;
        if (duration < 0)
        {
            duration = 100;
        }
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _originalPos.z);

            if(timed)
            {
                elapsed += Time.deltaTime;
            }

            yield return null;
        }

        transform.localPosition = _originalPos;
    }


    public static void PanToCharacter(BattleCharacter battleCharacter, float panDuration = .3f)
    {


        Vector3 newPos = new Vector3(battleCharacter.transform.position.x, battleCharacter.transform.position.y + 2, _instance.zoomDistance); ;

        _instance.StartCoroutine(_instance.DoPan(newPos, panDuration));
    }


    public static void ResetView(float panDuration = .3f)
    {
        _instance.StartCoroutine(_instance.DoPan(_instance._originalCamPos, panDuration));

    }
    private IEnumerator DoPan(Vector3 position, float duration)
    {

        Vector3 start = _cameraTransform.position;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            _cameraTransform.localPosition = Vector3.Lerp(start, position, elapsed / duration);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }


        _cameraTransform.localPosition = position;
    }


    public static IEnumerator DoPan(Vector3 position, float duration, float timeUntilReset)
    {
        Vector3 newPos = new Vector3(position.x, position.y + 1, _instance.zoomDistance);
        yield return _instance.StartCoroutine(_instance.DoPan(newPos,  duration));
        yield return new WaitForSeconds(timeUntilReset);
        yield return _instance.StartCoroutine(_instance.DoPan(_instance._originalPos, duration));
    }
}
