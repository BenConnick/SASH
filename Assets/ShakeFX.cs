using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeFX : MonoBehaviour {

    public float ShakeAmount = 10;
	public float GrowAmount = 0;

    private Vector2 startPos;
    private RectTransform rt;

	// Use this for initialization
	void Start () {
        rt = transform as RectTransform;
        startPos = rt.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {
        rt.anchoredPosition = startPos + Random.insideUnitCircle * ShakeAmount;
		transform.localScale += GrowAmount * transform.localScale * Time.deltaTime;
	}
}
