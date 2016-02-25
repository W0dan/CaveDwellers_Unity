using System.Diagnostics;
using UnityEngine;

namespace Assets.Code.Objects
{
    public class Switch : MonoBehaviour, IActionable
    {
        public SpriteRenderer Renderer;

        public Sprite SwitchOn;
        public Sprite SwitchOff;

        public int OnValue = 1;
        private Stopwatch _lastToggled;

        public int Value { get; private set; }

        void Start()
        {
            Value = 0;
            Renderer.sprite = SwitchOff;
        }

        public void Toggle()
        {
            if (!Ready())
            {
                return;
            }
            _lastToggled = Stopwatch.StartNew();

            if (Value != OnValue)
            {
                Value = OnValue;
                Renderer.sprite = SwitchOn;
            }
            else
            {
                Value = 0;
                Renderer.sprite = SwitchOff;
            }
        }

        private bool Ready()
        {
            if (_lastToggled == null) return true;

            if (_lastToggled.ElapsedMilliseconds > 200) return true;

            return false;
        }

        public void ExecuteAction()
        {
            Toggle();
        }
    }
}
