using UnityEngine;
using System.Collections;

public class SearchCharacter : MonoBehaviour {

    private Enemy Enemy;                        //Enemyスクリプト


    void Start() {
        Enemy = GetComponentInParent<Enemy>();  //自身の親要素のコンポネートを取得
    }


    void OnTriggerStay(Collider col) {                  //コライダーに触れている間
        if (col.tag == "Player") {                      //プレイヤー発見
            Debug.Log("見つけた!");
            Enemy.EnemyState state = Enemy.GetState();  //敵キャラクターの状態を取得
            //プレイヤーを追いかける設定に変更
            if (state == Enemy.EnemyState.Wait || state == Enemy.EnemyState.Walk) {
                Enemy.SetState(Enemy.EnemyState.Chase, col.transform);
            }
        }
    }


    void OnTriggerExit(Collider col) {                  //コライダーから出た時
        if (col.tag == "Player") {
            Debug.Log("見失う");
            Enemy.SetState(Enemy.EnemyState.Wait);      //敵はwait状態になる
        }
    }
}