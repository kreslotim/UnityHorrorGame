using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Cinemachine;

public class BreakOnProximity : MonoBehaviour
{
    public float triggerRadius = 1.5f;
    public Transform player;
    public Transform interactionTarget;
    public TextMeshProUGUI promptText;

    public GameObject candleToHide;
    public TextMeshProUGUI screenMessageText;
    public Image backgroundPanel;
    public CinemachineVirtualCamera virtualCam;
    public float shakeDuration = 2f;
    public float shakeIntensity = 1f;

    public GameObject screamerImage;
    public AudioSource screamerSound;
    public Image fadeToBlackPanel;
    public string nextSceneName = "Level3";

    public DestructibleObject destructible;
    public GameObject wallTextObject;
    public Light[] flickerLights;
    public AudioSource heartbeatAudio;

    private bool hasTriggered = false;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("Player not found. Please assign it manually.");
        }

        if (promptText != null) promptText.gameObject.SetActive(false);
        if (screenMessageText != null) screenMessageText.gameObject.SetActive(false);
        if (backgroundPanel != null) backgroundPanel.gameObject.SetActive(false);
        if (screamerImage != null) screamerImage.SetActive(false);
        if (fadeToBlackPanel != null) fadeToBlackPanel.color = new Color(0, 0, 0, 0);
        if (wallTextObject != null) wallTextObject.SetActive(false);
        if (heartbeatAudio != null) heartbeatAudio.Stop();
    }

    void Update()
    {
        if (player == null || interactionTarget == null || hasTriggered)
            return;

        float distance = Vector3.Distance(player.position, interactionTarget.position);

        if (distance < triggerRadius)
        {
            if (promptText != null)
            {
                promptText.gameObject.SetActive(true);
                promptText.text = "Press 'E' to read Book";
                promptText.fontSize = 50;
                promptText.fontStyle = FontStyles.Bold;
                promptText.color = Color.white;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                hasTriggered = true;
                if (promptText != null) promptText.gameObject.SetActive(false);
                StartCoroutine(DisplayMessageThenBreak());
            }
        }
        else
        {
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }

    IEnumerator DisplayMessageThenBreak()
    {
        string[] lines = new string[] {
            "They told me I'm a patient here... in a psychiatric hospital.",
            "They said I'm dangerous. That I can't tell what's real anymore.",
            "They called me a schizophrenic. A psychopath..."
        };

        string innerVoice = "- No... I am not a PSYCHO !!!!!!!!!!";

        screenMessageText.text = "";
        backgroundPanel.gameObject.SetActive(true);
        screenMessageText.gameObject.SetActive(true);

        float delayPerChar = 0.02f;

        screenMessageText.color = Color.white;
        foreach (string line in lines)
        {
            foreach (char c in line)
            {
                screenMessageText.text += c;
                yield return new WaitForSeconds(delayPerChar);
            }
            screenMessageText.text += "\n";
            yield return new WaitForSeconds(0.5f);
        }

        screenMessageText.text += "\n\n<color=red>";
        foreach (char c in innerVoice)
        {
            screenMessageText.text += c;
            yield return new WaitForSeconds(delayPerChar);
        }
        screenMessageText.text += "</color>";

        if (virtualCam != null)
            StartCoroutine(ShakeCamera());

        yield return new WaitForSeconds(shakeDuration);

        backgroundPanel.gameObject.SetActive(false);
        screenMessageText.gameObject.SetActive(false);

        if (destructible != null)
            destructible.Break();
        if (candleToHide != null)
            candleToHide.SetActive(false);

        if (wallTextObject != null)
            wallTextObject.SetActive(true);

        StartCoroutine(FlickerAndHeartbeatThenScreamer());
    }

    IEnumerator ShakeCamera()
    {
        var perlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.LogWarning("Perlin component not found on virtual cam.");
            yield break;
        }

        perlin.m_AmplitudeGain = shakeIntensity;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        perlin.m_AmplitudeGain = 0f;
    }

    IEnumerator FlickerAndHeartbeatThenScreamer()
    {
        if (heartbeatAudio != null)
            heartbeatAudio.Play();

        float totalTime = 5f;
        float flickerRate = 0.1f;
        float elapsed = 0f;

        while (elapsed < totalTime)
        {
            bool state = Random.value > 0.5f;
            foreach (var light in flickerLights)
            {
                if (light != null) light.enabled = state;
            }

            yield return new WaitForSeconds(flickerRate);
            elapsed += flickerRate;
        }

        foreach (var light in flickerLights)
        {
            if (light != null) light.enabled = true;
        }

        if (heartbeatAudio != null)
            heartbeatAudio.Stop();

        StartCoroutine(PlayScreamerThenFade());
    }

    IEnumerator PlayScreamerThenFade()
    {
        if (screamerImage != null) screamerImage.SetActive(true);
        if (screamerSound != null) screamerSound.Play();

        yield return new WaitForSeconds(2f);

        if (fadeToBlackPanel != null)
        {
            float fadeDuration = 0.5f;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                float alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
                fadeToBlackPanel.color = new Color(0, 0, 0, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }
            fadeToBlackPanel.color = new Color(0, 0, 0, 1);
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}
