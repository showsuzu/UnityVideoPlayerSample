using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VedeoPlayButton : ButtonBase {
	// ButtonBaseクラスを継承したVideo Player Sceneのボタン処理
	protected override void OnClick(string objectName)
	{
		// 渡されたオブジェクト名で処理を分岐
		if ("PlayButton".Equals (objectName)) {
			// PlayButton 用処理
			this.VideoStart ();
		} else {
			throw new System.Exception("Not implemented!!");
		}
	}

	public void VideoStart(){
		// ボタンクリック処理
		GameObject.Find ("VideoPlayerSceneController").GetComponent<VideoPlayController> ().VideoStart ();
	}
}
