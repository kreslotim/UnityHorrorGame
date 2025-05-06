using UnityEngine;

public class LampToggle : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public Light lampLight;

    private bool isOn = true;

    void Start()
    {
        // Find Player by tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("Player with tag 'Player' not found!");
        }

        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>();
        }

        if (lampLight == null)
        {
            Debug.LogWarning("No Light component assigned or found in children!");
        }
    }

    void Update()
    {
        if (player == null || lampLight == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            isOn = !isOn;
            lampLight.enabled = isOn;
        }
    }
}
