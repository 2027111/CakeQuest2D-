using UnityEngine;

public class DSErrorData
{

    public Color Color { get; set; }

    private void GenerateRandomColor()
    {
        Color = new Color32(
            (byte)Random.Range(65, 256), (byte)Random.Range(50, 176), (byte)Random.Range(50, 176), 255);
    }

    public DSErrorData()
    {
        GenerateRandomColor();
    }
}