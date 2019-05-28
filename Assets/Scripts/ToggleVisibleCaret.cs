using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisibleCaret : MonoBehaviour {

	public List<InputFieldOnScreenKeyboard> _InputField;

	

	public void testeDown(int id) {

		for(int i = 0; i< _InputField.Count; i++) 
		{

			if(i == id )
				_InputField[i].caretColor = new Color(2, 5, 5, 1);
			else
				_InputField[i].caretColor = new Color(2, 5, 5, 0);

		}

		
	}

}
