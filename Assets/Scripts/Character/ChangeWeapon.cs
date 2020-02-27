using UnityEngine;
using System.Collections;

public class ChangeWeapon : MonoBehaviour {

    [SerializeField] private GameObject[] weapons;                      //装備中の武器
    private int equipment;                                              //武器の配列番号
    private CharaAnimEvent CharaAnimEvent;                              //CharaAnimEventスクリプト
    private Character character;                                        //characterスクリプト
    [SerializeField] private MyStatus myStatus;                         //MyStatusスクリプト
    [SerializeField] private Transform equipTransform;                  //武器の親のトランスフォーム


    void Start() {
        character = GetComponentInParent<Character>();                  //自身の親要素のコンポネートを取得
        CharaAnimEvent = transform.root.GetComponent<CharaAnimEvent>(); //プレイヤー(root)要素のコンポネートを取得

        equipment = -1;       //初期装備設定(-1で装備なし)
    }


    void Update() {
        if (Input.GetKeyDown("1")                                       //装備変更ボタン
            && character.GetState() == Character.MyState.Normal) {
            InstantiateWeapon();                                        //InstantiateWeaponメソッドを呼ぶ
        }
    }


    void InstantiateWeapon() {
        //武器の配列番号が１つずつ進む
        equipment++;

        //武器の配列番号に設定されている武器が無ければ装備なし(-1)に戻る
        if (equipment >= weapons.Length) {
            equipment = -1;
    }
        //現在装備している武器を削除
        if (equipTransform.childCount != 0) {
            Destroy(equipTransform.GetChild(0).gameObject);
        }
        //素手ではない時、武器をインスタンス化
        if (equipment != -1) {
            //新しく装備する武器をインスタンス化
            var weapon = Instantiate<GameObject>(weapons[equipment]);
            weapon.name = weapons[equipment].name;
            CharaAnimEvent.SetCollider(weapon.GetComponent<Collider>());

            //コンポネートを取得
            var weaponStatus = weapon.GetComponent<WeaponStatus>();

            //武器の位置,角度,大きさを設定
            weapon.transform.SetParent(equipTransform);
            weapon.transform.localPosition = weaponStatus.GetPos();
            weapon.transform.localEulerAngles = weaponStatus.GetRot();
            weapon.transform.localScale = weaponStatus.GetScale();

            //MyStatusスクリプトのSetEquipメソッドに武器を渡す
            myStatus.SetEquip(weapon);
        }
    }
}