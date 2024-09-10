using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace Com.SmokeyShadow.Trumpolia
{
    public class DoTweenController : MonoBehaviour
    {
        #region SERIALIZABLE FIELDS
        [SerializeField]
        private Image loadingSprite;

        [SerializeField]
        private RectTransform panelRect;
        #endregion

        #region FIELDS
        private float rightX;
        private float leftX;
        private float bottomY;
        #endregion

        #region MONO BEHAVIOURS
        void Awake()
        {
            GameManager.Instance.InjectDoTween(this);
            StartTweenLoading();
        }
        #endregion

        #region PUBLIC METHODS
        public void InitDoTween()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
            rightX = RectTransformToScreenSpace(panelRect).x + RectTransformToScreenSpace(panelRect).width * 3 / 2;
            leftX = RectTransformToScreenSpace(panelRect).x - RectTransformToScreenSpace(panelRect).width * 1 / 2f;
            bottomY = RectTransformToScreenSpace(panelRect).y - RectTransformToScreenSpace(panelRect).height * 1 / 2f;
        }

        public void StartTweenLoading()
        {
            loadingSprite.DOColor(RandomColor(), 1f).SetEase(Ease.Linear);
            loadingSprite.DOFillAmount(0, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo)
                    .OnStepComplete(() =>
                    {
                        loadingSprite.fillClockwise = !loadingSprite.fillClockwise;
                        loadingSprite.DOColor(RandomColor(), 1f).SetEase(Ease.Linear);
                    });
        }

        //change alpha from visible to hidden
        public void FadeOut(GameObject obj)
        {
            obj.GetComponent<Image>().DOFade(0, 0.5f).SetAutoKill(false).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                obj.SetActive(false);
            });
        }

        //change alpha from hidden to visible
        public void FadeIn(GameObject obj)
        {
            obj.GetComponent<Image>().DOFade(1, 0.5f).SetAutoKill(false).SetEase(Ease.InQuad)
           .OnComplete(() =>
           {
               obj.SetActive(true);
           });
        }

        public void TweenPanels(GameObject movingIn, GameObject movingOut)
        {
            movingIn.SetActive(true);
            movingIn.transform.position = new Vector3(rightX, movingIn.transform.position.y, movingIn.transform.position.z);
            movingIn.transform.DOMoveX(-leftX, 0.5f)
            .OnComplete(() =>
            {
                movingIn.transform.position = new Vector3(-leftX, movingIn.transform.position.y, movingIn.transform.position.z);
            });
            movingOut.transform.DOMoveX(leftX, 0.5f)
            .OnComplete(() =>
            {
                movingOut.SetActive(false);
                movingOut.transform.position = new Vector3(rightX, movingOut.transform.position.y, movingOut.transform.position.z);

            });
        }

        public void MoveUpPanel(GameObject obj, Vector3 to)
        {
            obj.transform.position = new Vector3(0, bottomY, 0);
            obj.transform.DOMove(to, 0.5f).SetEase(Ease.OutBounce);
        }

        public void MoveDownPanel(GameObject obj)
        {
            obj.transform.DOMove(new Vector3(obj.transform.position.y, bottomY, obj.transform.position.z), 0.5f).SetEase(Ease.InBounce)
            .OnComplete(() =>
            {
                obj.SetActive(false);
            });
        }

        public Tweener AppearElementEffect(GameObject obj, Vector3 endScale, float delay)
        {
            obj.transform.localScale = Vector3.zero;
            return obj.transform.DOScale(endScale, 0.5f).SetEase(Ease.InOutElastic).SetDelay(delay);
        }

        public void DisappearElementEffect(GameObject obj, Vector3 endScale, float delay)
        {
            obj.transform.DOScale(endScale, 0.5f).SetEase(Ease.OutCirc).SetDelay(delay);
        }

        public Tweener MoveTOFrom(GameObject obj, Vector3 from, float delay)
        {
            return obj.transform.DOMove(from, 0.3f).From().SetDelay(delay);
        }

        public Tweener MoveTO(GameObject obj, Vector3 to, float delay)
        {
            return obj.transform.DOMove(to, 0.3f).SetEase(Ease.InQuad).SetDelay(delay);
        }

        public void fadeAll(GameObject obj)
        {
            obj.GetComponent<Image>().color = new Color(obj.GetComponent<Image>().color.r,
                obj.GetComponent<Image>().color.g,
                obj.GetComponent<Image>().color.b, 0);
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                obj.transform.GetChild(i).GetComponent<Image>().color = new Color(obj.transform.GetChild(i).GetComponent<Image>().color.r,
                obj.transform.GetChild(i).GetComponent<Image>().color.g,
                obj.transform.GetChild(i).GetComponent<Image>().color.b, 0);
            }
        }
        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            return new Rect((Vector2)transform.position - (size * 0.5f), size);
        }
        #endregion

        #region PRIVATE METHODS
        private Color RandomColor()
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        }
        #endregion
    }
}