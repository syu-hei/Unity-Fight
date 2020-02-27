using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static float time;       //経過時間の変数


    void Start() {
        time = 0;       //ゲーム開始時にtime変数を０にする
    }


    void Update() {
        if (Enemy.End == false) {       //敵を倒すまでタイマーの処理を継続する
            time += Time.deltaTime;
        }
        int t = Mathf.FloorToInt(time);     //time変数をfloat型からint型に変換
        Text uiText = GetComponent<Text>();     //テキストにtime変数を表示
        uiText.text = "Time:" + t;
    }
}