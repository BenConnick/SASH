using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class DialogueUI : MonoBehaviour {

    [SerializeField]
    private DialogueOption[] options;
    [SerializeField]
    private TextMeshProUGUI mainLabel;
    [SerializeField]
    private Image characterImage;

    private const int MAX_CHOICES = 4;

    public void ShowDialogue(Passage passage)
    {
        print(passage.name);
        if (passage.name.Equals("end"))
        {
            Manager.inst.HideDialogue();
            return;
        }
        characterImage.sprite = Manager.inst.CharacterSprites[passage.pid];
        SetText(passage.text);
        
        for (int i = 0; i < MAX_CHOICES; i++)
        {
            if (i >= passage.links.Length)
            {
                // clear
                options[i].SetLink(null);
            } else
            {
                // link
                options[i].SetLink(passage.links[i]);
            }
            
        }
    }

    private void SetText(string t)
    {
        string pattern = "\\[\\[.*\\]\\]+";
        Regex rx = new Regex(pattern);
        string cleaned = rx.Replace(t, "");
        pattern = "\\n+";
        Regex newlineRX = new Regex(pattern);
        cleaned = newlineRX.Replace(cleaned, " ");
        mainLabel.text = cleaned;
    }
}
