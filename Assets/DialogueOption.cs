using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;

namespace SASH
{
    [RequireComponent(typeof(Image))]
    public class DialogueOption : MonoBehaviour
    {

        private Image background;
        private TextMeshProUGUI label;
        private Link dialogueLink;

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; UpdateUI(); }
        }

        // Use this for initialization
        void Start()
        {
            background = GetComponent<Image>();
            label = GetComponentInChildren<TextMeshProUGUI>();
            UpdateUI();
        }

        public void UpdateUI()
        {
            background.color = selected ? Color.white : Color.black;
            label.color = selected ? Color.black : Color.white;
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

        public void Follow()
        {
            if (dialogueLink == null) return;
            Manager.inst.DialogueUIComp.ShowDialogue(Manager.inst.Dialogue.passages[dialogueLink.pid - 1]);
        }

        public string GetText()
        {
            return label.text;
        }
    }
}
