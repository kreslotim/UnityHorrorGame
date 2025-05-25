using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;
    public float killDistance = 1f;
    public float attackDistance = 3f;
    public float detectionRadius = 8.7f;
    public GameObject deathText;
    public AudioSource growlAudio;
    public AudioSource roarAudio;

    private bool hasKilledPlayer = false;
    private bool isChasing = false;
    private bool growlPlayed = false;
    private bool roarPlayed = false;

    void Update()
    {
        if (hasKilledPlayer || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distance <= detectionRadius)
        {
            isChasing = true;
            if (!growlPlayed && growlAudio != null)
            {
                growlAudio.Play();
                growlPlayed = true;
            }
        }

        if (isChasing)
        {
            if (distance > attackDistance)
            {
                agent.SetDestination(player.position);
                if (animator != null)
                    animator.SetBool("isWalking", true);
            }
            else
            {
                if (!roarPlayed && roarAudio != null)
                {
                    roarAudio.Play();
                    roarPlayed = true;
                }

                if (animator != null)
                {
                    animator.SetBool("isWalking", false);
                    animator.SetTrigger("attack");
                }

                if (distance <= killDistance && !hasKilledPlayer)
                {
                    hasKilledPlayer = true;
                    Invoke(nameof(ShowDeathScreen), 1.5f);
                }
            }
        }
    }

    void ShowDeathScreen()
    {
        if (deathText != null)
            deathText.SetActive(true);

        Invoke(nameof(RestartLevel), 2f);
    }

    void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }
}
