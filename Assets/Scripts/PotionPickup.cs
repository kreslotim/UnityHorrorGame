using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PotionPickup : MonoBehaviour
{
    public Transform player;
    public float pickupDistance = 1.5f;
    public string nextSceneName = "Level2";
    public TextMeshProUGUI promptText;

    private bool pickedUp = false;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("Player with tag 'Player' not found!");
        }

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (pickedUp || player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= pickupDistance)
        {
            if (promptText != null)
            {
                promptText.text = "Press 'E' to interact with Potion";
                promptText.gameObject.SetActive(true);
                promptText.fontSize = 50;
                promptText.fontStyle = FontStyles.Bold;
                promptText.color = Color.white;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpPotion();
            }
        }
        else
        {
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }

    void PickUpPotion()
    {
        pickedUp = true;

        if (promptText != null)
            promptText.gameObject.SetActive(false);

        Debug.Log("Potion picked up, loading next scene...");

        gameObject.SetActive(false);

        SceneManager.LoadScene(nextSceneName);
    }
}
