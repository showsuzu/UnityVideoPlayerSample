using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayController : MonoBehaviour {

	// とりあえず特定のファイル名を指定
	string videoclipfile = "Movie/***";

	// 再生画面用のRaw Imageは「MovieRawImage」という名称でプレハブ化されている前提
	string rawImage = "Prefabs/MovieRawImage";

	public void VideoStart(){
		// ボタンクリック処理
		StartCoroutine (VideoPlayStart ());
	}

	private IEnumerator VideoPlayStart()
	{
		var obj = GameObject.Find ("Video Player");
		VideoPlayer videoPlayer = obj.GetComponent<VideoPlayer> ();

		Application.runInBackground = true;
		var audioSource = videoPlayer.GetComponent<AudioSource>();

		// ファイルをロードし、再生する動画のサイズに合わせてRender Textureを準備する
		VideoClip vclip = (VideoClip)Resources.Load (videoclipfile);
		RenderTexture _renderTexture = new RenderTexture ((int)vclip.width, (int)vclip.height, 24);

		// Video Playerの設定を行う
		videoPlayer.playOnAwake = false;
		audioSource.playOnAwake = false;
		videoPlayer.source = VideoSource.VideoClip;
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoPlayer.renderMode = VideoRenderMode.RenderTexture;
		videoPlayer.EnableAudioTrack(0, true);
		// Audio SourceにあらかじめVideo Playerに追加したAudio Sourceを設定する
		videoPlayer.SetTargetAudioSource(0, audioSource);
		videoPlayer.clip = vclip;
		videoPlayer.targetTexture = _renderTexture;

		// Video Playerの準備（完了まで待つ）
		videoPlayer.Prepare();
		while (!videoPlayer.isPrepared)
			yield return null;

		// Video Playerの準備が完了した後
		// プレハブから動画再生用のRawImageをロードする
		var _prefab = Resources.Load(rawImage);
		GameObject _rawImg = Instantiate(_prefab, Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;
		// Canvasが親になるようにtransformを代入する
		GameObject cvs = GameObject.Find("Canvas");
		_rawImg.transform.parent = cvs.transform;
		_rawImg.transform.localPosition = Vector3.zero;
		_rawImg.transform.localScale = new Vector3 (1, 1, 1);

		// RawImageに動画再生用のRenderTextureを設定する
		RawImage screen = _rawImg.GetComponent<RawImage>();
		screen.texture = _renderTexture;

		// 動画の再生開始（再生完了まで待つ）
		videoPlayer.Play();
		while (videoPlayer.isPlaying)
			yield return null;

		// 動画の再生完了
		videoPlayer.clip = null;
		videoPlayer.targetTexture = null;
		GameObject.Destroy (_rawImg.gameObject);
	}
}

