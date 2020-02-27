using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {


	//攻撃判定
	void OnTriggerEnter(Collider col) {
		if(col.tag == "Player") {
			Debug.Log("敵から攻撃された!");
			//プレイヤーに攻撃が当たった時プレイヤーのTakeDamageメソッドを呼ぶ
			col.GetComponent<Character>().TakeDamage(transform.root, col.ClosestPoint(transform.position));
		}
	}
}