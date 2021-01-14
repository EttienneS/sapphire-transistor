using Assets.Factions;
using TMPro;
using UnityEngine;

namespace Assets.UI
{
    public class CurrentPlayerLabel : MonoBehaviour
    {
        private IFaction _currentFaction;
        private TMP_Text _label;

        internal void Hide()
        {
            gameObject.SetActive(false);
        }

        internal void Show(IFaction currentFaction)
        {
            gameObject.SetActive(true);
            _currentFaction = currentFaction;
        }

        private void Start()
        {
            _label = GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            if (_currentFaction == null)
            {
                return;
            }
            _label.text = $"Waiting for {_currentFaction.Name}'s turn..";
        }
    }
}