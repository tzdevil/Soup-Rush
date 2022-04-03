using System.Collections;
using System.Collections.Generic;
using tzdevil.GameRelated;
using UnityEngine;

namespace tzdevil.GameRelated
{
    public class Draggable : MonoBehaviour
    {
        public GameManager _gameManager => GameObject.Find("GameManager").GetComponent<GameManager>();
        public Rigidbody2D Rb;

        public SpriteRenderer _servingCollider, _ingredientCollider;

        public Camera Cam => GameObject.Find("Main Camera").GetComponent<Camera>();
        public Vector3 DistanceToMiddle;

        public virtual void OnDown()
        {
            if (gameObject.CompareTag("Tool")) _gameManager.CurrentTool = gameObject;
            else if (gameObject.CompareTag("Ingredient")) _gameManager.CurrentIngredient = gameObject;
            else if (gameObject.CompareTag("Plate")) _gameManager.CurrentPlate = gameObject;
            
            if (gameObject.name == "Pitcher" || gameObject.CompareTag("Ingredient"))
            {
                _gameManager.PlaceIngredientHere.gameObject.SetActive(true);
                _gameManager.PlaceIngredientBool = true;
                _gameManager.PlaceIngredientTimer = 1.0f;
            }
            else if (gameObject.CompareTag("Plate"))
            {
                _gameManager.PlaceFoodHere.gameObject.SetActive(true);
                _gameManager.PlaceFoodBool = true;
                _gameManager.PlaceFoodTimer = 1.0f;
            }

            Rb.gravityScale = 0;
            DistanceToMiddle = transform.position - Cam.ScreenToWorldPoint(Input.mousePosition);
            DistanceToMiddle.z = 0;

            _servingCollider.sortingOrder = 2;
            _ingredientCollider.sortingOrder = 2;
        }

        public virtual void OnDrag()
        {
            var pos = Cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            var _t = transform;
            _t.position = pos + DistanceToMiddle;
        }

        public virtual void OnUp() { 
            Rb.gravityScale = 1;
            _servingCollider.sortingOrder = -1;
            _ingredientCollider.sortingOrder = -1;

            if (gameObject.name == "Pitcher" || gameObject.CompareTag("Ingredient"))
            {
                _gameManager.PlaceIngredientHere.gameObject.SetActive(false);
                _gameManager.PlaceIngredientBool = false;
            }
            else if (gameObject.CompareTag("Plate"))
            {
                _gameManager.PlaceFoodHere.gameObject.SetActive(false);
                _gameManager.PlaceFoodBool = false;
            }
        }
    }
}