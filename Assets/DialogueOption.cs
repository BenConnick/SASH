using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;


[RequireComponent (typeof(Image))]
public class DialogueOption : MonoBehaviour {

    private Image background;
    private TextMeshProUGUI label;
    private Link dialogueLink;

	// Use this for initialization
	void Start () {
        background = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        addEvent(trigger, EventTriggerType.PointerEnter, OnPointerEnterDelegate);
        addEvent(trigger, EventTriggerType.PointerExit, OnPointerExitDelegate);
        addEvent(trigger, EventTriggerType.PointerClick, OnClickDelegate);
    }

    private static void addEvent(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> callbackHandler)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callbackHandler);
        trigger.triggers.Add(entry);
    }
	
	private void OnPointerEnterDelegate(BaseEventData data)
    {
        background.color = Color.white;
        label.color = Color.black;
    }

    private void OnPointerExitDelegate(BaseEventData data)
    {
        background.color = new Color(0,0,0,0);
        label.color = Color.white;
    }

    private void OnClickDelegate(BaseEventData data)
    {
        if (dialogueLink == null) return;
        Manager.inst.DialogueUIComp.ShowDialogue(Manager.inst.Dialogue.passages[dialogueLink.pid-1]);
    }

    public void SetLink(Link link)
    {
        dialogueLink = link;
        SetText(link == null ? "" : link.name);
    }

    public void SetText(string t)
    {
        string pattern = "\\n+";
        Regex newlineRX = new Regex(pattern);
        string cleaned = newlineRX.Replace(t, "");

        if (label == null) label = GetComponentInChildren<TextMeshProUGUI>();
        label.text = cleaned;
    }
}
