using UnityEngine;
using TMPro;

public class DoorRotation : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 1.5f;
    public float openAngle = 90f;
    public float rotationSpeed = 0.1f;
    public TextMeshProUGUI doorPromptText;
    public AudioSource doorOpenAudioSource; // 🔊 Add this field for the sound

    private bool isDoorOpen = false;
    private Quaternion closedRotation;
    private Quaternion openedRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openedRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + openAngle, transform.eulerAngles.z);

        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
            else
                Debug.LogWarning("Player with tag 'Player' not found!");
        }

        if (doorPromptText != null)
        {
            doorPromptText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Door prompt text not assigned!");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < triggerDistance)
        {
            if (doorPromptText != null)
            {
                doorPromptText.gameObject.SetActive(true);
                doorPromptText.fontSize = 50;
                doorPromptText.fontStyle = FontStyles.Bold;
                doorPromptText.color = Color.white;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                isDoorOpen = !isDoorOpen;

                // 🔊 Play sound when toggling door
                if (doorOpenAudioSource != null)
                {
                    doorOpenAudioSource.Play();
                }
            }
        }
        else
        {
            if (doorPromptText != null)
                doorPromptText.gameObject.SetActive(false);
        }

        // Rotate door
        if (isDoorOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openedRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
