using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.SmokeyShadow.Trumpolia
{
    public class GameManager : MonoBehaviour
    {
        #region STATIC FIELDS
        private static GameManager _instance;
        #endregion

        #region PROPERTIES
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }
        #endregion

        #region FIELDS
        [SerializeField]
        private UIController uiController;
        [SerializeField]
        private TurnManager turnManager;
        [SerializeField]
        private AssignCards cardAssigner;
        [SerializeField]
        private BoardGeneration board;

        private Dictionary<int, List<Card>> playersCards;
        #endregion

        #region EVENTS
        public delegate void OnInitDeleg();
        public delegate void OnInitActionsDeleg();
        public static event OnInitDeleg OnInit = delegate { };
        public static event OnInitActionsDeleg OnInitActions = delegate { };

        public delegate void OnBoardDeleg();
        public static event OnBoardDeleg OnBoardGenerate = delegate { };

        public delegate void OnTurnDeleg();
        public static event OnTurnDeleg OnTurnChange = delegate { };
        public static event OnTurnDeleg OnExecuteTurn = delegate { };

        public delegate void UnSubscribeDeleg();
        public static event UnSubscribeDeleg OnUnSubscribe = delegate { };
        #endregion

        #region MONO BEHAVIOURS
        private void Start()
        {
            playersCards = new Dictionary<int, List<Card>>();
            Init();
        }
        #endregion

        #region PUBLIC METHODS
        public void Init()
        {
            playersCards.Clear();
            OnInit += uiController.Init;
            OnExecuteTurn += turnManager.TurnExecute;
            OnTurnChange += turnManager.ChangeTurn;
            OnBoardGenerate += board.FillDesks;
            OnInit?.Invoke();
            uiController.OnStartMenu();
        }

        public void InjectDoTween(DoTweenController doTween)
        {
            uiController.DoTween = doTween;
        }

        public void StartMatch()
        {
            cardAssigner.StartMatch(turnManager.GetPlayer().GetId().ToString());
        }

        public void JoinMatch()
        {
            OnPlayGame();
        }

        public void OnPlayGame()
        {
            GameState.Instance.ChangeState(GameState.State.BoardGenerate);
            uiController.SwitchToGame();
            turnManager.GetPlayer().SetCards(playersCards[turnManager.GetPlayer().GetId()]);
            turnManager.GetBot().SetCards(playersCards[turnManager.GetBot().GetId()]);
            OnBoardGenerate();
        }

        public void AssignPlayerCards(List<Card> cards, int playerID)
        {
            playersCards.Add(playerID, cards);
        }

        public void OnBoardReady()
        {
            GameState.Instance.ChangeState(GameState.State.Play);
            OnExecuteTurn();
        }

        public void ChangeTurn()
        {
            OnTurnChange();
        }

        public void UnSubscribeAll()
        {
            OnUnSubscribe();
            OnInit -= uiController.Init;
            OnExecuteTurn -= turnManager.TurnExecute;
            OnTurnChange -= turnManager.ChangeTurn;
            OnBoardGenerate -= board.FillDesks;
        }
        #endregion
    }
}
