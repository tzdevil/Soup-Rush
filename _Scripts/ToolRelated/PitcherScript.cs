using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using tzdevil.GameRelated;
using UnityEngine;

namespace tzdevil.ToolRelated
{
    public class PitcherScript : Draggable
    {
        public bool isFull; // is there water inside?
        public bool isClickable; // is there water inside?

        [SerializeField] private Sprite _pitcherFull, _pitcherEmpty;

        private void Awake() => Init();

        private void Init()
        {
            isClickable = true;
            _servingCollider = GameObject.Find("ServingCollider").GetComponent<SpriteRenderer>();
            _ingredientCollider = GameObject.Find("IngredientCollider").GetComponent<SpriteRenderer>();
        }

        public void ChangePitcherSprite() => GetComponent<SpriteRenderer>().sprite = isFull ? _pitcherFull : _pitcherEmpty;

        public void OnPitcherDown()
        {
            if (!isClickable) return;

            _gameManager.CurrentTool = gameObject;
        }

        public override void OnDown()
        {
            if (!isClickable) return;

            base.OnDown();
        }

        public override void OnDrag()
        {
            if (!isClickable) return;

            base.OnDrag();
        }

        public override void OnUp()
        {
            if (!isClickable) return;

            _gameManager.PlaceIngredientHere.gameObject.SetActive(false);
            _gameManager.PlaceIngredientBool = false;

            Rb.gravityScale = 1;
            _servingCollider.sortingOrder = -1;
            _ingredientCollider.sortingOrder = -1;

            if (isFull) return;

            base.OnDrag();
        }
    }
}