﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBox : MonoBehaviour {

	[SerializeField] Material skyboxMaterial;					//SkyboxMaterial

	//昼の設定
	[SerializeField] Light sunLight;							//太陽のDirectionalLight
	[SerializeField] Color sunSkyColor;                     	//昼の空の色
	[SerializeField] Color sunSetColor;							//太陽が沈むときの空の色

	//夜の設定
	[SerializeField] Light moonLight;							//月のDirectionaLight
	[SerializeField] Color moonSkyColor;						//夜の空の色
	[SerializeField] Color moonSetColor;						//月が沈むときの空の色

	//時間の設定
	[SerializeField, Range(0,23)] float currentHour;			//現在時間
	[SerializeField] float hourStepFromSeconds;					//時間が1秒にどれくらいの速さで進むか
	[SerializeField, Range(4,8)] int dayStartHour;				//昼が始まる時間
	[SerializeField, Range(18,23)] int nightStartHour;     	 	//夜が始まる時間

	public float TotalHour = 0.0f;								//総合計経過時間

	float sunDuration;											//昼の時間が進んだ割合
	float moonDuration;											//夜の時間が進んだ割合

	readonly string skyColorName = "_SkyTint";					//MaterialのColorを設定するときのパラメータ名


	void Start() {
		RenderSettings.skybox = skyboxMaterial;					//スカイボックスのマテリアル設定
	}

	void Update() {
		TimeUpdate();											//時間の進行計算(２４時になると０時に時間をリセット)
		LightUpdate();											//進行時間に応じて太陽や月の角度、光の強弱を設定
		SkyboxUpdate();											//空の色合いを変化させる
	}

	void TimeUpdate() {
		currentHour += hourStepFromSeconds * Time.deltaTime;	//現在時間を進める
		TotalHour += hourStepFromSeconds * Time.deltaTime;		//総合計経過時間を更新

		if (currentHour >= 24.0f) {								//24時を0時にする
			currentHour = 0.0f;
		}
	}

	void LightUpdate() {
		//昼の場合
		if(currentHour >= dayStartHour && currentHour < nightStartHour) {
			//マテリアルを昼の太陽に更新
			RenderSettings.sun = sunLight;

			//月を非表示
			moonLight.enabled = false;
			moonLight.intensity = 0.0f;
			moonLight.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

			//昼が終了するまでの割合を計算する(0.0 ~ 1.0)
			sunDuration = (currentHour - dayStartHour) / (nightStartHour - dayStartHour);

			//太陽を表示
			sunLight.enabled = true;

			//太陽の角度を割合に応じて変えていく
			sunLight.transform.rotation = Quaternion.Euler(180.0f * sunDuration, 0.0f, 0.0f);

			if (sunDuration < 0.5f) {
				//太陽が頂点に達するまでは強くしていく
				sunLight.intensity = sunDuration * 2.0f;
			} else {
				//太陽が頂点に達したら太陽が沈むまで光を弱くしていく
				sunLight.intensity = 1.0f - ((sunDuration - 0.5f) * 2.0f);
			}
		} else {	//夜の場合
			//マテリアルを月に更新
			RenderSettings.sun = moonLight;

			//太陽を非表示
			sunLight.enabled = false;
			sunLight.intensity = 0.0f;
			sunLight.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

			moonDuration = 0.0f;

			//24時を過ぎた時に0時に戻るので、それを加味して現在の夜の終了までの割合を出す(0.0 ~ 1.0)
			if(currentHour < dayStartHour) {
				moonDuration = (currentHour + (24.0f - nightStartHour)) / ((24.0f - nightStartHour) + dayStartHour);
			} else {
				moonDuration = (currentHour - nightStartHour) / ((24.0f - nightStartHour) + dayStartHour);
			}
			//月を表示
			moonLight.enabled = true;
			moonLight.transform.rotation = Quaternion.Euler(180.0f * moonDuration, 0.0f, 0.0f);

			if (moonDuration < 0.5f) {
				moonLight.intensity = moonDuration * 2.0f;
			} else {
				moonLight.intensity = 1.0f - ((moonDuration - 0.5f) * 2.0f);
			}
		}
	}


	void SkyboxUpdate() {
		//空の色を現在時間に応じて変化させていく
		if (currentHour >= dayStartHour && currentHour < nightStartHour) {
			if (sunDuration < 0.5f) {
				//月が沈んだ色 ～ 昼の色
				skyboxMaterial.SetColor(skyColorName, Color.Lerp(moonSetColor, sunSkyColor, sunDuration * 2.0f));
			} else {
				//昼の色 ～ 太陽が沈んだ色
				skyboxMaterial.SetColor(skyColorName, Color.Lerp(sunSkyColor, sunSetColor, (sunDuration - 0.5f) * 2.0f));
			}
		} else {
			if (moonDuration < 0.5f) {
				//太陽が沈んだ色 ～ 夜の色
				skyboxMaterial.SetColor(skyColorName, Color.Lerp(sunSetColor, moonSkyColor, moonDuration * 2.0f));
			} else {
				//夜の色 ～ 月が沈んだ色
				skyboxMaterial.SetColor(skyColorName, Color.Lerp(moonSkyColor, moonSetColor, (moonDuration - 0.5f) * 2.0f));
			}
		}
	}
}
