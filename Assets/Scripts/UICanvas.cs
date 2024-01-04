using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{

    private static UICanvas _singleton;

    public static UICanvas Singleton
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
                Debug.Log($"{nameof(UICanvas)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }


    private void Start()
    {
        Singleton = this;
        DontDestroyOnLoad(Singleton);
    }
}
