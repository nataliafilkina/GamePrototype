using UnityEngine;

namespace MessageSystem
{
    public class MessageSpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _initialPosition;

        [SerializeField]
        private Transform _owner;

        [SerializeField]
        private GameObject _messagePrefab;

        public void SpawnMessage(MessageData message)
        {
            var messageObject = Instantiate(_messagePrefab, GetSpawnPosition(), Quaternion.identity, transform);
            var inGameMessage = messageObject.GetComponent<IGameMessage>();
            inGameMessage.SetMessage(message);
        }

        private Vector3 GetSpawnPosition()
        {
            return _owner.position + (Vector3)_initialPosition;
        }
    }
}
