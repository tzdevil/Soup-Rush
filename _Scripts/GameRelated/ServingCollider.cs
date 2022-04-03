using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace tzdevil.GameRelated
{
    public class ServingCollider : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;

        [SerializeField] private SpriteRenderer _sr;

        private void Awake() => _sr.sortingOrder = -1;

        public void OnServingDown()
        {
            _sr.sortingOrder = -1;
        }

        public void OnServingDrop()
        {
            if (!_gameManager.CurrentPlate) return;

            _gameManager.MainPlate = _gameManager.CurrentPlate;

            _gameManager.CurrentPlate.transform.DOMove(transform.position, .2f);
            _gameManager.ServeFood(_gameManager.CurrentPlate.GetComponent<Customer>().CalculateSatisfaction());
            _gameManager.CurrentPlate = null;

            _gameManager.PlaceFoodHere.gameObject.SetActive(false);
            _gameManager.PlaceFoodBool = false;
        }
    }
}