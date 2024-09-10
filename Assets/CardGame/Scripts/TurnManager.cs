using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.SmokeyShadow.Trumpolia
{
    public class TurnManager : MonoBehaviour
    {
        #region STATIC FIELDS
        private static TurnManager _instance;
        #endregion

        #region SERIALIZABLE FIELDS
        [SerializeField]
        private PlayerData player;
        [SerializeField]
        private PlayerData bot;
        [SerializeField]
        private List<RoundStruct> rounds;

        [SerializeField]
        private Text abilityText;
        [SerializeField]
        private Image abilityImage;
        [SerializeField]
        private List<Button> abilityBtns;
        #endregion

        #region FIELDS
        private int currentRound = 0;
        private Card.CardAbility.AbilityType currentAbility;
        private Turn currentTurn;
        List<Card> onHoldCards = new List<Card>();
        #endregion

        #region PROPERTIES
        public static TurnManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<TurnManager>();
                return _instance;
            }
        }
        #endregion

        #region ENUMS
        enum Turn
        {
            PlayerTurn = 0,
            OpponentTurn
        }

        enum RoundStatus
        {
            PlayerWin = 0,
            OpponentWin,
            Draw
        }
        #endregion

        #region PUBLIC METHODS
        public PlayerData GetPlayer()
        {
            return player;
        }

        public PlayerData GetBot()
        {
            return bot;
        }

        public void SetTurn(int playerId)
        {
            if (playerId == player.GetId())
                currentTurn = Turn.PlayerTurn;
            else
                currentTurn = Turn.OpponentTurn;
        }

        public void OnCalculateRound()
        {
            CalculateRewards(GetWinnerID(currentAbility));
        }

        public int GetWinnerID(Card.CardAbility.AbilityType ability)
        {
            if (player.GetPlayedCard().GetAbilities()[(int)ability].AbilityVal == bot.GetPlayedCard().GetAbilities()[(int)ability].AbilityVal)
                return -1;
            else if (player.GetPlayedCard().GetAbilities()[(int)ability].AbilityVal >= bot.GetPlayedCard().GetAbilities()[(int)ability].AbilityVal)
                return player.GetId();
            return bot.GetId();
        }

        public void SetPlayerInfo(string playerId, string playerName)
        {
            player.SetPlayerInfo(int.Parse(playerId), playerName);
        }

        public void ChooseAbility(int ability)
        {
            currentAbility = (Card.CardAbility.AbilityType)ability;
            abilityText.text = currentAbility.ToString().ToUpper();
            abilityImage.sprite = GameRefs.Instance.AbilitySprites[ability];
            abilityImage.enabled = true;
            abilityText.enabled = true;
        }

        public bool IsMyTurn()
        {
            if (currentTurn == Turn.PlayerTurn)
                return true;
            return false;
        }

        public void LockPlayPanel(bool enable)
        {
            if (!enable)
                SetTurnInteractable(enable);
            if (enable && AbilityChoosed())
                return;
            foreach (var btn in abilityBtns)
                btn.interactable = enable;
        }

        public void SetTurnInteractable(bool enable)
        {
            GameRefs.Instance.TurnBtn.gameObject.GetComponent<Button>().interactable = enable;
        }

        public void AddToOnHoldList(Card card)
        {
            onHoldCards.Add(card);
            GameRefs.Instance.OnHoldCardsTxt.text = onHoldCards.Count.ToString();
        }

        /// <summary>
        /// call this function when someone finish his turn
        /// </summary>
        /// <param name="id"></param>
        public void FinishTurn(int id)
        {
            if (id != GetBot().GetId())
            {
                if (player.AnyCardsLeft())
                    player.FinishTurnRequest(((int)currentAbility).ToString());
            }
            else
            {
                if (bot.AnyCardsLeft())
                    bot.FinishTurnRequest(((int)currentAbility).ToString());
            }
        }

        public void ChangeTurn()
        {
            currentTurn = 1 - currentTurn;
            TurnExecute();
        }

        public void CalculateRewards(int winnerID)
        {
            RoundStatus status = GetWinStatus(winnerID);
            StartCoroutine(RewardRoutine(status));
        }

        public int GetCurrentAbility()
        {
            return (int)currentAbility;
        }

        public bool AbilityChoosed()
        {
            return abilityImage.enabled;
        }

        public bool OnCalculateReward()
        {
            if (AbilityChoosed() && player.CardPlaced() && bot.CardPlaced())
                return true;
            else
                return false;
        }

        public void TurnExecute()
        {
            SetTurnState();
            if (currentTurn == 0)
                player.MoveCard();
            else
                bot.MoveCard();
        }
        #endregion

        #region PRIVATE METHODS
        private void SetTurnState()
        {
            bool yourTurn = currentTurn == Turn.PlayerTurn;
            GameRefs.Instance.TurnBtn.GetComponent<Image>().color = yourTurn ? GameRefs.Instance.YourTurnColor : GameRefs.Instance.OpponentTurnColor;
            LockPlayPanel(yourTurn);
        }

        private RoundStatus GetWinStatus(int WinnerId)
        {
            if (WinnerId < 0)
                return RoundStatus.Draw;
            else if (WinnerId == player.GetId())
                return RoundStatus.PlayerWin;
            return RoundStatus.OpponentWin;
        }

        private void OnNextRound()
        {
            currentRound++;
            NextRound();
        }
        /// <summary>
        /// check if game is finished -> show game over panel else execute next turn
        /// </summary>
        private void NextRound()
        {
            if (GameState.Instance.GetState() == GameState.State.Finished || !player.AnyCardsLeft() || !bot.AnyCardsLeft())
            {
                RoundStatus youWin = rounds[currentRound - 1].PlayerScore > rounds[currentRound - 1].OpponentScore ? RoundStatus.PlayerWin
                    : rounds[currentRound - 1].PlayerScore < rounds[currentRound - 1].OpponentScore ? RoundStatus.OpponentWin
                    : RoundStatus.Draw;
                ShowGameOverPanel(youWin);
            }
            else
            {
                currentTurn = 1 - PrevRoundTurn();
                TurnExecute();
            }
        }

        private void RemoveHoldCards()
        {
            foreach (Card c in onHoldCards)
                c.DeHold();
            GameRefs.Instance.OnHoldCardsTxt.text = "0";
            onHoldCards.Clear();
        }

        private void ShowRoundStatusPanel(RoundStatus status) 
        {
            bool youWin = status == RoundStatus.PlayerWin;
            bool draw = status == RoundStatus.Draw;

            GameRefs.Instance.RoundWinText.text = draw ? ("Equal Abilities") :
                ((youWin ? "You" : "Opponent") + " win this round.");
            int abilityNo = (int)rounds[currentRound].RewardAbility;
            try
            {
                GameRefs.Instance.roundAbilities.text = rounds[currentRound].RewardAbility.ToString() + " ability Values -> "
               + "Your's : " + rounds[currentRound].PlayerCard.GetAbilities()[abilityNo].abilityValue + " , opponent's : " +
                rounds[currentRound].OpponentCard.GetAbilities()[abilityNo].abilityValue;

            }
            catch (System.Exception e)
            {
                Debug.Log("server must add draw behaciour" + e.ToString());
            }

            GameRefs.Instance.RoundScore.text = player.GetScore() + ":" + bot.GetScore();
            GameRefs.Instance.RoundGainRewards.text = draw ? "Cards holds" :
               (youWin ? "Ability : " + rounds[currentRound].RewardAbility.ToString() + ", " + 2.ToString() + " Cards, Total " +
               (2 + onHoldCards.Count).ToString() + " Cards" : "0 Cards");

            GameRefs.Instance.RoundStatePanel.SetActive(true);
        }

        private void ShowGameOverPanel(RoundStatus status)
        {
            bool youWin = status == RoundStatus.PlayerWin;
            bool draw = status == RoundStatus.Draw;

            GameRefs.Instance.GameOverWinText.text = draw ? ("Draw game") : ((youWin ? "You" : "Opponent") + " win this Game.");

            GameRefs.Instance.GameOverAbilities.text = GetAllWinAbilities();

            GameRefs.Instance.GameOverGainRewards.text = draw ? "3 Ability Cards, +Exp : " + player.GetWinCards() :
                ((youWin) ? ("6 tokens" + " , +Exp : " + player.GetWinCards()) : ("+Exp : " + player.GetWinCards())) ;
            GameRefs.Instance.GameOverPanel.SetActive(true);
        }

        private string GetAllWinAbilities()
        {
            string allAbilities = "Your ability tokens :";
            foreach (var round in rounds)
            {
                if (round.WinnerId == player.GetId())
                    allAbilities += ", " + round.RewardAbility.ToString();
            }
            return allAbilities;
        }

        private Turn PrevRoundTurn()
        {
            if (rounds[currentRound - 1].TurnStarterId == bot.GetId())
                return Turn.OpponentTurn;
            return Turn.PlayerTurn;
        }

        private void CalculateRoundReward(RoundStatus winStatus)
        {
            RoundStruct round = new RoundStruct();
            round.TurnStarterId = currentTurn == 0 ? bot.GetId() : player.GetId();
            round.RoundNum = currentRound;
            round.PlayerCard = player.GetPlayedCard();
            round.OpponentCard = bot.GetPlayedCard();
            round.WinnerId = winStatus == RoundStatus.PlayerWin ? player.GetId() : winStatus == RoundStatus.Draw ? -1 : bot.GetId();
            round.RewardAbility = currentAbility;
            round.PlayerScore = (currentRound > 0 ? rounds[currentRound - 1].PlayerScore : 0);
            round.OpponentScore = (currentRound > 0 ? rounds[currentRound - 1].OpponentScore : 0);
            if (winStatus == RoundStatus.PlayerWin)
            {
                round.PlayerScore += 1 + (int)(onHoldCards.Count / 2f);
                player.UpdateScore(round.PlayerScore);
            }
            else if (winStatus == RoundStatus.OpponentWin)
            {
                round.OpponentScore += 1 + (int)(onHoldCards.Count / 2f);
                bot.UpdateScore(round.OpponentScore);
            }
            round.WinCards = new List<Card> { round.PlayerCard, round.OpponentCard };
            rounds.Add(round);
        }
        #endregion

        #region CO ROUTINE
        private IEnumerator RewardRoutine(RoundStatus status)
        {
            CalculateRoundReward(status);
            yield return new WaitForSeconds(2f);
            ShowRoundStatusPanel(status);
            yield return new WaitForSeconds(2f);
            GameRefs.Instance.RoundStatePanel.SetActive(false);
            if (status == RoundStatus.PlayerWin)
            {
                player.UpdateAbility((int)currentAbility, 1);
                player.UpdateWinCards(2);
            }
            else if(status == RoundStatus.OpponentWin)
            {
                bot.UpdateAbility((int)currentAbility, 1);
                bot.UpdateWinCards(2);
            }
            abilityImage.enabled = false;
            abilityText.enabled = false;
            if (status == RoundStatus.PlayerWin)
            {
                player.GetPlayedCard().OnHold();
                bot.GetPlayedCard().OnHold();
            }
            else
            {
                player.RemoveCard();
                bot.RemoveCard();
            }
            OnNextRound();
        }
        #endregion

        #region STRUCTS
        [System.Serializable]
        struct RoundStruct
        {
            public int RoundNum;
            public Card.CardAbility.AbilityType RewardAbility;
            public List<Card> WinCards;
            public Card PlayerCard;
            public Card OpponentCard;
            public int TurnStarterId;
            public int WinnerId;
            public int PlayerScore;
            public int OpponentScore;
        }
        #endregion
    }
}
