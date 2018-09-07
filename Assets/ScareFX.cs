using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScareFX : MonoBehaviour {

    [SerializeField]
    private string choice;
    [SerializeField]
    private string response;
    [SerializeField]
    private Text choiceLabel;
    [SerializeField]
    private Text responseLabel;

    private RectTransform rt;
    private const int numLoops = 20;
    private const float startInterval = 1f;
    private float interval = 1f;
    private bool playing;

    public void Awake()
    {
        rt = transform as RectTransform;
        choiceLabel.text = choice;
        responseLabel.text = response;
    }

    public void Play()
    {
        if (playing) return;
        playing = true;
        choiceLabel.gameObject.SetActive(false);
        responseLabel.gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(SpawnResponses());
    }

    IEnumerator SpawnResponses()
    {
        interval = startInterval;
        for (int i = 0; i < numLoops; i++)
        {
            yield return new WaitForSeconds(interval);
            interval *= Random.Range(0.6f, 0.7f);
            CreateLabel(choiceLabel.gameObject, false);
            yield return new WaitForSeconds(interval);
            CreateLabel(responseLabel.gameObject);
			yield return new WaitForSeconds(interval);
			CreateLabel(responseLabel.gameObject);
			yield return new WaitForSeconds(interval);
			CreateLabel(responseLabel.gameObject);
        }
        playing = false;
    }

	public void CreateLabel(GameObject original, bool toFront = true)
    {
        GameObject newLabel = GameObject.Instantiate(original,rt);
        ((RectTransform)newLabel.transform).anchoredPosition = 
            new Vector2(
                Random.Range(rt.rect.min.x, rt.rect.max.x), 
                Random.Range(rt.rect.min.y, rt.rect.max.y));
        newLabel.SetActive(true);
		if (toFront) newLabel.transform.SetAsLastSibling();
    }
}
