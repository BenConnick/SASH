using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

    public float height = 0.05f;
    public float speed = 2;

    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * speed) * height;
	}
}
