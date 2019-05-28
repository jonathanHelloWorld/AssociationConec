using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DropAssociation : MonoBehaviour, IDropHandler {

	public int Id;

	private SetId setId;
	private MoveObject mObj;

	public void OnDrop(PointerEventData eventData) {

		if(eventData.pointerDrag.gameObject != null) 
		{
			setId = eventData.pointerDrag.gameObject.GetComponent<SetId>();
			mObj = eventData.pointerDrag.gameObject.GetComponent<MoveObject>();


			if (Id == setId.id) 
			{
				ControllerNewAssociation.Instance.CorrectSong.Play();
				eventData.pointerDrag.gameObject.GetComponent<RectTransform>().DOMove(transform.position, 0.3f).SetEase(Ease.OutCubic).Play();
				eventData.pointerDrag.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
				eventData.pointerDrag.gameObject.GetComponent<CanvasGroup>().interactable = false;
				ControllerNewAssociation.Instance.checkQtd++;
			}
			else
				eventData.pointerDrag.gameObject.GetComponent<RectTransform>().DOMove(mObj.posinit, 0.3f).SetEase(Ease.OutCubic).Play();

		}

	}
}
