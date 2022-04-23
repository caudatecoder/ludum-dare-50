using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public string path = "center";
    public string type = "press";
    public AudioClip missedCopSFX;
    public AudioClip missedPressSFX;
    public GameObject deathFx;

    private GameObject player;
    private Vector3 targetPosition;
    private InfoSystem system;
    private int moveCount;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        system = GameObject.FindWithTag("InfoSystem").GetComponent<InfoSystem>();
    }
    void Start()
    {
        targetPosition = player.GetComponent<PlayerControl>().initialPosition;
        if (path == "right")
        {
            targetPosition.x += player.GetComponent<PlayerControl>().movementDelta;
        } else if (path == "left")
        {
            targetPosition.x -= player.GetComponent<PlayerControl>().movementDelta;

        }

        system.stepEvent.AddListener(Move);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    private void Move()
    {
        moveCount++;

        if (type == "press" && moveCount % 2 == 1)
        {
            return;
        }

        Vector3 heading = targetPosition - gameObject.transform.position;
        Vector3 direction = heading / heading.magnitude;

        if (heading.magnitude < 0.7f)
        {   
            Missed();
            return;
        }

        gameObject.transform.position = gameObject.transform.position + (direction * 1f);
    }

    private void Missed()
    {
        Destroy(gameObject);
        if (type == "press")
        {
            AudioSource.PlayClipAtPoint(missedPressSFX, gameObject.transform.position, 2f);
            system.PressMissed();
        }
        else if (type == "cop")
        {
            AudioSource.PlayClipAtPoint(missedCopSFX, gameObject.transform.position, 1f);
            system.CopMissed();
        }
    }

    private void Die()
    {
        Instantiate(deathFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
        if (type == "press")
        {
            system.PressBusted();
        } else if (type == "cop")
        {
            system.CopBusted();
        }
    }
}
