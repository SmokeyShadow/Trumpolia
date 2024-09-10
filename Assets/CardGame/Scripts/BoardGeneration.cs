using System.Collections;
using UnityEngine;

namespace Com.SmokeyShadow.Trumpolia
{
    public class BoardGeneration : MonoBehaviour
    {
        #region PUBLIC METHODS
        public void FillDesks()
        {
            StartCoroutine(FillDesksRoutine());
        }
        #endregion

        #region PRIVATE METHODS
        private void AddCardToDesk(PlayerData player)
        {
            player.DrawCard();
        }
        #endregion

        #region CO ROUINES
        /// <summary>
        /// fill desk with amounts of frontier cards count. if card desk frontier filled, Change game state to play
        /// </summary>
        /// <returns></returns>
        IEnumerator FillDesksRoutine()
        {
            int count = 0;
            yield return new WaitForSeconds(2f);
            while (true)
            {
                if (count < GameRefs.Instance.FrontierLimit)
                {
                    AddCardToDesk(TurnManager.Instance.GetPlayer());
                    AddCardToDesk(TurnManager.Instance.GetBot());
                    count++;
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    GameManager.Instance.OnBoardReady();
                    yield break;
                }
                yield return null;
            }
        }
        #endregion
    }
}