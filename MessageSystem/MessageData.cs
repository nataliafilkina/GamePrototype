using UnityEngine;

namespace MessageSystem
{
    public class MessageData
    {
        public string Text { get; private set; }
        public Color FaceColor { get; set; } = Color.black;


        public MessageData(string text) => Text = text;
    }
}
