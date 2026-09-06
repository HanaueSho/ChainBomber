/*
    SceneManager_Base
    20260906  hanaue sho
    各シーンのマネージャーの基底クラス
*/ 
using UnityEngine;
using UnityEngine.Events;

public class SceneManager_Base : MonoBehaviour 
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    private UnityAction<int> _nextSceneMoveAction;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public UnityAction<int> NextSceneMoveAction { set { _nextSceneMoveAction = value; } }

    // ==================================================
    // ----- Lifecycle -----
    // ==================================================
    virtual public void Enter()
    {

    }
    virtual public void Exit()
    {
        // 実行
        _nextSceneMoveAction?.Invoke(0);
    }


}
