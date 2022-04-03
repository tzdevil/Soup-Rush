using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using tzdevil.GameRelated;
using UnityEngine;

namespace tzdevil.IngredientRelated
{
    public class IngredientScript : Draggable
    {
        public float remainingPercent;

        [SerializeField] private TMP_Text remainingPercentText;

        [SerializeField] private Sprite saltEmpty, saltFull, spiceEmpty, spiceFull, oilEmpty, oilFull;

        private void Awake() => Init();

        void Init()
        {
            _servingCollider = GameObject.Find("ServingCollider").GetComponent<SpriteRenderer>();
            _ingredientCollider = GameObject.Find("IngredientCollider").GetComponent<SpriteRenderer>();
            remainingPercent = 100;
        }

        private void Update()
        {
            remainingPercentText.text = remainingPercent.ToString();

            // todo after jam, this shouldn't be in Update.
            ChangeIngredient();
        }

        public async override void OnUp()
        {
            base.OnUp();

            await Task.Delay(250);
            _gameManager.CurrentIngredient = null;
        }

        public void ChangeIngredient() => GetComponent<SpriteRenderer>().sprite = gameObject.name switch
        {
            "Salt" => remainingPercent == 0 ? saltEmpty : saltFull,
            "Spice" => remainingPercent == 0 ? spiceEmpty : spiceFull,
            "Oil" => remainingPercent == 0 ? oilEmpty : oilFull,
            _ => throw new System.NotImplementedException(),
        };
    }
}