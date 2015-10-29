using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ctrlMenu : MonoBehaviour {
    //public string playScene;
	// Use this for initialization

	public void Play(){
		Application.LoadLevel("scene_base");
		//Debug.Log ("Nova Cena");
	}

	public void Option(){
		//Application.LoadLevel("");
		Debug.Log ("Tela de Opcoes");
	}
	public void Exit(){
		//Application.Quit();
		Debug.Log ("Sair");
	}
}
