using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour {

	private Vector3 startPosition;							//初期位置
	private Vector3 destination;							//目的地


	void Start () {
		startPosition = transform.position;					//初期位置を設定
		SetDestination(transform.position);
	}

	//ランダムな位置の作成
	public void CreateRandomPosition() {
		var randDestination = Random.insideUnitCircle * 8;	//ランダムなVector2の値を得る
		//現在地にランダムな位置を足して目的地とする
		SetDestination(startPosition + new Vector3(randDestination.x, 0, randDestination.y));
	}

	//目的地を設定
	public void SetDestination(Vector3 position) {
		destination = position;
	}

	//目的地を取得
	public Vector3 GetDestination() {
		return destination;
	}
}