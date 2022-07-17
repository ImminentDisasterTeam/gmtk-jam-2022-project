using _Game.Scripts.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class ItemTooltip : Tooltip {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _stats;
        [SerializeField] private TextMeshProUGUI _money;
        [SerializeField] private Image _dices;

        public void Load(Item data) {
            _name.text = data.Data.displayName;
            _description.text = data.Data.desc;
            _money.text = data.Data.cost.ToString();

            var diceCount = data.Data.stats.DiceCount;
            _stats.text = data.Data.stats + (diceCount == 0 ? "" : $" <color=orange>{diceCount}");
            _dices.enabled = diceCount != 0;
        }
    }
}