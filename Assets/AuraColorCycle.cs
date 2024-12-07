using UnityEngine;

public class AuraColorCycle : MonoBehaviour
{
    // Toggle for rainbow cycle
    [SerializeField] private bool rainbowCycle = true;

    // Speed of the color cycle
    [SerializeField] private float cycleSpeed = 1f;

    // Custom color to use when rainbowCycle is false
    [SerializeField] private Color auraColor = Color.white;

    // Alpha value for transparency (0 = fully transparent, 1 = fully opaque)
    [Range(0f, 1f)]
    [SerializeField] private float alpha = 1f;

    // Reference to the SpriteRenderer
    private SpriteRenderer spriteRenderer;

    // Current hue value (0 to 1)
    private float hue = 0f;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
            return;
        }

        // Set the sprite's color to the specified auraColor with alpha applied
        auraColor.a = alpha;
        spriteRenderer.color = auraColor;
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        if (rainbowCycle)
        {
            // Increment the hue value over time
            hue += cycleSpeed * Time.deltaTime;

            // Keep hue within the range of 0 to 1
            if (hue > 1f) hue -= 1f;

            // Convert HSV to RGB, apply alpha, and set it to the sprite's color
            Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
            rainbowColor.a = alpha;
            spriteRenderer.color = rainbowColor;
        }
    }
}
