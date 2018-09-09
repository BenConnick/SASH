using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScareFX : MonoBehaviour {

    [SerializeField]
    private Text choiceLabel;
    [SerializeField]
    private Text responseLabel;
    [SerializeField]
    private Text restartLabel;

    private RectTransform rt;
    private const int numLoops = 150;
    private const float startInterval = 0.25f;
    private float interval = 1f;
    private bool playing;

    private List<string> customBadThoughts;
    private int cursor = 1;
    private string redText;

    public void Awake()
    {
        rt = transform as RectTransform;
    }

    public void Play(string choice, string optionalText = "")
    {
        if (playing) return;
        playing = true;
        redText = choice;
        customBadThoughts = new List<string>(optionalText.Split('\n'));
        cursor = 1;
        restartLabel.gameObject.SetActive(false);
        choiceLabel.gameObject.SetActive(false);
        responseLabel.gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(SpawnResponses());
    }

    IEnumerator SpawnResponses()
    {
        CreateChoiceLabel();
        interval = startInterval;
        for (int i = 0; i < numLoops; i++)
        {
            yield return new WaitForSeconds(interval);
            interval *= Random.Range(0.8f, 0.9f);
            yield return new WaitForSeconds(interval);
            CreateLabel();
        }
        Image curtain = CreateCurtain();
        float duration = 1f;
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            curtain.color = new Color(0, 0, 0, t);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        startTime = Time.time;
        restartLabel.gameObject.SetActive(true);
        restartLabel.transform.SetAsLastSibling();
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            restartLabel.color = new Color(1, 1, 1, t);
            yield return new WaitForEndOfFrame();
        }
        playing = false;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

    public void CreateChoiceLabel()
    {
        GameObject original = choiceLabel.gameObject;
        GameObject newLabel = GameObject.Instantiate(original, rt);
        Vector2 midpoint = new Vector2((rt.rect.max.x - rt.rect.min.x) * 0.5f + rt.rect.min.x, (rt.rect.max.y - rt.rect.min.y) * 0.5f + rt.rect.min.y);
        ((RectTransform)newLabel.transform).anchoredPosition = midpoint;
        string text = redText;
        newLabel.GetComponent<Text>().text = text;
        newLabel.SetActive(true);
    }

    public void CreateLabel()
    {
        GameObject original = responseLabel.gameObject;
        GameObject newLabel = GameObject.Instantiate(original,rt);
        ((RectTransform)newLabel.transform).anchoredPosition = 
            new Vector2(
                Random.Range(rt.rect.min.x, rt.rect.max.x), 
                Random.Range(rt.rect.min.y, rt.rect.max.y));
        string text = GetRandomScareString();
        if (cursor < customBadThoughts.Count)
        {
            text = customBadThoughts[cursor];
            cursor++;
        }
        newLabel.GetComponent<Text>().text = text;
        newLabel.SetActive(true);
		// newLabel.transform.SetAsLastSibling(); unnecessary
    }

    public Image CreateCurtain()
    {
        GameObject curtain = new GameObject();
        Image img = curtain.AddComponent<Image>();
        img.color = Color.clear;
        curtain.transform.SetParent(transform, false);
        curtain.transform.localPosition = Vector3.zero;
        curtain.transform.localScale = Vector3.one * 1000f;
        return img;
    }

    private string[] badThoughts =
    {
        "I’m so stupid",
        "why did I say it?",
        "I hate myself",
        "so embarrassing",
        "hates me now",
        "what was I thinking?",
        "how could I..?",
        "this is the worst",
        "I want to disappear",
        "obviously upset",
        "always like this",
        "total disaster",
        "shouldn’t have come",
        "Oh no…"
    };

    public string GetRandomScareString()
    {
        return badThoughts[Random.Range(0, badThoughts.Length)];
    }
}
