using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    // TODO: We can probably generalize this with ScriptableObjects
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
    
    // Animations
    [SerializeField] private float animationSpeed = 5f;

    // Selection frame bounds
    private Vector3 TopPosition() {
        return shutterOption.GetPosition();
    }
    
    private Vector3 BottomPosition() {
        return isoOption.GetPosition();
    }

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

        (Vector2, float) dotProductPairUp      = (Vector2.up,       Vector2.Dot(adjustmentVector, Vector2.up));
        (Vector2, float) dotProductPairDown    = (Vector2.down,     Vector2.Dot(adjustmentVector, Vector2.down));
        (Vector2, float) dotProductPairLeft    = (Vector2.left,     Vector2.Dot(adjustmentVector, Vector2.left));
        (Vector2, float) dotProductPairRight   = (Vector2.right,    Vector2.Dot(adjustmentVector, Vector2.right));
        Vector2 intentionVector = VectorWithLargestDotProduct(new (Vector2, float)[] {
            dotProductPairUp, 
            dotProductPairDown, 
            dotProductPairLeft, 
            dotProductPairRight
        });

        // Adjusting values
        if (intentionVector == Vector2.left) {
            activeOption.SwitchToLeftValue(adjustmentVector.magnitude * Time.deltaTime);
        } else if (intentionVector == Vector2.right) {
            activeOption.SwitchToRightValue(adjustmentVector.magnitude * Time.deltaTime);
        }

        // Updating options
        bool isChangingOption = intentionVector == Vector2.up || intentionVector == Vector2.down;
        if (isChangingOption) {
            Vector3 targetPosition = intentionVector == Vector2.up ? TopPosition() : BottomPosition();
            selectionFrame.transform.position = Vector3.Lerp(
                selectionFrame.transform.position,
                targetPosition,
                Time.deltaTime * animationSpeed
            );
        } else {
            Option targetOption = CloestOption(selectionFrame.transform.position);
            Vector3 targetPosition = targetOption.GetPosition();
            // TODO: To make it smoother, consider the final velocity of the last motion.
            selectionFrame.transform.position = Vector3.Lerp(
                selectionFrame.transform.position,
                targetPosition,
                Time.deltaTime * animationSpeed * 2
            );
            if (Math.Abs(selectionFrame.transform.position.y - targetPosition.y) < 1) {
                activeOption = targetOption;
            }
        }
    }

    private Option CloestOption(Vector3 toPosition) {
        float minDistance = float.MaxValue;
        Option cloestOption = activeOption;

        float distance = Vector3.Distance(toPosition, shutterOption.GetPosition());
        if (distance < minDistance) {
            cloestOption = shutterOption;
            minDistance = distance;
        }

        distance = Vector3.Distance(toPosition, apertureOption.GetPosition());
        if (distance < minDistance) {
            cloestOption = apertureOption;
            minDistance = distance;
        }

        distance = Vector3.Distance(toPosition, isoOption.GetPosition());
        if (distance < minDistance) {
            cloestOption = isoOption;
            minDistance = distance;
        }

        return cloestOption;
    }

    private Vector2 VectorWithLargestDotProduct((Vector2, float)[] vectorProductPairs) {
        float maxDotProduct = 0f;
        Vector2 resultingVector = Vector2.zero;
        foreach ((var vector, var product) in vectorProductPairs) {
            if (product > maxDotProduct) {
                maxDotProduct = product;
                resultingVector = vector;
            }
        }
        return resultingVector;
    }

    private abstract class Option {
        public enum Type {
            Mode,
            Shutter,
            Aperture,
            ISO
        }

        abstract public Type ParameterType { get; }

        abstract public Vector3 GetPosition();

        abstract public void SwitchToLeftValue(float byAmount);

        abstract public void SwitchToRightValue(float byAmount);
    }

    class ShutterOption: Option {
        private readonly Slider slider;
        private readonly Camera camera;
        private readonly float adjustmentSpeed = 1f;

        private float GetShutterSpeedFromSliderValue() {
            return slider.value / 1000;
        }

        override public Type ParameterType { get { return Type.Shutter; } }

        public ShutterOption(Slider slider, Camera camera) {
            this.slider = slider;
            this.camera = camera;
        }

        public override Vector3 GetPosition() {
            return slider.transform.parent.transform.position;
        }

        public override void SwitchToLeftValue(float byAmount) {
            if (slider.value >= 0) {
                slider.value -= byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetShutterSpeedFromSliderValue();
        }

        public override void SwitchToRightValue(float byAmount) {
            if (slider.value <= 1) {
                slider.value += byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetShutterSpeedFromSliderValue();
        }
    }

    class ApertureOption: Option {
        private readonly Slider slider;
        private readonly Camera camera;
        private readonly float adjustmentSpeed = 1f;

        private float GetApertureFromSliderValue() {
            // TODO: Update this formula. 
            return slider.value / 1000;
        }

        override public Type ParameterType { get { return Type.Aperture; } }

        public ApertureOption(Slider slider, Camera camera) {
            this.slider = slider;
            this.camera = camera;
        }

        public override Vector3 GetPosition() {
            return slider.transform.parent.transform.position;
        }

        public override void SwitchToLeftValue(float byAmount) {
            if (slider.value >= 0) {
                slider.value -= byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetApertureFromSliderValue();
        }

        public override void SwitchToRightValue(float byAmount) {
            if (slider.value <= 1) {
                slider.value += byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetApertureFromSliderValue();
        }
    }

    class ISOOption: Option {
        private readonly Slider slider;
        private readonly Camera camera;
        private readonly float adjustmentSpeed = 1f;

        private int GetISOFromSliderValue() {
            // TODO: Update this formula. 
            return (int)(slider.value * 1000);
        }

        override public Type ParameterType { get { return Type.ISO; } }

        public ISOOption(Slider slider, Camera camera) {
            this.slider = slider;
            this.camera = camera;
        }

        public override Vector3 GetPosition() {
            return slider.transform.parent.transform.position;
        }

        public override void SwitchToLeftValue(float byAmount) {
            if (slider.value >= 0) {
                slider.value -= byAmount * adjustmentSpeed;
            }
            camera.iso = GetISOFromSliderValue();
        }

        public override void SwitchToRightValue(float byAmount) {
            if (slider.value <= 1) {
                slider.value += byAmount * adjustmentSpeed;
            }
            camera.iso = GetISOFromSliderValue();
        }
    }
}
