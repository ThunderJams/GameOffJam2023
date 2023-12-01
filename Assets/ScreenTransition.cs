using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public Image background;
    public static ScreenTransition instance;
    [SerializeField] Image mask;
    [SerializeField] Image maskBorder;
    Coroutine changeSceneCoroutine;
    Vector2 initialMaskSize;
    Vector2 initialMaskBorderSize;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(this.gameObject);

        initialMaskSize = mask.rectTransform.sizeDelta;
        initialMaskBorderSize = maskBorder.rectTransform.sizeDelta;
    }

    public void ChangeScene(string sceneName)
    {
        maskBorder.transform.position = Input.mousePosition;
        if (changeSceneCoroutine == null)
        {
            changeSceneCoroutine = StartCoroutine(changeScene(sceneName));
        }

    }

    private IEnumerator changeScene(string sceneName)
    {
        Time.timeScale = 0f;
        FadeIn();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSecondsRealtime(0.5f);
        FadeOut();

    }

    void FadeIn()
    {
        background.color = background.color;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mask.rectTransform.sizeDelta = initialMaskSize ;
        maskBorder.rectTransform.sizeDelta = initialMaskBorderSize ;

        mask.rectTransform.DOSizeDelta(Vector2.zero, 1).SetEase(Ease.OutQuint).SetUpdate(true);
        maskBorder.rectTransform.DOSizeDelta(Vector2.zero, 1).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    void FadeOut()
    {

        background.color = background.color;
        mask.rectTransform.sizeDelta = Vector2.zero;
        maskBorder.rectTransform.sizeDelta = Vector2.zero;

        mask.rectTransform.DOSizeDelta(initialMaskSize, 0.5f).SetEase(Ease.InQuint).SetUpdate(true);
        maskBorder.rectTransform.DOSizeDelta(initialMaskBorderSize , 0.5f).SetEase(Ease.InQuint).SetUpdate(true).onComplete += () =>
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            changeSceneCoroutine = null;
            Time.timeScale = 1f;
        };

    }
}
