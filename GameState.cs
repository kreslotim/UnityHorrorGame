using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    // === GLOBAL VARIABLES ===
    public int score = 0;
    public bool hasKey = false;
    public string currentCheckpointId = "";

    [Header("Lighting Control")]
    public int maxScoreForFullRed = 100;
    private Light[] sceneLights;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Find all lights in the scene
        sceneLights = FindObjectsOfType<Light>();
    }

    private void Update()
    {
        UpdateLightColors();
    }

    private void UpdateLightColors()
    {
        float t = Mathf.Clamp01(score / (float)maxScoreForFullRed);
        Color color = new Color(1f, 1f - t, 1f - t); // redder as score increases

        foreach (var light in sceneLights)
        {
            if (light != null)
            {
                light.color = color;
            }
        }
    }
}
