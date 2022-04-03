using System.Collections;
using System.Collections.Generic;
using tzdevil.GameRelated;
using UnityEngine;

namespace tzdevil.IngredientRelated
{
    public class RefillBox : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;

        [SerializeField] private bool _isRefilling;
        [SerializeField] private float _refillTimer;

        [SerializeField] private GameObject _destinedIngredient;

        private void Update()
        {
            CheckRefill();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _destinedIngredient)
            {
                _refillTimer = 1.0f;
                _isRefilling = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == _destinedIngredient)
                _isRefilling = false;
        }

        private void CheckRefill()
        {
            if (!_isRefilling) return;

            if (_refillTimer > 0)
                _refillTimer -= Time.deltaTime;
            else
            {
                _destinedIngredient.GetComponent<IngredientScript>().remainingPercent += _destinedIngredient.GetComponent<IngredientScript>().remainingPercent.ReturnClampedValue(10, true);
                _refillTimer = 1.0f;
            }
        }
    }
}