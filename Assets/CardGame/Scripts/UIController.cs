using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

namespace Com.SmokeyShadow.Trumpolia
{
    public class UIController : MonoBehaviour
    {
        #region SERIALIZABLE FIELDS
        [SerializeField]
        private GameObject[] settingDisplays;

        [SerializeField]
        private GameObject[] panels;

        [SerializeField]
        GameObject[] onGameDisplay;

        [SerializeField]
        private Button onPlayBtn;

        [SerializeField]
        private AudioSource mainAudio;
        #endregion

        #region FIELDS
        private DoTweenController doTween;
        private Vector3 settingStartPosition;
        private Vector3[] settingDisplaysPositions = new Vector3[3];
        private bool settingOpen;
        private PageTransition currentState = PageTransition.Loading;
        #endregion

        #region ENUMS
        public enum PageTransition { Loading = 0, StartMenu, Option, GameRoom };
        #endregion

        #region PROPERTIES
        public DoTweenController DoTween
        {
            set
            {
                doTween = value;
            }
            get
            {
                return doTween;
            }
        }

        #endregion

        #region PUBLIC METHODS
        public void Init()
        {
            InitilizeUIS();
            DoTween.InitDoTween();
        }


        public void OnStartMenu()
        {
            Invoke("SwitchToStartMenu", 2f);
        }

        public void OnSettingClicked()
        {
            settingOpen = !settingOpen;
            for (int i = 0; i < settingDisplays.Length; i++)
            {
                if (settingOpen)
                {
                    DoTween.MoveTO(settingDisplays[i], settingDisplaysPositions[i], i * 1 / 3f);
                    DoTween.AppearElementEffect(settingDisplays[i], new Vector3(1, 1, 1), i * 1 / 3f);
                }
                else
                {
                    DoTween.MoveTO(settingDisplays[i], settingStartPosition, i * 1 / 6f);
                    DoTween.DisappearElementEffect(settingDisplays[i], Vector3.zero, i * 1 / 6f);
                }
            }
        }

        public void SwitchOnPanels(PageTransition state)
        {
            DoTween.TweenPanels(panels[(int)state], panels[(int)currentState]);
            currentState = state;
        }

        public void ShowPanel(PageTransition state)
        {
            panels[(int)state].SetActive(true);
        }

        public void enableSetting(bool enable)
        {
            panels[(int)PageTransition.Option].SetActive(enable);
        }

        public void SwitchToStartMenu()
        {
            panels[1].transform.GetChild(0).gameObject.SetActive(true);
            SwitchOnPanels(PageTransition.StartMenu);
            for (int i = 1; i < panels[1].transform.childCount; i++)
            {
                DoTween.AppearElementEffect(panels[1].transform.GetChild(i).gameObject, panels[1].transform.GetChild(i).localScale, i * 1 / 2f);
            }
            enableSetting(true);
        }

        public void SwitchToOption()
        {
            SwitchOnPanels(PageTransition.Option);
        }

        public void BacktToMenu()
        {
            enableSetting(true);
            SwitchToStartMenu();
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void Retry()
        {
            onPlayBtn.interactable = true;
            GameManager.Instance.UnSubscribeAll();
            SceneManager.LoadScene("Board");
        }

        public void SwitchToGameRoom()
        {
            onPlayBtn.interactable = false;
            GameManager.Instance.StartMatch();
        }

        public void SwitchToGame()
        {
            SwitchOnPanels(PageTransition.GameRoom);
        }

        public void OnEnableSound()
        {
            if (settingDisplays[0].activeInHierarchy)
            {
                settingDisplays[0].SetActive(false);
                settingDisplays[2].SetActive(true);
                mainAudio.gameObject.SetActive(false);
            }
            else
            {
                settingDisplays[2].SetActive(false);
                settingDisplays[0].SetActive(true);
                mainAudio.gameObject.SetActive(true);
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void InitilizeUIS()
        {
            SetSettingPositions();
            panels[1].transform.GetChild(0).DOScale(new Vector3(0.8f, 0.8f, 1), 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

        private void EnableGameObjects(GameObject[] disableList, bool enable)
        {
            for (int i = 0; i < disableList.Length; i++)
            {
                if (enable)
                    DoTween.AppearElementEffect(disableList[i], disableList[i].transform.localScale, i * 1 / 2f);
                disableList[i].SetActive(enable);
            }
        }

        private void SetSettingPositions()
        {
            int num = 0;
            settingStartPosition = panels[(int)PageTransition.Option].transform.position;
            foreach (var setting in settingDisplays)
                settingDisplaysPositions[num++] = setting.transform.position;
        }
        #endregion

    }
}