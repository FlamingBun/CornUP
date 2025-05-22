using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageScreen : MonoBehaviour
{
    private Image image;
    [SerializeField]private float flashSpeed;

    private Coroutine coroutine;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        GameManager.Instance.CharacterManager.Player.onDamage += Flash;
    }

    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            // flashSpeed: n초
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }

        image.enabled = false;
    }
}
