using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Self_destruction : MonoBehaviour
{
    public float expireTime = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Expire", expireTime);
    }

    void Expire()
    {
        Destroy(gameObject);
    }
}
