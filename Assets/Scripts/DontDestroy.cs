using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy _singleton;

    public static DontDestroy Singleton
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
                Debug.Log($"{nameof(DontDestroy)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }

    private void Start()
    {
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }
}
