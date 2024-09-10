using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Com.SmokeyShadow.Trumpolia
{
    public class Player : PlayerData
    {
        #region FIELDS
        private bool endturn = false;
        #endregion

        #region MONOBEHAVIOURS
        private void Start()
        {
            GameRefs.Instance.TurnBtn.onClick.AddListener(EndTurn);
            InitDictionary();
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
            TurnManager.Instance.SetTurnInteractable(true);
            endturn = false;
            while (true)
            {
                timeDecrease -= Time.deltaTime;
                timeCounter.text = string.Format("{0}:{1}", "00", timeDecrease.ToString("0"));
                if (timeDecrease <= 0.5f)
                {
                    if (!TurnManager.Instance.AbilityChoosed())
                    {
                        if (!CardPlaced())
                            PickCard();
                        else
                            ChooseBestAbility(GetPlayedCard());
                    }
                    else
                    {
                        if (!CardPlaced())
                            MoveCardToCardPlace(PickCard(TurnManager.Instance.GetCurrentAbility()));
                    }
                    TurnManager.Instance.FinishTurn(id);

                    yield break;
                }
                else if (endturn)
                {
                    endturn = false;
                    timeDecrease = turnTime;
                    timeCounter.text = string.Format("{0}:{1}", "00", timeDecrease.ToString("0"));
                    yield break;
                }
                yield return null;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void EndTurn()
        {
            TurnManager.Instance.LockPlayPanel(false);
            TurnManager.Instance.FinishTurn(id);
            endturn = true;
        }
        #endregion
    }
}