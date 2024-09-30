using TMPro;
using UnityEngine;

namespace MessageSystem
{
    public class FloatingMessage : MonoBehaviour, IGameMessage
    {
        #region Fields
        private Rigidbody2D _rigidbody;
        private TMP_Text _textMessage;

        public float InitialYVelocity = 7f;
        public float InitialXVelocityRange = 3f;
        public float LifeTime = 0.8f;
        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _textMessage = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _rigidbody.velocity = new Vector2(Random.Range(-InitialXVelocityRange, InitialXVelocityRange), InitialYVelocity);
            Destroy(gameObject, LifeTime);
        }

        public void SetMessage(MessageData message)
        {
            _textMessage.SetText(message.Text);
            _textMessage.faceColor = message.FaceColor;
        }
    }
}