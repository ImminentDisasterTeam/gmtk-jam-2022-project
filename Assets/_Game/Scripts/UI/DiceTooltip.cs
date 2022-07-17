using System.Linq;
using _Game.Scripts.GamePlay;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class DiceTooltip : Tooltip {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _avg;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _faces;

        public void Load(Dice data) {
            _name.text = data.Data.displayName;
            _description.text = data.Data.desc;
            _faces.text = string.Join(", ", data.Data.faces);
            _avg.text = data.Data.faces.Average().ToString("F2");
        }
    }
}