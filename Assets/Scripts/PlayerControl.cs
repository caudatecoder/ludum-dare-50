using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float movementDelta = 1f;
    public float movementCooldown = 500;
    public Vector3 initialPosition;

    private string currentPosition = "center";
    private float horizontalMove = 0f;
    private DateTime lastMovement;

    void Awake()
    {
        initialPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
       if (DateTime.Now.Subtract(lastMovement).TotalMilliseconds < movementCooldown) { return; }

        horizontalMove = Input.GetAxisRaw("Horizontal");


        if ( horizontalMove > 0)
        {
            lastMovement = DateTime.Now;
            MoveRight();
        }
        else if (horizontalMove < 0)
        {
            lastMovement = DateTime.Now;
            MoveLeft();
        }
    }

    public void MoveLeft()
    {
        if (currentPosition == "left") { return; }

        currentPosition = currentPosition == "center" ? "left" : "center";
        Vector3 newPosition = gameObject.transform.position;
        newPosition.x -= movementDelta;
        gameObject.transform.position = newPosition;
    }

    public void MoveRight()
    {
        if (currentPosition == "right") { return; }

        currentPosition = currentPosition == "center" ? "right" : "center";
        Vector3 newPosition = gameObject.transform.position;
        newPosition.x += movementDelta;
        gameObject.transform.position = newPosition;
    }
}
