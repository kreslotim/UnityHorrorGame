using UnityEngine;
using TMPro;

public class LampToggle3 : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 1.5f;
    public Light lampLight;
    public TextMeshProUGUI toggleText;  // Floor text ("RUN")
    public TextMeshProUGUI promptText;  // UI text ("Press 'E' to interact...")

    private bool isOn = false;

    void Start()
    {
        // Find Player
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("Player with tag 'Player' not found!");
        }

        // Find lamp light
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>();
            if (lampLight == null)
                Debug.LogWarning("No Light component assigned or found!");
        }

        // Setup toggle text (on floor)
        if (toggleText != null)
        {
            toggleText.gameObject.SetActive(false);
            toggleText.fontSize = 600;
            toggleText.fontStyle = FontStyles.Bold;
        }

        // Setup prompt UI text
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);  // Hidden initially
            promptText.fontSize = 50;
            promptText.fontStyle = FontStyles.Bold;
            promptText.color = Color.white;
        }
        else
        {
            Debug.LogWarning("No Prompt Text assigned!");
        }
    }

    void Update()
    {
        if (player == null || lampLight == null || toggleText == null || promptText == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance)
        {
            promptText.text = "Press 'E' to interact with Lamp";
            promptText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                isOn = !isOn;
                lampLight.enabled = isOn;

                toggleText.text = isOn ? "Remember" : " ";
                toggleText.gameObject.SetActive(true);
            }
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
