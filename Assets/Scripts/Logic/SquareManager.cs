using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour {

	public Transform WhiteSquare;
	public Transform BlackSquare;

	//Reference to main configuration script
	private FPSChanger fps;

	void Awake() {
		fps = GameObject.FindWithTag ("Logic").GetComponent<FPSChanger> ();
	}

	public void Swap() {
		WhiteSquare.gameObject.SetActive(!WhiteSquare.gameObject.activeSelf);
		BlackSquare.gameObject.SetActive(!BlackSquare.gameObject.activeSelf);
		fps.isWhite = !fps.isWhite;
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
