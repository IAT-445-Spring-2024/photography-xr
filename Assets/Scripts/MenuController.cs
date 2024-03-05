using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] private Camera physicalCamera;
    [SerializeField] private GameObject selectionFrame;
    [SerializeField] private Slider shutterSlider;
    [SerializeField] private Slider apertureSlider;
    [SerializeField] private Slider isoSlider;

    private PlayerInputActions inputActions;

    private ShutterOption shutterOption;
    private ApertureOption apertureOption;
    private ISOOption isoOption;
    private Option activeOption;
    private bool isSwitchingOption = false;

    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
    }

    private void Start() {
        shutterOption = new ShutterOption(shutterSlider, physicalCamera);
        apertureOption = new ApertureOption(apertureSlider, physicalCamera);
        isoOption = new ISOOption(isoSlider, physicalCamera);

        activeOption = shutterOption;
    }

    private void Update() {
        Vector2 adjustmentVector = inputActions.Player.Adjust.ReadValue<Vector2>();
        adjustmentVector = adjustmentVector.normalized;

        // TODO: Determine user intent.
        bool isAdjustingValue = false;
        if (isAdjustingValue) {
            // TODO: Convert magnitude to speed.
            activeOption.SwitchToRightValue();
        } else {
            // TODO: Lerp option changes

        }
    }

    private abstract class Option {
        public enum Type {
            Mode,
            Shutter,
            Aperture,
            ISO
        }

        abstract public Type ParameterType { get; }

        abstract public void SwitchToLeftValue();

        abstract public void SwitchToRightValue();
    }

    class ShutterOption: Option {
        private readonly Slider slider;
        private readonly Camera camera;
        private readonly float adjustmentSpeed = 0.01f;

        private float GetShutterSpeedFromSliderValue() {
            return slider.value / 1000;
        }

        override public Type ParameterType { get { return Type.Shutter; } }

        public ShutterOption(Slider slider, Camera camera) {
            this.slider = slider;
            this.camera = camera;
        }

        public override void SwitchToLeftValue() {
            if (slider.value >= 0) {
                slider.value -= 1 * adjustmentSpeed;
            }
            camera.shutterSpeed = GetShutterSpeedFromSliderValue();
        }

        public override void SwitchToRightValue() {
            if (slider.value <= 1) {
                slider.value -= 1 * adjustmentSpeed;
            }
            camera.shutterSpeed = GetShutterSpeedFromSliderValue();
        }
    }

    class ApertureOption: Option {
        private readonly Slider slider;
        private readonly Camera camera;
        private readonly float adjustmentSpeed = 0.01f;

        private float GetApertureFromSliderValue() {
            // TODO: Update this formula. 
            return slider.value / 1000;
        }

        override public Type ParameterType { get { return Type.Aperture; } }

        public ApertureOption(Slider slider, Camera camera) {
            this.slider = slider;
            this.camera = camera;
        }

        public override void SwitchToLeftValue() {
            if (slider.value >= 0) {
                slider.value -= 1 * adjustmentSpeed;
            }
            camera.shutterSpeed = GetApertureFromSliderValue();
        }

        public override void SwitchToRightValue() {
            if (slider.value <= 1) {
                slider.value -= 1 * adjustmentSpeed;
            }
            camera.shutterSpeed = GetApertureFromSliderValue();
        }
    }

    class ISOOption: Option {
        private readonly Slider slider;
        private readonly Camera camera;
        private readonly float adjustmentSpeed = 0.01f;

        private float GetISOFromSliderValue() {
            // TODO: Update this formula. 
            return slider.value * 1000;
        }

        override public Type ParameterType { get { return Type.ISO; } }

        public ISOOption(Slider slider, Camera camera) {
            this.slider = slider;
            this.camera = camera;
        }

        public override void SwitchToLeftValue() {
            if (slider.value >= 0) {
                slider.value -= 1 * adjustmentSpeed;
            }
            camera.shutterSpeed = GetISOFromSliderValue();
        }

        public override void SwitchToRightValue() {
            if (slider.value <= 1) {
                slider.value -= 1 * adjustmentSpeed;
            }
            camera.shutterSpeed = GetISOFromSliderValue();
        }
    }
}
