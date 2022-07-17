using System;
using UnityEngine;

namespace _Game.UI
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }

        public UIController() {
            if (Instance != null)
                throw new ApplicationException("ONLY ONE UICONTROLLER ALLOWED");
            Instance = this;
        }
    }
}
