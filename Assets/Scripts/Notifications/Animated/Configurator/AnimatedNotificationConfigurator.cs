using UnityEngine;

public class NotificationConfigurator : MonoBehaviour
{
    #region FIELDS
    [Header("Timers Area -----------------------------------")]
    [SerializeField, Min(0.1f)]
    private float horizontalTime = 1f;
    [SerializeField, Min(0.1f)]
    private float verticalTime = 1f;
    [SerializeField, Min(0.1f)]
    private float transparencyTime = 1f;
    [SerializeField, Min(0.1f)]
    private float timeOnScreen = 1f;

    [SerializeField]
    private GameObject notificationPrefab = null;
    [SerializeField]
    private GameObject errorNPrefab = null;
    [SerializeField]
    private float initCGAlpha = 1f;
    [SerializeField]
    private float endCGAlpha = 0f;
    #endregion

    #region PROPERTIES
    public float HorizontalTime { get => horizontalTime; set => horizontalTime = value; }
    public float VerticalTime { get => verticalTime; set => verticalTime = value; }
    public float TransparencyTime { get => transparencyTime; set => transparencyTime = value; }
    public GameObject NotificationPrefab { get => notificationPrefab; set => notificationPrefab = value; }
    public float TimeOnScreen { get => timeOnScreen; set => timeOnScreen = value; }
    public float InitCGAlpha { get => initCGAlpha; set => initCGAlpha = value; }
    public float EndCGAlpha { get => endCGAlpha; set => endCGAlpha = value; }
    public GameObject ErrorNPrefab { get => errorNPrefab; set => errorNPrefab = value; }
    #endregion

    #region METHODS
    private void Awake()
    {
        AnimatedNotificationManager.Configurator = this;
    }
    /// <summary>
    /// Method to obtain a configured instance of AnimatedNotification.
    /// </summary>
    /// <param name="type">0 means normal notification, 1 means error notification</param>
    /// <returns></returns>
    public AnimatedNotification GetConfiguredNotification(byte type = 0)
    {
        // Instancia un AnimatedNotification
        if (Instantiate(GetNotification(type), transform).TryGetComponent<AnimatedNotification>(out var notification))
        {
            notification.TryGetComponent<RectTransform>(out var rect);

            // Configura el AnimatedNotification
            SetPositions(notification, rect);
            SetTimers(notification);

            notification.InitCGAlpha = InitCGAlpha;
            notification.EndCGAlpha = EndCGAlpha;

            return notification;
        }

        Debug.LogError("Provided Prefab does not have an AnimatedNotification component attached to it!");
        return null;
    }
    private GameObject GetNotification(byte type)
    {
        switch (type)
        {
            case 0:
                return notificationPrefab;
            case 1:
                return errorNPrefab;
            default:
                return null;
        }
    }
    private void SetTimers(AnimatedNotification notification)
    {
        notification.HorizontalTime = HorizontalTime;
        notification.VerticalTime = VerticalTime;
        notification.TransparencyTime = TransparencyTime;
        notification.TimeOnScreen = TimeOnScreen;
    }
    private void SetPositions(AnimatedNotification notification, RectTransform rect)
    {
        notification.HorizontalStart = transform.position;
        notification.HorizontalEnd = new Vector3(
            transform.position.x - rect.sizeDelta.x,
            transform.position.y,
            transform.position.z
            );
        notification.VerticalStart = notification.HorizontalEnd;
        notification.VerticalEnd = new Vector3(
            transform.position.x - rect.sizeDelta.x,
            transform.position.y + rect.sizeDelta.y,
            transform.position.z
            );
    }
    #endregion
}