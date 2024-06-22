using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class AnimatedNotificationManager
{
    #region FIELDS
    private static NotificationConfigurator configurator = null;
    #endregion

    #region PROPERTIES
    public static NotificationConfigurator Configurator { get => configurator; set => configurator = value; }
    #endregion

    #region METHODS
    public static async void Send(List<(string name, string text, Color color)> texts)
    {
        // Si no tenemos una referencia al NotificationConfigurator, la obtenemos
        if (Configurator == null)
        {
            Configurator = GameObject.FindFirstObjectByType<NotificationConfigurator>();
        }

        // Obtenemos una instancia de AnimatedNotification configurada
        AnimatedNotification notification = Configurator.GetConfiguredNotification();

        // Configuramos el texto de la notificación
        notification.UpdateTexts(texts);

        // Iniciamos la animación de la notificación de forma asíncrona
        await SendAsync(notification);
    }
    private static async Task SendAsync(AnimatedNotification notification)
    {
        // Iniciamos la animación de la notificación
        await Task.Yield(); // Esto es necesario para que Unity pueda manejar correctamente las tareas asíncronas
        notification.StartCoroutine(notification.StartAnimation());
    }
    #endregion
}