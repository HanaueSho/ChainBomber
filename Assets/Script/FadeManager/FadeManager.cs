/*
    FadeManager
    20260906  hanaue sho
    フェードマネージャー
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeManager : MonoBehaviour
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    [SerializeField] private bool _isStartFadeIn = true;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private Canvas _fadeCanvas;
    [SerializeField] private Image _fadeImage;


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        // 取得
        if (_fadeCanvas == null)
        {
            _fadeCanvas = GetComponentInChildren<Canvas>(true);
        }
        _fadeCanvas.gameObject.SetActive(true); // 有効化
        if (_fadeImage == null)
        {
            _fadeImage = GetComponentInChildren<Image>();
        }

        // ----- Start FadeIn -----
        Color color = _fadeImage.color;
        if (_isStartFadeIn)
        {
            color.a = 1.0f;
            _fadeImage.color = color;
            StartCoroutine(FadeIn());
        }
        else
        {
            color.a = 0.0f;
            _fadeImage.color = color;
        }
    }


    // ==================================================
    // ----- Fade -----
    // ==================================================
    private IEnumerator FadeIn()
    {
        yield return Fade(true);
    }
    private IEnumerator FadeOut()
    {
        yield return Fade(false);
    }
    private IEnumerator Fade(bool isFadeIn)
    {
        // セーフティ
        if (_fadeImage == null)
        {
            yield break;
        }

        // フェード処理
        float elapsedTime = 0.0f;
        Color color = _fadeImage.color;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / _fadeDuration);
            if (isFadeIn)
            {
                color.a = 1.0f - t;
            }
            else
            {
                color.a = t;
            }
            _fadeImage.color = color;

            yield return null; // 1フレーム待ち
        }

        if (isFadeIn)
        { 
            color.a = 0.0f; 
        }
        else
        {
            color.a = 1.0f;
        }
        _fadeImage.color = color;

        yield break;
    }

    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public IEnumerator FadeOutIn(UnityAction action)
    {
        // FadeOut
        yield return FadeOut();
        yield return new WaitForSeconds(1.0f);

        // MoveScene
        action?.Invoke();
        yield return new WaitForSeconds(1.0f);

        // FadeIn
        yield return FadeIn();
    }
}
