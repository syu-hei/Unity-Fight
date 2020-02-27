using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour {

    private int highScore;
    public Text resultTime;
    public Text bestTime;
    public GameObject resultUI;


    void Start() {
        if (PlayerPrefs.HasKey("HighScore")) {              //セーブデータのHighScoreをhighScore変数に格納
            highScore = PlayerPrefs.GetInt("HighScore");
        } else {
            highScore = 999;                                //HighScoreが無い場合999を格納
        }
    }


    void Update() {
        if (Enemy.End) {                                    //Enemy.Endがtrueの時に実行
            resultUI.SetActive(true);                       //リザルトUIを有効にしてリザルトタイムとベストタイムを表示
            int result = Mathf.FloorToInt(Timer.time);
            resultTime.text = "ResultTime:" + result;
            bestTime.text = "BestTime:" + highScore;

            if (result < highScore) {                       //ベストタイムよりもリザルトタイムの値が小さければベストタイムを更新する
                PlayerPrefs.SetInt("HighScore", result);
            }
        }
    }


    public void OnRetry() {                                 //リトライボタンの設定
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }
}