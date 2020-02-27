using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour {

	//スタートボタンを押したらMainを実行する
	public void StartGame() {
		SceneManager.LoadScene ("Main");
	}
}