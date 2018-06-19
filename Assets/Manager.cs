using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager inst;

    #region inspector variables
    public DialogueUI DialogueUIComp;
    public TextAsset DialogueJSON;
    public Sprite[] CharacterSprites;
    #endregion

    public bool Paused { get; set; }
    public TwineStory Dialogue { get; private set; }

    private NPC selectedNPC;

    // Use this for initialization
    void Start () {
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
        DialogueUIComp.ShowDialogue(Dialogue.passages[pid]);
        DialogueUIComp.gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        DialogueUIComp.gameObject.SetActive(false);
        if (selectedNPC != null) selectedNPC.StartCooldown();
        Paused = false;
    }
}
