using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep music across scenes

        // Optional: prevent duplicates if multiple scenes have this object
        if (FindObjectsOfType<PersistentMusic>().Length > 1)
            Destroy(gameObject);
    }
}
