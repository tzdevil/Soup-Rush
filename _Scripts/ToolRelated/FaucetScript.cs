using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tzdevil.ToolRelated
{
    public class FaucetScript : MonoBehaviour
    {
        [SerializeField] private PitcherScript _pitcher;

        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Sprite _faucetEmpty, _faucetFilling;

        [SerializeField] private bool _isFillingPitcher;
        [SerializeField] private float _fillingPitcherTimer;

        private void Update()
        {
            if (_isFillingPitcher)
            {
                if (_fillingPitcherTimer > 0) _fillingPitcherTimer -= Time.deltaTime;
                else
                {
                    FillPitcher();
                    _isFillingPitcher = false;
                }
            }
        }

        public void OnMouseDown()
        {
            if (!_pitcher.isFull)
            {
                _fillingPitcherTimer = 2.0f;
                _isFillingPitcher = true;
                _sr.sprite = _faucetFilling;
                _pitcher.isClickable = false;
                transform.position = new Vector3(8.14f, -1.858f, 0);
            }
        }

        void FillPitcher() {
            _pitcher.isFull = true;
            _pitcher.ChangePitcherSprite();
            _pitcher.isClickable = true;
            _sr.sprite = _faucetEmpty;
            transform.position = new Vector3(8.14f, -0.649f, 0);
        }
    }
}