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
            NPC npc = hit.gameObject.GetComponent<NPC>();
            if (npc != null)
            {
                if (!npc.isOnCooldown() && npc.pid > 0)
                {
                    // look at npc
                    transform.forward = Vector3.ProjectOnPlane(transform.position - npc.transform.position, Vector3.up);
                    // unlock mouse
                    fpsController.LockMouse(false);
                    // play dialogue
                    Manager.inst.ShowDialogue(npc);
                }
            }
        }

        public void Unlock()
        {
            fpsController.LockMouse(true);
        }
    }
}
