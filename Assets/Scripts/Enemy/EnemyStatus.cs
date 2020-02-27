using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {


	[SerializeField] private int hp = 3;			//敵のHP値
	[SerializeField] private int attackPower = 1;	//敵の攻撃力


	public void SetHp(int hp) {						//敵の現在のHPを返すメソッド
		this.hp = hp;
	}

	public int GetHp() {							//敵のHP値を返すメソッド
		return hp;
	}
}