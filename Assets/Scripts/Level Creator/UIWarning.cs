using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWarning : MonoBehaviour
{
    [SerializeField] private Image warningImage;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private float fadeOutDuration = 5f;

    private Coroutine fadeOut;

    public void SetActive(string newText)
    {
        gameObject.SetActive(true);
        if (fadeOut != null)
        {
            StopCoroutine(fadeOut);
        }

        Color imageColor = warningImage.color;
        imageColor.a = 1f;
        warningImage.color = imageColor;
        Color textColor = warningText.color;
        textColor.a = 1f;
        warningText.color = textColor;
        
        warningText.SetText(newText);
        
        fadeOut = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float time = 0f;

        Color imageColor = warningImage.color;
        Color textColor = warningText.color;
        
        while (time < fadeOutDuration)
        {
            imageColor.a = Mathf.Lerp(1f, 0f, time / fadeOutDuration);
            textColor.a = Mathf.Lerp(1f, 0f, time / fadeOutDuration);

            warningImage.color = imageColor;
            warningText.color = textColor;

            time += Time.deltaTime;

            yield return null;
        }
        
        gameObject.SetActive(false);
    }
}
