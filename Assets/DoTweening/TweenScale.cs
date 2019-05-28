using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenScale : MonoBehaviour {

	[SerializeField]
	private RectTransform img;
	[SerializeField]
	private bool scale;
	[SerializeField]
	private float velS;

	private void Start() {

		img = GetComponent<Image>().rectTransform;
	}

	private void Update() {
		AnimacaoScale();
	}

	void AnimacaoScale()
	{
		if(scale) 
		{
			if(img.localScale.x == 1f) 
				img.DOScale(new Vector3(1.2f, 1.2f, 1.2f), velS);
			else if (img.localScale.x == 1.2f)
				img.DOScale(new Vector3(1.0f, 1.0f, 1.0f), velS);

		}
	}

}
