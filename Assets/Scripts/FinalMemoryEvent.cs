using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalMemoryEvent : MonoBehaviour
{
    public Transform player;
    public GameObject promptText;
    public float triggerRadius = 2f;
    public Rigidbody[] paintings;
    public GameObject memoryCanvas;
    public TextMeshProUGUI memoryText;
    public GameObject fadePanel;
    public TextMeshProUGUI endTitleText;
    public AudioSource fallAudioSource;

    private bool hasTriggered = false;

    void Start()
    {
        if (promptText != null) promptText.SetActive(false);
        foreach (var rb in paintings)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        if (memoryCanvas != null) memoryCanvas.SetActive(false);
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
            Image panelImage = fadePanel.GetComponent<Image>();
            panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 0);
        }
        if (endTitleText != null)
        {
            endTitleText.text = "The End.";
            endTitleText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (hasTriggered) return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= triggerRadius)
        {
            if (promptText != null) promptText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (promptText != null) promptText.SetActive(false);
                hasTriggered = true;

                foreach (var rb in paintings)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }

                if (fallAudioSource != null)
                {
                    fallAudioSource.Play();
                }

                StartCoroutine(TriggerMemorySequence());
            }
        }
        else
        {
            if (promptText != null) promptText.SetActive(false);
        }
    }

    IEnumerator TriggerMemorySequence()
    {
        yield return new WaitForSeconds(2f);

        if (memoryCanvas != null) memoryCanvas.SetActive(true);
        if (memoryText != null)
        {
            string fullText = memoryText.text;
            memoryText.text = "";
            foreach (char c in fullText)
            {
                memoryText.text += c;
                yield return new WaitForSeconds(0.04f);
            }
        }

        yield return new WaitForSeconds(2f);

        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
            Image panelImage = fadePanel.GetComponent<Image>();
            Color startColor = panelImage.color;

            for (float t = 0; t < 1; t += Time.deltaTime / 2f)
            {
                float alpha = Mathf.Lerp(0, 1, t);
                panelImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
            panelImage.color = new Color(startColor.r, startColor.g, startColor.b, 1);
        }

        yield return new WaitForSeconds(1f);

        if (endTitleText != null)
        {
            endTitleText.gameObject.SetActive(true);
            Color endColor = endTitleText.color;
            endTitleText.color = new Color(endColor.r, endColor.g, endColor.b, 0);

            for (float t = 0; t < 1; t += Time.deltaTime / 2f)
            {
                float alpha = Mathf.Lerp(0, 1, t);
                endTitleText.color = new Color(endColor.r, endColor.g, endColor.b, alpha);
                yield return null;
            }
            endTitleText.color = new Color(endColor.r, endColor.g, endColor.b, 1);
        }
    }
}
