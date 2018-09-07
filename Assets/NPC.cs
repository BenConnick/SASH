using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SASH
{
    public class NPC : MonoBehaviour
    {
        public enum BehaviorType { IDLE, WALK_UP_DOWN, WALK_LEFT_RIGHT }

        #region Inspector Variables
        public BehaviorType Mode;
        public float WalkSpeed = 1f;
        public float maxDist = 1f;
        public int pid = 1;
        [SerializeField]
        private float rotationDeg;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Collider trigger;
        #endregion

        private Vector3 startingPos;
        private bool reverse;
        private float cachedTime;
        private Vector3 vel;
        private float cooldownEnd;
        private const float cooldownDuration = 5f;
        private bool move = false;

        // Use this for initialization
        void Start()
        {
            startingPos = transform.position;
            animator = GetComponent<Animator>();
            if (trigger == null) StartCoroutine(RandomIntervalSwitch());
        }

        // Update is called once per frame
        void Update()
        {
            if (Manager.inst.Paused)
            {
				animator.SetFloat("Speed", 0);
                animator.SetFloat("Playback", 0.2f);
                return;
            }
            else
            {
                animator.SetFloat("Playback", 1);
            }
            if (move) Move();
            RotateSprite();
        }

        public IEnumerator RandomIntervalSwitch()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(3, 4));
                move = !move;
            }

        }

        void Move()
        {

            switch (Mode)
            {
			case BehaviorType.IDLE:
                break;
            case BehaviorType.WALK_UP_DOWN:
                WalkUpDown();
                break;
            case BehaviorType.WALK_LEFT_RIGHT:
                WalkLeftRight();
                break;
            }
            animator.SetFloat("Speed", vel.sqrMagnitude);
        }

        void Idle()
        {
            if (Time.time - cachedTime > 0.2f)
            {
                cachedTime = Time.time;
                reverse = !reverse;
            }
            //transform.position = startingPos + (reverse ? Vector3.forward : -Vector3.forward) * .01f;
        }

        void WalkUpDown()
        {
            vel = Vector3.forward * WalkSpeed;
            if (reverse)
            {
                if (transform.position.z - startingPos.z < -maxDist)
                {
                    reverse = !reverse;
                }
                vel *= -1;
            }
            else
            {
                if (transform.position.z - startingPos.z > maxDist)
                {
                    reverse = !reverse;
                }
            }
            //transform.position += vel * Time.deltaTime;
        }

        void WalkLeftRight()
        {
            vel = Vector3.right * WalkSpeed;
            if (reverse)
            {
                if (transform.position.x - startingPos.x < -maxDist)
                {
                    reverse = !reverse;
                }
                vel *= -1;
            }
            else
            {
                if (transform.position.x - startingPos.x > maxDist)
                {
                    reverse = !reverse;
                }
            }
            //transform.position += vel * Time.deltaTime;
        }

        void RotateSprite()
        {
            if (vel.sqrMagnitude != 0)
            {
                if (vel.x < 0)
                {
                    rotationDeg = 90;
                }
                else if (vel.x > 0)
                {
                    rotationDeg = -90;
                }
                if (vel.z < 0)
                {
                    rotationDeg = 180;
                }
                else if (vel.z > 0)
                {
                    rotationDeg = 0;
                }
            }
            transform.rotation = Quaternion.Euler(0, rotationDeg, 0);
        }



        public bool isOnCooldown()
        {
            return Time.time <= cooldownEnd;
        }

        public void StartCooldown()
        {
            cooldownEnd = cooldownDuration + Time.time;
        }
    }
}
