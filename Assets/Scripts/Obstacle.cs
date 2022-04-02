using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public string path = "center";
    public float moveInterval = 1f;
    public string type = "press";

    private GameObject player;
    private Vector3 targetPosition;
    private InfoSystem system;

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

        InvokeRepeating("Move", moveInterval, moveInterval);
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
            system.PressMissed();
        }
        else if (type == "cop")
        {
            system.CopMissed();
        }
    }

    private void Die()
    {
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
