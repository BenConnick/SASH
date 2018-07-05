using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region inspector vars
    [SerializeField]
    private float frictionCoeff;
    [SerializeField]
    private float accelCoeff;
    [SerializeField]
    private float maxSpeed;

    public Vector3 velocityRO;
    #endregion

    private Vector3 accel;
    private Vector3 vel;
    private float rotationDeg;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Manager.inst.Paused) return;
        HandleInput();
        Move();
        RotateSprite();
	}

    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v);
        velocityRO = dir;
        if (dir.SqrMagnitude() == 0)
        {
            accel = Vector3.zero;
        } else
        {
            accel = dir.normalized * accelCoeff;
        }
        
    }

    void Move()
    {
        vel += accel;
        vel *= (1-frictionCoeff);
        vel = Vector3.ClampMagnitude(vel, maxSpeed);
        transform.position += vel * Time.deltaTime;
    }

    void RotateSprite()
    {
        if (velocityRO.sqrMagnitude != 0)
        {
            if (velocityRO.x < 0)
            {
                rotationDeg = 90;
            }
            else if (velocityRO.x > 0)
            {
                rotationDeg = -90;
            }
            if (velocityRO.y < 0)
            {
                rotationDeg = 180;
            }
            else if (velocityRO.y > 0)
            {
                rotationDeg = 0;
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, rotationDeg);
    }

    public Vector3 GetVelocity()
    {
        return vel;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        NPC npc = collision.gameObject.GetComponent<NPC>();
        if (npc != null)
        {
            if (!npc.isOnCooldown() && npc.pid > 0) Manager.inst.ShowDialogue(npc);
            if (npc.pid < 0)
            {
                Manager.inst.ScareEffect.Play();
            }
        }
    }
}
