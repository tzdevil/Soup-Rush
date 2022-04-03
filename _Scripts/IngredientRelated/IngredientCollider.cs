using System.Collections;
using System.Collections.Generic;
using tzdevil.GameRelated;
using tzdevil.ToolRelated;
using UnityEngine;

namespace tzdevil.IngredientRelated
{
    public class IngredientCollider : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;

        [SerializeField] private SpriteRenderer _sr;

        private void Awake() => _sr.sortingOrder = -1;

        public void OnServingDrop()
        {
            if (!_gameManager.CurrentTool && !_gameManager.CurrentIngredient) return;

            CheckTool();
            CheckIngredient();
        }

        private void CheckIngredient()
        {
            if (!_gameManager.CurrentIngredient) return;
           
            AddIngredient(_gameManager.CurrentIngredient);
        }

        void CheckTool()
        {
            if (!_gameManager.CurrentTool) return;

            if (_gameManager.CurrentTool.name == "Pitcher")
            {
                AddWater(_gameManager.CurrentTool);
            }
        }

        void AddWater(GameObject pitcher)
        {
            if (!pitcher.GetComponent<PitcherScript>().isFull) return;

            // The food gets cooler and remaining food increases. IngredientRatio decreases and so does Tastiness.
            _gameManager.gameData.BurntRatio -= _gameManager.gameData.BurntRatio.ReturnClampedValue(3, false);
            _gameManager.gameData.IngredientRatio -= _gameManager.gameData.IngredientRatio.ReturnClampedValue(1, false);
            _gameManager.gameData.Tastiness -= _gameManager.gameData.Tastiness.ReturnClampedValue(1, false);
            _gameManager.gameData.RemainingFood += _gameManager.gameData.RemainingFood.ReturnClampedValue(1, true);

            pitcher.GetComponent<PitcherScript>().isFull = false;
            pitcher.GetComponent<PitcherScript>().ChangePitcherSprite();
            _gameManager.CurrentTool = null;
        }

        void AddIngredient(GameObject ingredient)
        {
            if (ingredient.GetComponent<IngredientScript>().remainingPercent < 20) return;

            _gameManager.gameData.IngredientRatio += _gameManager.gameData.IngredientRatio.ReturnClampedValue(3, true);
            _gameManager.gameData.Tastiness += _gameManager.gameData.Tastiness.ReturnClampedValue(1, true);
            ingredient.GetComponent<IngredientScript>().remainingPercent -= 20; // it decreases fast but you can always refresh it from top left
        }
    }

}