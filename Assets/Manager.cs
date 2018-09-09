using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SASH
{
    public class Manager : MonoBehaviour
    {

        public static Manager inst;

        #region inspector variables
        public DialogueUI DialogueUIComp;
        public ScareFX ScareEffect;
        public TextAsset DialogueJSON;
        public Sprite[] CharacterSprites;
        public Player player;
        #endregion

        public bool Paused { get; set; }
        public TwineStory Dialogue { get; private set; }
		public bool GameOverStarted { get; private set; }

        private NPC selectedNPC;

        // Use this for initialization
        void Start()
        {
            inst = this;
            Dialogue = DialogueLoader.loadStory(DialogueJSON);
        }

        public void ShowDialogue(NPC npc)
        {
            selectedNPC = npc;
            ShowDialogue(npc.pid);
        }

        public void ShowDialogue(int pid)
        {
            Paused = true;
            DialogueUIComp.ShowDialogue(Dialogue.passages[pid - 1]);
            DialogueUIComp.gameObject.SetActive(true);
        }

        public void HideDialogue()
        {
            DialogueUIComp.gameObject.SetActive(false);
            if (selectedNPC != null) selectedNPC.StartCooldown();
            Paused = false;
            player.Unlock();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Manager.inst.DialogueUIComp.StopSpinner();
            }
        }

		public void StartGameOver(string triggerReason, string optionalText = null) {
			ScareEffect.Play(triggerReason, optionalText);
			GameOverStarted = true;
		}

        public void RestartFromCheckpoint()
        {

        }
    }
}
