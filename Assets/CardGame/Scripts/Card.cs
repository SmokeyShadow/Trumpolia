using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.SmokeyShadow.Trumpolia
{
    public class Card : MonoBehaviour
    {
        #region SERIALIZABLE FIELDS
        [SerializeField]
        private Image image;
        [SerializeField]
        private GameObject cardCover;
        [SerializeField]
        private List<CardAbility> abilityList;
        #endregion

        #region FIELDS
        private int id;
        private Sprite cardImage;
        private bool covered = true;
        #endregion

        #region PUBLIC METHODS
        public void SetConfig(int id, List<CardAbility> abilityList)
        {
            this.id = id;
            cardImage = GameRefs.Instance.CardSprites[id];
            this.abilityList = abilityList;
            image.sprite = cardImage;
        }

        public void OnHold()
        {
            transform.parent = GameRefs.Instance.OnHoldPanel.transform;
            TurnManager.Instance.AddToOnHoldList(this);
        }

        public void DeHold()
        {
            transform.parent = null;
            gameObject.SetActive(false);
        }

        public int GetCardId()
        {
            return id;
        }

        public void DisableCover()
        {
            cardCover.SetActive(false);
            covered = false;
        }

        public bool IsCovered()
        {
            return covered;
        }

        public List<CardAbility> GetAbilities()
        {
            return abilityList;
        }
        #endregion

        #region CLASSES
        [System.Serializable]
        public class CardAbility
        {
            public int abilityValue;
            public AbilityType abilityType;
            public CardAbility(AbilityType type, int val)
            {
                abilityType = type;
                abilityValue = val;
            }
            public enum AbilityType
            {
                Height = 0, Intelligence, Strength, Agility, FightingSkills, Tech
            }
          
            public int AbilityVal
            {
                get
                {
                    return abilityValue;
                }
                set
                {
                    abilityValue = value;
                }
            }

        }
        #endregion
    }
}