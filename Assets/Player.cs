using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace SASH
{
    public class Player : MonoBehaviour
    {
        private Collider itemTrigger;
        private CustomFPSController fpsController;
        [SerializeField] private GameObject phone;
        private Vector3 phoneDownPos = Vector3.zero;
        private Vector3 phoneUpPos = new Vector3(0,-0.02f,0.16f);

        private void Start()
        {
            fpsController = GetComponent<CustomFPSController>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (itemTrigger != null)
                {
                    Destroy(itemTrigger.gameObject);
                } else
                {
                    phone.transform.localPosition = phoneUpPos;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                phone.transform.localPosition = phoneDownPos;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            NPC npc = other.GetComponent<NPC>();
            if (npc != null)
            {
                if (!npc.isOnCooldown() && npc.pid > 0 && !Manager.inst.GameOverStarted)
                {
                    fpsController.enabled = false;
                    // unlock mouse
                    fpsController.LockMouse(false);
                    // look at npc
                    Vector3 lookDir = Vector3.ProjectOnPlane(transform.position - npc.transform.position, Vector3.up);
                    float y = Vector3.Angle(Vector3.forward, lookDir);
                    StartCoroutine(LookCoroutine(lookDir));
                    // play dialogue
                    Manager.inst.ShowDialogue(npc);
                }
            } else
            {
                if (other.name == "Milk")
                {
                    itemTrigger = other;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name == "Milk")
            {
                itemTrigger = null;
            }
        }

        private IEnumerator LookCoroutine(Vector3 lookDir)
        {
            const float duration = 0.4f;
            float timer = duration;
            yield return new WaitUntil(() =>
            {
                // non linear but that's ok
                transform.forward = Vector3.Lerp(transform.forward, -lookDir, (duration - timer) / timer);
                if (timer <= 0)
                {
                    transform.forward = -lookDir;
                    return true;
                }
                timer -= Time.deltaTime;
                return false;
            });
        }

        public void Unlock()
        {
            fpsController.enabled = true;
            fpsController.LockMouse(true);
        }

        
    }
}
