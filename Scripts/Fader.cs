﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// シーン遷移時のフェードイン・アウトを制御
public class Fader : MonoBehaviour
{
    #region Singleton
    private static Fader instance;
    public static Fader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Fader)FindObjectOfType(typeof(Fader));
                if (instance == null)
                {
                    Debug.LogError(typeof(Fader) + "is nothing");
                }
            }
            return instance;
        }
    }
    #endregion Singleton
    private float fadeAlpha = 0;          // フェード中の透明度
    private bool isFading = false;        // フェード中かどうか
    public Color fadeColor = Color.black; // フェード色,今回は黒

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // フェード
    public void OnGUI()
    {
        if (this.isFading)
        {   // α値を更新,描写
            this.fadeColor.a = this.fadeAlpha;
            GUI.color = this.fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    // 画面遷移 シーン名+遷移時間
    public void LoadScene(string scene, float interval)
    {
        StartCoroutine(TransScene(scene, interval));
    }

    // シーン遷移用 シーン名+遷移時間
    private IEnumerator TransScene(string scene, float interval)
    {
        //暗く
        this.isFading = true;
        float time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }

        //シーン切替
        SceneManager.LoadScene(scene);

        //明るく
        time = 0;
        while (time <= interval - 1.0f)
        {
            this.fadeAlpha = Mathf.Lerp(1f, 0f, time / (interval - 1.0f));
            time += Time.deltaTime;
            yield return 0;
        }
        this.isFading = false;
        Destroy(this.gameObject);
        
    }

}
