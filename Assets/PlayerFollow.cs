using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour { 

    private Player player;
    private Vector3 dampened = Vector3.zero;
    private float accel = 5;
    private float dist = 5;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Manager.inst.Paused) return;
        Move();
    }

    void Move()
    {
        dampened = Vector3.Lerp(dampened, player.GetVelocity() * 1/player.GetMaxSpeed(), Time.deltaTime * accel);
        transform.position = player.transform.position + Vector3.forward * -10f;
        transform.position += dampened * dist;
    }
}
