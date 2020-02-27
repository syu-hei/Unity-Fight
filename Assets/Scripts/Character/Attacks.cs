using UnityEngine;
using System.Collections;

public class Attacks : MonoBehaviour {

    private MyStatus myStatus;                              //MyStatusスクリプト


    private void Start() {
        myStatus = transform.root.GetComponent<MyStatus>(); //ルートのオブジェクトのコンポネート(MyStatus)を取得
    }


    //攻撃判定
	void OnTriggerEnter(Collider col) {
        //BreakableObjectに攻撃が当たった時、BreakObjectスクリプトのPlayDestroyEffectメソッドを呼び、オブジェクトを破壊する
        if (col.tag == "BreakableObject") {
            Debug.Log("障害物を破壊した!");
            col.GetComponent<BreakObject>().PlayDestroyEffect(col.ClosestPointOnBounds(transform.position));
            Destroy(col.gameObject);

		}else if (col.tag == "Enemy") {
            Debug.Log("敵に攻撃が当たった!");
            //敵に攻撃が当たった時、敵のTakeDamageメソッドを呼ぶ
            var enemyScript = col.GetComponent<Enemy>();
            //敵がダメージ状態またはデッド状態の場合無効
            if (enemyScript.GetState() != Enemy.EnemyState.Damage && enemyScript.GetState() != Enemy.EnemyState.Dead) {
                //MyStatusスクリプトのGetAttackPowerメソッドの値分ダメージ
                col.GetComponent<Enemy>().TakeDamage(myStatus.GetAttackPower(), col.ClosestPointOnBounds(transform.position));
			}
		}
	}
}
