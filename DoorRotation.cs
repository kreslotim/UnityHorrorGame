using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 3.0f;
    public float openAngle = 90f;
    public float rotationSpeed = 0.1f; // slower rotation
    private bool isOpening = false;
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
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Player with tag 'Player' not found!");
            }
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < triggerDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isDoorOpen = !isDoorOpen; // Toggle door open/close
            }
        }

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
