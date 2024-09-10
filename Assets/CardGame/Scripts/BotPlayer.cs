using System.Collections;
using UnityEngine;
using System;
namespace Com.SmokeyShadow.Trumpolia
{
    public class BotPlayer : PlayerData
    {
        #region MONOBEHAVIOURS
        private void Start()
        {
            InitDictionary();
            SetPlayerInfo(22, "Bot");
        }
        #endregion

        #region PROTECTED METHODS
        /// <summary>
        /// Pick a card and ability if no ability selects else pick a card based on the selected ability
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator TimeRoutine() 
        {
            float timeDecrease = turnTime;
            int responseTime = UnityEngine.Random.Range(2, 8);
            while (true)
            {
                timeDecrease -= Time.deltaTime;
                timeCounter.text = string.Format("{0}:{1}", "00", timeDecrease.ToString("0"));
                if (timeDecrease <= (turnTime - responseTime))
                {
                    if (!TurnManager.Instance.AbilityChoosed())
                        PickCard();
                    else
                        MoveCardToCardPlace(PickCard(TurnManager.Instance.GetCurrentAbility()));

                    TurnManager.Instance.FinishTurn(id);
                    yield break;
                } 

                yield return null;
            }
        }
        #endregion
    }
}