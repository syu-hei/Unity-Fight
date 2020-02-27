using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public enum  MyState {									//プレイヤーの状態を設定
		Normal,
		Attack,
		Damage
	};

	private CharacterController characterController;		//キャラクターコントローラー機能を変数に代入
    private Animator animator;								//アニメーター機能を変数に代入
	private Vector3 moveDirection;							//フィールド宣言(x, y, z)
	[SerializeField] private float walkSpeed = 3.0f;		//歩くスピード
    public float jumpSpeed = 2.0f;							//ジャンプの飛距離
	private MyState state;									//プレイヤーの状態
	[SerializeField] private GameObject damageEffect;       //ダメージエフェクト
	[SerializeField] private MyStatus myStatus;             //MyStatusスクリプト



	void Start () {
		characterController = GetComponent <CharacterController> ();	///コンポネートを取得
        animator = GetComponent <Animator> ();
		moveDirection = Vector3.zero;									//ベクトルを初期化
		myStatus = GetComponent<MyStatus>();
	}


	void Update () {
		if (state == MyState.Normal) {
			if(characterController.isGrounded) {		//接地判定
				moveDirection = Vector3.zero;

				//x方向キー(Horizontal), y方向キー(0), z方向キー(Vertical)で入力キーを取得, (y方向は0で初期化)
				var input = new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));
            	if(input.magnitude > 0f) {									//入力方向のベクトルが0以上なら有効
					animator.SetFloat("Speed", input.magnitude);			//アニメーターのスピード値
					transform.LookAt(transform.position + input);			//キャラクターが押した方向を見るようにする
					moveDirection += input.normalized * walkSpeed;			//ベクトルを正規化
				} else {
					animator.SetFloat("Speed", 0f);							//入力方向のベクトルが0なら移動しない
				}
				if(Input.GetButtonDown("Jump")) {							//ジャンプボタン(スペースキー)
					moveDirection.y += jumpSpeed;							//jumpSpeed変数の値をy方向に足す
					animator.SetTrigger("Jump");							//ジャンプアニメーション
				}
				if(Input.GetButtonDown("Fire1")) {							//攻撃ボタン(controlキー)
					SetState(MyState.Attack);								//攻撃状態
				}
			}
		}

        moveDirection.y += Physics.gravity.y * Time.deltaTime;					//重力を働かせる
		characterController.Move(moveDirection * walkSpeed * Time.deltaTime);	//指定速度で移動
		}


	public void TakeDamage(Transform enemyTransform, Vector3 attackedPlace) {
		state = MyState.Damage;													//ダメージ状態
		moveDirection = Vector3.zero;
		animator.SetTrigger("Damage");											//ダメージアニメーション
		//ダメージエフェクト取得
		var damageEffectIns = Instantiate<GameObject>(damageEffect, attackedPlace, Quaternion.identity);
    	Destroy(damageEffectIns, 1f);											//ダメージエフェクトを1秒再生
	}


	public void SetState(MyState tempState) {									//プレイヤーの状態変更メソッド
    	if (tempState == MyState.Normal) {										//ノーマル状態
			state = MyState.Normal;
    	} else if (tempState == MyState.Attack) {								//攻撃状態
       		moveDirection = Vector3.zero;
			state = MyState.Attack;
			animator.SetTrigger("Attack");										//攻撃アニメーション
		}
	}


	public MyState GetState() {													//プレイヤーの状態取得メソッド
		return state;
	}
}