using UnityEngine;
using UnityEngine.EventSystems;
namespace Com.SmokeyShadow.Trumpolia
{
    public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region FIELDS
        [SerializeField]
        private Draggable.DraggableType dropType = Draggable.DraggableType.PlayerCard;
        #endregion

        #region PUBLIC METHODS
        public void OnPointerEnter(PointerEventData data)
        {

        }
        public void OnDrop(PointerEventData data)
        {

            Draggable d = data.pointerDrag.GetComponent<Draggable>();


            if (d != null && dropType == d.GetDraggableType())
            {
                if (dropType == Draggable.DraggableType.PlayerCard)
                {
                    if (gameObject.transform.childCount < 2)
                    {
                        d.SetParent(transform);
                    }
                }

                else if (dropType == Draggable.DraggableType.OpponentCard)
                {
                    if (gameObject.transform.childCount < 1)
                    {
                        d.SetParent(transform);
                    }
                }
                
            }
        }
        public void OnPointerExit(PointerEventData data)
        {

        }
        #endregion
    }
}