using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using tzdevil.GameRelated;
using UnityEngine;

namespace tzdevil.ToolRelated
{
    public class SpoonScript : Draggable
    {
        public bool isFull; // is there a food on the spoon?
        [SerializeField] private Sprite _spoonFull, _spoonEmpty;

        [SerializeField] private PolygonCollider2D _pc;

        private void Awake() => Init();

        private void Init()
        {
            _servingCollider = GameObject.Find("ServingCollider").GetComponent<SpriteRenderer>();
            _ingredientCollider = GameObject.Find("IngredientCollider").GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Food" && !isFull)
            {
                isFull = true;
                ChangeSpoonSprite();
            }
        }

        public void ChangeSpoonSprite() => GetComponent<SpriteRenderer>().sprite = isFull ? _spoonFull : _spoonEmpty;

        private async void FillPlate()
        {
            if (!isFull) return;

            if (_gameManager.Customers.Count <= 0) return;

            // Serve the food (you can't touch the spoon while you are serving the food).
            _pc.enabled = false;
            await Task.Delay(600);
            _pc.enabled = true;

            _gameManager.Customers[0].isFull = true;
        }
    }
}