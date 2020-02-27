using UnityEngine;
using System.Collections;

public class EnemyAnimEvent : MonoBehaviour {

    private Enemy enemy;                                //Enemyスクリプト
    [SerializeField] private Collider AttackCollider;   //攻撃判定するコライダー


    void Start() {
        enemy = GetComponent<Enemy>();                  //コンポネートを取得
    }

    void AttackStart() {                                //攻撃時コライダーをON
        AttackCollider.enabled = true;
    }

    public void AttackEnd() {                           //攻撃終了時コライダーをOFF
        AttackCollider.enabled = false;
    }

    public void StateEnd() {                            //攻撃終了後、フリーズ状態
        enemy.SetState(Enemy.EnemyState.Freeze);
    }

    public void EndDamage() {                           //ダメージ状態終了後、ウォーク状態に移る
        enemy.SetState(Enemy.EnemyState.Walk);
    }
}