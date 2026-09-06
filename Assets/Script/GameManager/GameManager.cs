/*
    GameManager
    20260906  hanaue sho
    ゲームを統括するマネージャー
*/ 
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ==================================================
    // ----- Singleton -----
    // ==================================================
    static GameManager _instance;

    // ==================================================
    // ----- FadeManager -----
    // ==================================================
    private FadeManager _fadeManager;

    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    void Awake()
    {
        // シングルトン
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // イベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;

        // FadeManager
        _fadeManager = GetComponentInChildren<FadeManager>();
        if (_fadeManager == null )
        {
            Debug.LogWarning("[Warning] No FadeManager In GameManager!!!");
        }
    }
    private void OnDisable()
    {
        if (_instance == this)
        {
            // イベント削除
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // ==================================================
    // ----- OnSceneLoaded -----
    // ==================================================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // SceneManager_Base を探す
        SceneManager_Base sm = FindAnyObjectByType<SceneManager_Base>();
        if (sm != null)
        {
            sm.Enter();
            sm.NextSceneMoveAction = SceneMove;
        }
        else
        {
            Debug.LogError("[Error] Not Find SceneManager_Base!!!");
        }

    }

    private void SceneMove(int sceneIndex)
    {
        if (_fadeManager != null)
        {
            StartCoroutine(_fadeManager.FadeOutIn(() => SceneManager.LoadScene(sceneIndex)));
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

}
