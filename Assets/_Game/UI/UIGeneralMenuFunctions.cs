using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.UI
{
    public class UIGeneralMenuFunctions : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private void Awake()
        {
            Assert.IsNotNull(_startButton);
            _startButton.onClick.AddListener(delegate { SceneTransition("Game"); });
            // settingsButton.onClick.AddListener(DisplaySettings);
            // quitButton.onClick.AddListener(Application.Quit);
        }

        private void SceneTransition(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void DisplaySettings()
        {
        
        }
    }
}
