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
    private int selectedChoice = 0;
    private int step = 0;
    private bool alt;

    Coroutine spinRoutine;

    public void ShowDialogue(Passage passage)
    {
        gameObject.SetActive(true);
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

        spinRoutine = StartCoroutine(SpinTheWheel());
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



    private IEnumerator SpinTheWheel()
    {
        selectedChoice = Random.Range(0, 4);
        float startTime = Time.time;
        float duration = 4;
        float nextOptionInterval = 0.5f;
        float nxOpTimer = 0;
        while(Time.time - startTime < duration) { 

            //yield return new WaitForSeconds(oneMinus * mult);
            yield return new WaitForFixedUpdate();
            float percent = (Time.time - startTime) / duration;
            float oneMinus = 1 - percent;
            nxOpTimer += Time.deltaTime;
            if (nxOpTimer > nextOptionInterval)
            {
                nxOpTimer -= nextOptionInterval;
                SelectNext();
                nextOptionInterval *= 0.87f;
            }
            // also use this loop for grow
            characterImage.rectTransform.localScale = Vector3.one * (1 + 0.25f*percent);
        }
    }

    private void SelectNext() { 
        int[] steps = { 1, 3, 2, 0 };
        step = (step + 1) % 4;
        selectedChoice = steps[step];
        for (int i = 0; i < MAX_CHOICES; i++)
        {
            options[i].Selected = (i == selectedChoice);
        }
    }

    public void StopSpinner()
    {
        if (spinRoutine == null) return;
        StopCoroutine(spinRoutine);
        spinRoutine = null;
        StartCoroutine(DelayedNextPassage());
    }

    private IEnumerator DelayedNextPassage()
    {
        yield return new WaitForSeconds(1);
        options[selectedChoice].Follow();
    }
}
