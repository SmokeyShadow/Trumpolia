using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
namespace Com.SmokeyShadow.Trumpolia
{
    public class AssignCards : MonoBehaviour
    {
        #region CONST FIELDS
        const int EachTotalCards = 20;
        #endregion

        #region FIELDS
        [SerializeField]
        private List<Card> playercards;

        [SerializeField]
        private List<Card> opponentcards;
        #endregion

        #region PUBLIC METHODS
        public void StartMatch(string starterId)
        {
            AssignPlayerCards();
            AssignBotCards();
        }

        #endregion

        #region PRIVATE METHODS
        private void AssignPlayerCards()
        {         
            StartCoroutine(WaitForBotRoutine(2f));
            OnAssignCards(TurnManager.Instance.GetPlayer().GetId().ToString(), GameRefs.Instance.PlayerCards, out playercards);
            GameManager.Instance.AssignPlayerCards(playercards, TurnManager.Instance.GetPlayer().GetId());
        }

        private void AssignBotCards()
        {
            TurnManager.Instance.SetTurn(0);
            OnAssignCards(TurnManager.Instance.GetBot().GetId().ToString(), GameRefs.Instance.OpponentCards, out opponentcards);
            GameManager.Instance.AssignPlayerCards(opponentcards, TurnManager.Instance.GetBot().GetId());
        }

        private void OnAssignCards(string id, List<Card> playerCardsData, out List<Card> cards)
        {
            List<Card> cardQueue = new List<Card>();
            for (int i = 0; i < EachTotalCards; i++)
            {
                int randCard = UnityEngine.Random.Range(0, playerCardsData.Count);
                GameObject cardObj = Instantiate(playerCardsData[randCard].GetComponent<Card>().gameObject);
                List<Card.CardAbility> abilityList = new List<Card.CardAbility>(6);
                abilityList.Add(new Card.CardAbility(Card.CardAbility.AbilityType.Height, playerCardsData[randCard].GetAbilities()[(int)Card.CardAbility.AbilityType.Height].abilityValue));
                abilityList.Add(new Card.CardAbility(Card.CardAbility.AbilityType.Intelligence, playerCardsData[randCard].GetAbilities()[(int)Card.CardAbility.AbilityType.Intelligence].abilityValue));
                abilityList.Add(new Card.CardAbility(Card.CardAbility.AbilityType.Strength, playerCardsData[randCard].GetAbilities()[(int)Card.CardAbility.AbilityType.Strength].abilityValue));
                abilityList.Add(new Card.CardAbility(Card.CardAbility.AbilityType.Agility, playerCardsData[randCard].GetAbilities()[(int)Card.CardAbility.AbilityType.Agility].abilityValue));
                abilityList.Add(new Card.CardAbility(Card.CardAbility.AbilityType.FightingSkills, playerCardsData[randCard].GetAbilities()[(int)Card.CardAbility.AbilityType.FightingSkills].abilityValue));
                abilityList.Add(new Card.CardAbility(Card.CardAbility.AbilityType.Tech, playerCardsData[randCard].GetAbilities()[(int)Card.CardAbility.AbilityType.Tech].abilityValue));
                cardObj.GetComponent<Card>().SetConfig(randCard, abilityList);
                cardQueue.Add(cardObj.GetComponent<Card>());
            }
            cards = cardQueue;
        }
        #endregion

        #region CO ROUTINES
        IEnumerator WaitForBotRoutine(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            GameManager.Instance.JoinMatch();
        }
        #endregion
    }
}

