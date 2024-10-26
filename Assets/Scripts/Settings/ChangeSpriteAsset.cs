using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;
using UnityEngine.InputSystem;

public class ChangeSpriteAsset : MonoBehaviour
{
    [SerializeField] TMP_SpriteAsset currentSpriteAsset;
    [SerializeField] TMP_SpriteAsset keyboardSpriteAsset;
    [SerializeField] TMP_SpriteAsset controllerSpriteAsset;
    [SerializeField] TMP_Settings settings;



    public void OnControlsChanged(PlayerInput pi)
    {
        if (pi.currentControlScheme == pi.defaultControlScheme)
        {
            currentSpriteAsset = keyboardSpriteAsset;
        }
        else
        {
            currentSpriteAsset = controllerSpriteAsset;
        }
    }
}
