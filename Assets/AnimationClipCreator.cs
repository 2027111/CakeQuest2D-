using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AnimationClipCreator : MonoBehaviour
{
    [SerializeField] string characterName = "Jimi";
    [SerializeField] string animationName = "idle";
    [SerializeField] string spriteSheetName = "spritesheet-name";
    [SerializeField] SpriteRenderer sprite;
    AnimationClip animClip;

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CreateClip();
            PlayAnim();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayAnim();
        }
    }
    public void CreateClip()
    {
        // Load all sprites from the specified sprite sheet in the Resources folder
        string path = $"Assets/TurnBattleSystem/{characterName}/SpriteSheet/{spriteSheetName}.png";
        object[] objects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
        Sprite[] sprites = (objects.Where(q => q is Sprite).Cast<Sprite>()).ToArray<Sprite>();
            if (sprites.Length == 0)
            {
                Debug.LogError($"No sprites found in {spriteSheetName}. Ensure the path and name are correct.");
                return;
            }
            else
            {

            // Create a new AnimationClip
            animClip = new AnimationClip
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




    }
    public void PlayAnim()
    {

        if (animClip)
        {
            // Create a new AnimationClip
            AnimationClip clip = new AnimationClip();

            // Copy the properties from the original clip
            EditorUtility.CopySerialized(animClip, clip);

            // Set the clip to legacy if needed
            clip.legacy = true;

            // Get the Animation component
            Animation animation = sprite.GetComponent<Animation>();
            if (animation == null)
            {
                animation = sprite.gameObject.AddComponent<Animation>();
            }

            // Add the clip to the animation component
            animation.AddClip(clip, animationName);
            Debug.Log($"Added animation clip {animationName} to {sprite.name}");

            // Play the animation
            animation.Play(animationName);
            Debug.Log($"Playing animation {animationName} on {sprite.name}");
        }
    }


#endif
}
