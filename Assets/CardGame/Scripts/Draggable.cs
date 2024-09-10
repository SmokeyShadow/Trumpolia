using UnityEngine;
using UnityEngine.EventSystems;
namespace Com.SmokeyShadow.Trumpolia
{
    public class Draggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region FIELDS
        private CanvasGroup canvasGroup;
        private Transform parent;
        [SerializeField]
        private DraggableType typeOfCard = DraggableType.PlayerCard;
        private Card card;
        private Card scaledCard;
        #endregion

        #region ENUMS
        public enum DraggableType { PlayerCard, OpponentCard, CardAttribute };
        #endregion

        #region MONOBEHAVIOURS
        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            card = GetComponent<Card>();
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Scale the card for see ability details of each card.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!card.IsCovered())
            {
                if (scaledCard == null)
                {
                    scaledCard = Instantiate(card);
                    scaledCard.transform.parent = GameRefs.Instance.ScalePanel.transform;
                    scaledCard.transform.localScale = new Vector3(3, 3, 1);
                    scaledCard.gameObject.SetActive(true);
                    GameRefs.Instance.ScalePanel.SetActive(true);
                }
                else if (!scaledCard.gameObject.activeInHierarchy && !GameRefs.Instance.ScalePanel.activeInHierarchy)
                {
                    GameRefs.Instance.ScalePanel.SetActive(true);
                    scaledCard.gameObject.SetActive(true);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ExitScalePanel();
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (!TurnManager.Instance.IsMyTurn())
                return;
            parent = transform.parent;
            transform.SetParent(transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData data)
        {
            if (!TurnManager.Instance.IsMyTurn())
                return;

            transform.position = data.position;
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (!TurnManager.Instance.IsMyTurn())
                return;
            ExitScalePanel();
            transform.SetParent(parent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public DraggableType GetDraggableType()
        {
            return typeOfCard;
        }

        public void SetParent(Transform transform)
        {
            parent = transform;
        }
        #endregion

        #region PRIVATE METHODS
        private void ExitScalePanel()
        {
            if (!card.IsCovered())
            {
                scaledCard.gameObject.SetActive(false);
                GameRefs.Instance.ScalePanel.SetActive(false);
            }
        }
        #endregion
    }
}