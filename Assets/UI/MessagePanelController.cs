using TMPro;
using UnityEngine;

namespace Assets.UI
{
    public class MessagePanelController : MonoBehaviour
    {
        public TMP_Text Title;
        public TMP_Text Text;

        public void Show(string title, string text)
        {
            Title.text = title;
            Text.text = text;
            OnMessagePanelShown?.Invoke(this);
        }

        public void Close()
        {
            OnMessagePanelClosed?.Invoke(this);
            Destroy(gameObject);
        }

        public event MessagePanelDelegates.MessagePanelClosedDelegate OnMessagePanelClosed;

        public event MessagePanelDelegates.MessagePanelShownDelegate OnMessagePanelShown;
    }

    public static class MessagePanelDelegates
    {
        public delegate void MessagePanelClosedDelegate(MessagePanelController messagePanel);

        public delegate void MessagePanelShownDelegate(MessagePanelController messagePanel);
    }
}