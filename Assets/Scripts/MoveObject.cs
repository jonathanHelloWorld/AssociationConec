using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MoveObject : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler {

	private RectTransform rectInit;
	private bool isback = false;
	CanvasGroup cg;

	[HideInInspector]
	public Vector3 posinit;

	private void Start() {

		cg = GetComponent<CanvasGroup>();
		rectInit = GetComponent<RectTransform>();
		posinit = this.transform.position;
		ControllerNewAssociation.Instance.resetPos += movePositionInit;
	}

	public void OnBeginDrag(PointerEventData eventData) {

		cg.blocksRaycasts = false;

	}

	public void OnDrag(PointerEventData eventData) {

		Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f);
		
		Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);

		GetComponent<RectTransform>().DOMove(position, 0.3f).SetEase(Ease.OutCubic).Play();
	}

	public void OnEndDrag(PointerEventData eventData) {

		
		Debug.Log(eventData.pointerCurrentRaycast.gameObject);
	
		if(eventData.pointerCurrentRaycast.gameObject != null) 
		{

			if (eventData.pointerCurrentRaycast.gameObject.tag == "container")
			{
				if (eventData.pointerCurrentRaycast.gameObject.GetComponent<DropAssociation>().Id == GetComponent<SetId>().id)
					isback = true;
				else
					isback = false;
			}
			else
				isback = false;
			
		}

		if (!isback) {
			ControllerNewAssociation.Instance.LoseSong.Stop();
			GetComponent<RectTransform>().DOMove(posinit, 0.3f).SetEase(Ease.OutCubic).Play();
			cg.blocksRaycasts = true;
			ControllerNewAssociation.Instance.LoseSong.Play();
		}
			

	}

	public void movePositionInit() 
	{

		GetComponent<RectTransform>().DOMove(posinit, 0.3f).SetEase(Ease.OutCubic).Play();
		cg.interactable = true;
		cg.blocksRaycasts = true;

	}

	
}
