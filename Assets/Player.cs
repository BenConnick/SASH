using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace SASH
{
    public class Player : MonoBehaviour
    {

        private CustomFPSController fpsController;

        private void Start()
        {
            fpsController = GetComponent<CustomFPSController>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            
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
                    //if (lookDir.z < 0) y = -y;
                    //transform.eulerAngles = new Vector3(0, y, 0);
                    transform.forward = -lookDir;
                    // play dialogue
                    Manager.inst.ShowDialogue(npc);
                }
            }
        }

        public void Unlock()
        {
            fpsController.enabled = true;
            fpsController.LockMouse(true);
        }
    }
}
