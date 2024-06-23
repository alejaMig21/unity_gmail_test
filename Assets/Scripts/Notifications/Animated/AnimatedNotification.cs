using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class AnimatedNotification : MonoBehaviour
{
    #region FIELDS
    [SerializeField]
    private Vector3 horizontalStart = Vector3.zero;
    [SerializeField]
    private Vector3 horizontalEnd = Vector3.zero;
    [SerializeField]
    private Vector3 verticalStart = Vector3.zero;
    [SerializeField]
    private Vector3 verticalEnd = Vector3.zero;

    [SerializeField, Min(0.1f)]
    private float horizontalTime = 1f;
    [SerializeField, Min(0.1f)]
    private float verticalTime = 1f;
    [SerializeField, Min(0.1f)]
    private float transparencyTime = 1f;
    [SerializeField, Min(0.1f)]
    private float timeOnScreen = 1f;

    [SerializeField]
    private List<TextInfo> texts = new();
    [SerializeField]
    private CanvasGroup canvasGroup = null;
    [SerializeField]
    private float initCGAlpha = 1f;
    [SerializeField]
    private float endCGAlpha = 0f;
    private float currentCGAlpha = 1f;
    private bool isMovingVertically = false;
    private bool isChangingTransparency = false;
    #endregion

    #region PROPERTIES
    public Vector3 HorizontalStart { get => horizontalStart; set => horizontalStart = value; }
    public Vector3 HorizontalEnd { get => horizontalEnd; set => horizontalEnd = value; }
    public Vector3 VerticalStart { get => verticalStart; set => verticalStart = value; }
    public Vector3 VerticalEnd { get => verticalEnd; set => verticalEnd = value; }
    public float HorizontalTime { get => horizontalTime; set => horizontalTime = value; }
    public float VerticalTime { get => verticalTime; set => verticalTime = value; }
    public float TransparencyTime { get => transparencyTime; set => transparencyTime = value; }
    public float TimeOnScreen { get => timeOnScreen; set => timeOnScreen = value; }
    public bool IsMovingVertically { get => isMovingVertically; set => isMovingVertically = value; }
    public bool IsChangingTransparency { get => isChangingTransparency; set => isChangingTransparency = value; }
    public CanvasGroup _CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
    public float CurrentCGAlpha { get => currentCGAlpha; set => currentCGAlpha = value; }
    public float InitCGAlpha { get => initCGAlpha; set => initCGAlpha = value; }
    public float EndCGAlpha { get => endCGAlpha; set => endCGAlpha = value; }
    public List<TextInfo> Texts { get => texts; set => texts = value; }
    #endregion

    #region METHODS
    public void UpdateTexts(List<(string name, string text)> newTexts)
    {
        if (newTexts.Count != this.texts.Count)
        {
            Debug.LogError("NewTexts and local field Texts must have the same amount of elements!");
            return;
        }

        for (int i = 0; i < this.texts.Count; i++)
        {
            TextInfo item = this.texts[i];
            item.Name = newTexts[i].name;
            item.TextMesh.text = newTexts[i].text;
        }
    }
    public IEnumerator StartAnimation()
    {
        // Primero, ejecuta el movimiento horizontal.
        yield return StartCoroutine(HorizontalMove());

        yield return new WaitForSeconds(TimeOnScreen);

        // Cuando el movimiento horizontal termina, ejecuta el movimiento vertical y el cambio de transparencia al mismo tiempo.
        StartCoroutine(VerticalMove());
        StartCoroutine(ChangeTransparency(canvasGroup));

        // Espera a que las corrutinas de movimiento vertical y cambio de transparencia terminen.
        yield return new WaitUntil(() => !IsMovingVertically && !IsChangingTransparency);

        // Destruye este objeto de juego
        Destroy(gameObject);
    }
    public void SetUpValues(CanvasGroup canvasGroup, List<TextInfo> texts)
    {
        this.canvasGroup = canvasGroup;
        this.Texts = texts;
    }
    IEnumerator HorizontalMove()
    {
        float elapsedTime = 0;

        while (elapsedTime < horizontalTime)
        {
            transform.position = Vector3.Lerp(horizontalStart, horizontalEnd, (elapsedTime / horizontalTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = horizontalEnd;
    }
    IEnumerator VerticalMove()
    {
        float elapsedTime = 0;
        isMovingVertically = true;

        while (elapsedTime < verticalTime)
        {
            transform.position = Vector3.Lerp(verticalStart, verticalEnd, (elapsedTime / verticalTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = verticalEnd;
        isMovingVertically = false;
    }
    IEnumerator ChangeTransparency(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0;
        isChangingTransparency = true;

        while (elapsedTime < transparencyTime)
        {
            currentCGAlpha = Mathf.Lerp(initCGAlpha, endCGAlpha, (elapsedTime / transparencyTime));
            canvasGroup.alpha = currentCGAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = currentCGAlpha;
        isChangingTransparency = false;
    }
    #endregion
}