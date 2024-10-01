using System.Collections;
using UICommon;
using UnityEngine;

public class UICarrierItem : MonoBehaviour
{
    #region Fields

    public static UICarrierItem Instance { get; private set; }

    private RenderMode _canvasRenderMode;
    private Camera _uiCamera;

    //Drag
    [SerializeField]
    private float offsetX = 0f;
    [SerializeField]
    private float offsetY = 0f;

    private float distance = 10f;
    private Coroutine _coroutineDragging = null;
    public IHoldPortableItem FromSlot { get; private set; }

    //Item
    private ItemDefaultDataSO _item;
    private UIItemViewInSlot _itemView;

    public ItemDefaultDataSO Item
    {
        get => _item;
        private set
        {
            if(_item == null)
                _item = value;
        }
    }
    public int ItemAmount { get; private set; }

    private bool _isDrag = false;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _itemView = GetComponent<UIItemViewInSlot>();
        _canvasRenderMode = GetComponentInParent<Canvas>().renderMode;
        _uiCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsEmpty() && Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.Escape))
        {
            StopDrag();
            FromSlot.Refresh();
        }
    }

    public bool IsEmpty() => !_isDrag;

    public void StartDrag(ItemDefaultDataSO itemData, int amount, IHoldPortableItem fromSlot = null)
    {
        if (!_isDrag)
        {
            _isDrag = true;

            _item = itemData;
            FromSlot = fromSlot;
            ItemAmount = amount;

            _itemView.SetView(_item.UIIcon, amount);
            gameObject.SetActive(true);

            _coroutineDragging = StartCoroutine(DraggingItem());
        }
    }

    public void StopDrag(bool IsRefreshFromSlot = true)
    {
        if (_isDrag)
        {
            gameObject.SetActive(false);
            StopCoroutine(_coroutineDragging);
            _isDrag = false;

            if (IsRefreshFromSlot)
                FromSlot.Refresh();
        }
    }

    public void StopDrag(int droppedAmount)
    {
        if (_isDrag)
        {
            FromSlot.Remove(droppedAmount);
            StopDrag(false);
        }
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY, distance);
        transform.position = _canvasRenderMode != RenderMode.ScreenSpaceOverlay ?
                             _uiCamera.ScreenToWorldPoint(mousePosition) :
                             mousePosition;
    }

    private IEnumerator DraggingItem()
    {
        while (true)
        {
            OnMouseDrag();
            yield return new WaitForEndOfFrame();
        }
    }
}
