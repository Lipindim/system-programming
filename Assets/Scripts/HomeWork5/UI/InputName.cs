using Characters;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace HomeWork5
{
    public class InputName : MonoBehaviour
    {
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private InputField _nameInput;

        private void Start()
        {
            _nameInput.onValueChanged.AddListener(OnNameChanged);
            ShipController.Connected += OnConnected;
        }

        private void OnDestroy()
        {
            _nameInput.onValueChanged.RemoveAllListeners();
            ShipController.Connected -= OnConnected;
        }

        private void OnConnected()
        {
            gameObject.SetActive(false);
        }

        private void OnNameChanged(string name)
        {
            _playerSettings.PlayerName = name;
        }
    }
}
