using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour {

    [SerializeField] public Vector3 pos1 = new Vector3(-36f, 8.5f, 39f);    //出発地
    [SerializeField] public Vector3 pos2 = new Vector3(-36f, 8.5f, -60f);   //目的方向
    [SerializeField] private float speed = 10f;                             //移動速度
    private Rigidbody rigid;                                                //Rigidbody


    void Start() {
        transform.position = pos1;                                          //オブジェクトのtransform
        rigid = GetComponent<Rigidbody>();                                  //コンポネートを取得
        //pos2-pos1で得た方向ベクトルを正規化して移動速度をかける(ForceMode.Impulseで瞬時に速度を加える)
        rigid.AddForce((pos2 - pos1).normalized * speed, ForceMode.Impulse);
    }
}