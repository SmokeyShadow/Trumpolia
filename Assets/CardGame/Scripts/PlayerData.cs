using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.SmokeyShadow.Trumpolia
{
    public abstract class PlayerData : MonoBehaviour
    {
        #region SERIALIZABLE FIELDS
        [SerializeField]
        private int cardChildIndex;
        [SerializeField]
        protected DropArea cardPlace;
        [SerializeField]
        protected GameObject winCardsPlace;
        [SerializeField]
        protected Text timeCounter;
        [SerializeField]
        protected int turnTime;
        [SerializeField]
        protected Text winCardsCountText;
        [SerializeField]
        protected List<Text> abilitiesTexts;
        [SerializeField]
        private Image playerWinCardsImage;
        [SerializeField]
        private Sprite playerCardSprite;
        [SerializeField]
        private GameObject playerCardDesk;
        [SerializeField]
        private Text playerNameText;
        [SerializeField]
        private Text leftCardsNumberText;
        #endregion

        #region FIELDS
        protected Dictionary<int, Card.CardAbility> abilities = new Dictionary<int, Card.CardAbility>();
        protected int id = 0;
        protected int score;
        protected string playerName;
        protected int winCards = 0;
        protected Queue<Card> leftCardsQueue = new Queue<Card>();
        protected List<Card> frontierCards = new List<Card>();
        #endregion

        #region ABSTRACT METHODS
        protected abstract IEnumerator TimeRoutine();
        #endregion

        #region PUBLIC METHODS
        public void SetCards(List<Card> cards)
        {
            leftCardsQueue.Clear();
            foreach (Card val in cards)
            {
                leftCardsQueue.Enqueue(val);
            }
        }

        public void MoveCard()
        {
            SetTimeCounter();
        }

        public void SetTimeCounter()
        {
            StartCoroutine(TimeRoutine());
        }

        public void RemoveCard()
        {
            GameObject removeCard = GetPlayedCard().gameObject;
            removeCard.transform.parent = winCardsPlace.transform;
            removeCard.gameObject.SetActive(false);
        }

        public bool CardPlaced()
        {
            if (cardPlace.transform.childCount >= cardChildIndex + 1)
                return true;
            return false;
        }

        public Card GetPlayedCard()
        {
            if(CardPlaced())
                return cardPlace.transform.GetChild(cardChildIndex).gameObject.GetComponent<Card>();
            return new Card();
        }

        public int GetId()
        {
            return id;
        }

        public int GetScore()
        {
            return score;
        }

        public void UpdateAbility(int abilityNo, int count)
        {
            abilities[abilityNo].AbilityVal += count;
            abilitiesTexts[abilityNo].text = abilities[abilityNo].AbilityVal.ToString();
        }

        public void UpdateScore(int score)
        {
            this.score = score;
        }

        public void UpdateWinCards(int count)
        {
            if (winCards == 0)
            {
                playerWinCardsImage.sprite = playerCardSprite;
            }
            winCards += count;
            winCardsCountText.text = winCards.ToString();
        }

        public void SetPlayerInfo(int playerId, string playerName)
        {
            id = playerId;
            this.playerName = playerName;
            playerNameText.text = this.playerName + "'s Frontier Cards";
        }


        public void FinishTurnRequest(string ability)
        {
            OnFinishTurn(ability);
      
        }

        public void DrawCard()
        {
            StartCoroutine(DrawCardRoutine());
        }

        public bool AnyCardsLeft()
        {
            return (leftCardsQueue.Count > 0 || frontierCards.Count > 0) ? true : false;
        }

        public string GetWinCards()
        {
            return winCards.ToString();
        }
        #endregion

        #region PROTECTED METHODS
        protected void InitDictionary()
        {
            for (int i = 0; i < 6; i++)
            {
                Card.CardAbility ability = new Card.CardAbility((Card.CardAbility.AbilityType)i, 0);
                abilities.Add(i, ability);
            }
        }
   
        protected IEnumerator DrawCardRoutine()
        {
            Card newCard = leftCardsQueue.Dequeue();
            newCard.transform.parent = playerCardDesk.transform;
            if (id != TurnManager.Instance.GetBot().GetId()) //its not bot
                newCard.DisableCover();
            newCard.transform.localScale = new Vector3(1, 1, 1);
            leftCardsNumberText.text = leftCardsQueue.Count.ToString();
            frontierCards.Add(newCard);
            yield return null;        
        }

         /// <summary>
         /// On finish turn response -> parsing response, set rewards if round is finished, otherwise change turn
         /// </summary>
         /// <param name="str"></param>
        protected void OnFinishTurn(string str)
        {
            if (TurnManager.Instance.OnCalculateReward())
            {
                TurnManager.Instance.OnCalculateRound();
            }
            else
                GameManager.Instance.ChangeTurn();
            frontierCards.Remove(GetPlayedCard());

            DrawCard();
        }

        protected void PickCard()
        {
            int abilityNo = 0;
            int cardIndex = 0;
            int randomAbility = UnityEngine.Random.Range(0, frontierCards[0].GetAbilities().Count);
            int maxVal = frontierCards[0].GetAbilities()[randomAbility].abilityValue;

            for(int i=0; i < frontierCards.Count; i++)
            {
                if(frontierCards[i].GetAbilities()[randomAbility].abilityValue >= maxVal)
                {
                    maxVal = frontierCards[i].GetAbilities()[randomAbility].abilityValue;
                    abilityNo = randomAbility;
                    cardIndex = i;
                }
            }
            MoveCardToCardPlace(frontierCards[cardIndex]);
            TurnManager.Instance.ChooseAbility(abilityNo);
        }

        protected void ChooseBestAbility(Card c)
        {
            int abilityIndex = 0;
            for (int a = 0; a < c.GetAbilities().Count; a++)// number of abilities
            {
                if (c.GetAbilities()[a].abilityValue > c.GetAbilities()[abilityIndex].abilityValue)
                    abilityIndex = a;
            }
            TurnManager.Instance.ChooseAbility(abilityIndex);
        }
        protected Card PickCard(int abilityNo)
        {
            int cardIndex = 0;
            int maxVal = frontierCards[0].GetAbilities()[abilityNo].abilityValue;
            for (int i = 1; i < frontierCards.Count; i++) // frontier cards
            {
                if (frontierCards[i].GetAbilities()[abilityNo].abilityValue > maxVal)
                {
                    maxVal = frontierCards[i].GetAbilities()[abilityNo].abilityValue;
                    cardIndex = i;
                }
            }
            return frontierCards[cardIndex];
        }

        protected void MoveCardToCardPlace(Card movingCard)
        {
            movingCard.transform.parent = cardPlace.transform;
            movingCard.DisableCover();
        }
        #endregion

    }
}