using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

namespace SASH
{
    public class DialogueUI : MonoBehaviour
    {

        [SerializeField]
        private DialogueOption[] options;
        [SerializeField]
        private TextMeshProUGUI mainLabel;
        [SerializeField]
        private Image characterImage;
        [SerializeField]
        private DialogueOption selectedOption;
        private RectTransform selectedOptionRT;

        private const int MAX_CHOICES = 4;
        private int selectedChoice = 0;
        private int step = 0;
        private bool alt;
        private float prevPercent = 1; // do not reset zoom percent until 
        private float percent;

        Coroutine spinRoutine;

        public void Start()
        {
            selectedOptionRT = (RectTransform)selectedOption.transform;
            selectedOption.gameObject.SetActive(true);
            selectedOption.gameObject.SetActive(false);
        }

        public void ShowDialogue(Passage passage)
        {
            selectedOption.gameObject.SetActive(false);
            gameObject.SetActive(true);
            print(passage.name);
            if (passage.name.Equals("end"))
            {
                Manager.inst.HideDialogue();
                Manager.inst.ScareEffect.Play();
                return;
            }
            characterImage.sprite = Manager.inst.CharacterSprites[passage.pid];
            SetText(passage.text);

            for (int i = 0; i < options.Length; i++)
            {
                options[i].Selected = false;
            }

            for (int i = 0; i < MAX_CHOICES; i++)
            {
                if (i >= passage.links.Length)
                {
                    // clear
                    options[i].SetLink(null);
                }
                else
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
            while (Time.time - startTime < duration)
            {
                percent = (Time.time - startTime) / duration;
                float oneMinus = 1 - percent;
                nxOpTimer += Time.deltaTime;
                if (nxOpTimer > nextOptionInterval)
                {
                    nxOpTimer -= nextOptionInterval;
                    SelectNext();
                    nextOptionInterval *= 0.87f;
                }
                // also use this loop for grow
                characterImage.rectTransform.localScale = Vector3.one * (prevPercent + 0.25f * percent);
                yield return new WaitForFixedUpdate();
            }
        }

        private void SelectNext()
        {
            int[] steps = { 1, 3, 2, 0 };
            step = (step + 1) % 4;
            // skip blank
            if (options[steps[step]].GetText().Equals(""))
            {
                SelectNext();
                return;
            }
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
            selectedOption.gameObject.SetActive(true);
            selectedOption.Selected = true;
            RectTransform rt = (RectTransform)options[selectedChoice].transform;
            selectedOptionRT.sizeDelta = rt.rect.size;
            Vector2 startMin = selectedOptionRT.anchorMin = rt.anchorMin;
            Vector2 startMax = selectedOptionRT.anchorMax = rt.anchorMax;
            selectedOption.SetText(options[selectedChoice].GetText());
            const float duration = 0.2f;
            float endTime = Time.time + duration;
            while(Time.time < endTime)
            {
                float t = 1 - ((endTime - Time.time) / duration);
                selectedOptionRT.anchorMin = Vector2.Lerp(startMin, Vector2.zero, t);
                selectedOptionRT.anchorMax = Vector2.Lerp(startMax, Vector2.one, t);
                selectedOptionRT.sizeDelta = Vector2.zero;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.8f);
            prevPercent = 0.25f * percent + prevPercent;
            options[selectedChoice].Follow();
            selectedOption.gameObject.SetActive(false);
        }
    }
}