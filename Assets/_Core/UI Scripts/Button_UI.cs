using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Button_UI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private AudioSource hoverSource;
    private AudioSource clickSource;
    [SerializeField] private UnityEvent OnClick;

    [SerializeField] private Vector3 scaleOnHover = new Vector2(1.2f, 1.2f);
    [SerializeField] private float scaleSpeed = .3f;

    [SerializeField] private string hoverSourceName = "UI_PointerEnterSource";
    [SerializeField] private string clikcSourceName = "UI_ClickSource";

    private void Awake()
    {
        clickSource = GameObject.Find(clikcSourceName).GetComponent<AudioSource>();
        hoverSource = GameObject.Find(hoverSourceName).GetComponent<AudioSource>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverSource.Play();
        transform.DOScale(scaleOnHover, scaleSpeed);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetScale();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ResetScale();
        clickSource.Play();
        OnClick?.Invoke();
    }

    private void ResetScale()
    {
        transform.DOScale(new Vector3(1, 1, 1), scaleSpeed);
    }
}
