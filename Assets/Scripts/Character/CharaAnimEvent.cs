using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimEvent : MonoBehaviour {

    private Character character;                                //Characterスクリプト
    [SerializeField] private Collider weaponCollider;           //ウェポンコライダー
    private AudioSource audioSource;                            //オーディオソース
    [SerializeField] private AudioClip attackSound;             //攻撃音
    [SerializeField] private Transform equipTransform;          //武器の親のトランスフォーム


    private void Start() {
        character = GetComponent<Character>();                  ///コンポネートを取得
        audioSource = GetComponent <AudioSource> ();
    }

    void AttackStart() {                                        //攻撃時コライダーをON
        if (weaponCollider != null) {
            weaponCollider.enabled = true;
        }
        if (equipTransform.GetChild(0).CompareTag ("Sword")) {  //Swordで攻撃時、サウンドを鳴らす
            audioSource.PlayOneShot(attackSound);
        }
    }

    void AttackEnd() {                                          //攻撃終了時コライダーをOFF
        if (weaponCollider != null) {
        weaponCollider.enabled = false;
        }
    }

    void StateEnd() {                                           //攻撃終了後、ノーマル状態
        character.SetState(Character.MyState.Normal);
    }

    public void EndDamage() {                                   //ダメージ状態終了後、ノーマル状態
        character.SetState(Character.MyState.Normal);
    }

    public void SetCollider(Collider col) {                     //ウェポンコライダーをセット
        weaponCollider = col;
    }
}