using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] int defaultFrameRate= 25;
    [SerializeField] SpriteRenderer sprite;
    AnimationClip animClip;

#if UNITY_EDITOR



    [ContextMenu("Créer Animation")]
    public void Create()
    {
        SpliceSprite();
        CreateClip();
        PlayAnim();
    }

    public void SpliceSprite()
    {
        string directoryPath = $"Assets/TurnBattleSystem/{characterName}/SpriteSheet/";

        // Search for the file that matches the pattern (case-insensitive)
        string[] files = Directory.GetFiles(directoryPath);
        string pattern = $"{animationName}_Strip";
        string path = files.FirstOrDefault(f => Path.GetFileName(f).IndexOf(pattern, System.StringComparison.OrdinalIgnoreCase) >= 0);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            if(importer.spriteImportMode != SpriteImportMode.Multiple)
            {

            if (path == null)
        {
            Debug.LogError($"No sprite sheet found for {animationName} in {directoryPath}");
            return;
        }

        // Load the sprite sheet
        Texture2D spriteSheet = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (spriteSheet == null)
        {
            Debug.LogError($"Failed to load sprite sheet at path: {path}");
            return;
        }
        Debug.Log($"Loaded sprite sheet: {spriteSheet.name}");

        string[] split = spriteSheet.name.Split('_');
        string amount = split[split.Length - 1].ToLower().Replace("strip", "");
        int frames = int.Parse(amount);
        Debug.Log(frames);

        List<Sprite> spritesheetList = new List<Sprite>();
        float width = spriteSheet.width / frames;
        float height = spriteSheet.height;

        for (int i = 0; i < frames; i++)
        {
            spritesheetList.Add(Sprite.Create(spriteSheet, new Rect(width * i, 0.0f, width, height), new Vector2(0.5f, 0.5f), 100.0f));
        }

        // Create a new Texture2D
        Texture2D newTexture = new Texture2D(spriteSheet.width, spriteSheet.height);
        newTexture.SetPixels(spriteSheet.GetPixels());
        newTexture.Apply();

        // Save the new texture as a sprite sheet with multiple sprites
        SaveSpriteSheet(newTexture, path, frames, (int)width, (int)height);

        // Reimport the texture with the new import settings
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            }
        }
    }

    private void SaveSpriteSheet(Texture2D texture, string path, int frames, int frameWidth, int frameHeight)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            File.WriteAllBytes(path, pngData);
            AssetDatabase.Refresh();

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritePixelsPerUnit = 100;
                importer.isReadable = true;  // Enable Read/Write

                List<SpriteMetaData> metas = new List<SpriteMetaData>();
                for (int i = 0; i < frames; i++)
                {
                    SpriteMetaData meta = new SpriteMetaData
                    {
                        rect = new Rect(i * frameWidth, 0, frameWidth, frameHeight),
                        name = $"{animationName}_frame_{i}",
                        pivot = new Vector2(0.5f, 0.5f)
                    };
                    metas.Add(meta);
                }
                importer.spritesheet = metas.ToArray();
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }
    }

    public void CreateClip()
    {
        // Load all sprites from the specified sprite sheet in the Resources folder
        string directoryPathSprite = $"Assets/TurnBattleSystem/{characterName}/SpriteSheet/";

        // Search for the file that matches the pattern (case-insensitive)
        string[] files = Directory.GetFiles(directoryPathSprite);
        string pattern = $"{animationName}_Strip";
        string path = files.FirstOrDefault(f => Path.GetFileName(f).IndexOf(pattern, System.StringComparison.OrdinalIgnoreCase) >= 0);
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
                frameRate = this.defaultFrameRate // Set the frame rate
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
