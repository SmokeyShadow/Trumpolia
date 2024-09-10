using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.SmokeyShadow.Trumpolia
{
    public class GameRefs : MonoBehaviour
    {
        #region STATIC FIELDS
        private static GameRefs _instance;
        #endregion

        #region CONST FIELDS
        private const int frontierLimit = 4;
        #endregion

        #region FIELDS
        [SerializeField]
        private List<Color> _abilityColors;

        [SerializeField]
        private GameObject _cardPrefab;

        [SerializeField]
        private List<Card> _playerCards;

        [SerializeField]
        private List<Card> _opponentsCards;

        [SerializeField]
        private Button _turnBtn;

        [SerializeField]
        private Color _yourTurnColor;

        [SerializeField]
        private Color _opponentTurnColor;

        [SerializeField]
        private Sprite[] _abilitySprites;

        [SerializeField]
        private Sprite[] _cardSprites;

        [SerializeField]
        private Text _roundWinText;

        [SerializeField]
        private Text _roundScore;

        [SerializeField]
        private Text _roundAbilitysText;

        [SerializeField]
        private Text _roundGainRewards;

        [SerializeField]
        private GameObject _roundStatePanel;

        [SerializeField]
        private Text _gameOverWinText;

        [SerializeField]
        private Text _gameOverAbilitysText;

        [SerializeField]
        private Text _gameOverGainRewards;

        [SerializeField]
        private GameObject _gameOverPanel;

        [SerializeField]
        private GameObject _onHoldPanel;

        [SerializeField]
        private Text _onHoldCardsTxt;

        [SerializeField]
        private GameObject _scalePanel;
        #endregion

        #region PROPERTIES
        public static GameRefs Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameRefs>();
                return _instance;
            }
        }

        public int FrontierLimit
        {
            get
            {
                return frontierLimit;
            }
        }

        public List<Card> PlayerCards
        {
            get
            {
                return _playerCards;
            }
            set
            {
                _playerCards = value;
            }
        }
        public List<Card> OpponentCards
        {
            get
            {
                return _opponentsCards;
            }
            set
            {
                _opponentsCards = value;
            }
        }

        public List<Color> AbilityColors
        {
            get
            {
                return _abilityColors;
            }
        }
        public GameObject CardPrefab
        {
            get
            {
                return _cardPrefab;
            }
        }
        public Button TurnBtn
        {
            get
            {
                return _turnBtn;
            }
        }
        public Color YourTurnColor
        {
            get
            {
                return _yourTurnColor;
            }
        }
        public Color OpponentTurnColor
        {
            get
            {
                return _opponentTurnColor;
            }
        }
        public Sprite[] AbilitySprites
        {
            get
            {
                return _abilitySprites;
            }
        }
        public Text RoundWinText
        {
            get
            {
                return _roundWinText;
            }
        }
        public Text RoundScore
        {
            get
            {
                return _roundScore;
            }
        }
        public Text roundAbilities
        {
            get
            {
                return _roundAbilitysText;
            }
        }
        public Text RoundGainRewards
        {
            get
            {
                return _roundGainRewards;
            }
        }
        public GameObject RoundStatePanel
        {
            get
            {
                return _roundStatePanel;
            }
        }

        public Text GameOverWinText
        {
            get
            {
                return _gameOverWinText;
            }
        }
   
        public Text GameOverAbilities
        {
            get
            {
                return _gameOverAbilitysText;
            }
        }
        public Text GameOverGainRewards
        {
            get
            {
                return _gameOverGainRewards;
            }
        }
        public GameObject GameOverPanel
        {
            get
            {
                return _gameOverPanel;
            }
        }

        public GameObject OnHoldPanel
        {
            get
            {
                return _onHoldPanel;
            }
        }
        public Text OnHoldCardsTxt
        {
            get
            {
                return _onHoldCardsTxt;
            }
        }

        public Sprite[] CardSprites
        {
            get
            {
                return _cardSprites;
            }
        }

        public GameObject ScalePanel
        {
            get
            {
                return _scalePanel;
            }
        }

        #endregion
    }
}