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
    [SerializeField] Transform offPosition;
    Vector3 offScreenPosition
    {

        get { return offPosition.transform.position; }
    }
    Vector3 onScreenPosition
    {
        get { return showPosition.transform.position; }
    }
    private void Start()
    {
        Singleton = this;
    }


    public static void ShowTitle(string title)
    {
        Singleton?.SetText(title);
        // Singleton?.StartCoroutine(Singleton.ShowTitleCardAnimation());
    }

    private void SetText(string title)
    {


        string content = GetRoomName(title);
        townNameText.text = content;
    }

    public string GetRoomName(string title)
    {
        string desc = "";
        string newDesc = LanguageData.GetDataById("Locations").GetValueByKey(title);
        if (newDesc != "E404")
        {
            return newDesc;
        }
        return desc;
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
