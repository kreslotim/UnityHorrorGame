using UnityEngine;

public class LampColorScript : MonoBehaviour
{
    private Material lampMaterial;

    // Range of scores to map from (e.g., 0 to 100)
    public int maxScoreForFullRed = 100;

    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            lampMaterial = renderer.material; // Use instance
        }
        else
        {
            Debug.LogError("MeshRenderer not found on Lamp!");
        }
    }

    void Update()
    {
        if (lampMaterial != null && GameState.Instance != null)
        {
            UpdateLampColor();
        }
    }

    void UpdateLampColor()
    {
        // Clamp score between 0 and maxScoreForFullRed
        float t = Mathf.Clamp01(GameState.Instance.score / (float)maxScoreForFullRed);

        // Red increases, Green/Blue decrease
        Color newColor = new Color(1f, 1f - t, 1f - t);
        lampMaterial.color = newColor;
    }
}
