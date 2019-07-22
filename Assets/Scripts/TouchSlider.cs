﻿using Enumerable = System.Linq.Enumerable;

namespace IMRE.EmbodiedUserInput
{
    public class TouchSlider : UnityEngine.MonoBehaviour
    {
        /// <summary>
        ///     internal percentage of slider
        /// </summary>
        [UnityEngine.RangeAttribute(0, 1)] private float _sliderValue;

        /// <summary>
        ///     The point for visual representation on the line
        /// </summary>
        public UnityEngine.Transform point;

        public UnityEngine.Material slidermaterial;

        /// <summary>
        ///     Max distance for finger to point interaction to trigger
        /// </summary>
        public float tolerance = 0.2f;

        /// <summary>
        ///     Slider line renderer
        /// </summary>
        private UnityEngine.LineRenderer tSlider;

		/// <summary>
		/// internal percentage of slider
		/// </summary>
		[Range(0, 1)] private float _sliderValue;

        public Unity.Mathematics.float3 tSliderEndA;
        public Unity.Mathematics.float3 tSliderEndB;

		public static TouchSlider ins;

		/// <summary>
		/// Field for value to be used to determine what percentage the slider currently represents.
		/// </summary>
		public float SliderValue
		{
			get => _sliderValue;
			set
			{
				if (value > 1f)
				{
					value = 1f;
				}else if (value < 0f)
				{
					value = 0f;
				}
				point.position = value * (tSliderEndB - tSliderEndA) + tSliderEndA;
				_sliderValue = value;
			}
		}



		void Start()
		{
			SliderValue = 0f;
			//make our instance of line renderer equal to the component line renderer on the same Unity
			//game object as the script
			tSlider = gameObject.AddComponent<LineRenderer>();
			tSlider.SetPosition(0, tSliderEndA);
			tSlider.SetPosition(1, tSliderEndB);
			tSlider.startWidth = .05f;
			tSlider.endWidth = .05f;
			tSlider.useWorldSpace = true;
			tSlider.material = slidermaterial;
			point = Instantiate(point);
			//make sure the point aligns with the current position of the line
			ins = this;
		}

		private void Update()
		{
			tSlider.SetPosition(0, tSliderEndA);
			tSlider.SetPosition(1, tSliderEndB);

			EmbodiedUserInputClassifierAuthoring.classifiers.ToList().Where(c => c.type == type)
				.ToList().ForEach(classifier => checkClassifier(classifier));
			
			//Where(classifier => classifier.isEligible && classifier.type == type).
			//	ToList().
		}

		//these are kinda like function calls. They do the math and grab the data at the frame which they are
		//used due to the lambda operator (=>)
		//In this case we look at the Magnitude of a finger position with the position of the line subtracted
		//This will let us know the distance from the finger tip to the line
		//public float leftFingerMag => Vector3.Magnitude((Vector3) LeftFingerPos - transform.position);
		//public float rightFingerMag => Vector3.Magnitude((Vector3) RightFingerPos - transform.position);

		private void checkClassifier(EmbodiedClassifier classifier)
		{
			if (classifier.isEligible)
			{

				//Notice that this code is totally generic and can be used for any classifier - doesn't need to be pinch.
				//think lego blocks.   
				float3 tSliderProjection =
					Vector3.Project(classifier.origin - tSliderEndA, tSliderEndB - tSliderEndA) 
					+ (Vector3) tSliderEndA;
				if (Vector3.Magnitude(tSliderProjection - classifier.origin) < tolerance)
				{

					SliderValue = Vector3.Magnitude(Vector3.Project(classifier.origin - tSliderEndA, 
						                                tSliderEndB - tSliderEndA)) /
					              Vector3.Magnitude(tSliderEndB - tSliderEndA);
				}
				else
				{
					Debug.Log(Vector3.Magnitude(tSliderProjection - classifier.origin));
				}
			}
		}
	}
}
