using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ZoDream.Reader.Utils
{
    public static class Toast
    {
        /// <summary>
        /// Shows the specified text in a toast.
        /// </summary>
        public static void Show(string text)
        {
            Show(text, string.Empty);
        }

        /// <summary>
        /// Shows a toast with an info icon.
        /// </summary>
        public static void ShowInfo(string text)
        {
            Show(text, "StoreLogo.png");
        }

        /// <summary>
        /// Shows a toast with a warning icon.
        /// </summary>
        public static void ShowWarning(string text)
        {
            Show(text, "StoreLogo.png");
        }

        /// <summary>
        /// Shows a toast with an error icon.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void ShowError(string text)
        {
            Show(text, "StoreLogo.png");
        }

        /// <summary>
        /// Shows a toast with the specified text and icon.
        /// </summary>
        private static void Show(string text, string imagePath)
        {
            ShowToastNotification(imagePath, text, NotificationAudioNames.Default);
        }

        public static void ShowToastNotification(string assetsImageFileName, string text, NotificationAudioNames audioName)
        {
            // 1. create element
            var toastTemplate = ToastTemplateType.ToastImageAndText01;
            var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            // 2. provide text
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text.Length > 200 ? text.Substring(0, 200): text));

            // 3. provide image
            var toastImageAttributes = toastXml.GetElementsByTagName("image");
            if (assetsImageFileName.IndexOf("ms-appx:") < 0)
            {
                assetsImageFileName = $"ms-appx:///Assets/{assetsImageFileName}";
            }
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", assetsImageFileName);
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");

            // 4. duration
            var toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");

            // 5. audio
            var audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", $"ms-winsoundevent:Notification.{audioName.ToString().Replace("_", ".")}");
            toastNode.AppendChild(audio);

            // 6. app launch parameter
            //((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");

            // 7. send toast
            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

    }

    public enum NotificationAudioNames
    {
        Default,
        IM,
        Mail,
        Reminder,
        SMS,
        Looping_Alarm,
        Looping_Alarm2,
        Looping_Alarm3,
        Looping_Alarm4,
        Looping_Alarm5,
        Looping_Alarm6,
        Looping_Alarm7,
        Looping_Alarm8,
        Looping_Alarm9,
        Looping_Alarm10,
        Looping_Call,
        Looping_Call2,
        Looping_Call3,
        Looping_Call4,
        Looping_Call5,
        Looping_Call6,
        Looping_Call7,
        Looping_Call8,
        Looping_Call9,
        Looping_Call10,
    }
}
