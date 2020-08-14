using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.HandSystemUseCase.Domain;

namespace Conekton.ARUtility.HandSystemUseCase.Infrastructure
{
    public class EditorHandProvider : MonoBehaviour, IHandProvider
    {
        [SerializeField] private bool _showGUI = true;

        private IHand[] _hands = null;

        private EditorHand _leftHand = null;
        private EditorHand _rightHand = null;

        private bool _controlTargetIsLeft = true;

        #region ### MonoBehaviour ###
        private void Awake()
        {
            GameObject leftHandObj = new GameObject("EditorLeftHand", typeof(EditorHand));
            GameObject rightHandObj = new GameObject("EditorRightHand", typeof(EditorHand));

            _leftHand = leftHandObj.GetComponent<EditorHand>();
            _leftHand.HandType = HandType.Left;

            _rightHand = rightHandObj.GetComponent<EditorHand>();
            _rightHand.HandType = HandType.Right;

            _hands = new[]
            {
                _leftHand, _rightHand,
            };
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.I))
            {
                IncresePinchStrength();
            }

            if (UnityEngine.Input.GetKey(KeyCode.O))
            {
                DecresePinchStrength();
            }
        }

        private void OnGUI()
        {
            if (!_showGUI)
            {
                return;
            }

            if (GUI.Button(new Rect(10, 10, 150, 30), _controlTargetIsLeft ? "Controling Left" : "Controling Right"))
            {
                _controlTargetIsLeft = !_controlTargetIsLeft;
            }

            if (GUI.Button(new Rect(10, 50, 150, 30), "Increse pinch (I)"))
            {
                IncresePinchStrength();
            }

            if (GUI.Button(new Rect(10, 90, 150, 30), "Decrese pinch (O)"))
            {
                DecresePinchStrength();
            }
        }
        #endregion ### MonoBehaviour ###

        IHand[] IHandProvider.GetAllHands()
        {
            return _hands;
        }

        IHand IHandProvider.GetHand(HandType handType)
        {
            switch (handType)
            {
                case HandType.Left:
                    return _leftHand;

                case HandType.Right:
                    return _rightHand;

                default:
                    return _rightHand;
            }
        }

        private void IncresePinchStrength()
        {
            AddPinchStrength(0.05f);
        }

        private void DecresePinchStrength()
        {
            AddPinchStrength(-0.05f);
        }

        private void AddPinchStrength(float value)
        {
            (_controlTargetIsLeft ? _leftHand : _rightHand).PinchStrength += value;
        }
    }
}

