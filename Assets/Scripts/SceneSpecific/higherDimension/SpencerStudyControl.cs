﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IMRE.HandWaver.HigherDimensions
{
    /// <summary>
    /// Central control for scale and dimension study.
    /// also includes logic to make a slider out of MasterGeoObjs
    /// </summary>
	public class SpencerStudyControl : MonoBehaviour
    {
    /// <summary>
    /// The number of degrees that each vertex is folded by.
    /// Consider changing to percent;
    /// </summary>        
    internal static float degreeFolded = 0f;
        /// <summary>
    /// An override that automatically animates the slider and the folding process
    /// </summary>
        public bool animateFold = false;

        /// <summary>
    /// A reference to the hypercube net object in the scene (3D --> 4D)
    /// </summary>
        public HypercubeNet hypercube;
	        /// <summary>
    /// A reference to the five cell net in the scene (3D --> 4D)
    /// </summary>
        public fiveCellNet fivecell;
	
	        /// <summary>
    /// A reference to the cube net in the scene (2D --> 3D)
    /// </summary>
        public cubeNet cube;
	
	        /// <summary>
    /// A reference to the tetrahedron net in the scene (2D --> 3D)
    /// </summary>
        public tetrahedronNet pyramid;
	        /// <summary>
    /// A reference to the square net in the scene (1D-->2D)
    /// </summary>
        public squareNet square;
	        /// <summary>
    /// A reference to the triangle net in the scene (1D --> 2D)
    /// </summary>
        public triangleNet triangle;
	
	        /// <summary>
    /// The point on the slider that determines the position of the slider.
    /// </summary>
	private InteractablePoint sliderPoint;
	        /// <summary>
    /// The bounds of the slider.
    /// </summary>
	private DependentLineSegment slider;

        /// <summary>
    /// A boolean for debugging that allows the fold to be manipulated in the editor at play
    /// </summary>
        public bool foldOverride;
	        /// <summary>
    /// The override value with a slider in the editor.
    /// </summary>
        [Range(0, 360)]
        public float foldOverrideValue = 0f;

        private void Start()
        {
	//construct a slider as a dependent linesegment, with points at Vector3.zero and Vector3.right.  
	//Add Vector3.up for height
	    slider = GeoObjConstruction.dLineSegment(GeoObjConstruction.dPoint(Vector3.zero+Vector3.up),GeoObjConstruction.dPoint(Vector3.right*.1f+Vector3.up));
	    //construct a point on the slider (in the middle)
	    //this point will be bound to the slider on update.
	    sliderPoint = GeoObjConstruction.iPoint(Vector3.right*.05f);
        }

        void Update()
        {
	float deg = 0f;
	//if the override bool is set, use in editor override value

            if (foldOverride)
            {
                deg = foldOverrideValue;
		//update the slider's position to reflect the override value
		sldiderPoint.Position3 = (degreeFolded/360f)*(slider.point2.Position3 - slider.point1.Position3) + slider.point1.Position3;

            }
	    //if the boolean is set to animate the figure
	    else if (animateFold)
            {
	    //increment the degree folded by one degree. 
                deg = degreeFolded + 1f;
		//update the slider's position to reflect the override value
		sldiderPoint.Position3 = (degreeFolded/360f)*(slider.point2.Position3 - slider.point1.Position3) + slider.point1.Position3;
            }
	    // if the participant is directly manipulating the slider
	    else
	    {
	    	sliderPoint.Position3 = Vector3.Project(sliderPoint.Position3 - slider.point1.Position3,slider.point1.Position3 - slider.point2.Position3) + slider.point1.Position3;
	    	deg =360*(sliderPoint.Position3 - slider.point1.Position3).magnitude/(slider.point1.Position3 - slider.point2.Position3).magnitude;
	    }
	    #if Photon
	   	 photonView.RPC("setDegreeFolded", PhotonTargets.All, deg);
	    #else
	   	 setDegreeFolded(deg);
	    #endif
        }
	
	[PunRPC]
	private void setDegreeFolded(float degree){
		degreeFolded = degree;
		//update each of the figures to reflect the degree folded.
	        hypercube.Fold = degreeFolded;
                fivecell.Fold = degreeFolded;
                cube.Fold = degreeFolded;
                pyramid.Fold = degreeFolded;
                square.Fold = degreeFolded;
                triangle.Fold = degreeFolded;
	}
    }
}
