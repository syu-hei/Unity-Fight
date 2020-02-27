using UnityEngine;

public class BreakObject : MonoBehaviour {

    [SerializeField] private GameObject destroyEffect;                  //破壊エフェクト


	public void PlayDestroyEffect(Vector3 crush) {                      //破壊エフェクトのメソッド
        var destroyEffectIns = Instantiate<GameObject>(destroyEffect);
        destroyEffectIns.transform.position = crush;
        Destroy(destroyEffectIns, 1f);
    }

}