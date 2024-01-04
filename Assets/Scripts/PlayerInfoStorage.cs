using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoStorage : MonoBehaviour
{


    public PlayerStorage infoStorage;
    [SerializeField] Character character;


    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
        if (infoStorage)
        {

        if (infoStorage.forceNewInfo)
        {
            character.SetPosition(infoStorage.nextPosition);
            Camera.main.GetComponent<CameraMovement>().ForceToTarget();
            Camera.main.GetComponent<CameraMovement>().SetNewRoom(infoStorage.nextRoomInfo);
            infoStorage.forceNewInfo = false;
            }
        }
    }
}
