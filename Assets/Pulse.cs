using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float amount = 0.05f;
    public float speed = 2;

    Vector3 startScale;

    // Use this for initialization
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = startScale + Vector3.one * Mathf.Sin(Time.time * speed) * amount;
    }
}
