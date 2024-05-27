using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AnimationClipCreator : MonoBehaviour
{
    [SerializeField] string characterName = "Jimi";
    [SerializeField] string animationName = "idle";
    [SerializeField] string spriteSheetName = "spritesheet-name";


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CreateClip();
        }
    }
    public void CreateClip()
    {
#if UNITY_EDITOR
        // Load all sprites from the specified sprite sheet in the Resources folder
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteSheetName);

        if (sprites.Length == 0)
        {
            Debug.LogError($"No sprites found in {spriteSheetName}. Ensure the path and name are correct.");
            return;
        }

        // Create a new AnimationClip
        AnimationClip animClip = new AnimationClip
        {
            frameRate = 25 // Set the frame rate
        };

        // Create an EditorCurveBinding for the SpriteRenderer component's sprite property
        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        // Create keyframes for each sprite
        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteKeyFrames[i] = new ObjectReferenceKeyframe
            {
                time = i / animClip.frameRate,
                value = sprites[i]
            };
        }

        // Set the keyframes to the AnimationClip
        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);

        // Ensure the directory structure exists
        string directoryPath = $"Assets/TurnBattleSystem/{characterName}/Animation";
        if (!AssetDatabase.IsValidFolder(directoryPath))
        {
            AssetDatabase.CreateFolder($"Assets/TurnBattleSystem/{characterName}", "Animation");
        }

        // Create the asset and save it
        string assetPath = $"{directoryPath}/{characterName}_{animationName}.anim";
        AssetDatabase.CreateAsset(animClip, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Mark the new animation clip as dirty so Unity knows to save it
        EditorUtility.SetDirty(animClip);

        Debug.Log($"Animation clip created at {assetPath}");
    }
#endif
    }
