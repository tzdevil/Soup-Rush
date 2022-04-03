using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using tzdevil.ToolRelated;
using UnityEngine;

namespace tzdevil.GameRelated
{
    public class Customer : Draggable
    {
        // If the plate's full, you have to drag the plate to the top left to serve.
        public bool isFull;
        public bool isServed;

        public float CustomerSatisfaction; // for this person. this then adds to the GameData.CustomerSatisfaction.

        public float WaitingTime; // How long did this person wait for?
        [SerializeField] private TMP_Text _waitingTimeText;

        [SerializeField] private bool isServing;
        [SerializeField] private float servingTimer;

        [SerializeField] private Sprite _plateEmpty, _plateFull;


        private void Awake()
        {
            Init();
        }

        void Init()
        {
            Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _servingCollider = GameObject.Find("ServingCollider").GetComponent<SpriteRenderer>();
            _ingredientCollider = GameObject.Find("IngredientCollider").GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            WaitingTime += Time.deltaTime;
            _waitingTimeText.text = WaitingTime.ToString("F0");

            CheckServing();
        }

        void CheckServing()
        {
            if (isFull) return;

            if (!isServing) return;

            if (servingTimer > 0)
            {
                servingTimer -= Time.deltaTime;
            }
            else
            {
                isFull = true;
                _gameManager.gameData.RemainingFood -= _gameManager.gameData.RemainingFood.ReturnClampedValue(2, false);
                ChangePlateSprite();
                _gameManager.CurrentTool.GetComponent<SpoonScript>().isFull = false;
                _gameManager.CurrentTool.GetComponent<SpoonScript>().ChangeSpoonSprite();
            }
        }

        public void ChangePlateSprite() => GetComponent<SpriteRenderer>().sprite = isFull ? _plateFull : _plateEmpty;

        public float CalculateSatisfaction()
        {
            CustomerSatisfaction = 100;
            CustomerSatisfaction -= 100 - _gameManager.gameData.Tastiness;
            CustomerSatisfaction -= Mathf.Abs(50 - _gameManager.gameData.IngredientRatio);
            CustomerSatisfaction -= _gameManager.gameData.BurntRatio / 5;
            CustomerSatisfaction -= WaitingTime / 5;
            return CustomerSatisfaction;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name != "Spoon") return;

            if (!collision.gameObject.GetComponent<SpoonScript>().isFull) return;

            servingTimer = 0.35f;
            isServing = true;
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name != "Spoon") return;

            isServing = false;
            servingTimer = 0;
        }

        public override void OnDown()
        {
            if (isFull && !isServed)
            {
                base.OnDown();
                _servingCollider.GetComponent<SpriteRenderer>().sortingOrder = 2;
                Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        public override void OnDrag()
        {
            if (isFull && !isServed)
                base.OnDrag();
        }

        public override void OnUp()
        {
            _servingCollider.sortingOrder = -1;
            _ingredientCollider.sortingOrder = -1;

            if (!isFull) return;

            base.OnDrag();

            Rb.gravityScale = 0;
        }
    }
}