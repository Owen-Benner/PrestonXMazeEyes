using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour {

	public Transform WhiteSquare;
	public Transform BlackSquare;

	public void Swap() {
		WhiteSquare.gameObject.SetActive(!WhiteSquare.gameObject.activeSelf);
		BlackSquare.gameObject.SetActive(!BlackSquare.gameObject.activeSelf);
	}

	public void Start() {
		WhiteSquare.gameObject.SetActive(false);
		BlackSquare.gameObject.SetActive(true);
	}

	public void Blink(float time){
		StartCoroutine (ShowWhite (time));
	}

	public IEnumerator ShowWhite(float time) {
		Swap ();
		yield return new WaitForSeconds (time);
		Swap ();
	}
}
