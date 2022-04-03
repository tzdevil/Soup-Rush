using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using tzdevil.IngredientRelated;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace tzdevil.GameRelated
{
    public class GameManager : MonoBehaviour
    {
        [Header("Datas")]
        public GameData gameData;
        public List<Customer> Customers;

        [Header("Current Holding")]
        public GameObject CurrentTool; // what am I holding right now? Spoon? Pitcher?
        public GameObject CurrentIngredient; // what am I holding right now? Salt? Spice? Oil?
        public GameObject CurrentPlate; // which customer's plate am I holding right now?

        [Header("Gameplay Related")]
        [SerializeField] private bool _isPlaying; // is the game playing?

        // Burn
        private float _burnTimer;
        private float _burnTimerNext; // how many seconds till the next +%1 for burn? this might become tighter the longer the game goes on.

        // Burn
        private float _tastinessTimer; // her birka� saniyede bir azalacak
        private float _tastinessTimerNext; // how many seconds till the next +%1 for burn? this might become tighter the longer the game goes on.

        [SerializeField] private TMP_Text statsText;

        [SerializeField] private float refillTimer;
        [SerializeField] private bool isRefilling;

        public GameObject MainPlate;
        [SerializeField] private float _newCustomerTimer;
        [SerializeField] private float _newCustomerTimerNext; // how many seconds till the next plate?
        [SerializeField] private GameObject _customerPrefab;
        [SerializeField] private Transform _plates;


        [Header("Emotes")]
        [SerializeField] private GameObject _speechBubble;
        [SerializeField] private GameObject _emoteBubble;
        [SerializeField] private Sprite heartEmote, thumbsUpEmote, okEmote, thumbsDownEmote, badEmote;

        [Space]
        public Image PlaceFoodHere, PlaceIngredientHere;
        public bool PlaceFoodBool, PlaceIngredientBool;
        public float PlaceFoodTimer, PlaceIngredientTimer;

        private void Start() => Init();

        void Init()
        {
            _isPlaying = true;

            _newCustomerTimerNext = 3.0f;
            _newCustomerTimer = _newCustomerTimerNext;

            _tastinessTimerNext = 12.0f;
            _tastinessTimer = _tastinessTimerNext;

            _burnTimerNext = 2.0f;
            _burnTimer = _burnTimerNext;
        }

        private void Update()
        {
            StatsText();

            SpawnNewPlate();

            FoodBurningEveryFewSeconds();
            TastinessDecreasingEveryFewSeconds();

            CheckRefill();

            CheckPlaceFood();
            CheckPlaceIngredient();
        }

        void CheckPlaceFood()
        {
            if (!PlaceFoodHere) return;

            if (PlaceFoodTimer > 0) PlaceFoodTimer -= Time.deltaTime;
            else
                PlaceFoodTimer = 1.0f;

            var newColor = PlaceFoodHere.color;
            newColor.a = PlaceFoodTimer;
            PlaceFoodHere.color = newColor;
        }

        void CheckPlaceIngredient()
        {
            if (!PlaceIngredientHere) return;

            if (PlaceIngredientTimer > 0) PlaceIngredientTimer -= Time.deltaTime;
            else
                PlaceIngredientTimer = 1.0f;

            var newColor = PlaceIngredientHere.color;
            newColor.a = PlaceIngredientTimer;
            PlaceIngredientHere.color = newColor;
        }

        // will update whenever a stat change instead of every frame.
        private void StatsText()
        {
            StringBuilder stats = new();
            stats.Append($"<color=#9C56FC>Score:</color> {gameData.Score:F0}\n");
            stats.Append($"<color=#5A89E0>Remaining Food:</color> {(gameData.RemainingFood < 50 ? "<color=#fFA3020>" : "<color=#ffffff>")}{gameData.RemainingFood:F1}%</color>\n");
            stats.Append($"<color=#F03A24>Burnt Ratio:</color> {(gameData.BurntRatio >= 50 ? "<color=#fFA3020>" : "<color=#ffffff>")}{gameData.BurntRatio:F1}%</color>\n");
            stats.Append($"<color=#F08941>Tastiness:</color> {(gameData.Tastiness < 50 ? "<color=#fFA3020>" : "<color=#ffffff>")}{gameData.Tastiness:F1}%</color>\n");
            stats.Append($"<color=#F0E051>Ingredient Ratio:</color> {((gameData.IngredientRatio is < 25 or > 75) ? "<color=#FA3020>" : "<color=#ffffff>")}{gameData.IngredientRatio:F1} %</color>\n");
            stats.Append($"<color=#75D94A>Customer Satisfaction:</color> {(gameData.CustomerSatisfaction < 50 ? "<color=#fFA3020>" : "<color=#ffffff>")}{gameData.CustomerSatisfaction:F1}%</color>\n");
            statsText.text = stats.ToString();
        }

        void SpawnNewPlate()
        {
            if (!_isPlaying) return;

            if (Customers.Count >= 3) return;

            if (_newCustomerTimer > 0) _newCustomerTimer -= Time.deltaTime;
            else
            {
                GameObject customer = Instantiate(_customerPrefab, new Vector3(-10.9f, -3.23f, 0), Quaternion.identity, _plates);
                Customers.Add(customer.GetComponent<Customer>());
                MoveAccordingToYourPos();

                _newCustomerTimer = _newCustomerTimerNext;
            }
        }

        private void FoodBurningEveryFewSeconds()
        {
            if (!_isPlaying) return;

            if (_burnTimer > 0) _burnTimer -= Time.deltaTime;
            else
            {
                gameData.BurntRatio += 1;
                if (gameData.BurntRatio >= 100)
                    GameOver();
                else
                    _burnTimer = _burnTimerNext;
            }
        }

        private void TastinessDecreasingEveryFewSeconds()
        {
            if (!_isPlaying) return;

            if (_tastinessTimer > 0) _tastinessTimer -= Time.deltaTime;
            else
            {
                gameData.Tastiness -= 2;
                _tastinessTimer = _tastinessTimerNext;
            }
        }

        private void GameOver()
        {
            _isPlaying = false;
        }

        #region Refill Related
        private void CheckRefill()
        {
            if (!isRefilling) return;

            if (refillTimer > 0)
                refillTimer -= 1.0f;
            else
            {
                CurrentIngredient.GetComponent<IngredientScript>().remainingPercent += CurrentIngredient.GetComponent<IngredientScript>().remainingPercent.ReturnClampedValue(25, true);
                refillTimer = 1.0f;
            }
        }

        public void CheckRefillEnter(GameObject ingredient)
        {
            if (CurrentIngredient == ingredient)
            {
                refillTimer = 1.0f;
                isRefilling = true;
            }
        }

        public void CheckRefillExit(GameObject ingredient)
        {
            if (CurrentIngredient == ingredient)
            {
                isRefilling = false;
            }
        }
        #endregion

        #region Serving Related
        public Sprite SelectedEmote(float customerSatisfaction)
        {
            return customerSatisfaction switch
            {
                _ when customerSatisfaction >= 80 => heartEmote,
                _ when customerSatisfaction >= 60 && customerSatisfaction < 80 => thumbsUpEmote,
                _ when customerSatisfaction >= 40 && customerSatisfaction < 60 => okEmote,
                _ when customerSatisfaction >= 20 && customerSatisfaction < 40 => thumbsDownEmote,
                _ when customerSatisfaction >= 0 && customerSatisfaction < 20 => badEmote,
                _ => heartEmote
            };
        }

        public async void ServeFood(float customerSatisfaction)
        {
            Customers.Remove(MainPlate.GetComponent<Customer>());
            gameData.Score += customerSatisfaction;
            MoveAccordingToYourPos();

            _speechBubble.SetActive(true);

            await Task.Delay(100);

            MainPlate.transform.DOMoveX(-10.6f, .45f);

            await Task.Delay(650);

            Destroy(MainPlate);
            MainPlate = null;

            _speechBubble.transform.DOScale(Vector3.one, .35f);

            await Task.Delay(100);

            _emoteBubble.SetActive(true);
            _emoteBubble.GetComponent<Image>().sprite = SelectedEmote(customerSatisfaction);
            gameData.CustomerSatisfaction -= (100 - customerSatisfaction) / 10;

            await Task.Delay(600);

            _speechBubble.transform.DOScale(Vector3.zero, .35f);

            await Task.Delay(350);

            _speechBubble.SetActive(false);
            _emoteBubble.SetActive(false);
        }

        private void MoveAccordingToYourPos()
        {
            for (int i = 0; i < Customers.Count; i++)
            {
                Customers[i].transform.DOMoveX(-4 - i * 2.3f, .75f);
            }
        }
        #endregion
    }
}