using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisappear : MonoBehaviour
{
    [SerializeField] float speed = 100f;
    [SerializeField] float timToWait=1f;
    CanvasGroup group;
    bool fade = false;
    float value;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
        StartCoroutine(FadeInEffect());
    }


    IEnumerator FadeInEffect()
    {
        yield return new WaitForSeconds(timToWait);
        fade = true;
    }

    private void Update()
    {
        if (fade == true)
        {
            value += Time.deltaTime * speed;
            float alpha = Mathf.Lerp(0, 1, value);
            group.alpha = 1-alpha;
            if (alpha == 1)
            {
                Destroy(this.gameObject,1f);
            }

        }
    }
}
