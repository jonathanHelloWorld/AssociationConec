using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControllerNewAssociation : Singleton<ControllerNewAssociation> {

	public int qtdContainers;

	public delegate void GetPos();
	public event GetPos resetPos;

	public AudioSource CorrectSong, LoseSong;

	public List<pieceAssociation> pieces;
	public List<Image> imgRefs;
	public List<Image> panels;
	public List<Text> txtRefs;

	[HideInInspector]
	public int checkQtd = 0;
	[HideInInspector]
	public bool ended = false;

	private int[] pieceRandom, panelsRandom;
	

	void Start () {

		pieceRandom = new int[5];
		panelsRandom = new int[5];

		StartGame();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnGameStartReset() 
	{
		StartCoroutine(oneSecond());

	}

	public void StartGame() {

		pieceRandom = _Shuffle.Shuffle();
		panelsRandom = _Shuffle.Shuffle();

		for (int i = 0; i < imgRefs.Count; i++) 
		{
			imgRefs[i].sprite = pieces[pieceRandom[i]].pieceImage;
			imgRefs[i].gameObject.GetComponent<SetId>().id = pieces[pieceRandom[i]].Id;
			// 2 shuffles
			panels[panelsRandom[i]].gameObject.GetComponent<DropAssociation>().Id = pieces[pieceRandom[i]].Id;
			txtRefs[panelsRandom[i]].text = pieces[pieceRandom[i]].textAssociado;
		}

		
	}

	IEnumerator oneSecond() {

		yield return new WaitForSeconds(1f);
			
		if (resetPos != null) resetPos();
		StartGame();

	}

}

[System.Serializable]
public class pieceAssociation {

	public Sprite pieceImage;
	public int Id;
	public string textAssociado;
	

}

