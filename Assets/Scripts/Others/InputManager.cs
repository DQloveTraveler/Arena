using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputManagement
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        public Controller Inputter { get; private set; } = Controller.KeyboardMouse;

        void Start()
        {
            Time.timeScale = 1;
        }

        void Update()
        {
            _Check();
        }

        private void _Check()
        {
            var controllerNames = Input.GetJoystickNames();
            if (controllerNames.Length <= 0 || !controllerNames[0].Contains("Controller"))
            {
                Inputter = Controller.KeyboardMouse;
            }
            else
            {
                Inputter = Controller.GamePad;
            }
        }
    }
}

public enum Controller { GamePad, KeyboardMouse}
