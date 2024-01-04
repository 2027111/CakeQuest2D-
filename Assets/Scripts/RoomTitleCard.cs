using System.Collections;
using TMPro;
using UnityEngine;

public class RoomTitleCard : MonoBehaviour
{
    private static RoomTitleCard _singleton;

    public static RoomTitleCard Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(RoomTitleCard)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }

    [SerializeField] TMP_Text townNameText;
    [SerializeField] float animationDuration = 2f;
    [SerializeField] float displayDuration = 3f;
    [SerializeField] Transform showPosition;
    Vector3 offScreenPosition;
    Vector3 onScreenPosition;
    private void Start()
    {
        Singleton = this;
        InitializePosition();
    }

    private void InitializePosition()
    {
        // Set the initial position off-screen near the bottom right corner
        offScreenPosition = transform.position;
        onScreenPosition = showPosition.position;
        Destroy(showPosition.gameObject);
    }

    public static void ShowTitle(string title)
    {
        Singleton?.SetText(title);
        Singleton?.StartCoroutine(Singleton.ShowTitleCardAnimation());
    }

    private void SetText(string title)
    {
        townNameText.text = title;
    }

    private IEnumerator ShowTitleCardAnimation()
    {
        // Move the title card on screen
        StartCoroutine(MoveToPosition(onScreenPosition, animationDuration));

        yield return new WaitForSeconds(animationDuration + displayDuration);

        // Move the title card off screen
        StartCoroutine(MoveToPosition(offScreenPosition, animationDuration));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
