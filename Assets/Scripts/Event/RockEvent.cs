using UnityEngine;
using System.Collections;

public class RockEvent : MonoBehaviour {

    public GameObject Trap;                 //GameObjectにはRockを選択


    //プレイヤーがコライダーに触れた時トラップが発動する
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player") {
            Trap.SetActive(true);
        }
    }
}