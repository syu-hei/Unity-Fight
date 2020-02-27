using UnityEngine;
using System.Collections;

public class MyStatus : MonoBehaviour {

    [SerializeField] private int hp;							//プレイヤーのHP値
    [SerializeField] private int power;							//プレイヤーの攻撃力
    private WeaponStatus weaponStatus;							//WeaponStatusスクリプト
	private GameObject equip;									//装備


	public void SetHp(int hp) {									//プレイヤーの現在のHPを返すメソッド
		this.hp = hp;
	}

	public int GetHp() {										//プレイヤーのHP値を返すメソッド
		return hp;
	}

 	public WeaponStatus GetWeaponStatus() {						//武器のステータスを返すメソッド
	    return weaponStatus;
    }

	public void SetEquip(GameObject weapon) {					//武器を装備するメソッド
		equip = weapon;
        weaponStatus = equip.GetComponent <WeaponStatus> ();
	}

	public GameObject GetEquip() {								//装備した武器を返すメソッド
		return equip;
	}

    public int GetAttackPower() {								//プレイヤーと武器の攻撃力を合わせるメソッド
	    return power + weaponStatus.GetAttackPower ();
    }
}