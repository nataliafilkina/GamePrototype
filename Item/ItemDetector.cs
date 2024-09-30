using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject _tooltipPickUp;

    private GameItem _detectedItem;
    private RenderMode _tooltipRenderMode;

    private void Start()
    {
        _tooltipRenderMode = _tooltipPickUp.GetComponentInParent<Canvas>().renderMode;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _detectedItem?.PickUp(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<GameItem>(out var item)) 
            return;

        if (item.CanBePickedUp)
        {
            _tooltipPickUp.transform.position = _tooltipRenderMode == RenderMode.ScreenSpaceOverlay ?
                                        Camera.main.WorldToScreenPoint(transform.position) :
                                        transform.position;

            _tooltipPickUp.gameObject.SetActive(true);
            _detectedItem = item;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<GameItem>(out var item))
            return;

        _tooltipPickUp.gameObject.SetActive(false);
        _detectedItem = null;
    }
}
