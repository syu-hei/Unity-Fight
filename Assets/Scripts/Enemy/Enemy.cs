using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public enum EnemyState {                            //敵の状態を設定
        Walk,
        Wait,
        Chase,
		Attack,
		Freeze,
        Damage,
        Dead
    };


    private CharacterController enemyController;        //キャラクターコントローラー機能を変数に代入
    private Animator animator;                          //アニメーター機能を変数に代入
    private Vector3 destination;                        //目的地
    [SerializeField] private float walkSpeed = 1.0f;    //歩くスピード
    private Vector3 velocity;                           //速度
    private Vector3 direction;                          //移動方向
    private bool arrival;                                  //目的地に到着したかどうか
    private SetPosition setPosition;                    //SetPositionスクリプト
    [SerializeField] private float waitTime = 5f;       //待ち時間
    private float elapsedTime;                          //経過時間
    private EnemyState state;                           //敵の状態
    private Transform playerTransform;                  //プレイヤーのトランスフォーム
	[SerializeField] private float freezeTime = 0.5f;   //止まる時間
    [SerializeField] private GameObject damageEffect;   //ダメージエフェクト
    [SerializeField] private AudioClip attackSound;     //攻撃時のサウンド
    private AudioSource audioSource;                    //オーディオソース
    [SerializeField] private EnemyStatus enemyStatus;   //EnemyStatusスクリプト
    [SerializeField] private Collider attackCollider;   //攻撃のコライダー
    public static bool End;                             //ゲーム終了判定


    void Start() {
        enemyController = GetComponent<CharacterController>();      ///コンポネートを取得
        animator = GetComponent<Animator>();
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition();
        velocity = Vector3.zero;                                    //ベクトルを初期化
        arrival = false;                                            //目的地へ向かう
        elapsedTime = 0f;
        SetState(EnemyState.Walk);
        audioSource = GetComponent<AudioSource> ();
        End = false;
    }


    void Update() {
        //見回りまたはプレイヤーを追いかける状態
        if (state == EnemyState.Walk || state == EnemyState.Chase) {
            //プレイヤーを追いかける状態であればキャラクターの目的地を再設定
            if (state == EnemyState.Chase) {
                setPosition.SetDestination(playerTransform.position);
            }
            if (enemyController.isGrounded) {           //接地判定
                velocity = Vector3.zero;                //ベクトルを初期化
                animator.SetFloat("Speed", 2.0f);       //アニメーターのスピード値を設定
                direction = (setPosition.GetDestination() - transform.position).normalized;
                transform.LookAt(new Vector3(setPosition.GetDestination().x, transform.position.y, setPosition.GetDestination().z));
                velocity = direction * walkSpeed;
            }

            //目的地に到着したか判定
			if (state == EnemyState.Walk) {
            if (Vector3.Distance(transform.position, setPosition.GetDestination()) < 0.7f) {
                SetState(EnemyState.Wait);
                animator.SetFloat("Speed", 0.0f);
            	}
			} else if (state == EnemyState.Chase) {
			//攻撃する距離だったら攻撃
			if (Vector3.Distance (transform.position, setPosition.GetDestination ()) < 1f) {
				SetState(EnemyState.Attack);
				}
            //到着したら一定時間止まる
            } else if (state == EnemyState.Wait) {
                elapsedTime += Time.deltaTime;
            //待ち時間を越えたら次の目的地へ行く
                if (elapsedTime > waitTime) {
                    SetState(EnemyState.Walk);
                }
			//攻撃後のフリーズ状態
		    } else if(state == EnemyState.Freeze) {
		        elapsedTime += Time.deltaTime;

			    if (elapsedTime > freezeTime) {
				    SetState(EnemyState.Walk);
			    }
            }
            if (state == EnemyState.Dead) {
		        return;
            }
		}
        velocity.y += Physics.gravity.y * Time.deltaTime;                       //重力を働かせる
        enemyController.Move(velocity * Time.deltaTime);                        //指定速度で移動
    }


    public void SetState(EnemyState tempState, Transform target = null) {       //敵の状態変更メソッド
		state = tempState;
        if (tempState == EnemyState.Walk) {                                     //見回り状態
            arrival = false;
            elapsedTime = 0f;
            setPosition.CreateRandomPosition();
        } else if (tempState == EnemyState.Chase) {                             //プレイヤーを追いかける状態
            arrival = false;                                                    //待機状態から追いかける場合もあるのでOff
            playerTransform = target;                                           //追いかける対象にプレイヤーをセット
        } else if (tempState == EnemyState.Wait) {                              //止まっている状態
            elapsedTime = 0f;
            arrival = true;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
		} else if (tempState == EnemyState.Attack) {                            //攻撃時の設定
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", true);
            audioSource.PlayOneShot(attackSound);
        } else if (tempState == EnemyState.Freeze) {                            //攻撃後のフリーズ時の設定
            elapsedTime = 0f;
            velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Attack", false);
    	} else if (tempState == EnemyState.Damage) {                            //ダメージを受けた時の設定
            velocity = Vector3.zero;
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Damage");
	    } else if (tempState == EnemyState.Dead) {                              //死んだ時の設定
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 3f);
            velocity = Vector3.zero;
            End = true;                                                         //ゲーム終了
        }
    }
    public void TakeDamage(int damage, Vector3 attackedPlace) {                 //ダメージを受けたときのHPの設定
        SetState(EnemyState.Damage);
        attackCollider.enabled = false;                                         //ダメージ状態では攻撃出来ない
        var damageEffectIns = Instantiate<GameObject>(damageEffect);            //ダメージエフェクトをインスタンス化しセット
        damageEffectIns.transform.position = attackedPlace;
        Destroy(damageEffectIns, 1f);
        enemyStatus.SetHp(enemyStatus.GetHp() - damage);                        //ダメージ時にHPを減らし、0になったら死亡
        if (enemyStatus.GetHp() <= 0) {
            Dead();
        }
    }
    void Dead() {
        SetState(EnemyState.Dead);
    }
    //敵キャラクターの状態取得メソッド
    public EnemyState GetState() {
        return state;
    }
}
