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
    
    // When animating
    private Option targetOption = null;
    [SerializeField] private float animationSpeed = 5f;

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

        Option optionTargetedByIntentionVector = OptionTargeted(intentionVector);

        // Adjusting values
        if (intentionVector == Vector2.left) {
            activeOption.SwitchToLeftValue(adjustmentVector.magnitude * Time.deltaTime);
        } else if (intentionVector == Vector2.right) {
            activeOption.SwitchToRightValue(adjustmentVector.magnitude * Time.deltaTime);
        } 
        
        // Set target option if intention has updated
        // The target would either be null or another value initially
        else if (optionTargetedByIntentionVector != targetOption) {
            bool isSelectingOption = intentionVector == Vector2.up || intentionVector == Vector2.down;
            if (isSelectingOption && adjustmentVector.magnitude >= 0.3){
                targetOption = optionTargetedByIntentionVector;
            }
        }
        
        if (targetOption != null) {
            // Keep completing the animation and set the active option when done
            Transform currentTransform = selectionFrame.transform;
            Vector3 targetPosition = new(
                currentTransform.position.x, 
                targetOption.GetTransformY(), 
                currentTransform.position.z
            );
            
            selectionFrame.transform.position = Vector3.Lerp(
                selectionFrame.transform.position, 
                targetPosition, 
                Time.deltaTime * animationSpeed
            );

            if (selectionFrame.transform.position == targetPosition) {
                activeOption = targetOption;
                targetOption = null;
            }
        }
    }

    private Option OptionTargeted(Vector2 byVector) {
        if (byVector == Vector2.up) {
            if (activeOption.ParameterType == Option.Type.Shutter) {
                return null;
            } else if (activeOption.ParameterType == Option.Type.Aperture) {
                return shutterOption;
            } else if (activeOption.ParameterType == Option.Type.ISO) {
                return apertureOption;
            }
        } else if (byVector == Vector2.down) {
            if (activeOption.ParameterType == Option.Type.Shutter) {
                return apertureOption;
            } else if (activeOption.ParameterType == Option.Type.Aperture) {
                return isoOption;
            } else if (activeOption.ParameterType == Option.Type.ISO) {
                return null;
            }
        }

        return null;
    }

    private Vector2 VectorWithLargestDotProduct((Vector2, float)[] vectorProductPairs) {
        float maxDotProduct = float.MinValue;
        Vector2 resultingVector = Vector2.zero;
        foreach ((var vector, var product) in vectorProductPairs) {
            //Debug.Log("Comparing:");
            //Debug.Log(product);
            //Debug.Log(maxDotProduct);
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

        abstract public float GetTransformY();

        abstract public void SwitchToLeftValue(float byAmount);

        abstract public void SwitchToRightValue(float byAmount);
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

        public override float GetTransformY() {
            return slider.transform.position.y;
        }

        public override void SwitchToLeftValue(float byAmount) {
            if (slider.value >= 0) {
                slider.value -= byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetShutterSpeedFromSliderValue();
        }

        public override void SwitchToRightValue(float byAmount) {
            if (slider.value <= 1) {
                slider.value -= byAmount * adjustmentSpeed;
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

        public override float GetTransformY() {
            return slider.transform.position.y;
        }

        public override void SwitchToLeftValue(float byAmount) {
            if (slider.value >= 0) {
                slider.value -= byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetApertureFromSliderValue();
        }

        public override void SwitchToRightValue(float byAmount) {
            if (slider.value <= 1) {
                slider.value -= byAmount * adjustmentSpeed;
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

        public override float GetTransformY() {
            return slider.transform.position.y;
        }

        public override void SwitchToLeftValue(float byAmount) {
            if (slider.value >= 0) {
                slider.value -= byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetISOFromSliderValue();
        }

        public override void SwitchToRightValue(float byAmount) {
            if (slider.value <= 1) {
                slider.value -= byAmount * adjustmentSpeed;
            }
            camera.shutterSpeed = GetISOFromSliderValue();
        }
    }
}
