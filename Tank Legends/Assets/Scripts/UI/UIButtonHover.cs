using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 startScale;
    private Vector3 targetScale;
    private float time;
    private float duration = 0.2f;

    private bool isHovering = false;

    void Start()
    {
        startScale = transform.localScale;
        targetScale = startScale;
    }

    void Update()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        float eased = SimpleTween.EaseOut(t);

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, eased);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        time = 0;
        targetScale = startScale * 1.05f;

        UIAudioManager.Instance?.PlayHover();
        if (!isHovering)
        {
            isHovering = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        time = 0;
        targetScale = startScale;

        isHovering = false;
    }
}