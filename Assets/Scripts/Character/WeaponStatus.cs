using UnityEngine;
using System.Collections;
 
public class WeaponStatus : MonoBehaviour {
 
    public enum WeaponType {                            //武器の種類
        Sword,
        Other
    }


    [SerializeField] private int attackPower;           //武器の攻撃力
    [SerializeField] private WeaponType weaponType;     //武器の種類を決める
    //インスタンス化した時の武器の位置、角度、サイズ
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 rot;
    [SerializeField] private Vector3 scale;


    public int GetAttackPower() {                       //武器の攻撃力を返す
        return attackPower;
    }

    public WeaponType GetWeaponType() {                 //武器の種類返す
        return weaponType;
    }

    //武器の位置、角度、サイズを返す
    public Vector3 GetPos() {
        return pos;
    }

    public Vector3 GetRot() {
        return rot;
    }

    public Vector3 GetScale() {
        return scale;
    }
}