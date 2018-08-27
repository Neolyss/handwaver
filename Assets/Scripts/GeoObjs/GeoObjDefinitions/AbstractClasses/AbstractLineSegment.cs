/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	abstract class AbstractLineSegment : MasterGeoObj
    {
        public Vector3 vertex0;
        public Vector3 vertex1;

        private Vector3[] vertices = new Vector3[2];

        public Color startColor = Color.blue;
        public Color endColor = Color.blue;

        public float apothem = .01f;


        public override void initializefigure()
        {
            this.figType = GeoObjType.line;
            this.Position3 = (vertex0 + vertex1) / 2f;

            vertices[0] = vertex0;
            vertices[1] = vertex1;

            LineRenderer lineRenderer = this.GetComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
            lineRenderer.numCapVertices = 2;
            lineRenderer.SetPositions(vertices);

            CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
            collider.center = Vector3.zero;
            collider.height = .9f * Vector3.Magnitude(LocalPosition(vertex0) - LocalPosition(vertex1));
            collider.radius = Mathf.Min(.025f, collider.height);
			updateFigure();
        }


        public override void updateFigure()
        {
            Vector3[] vertices = new Vector3[2];
            vertices[0] = LocalPosition(vertex0);
            vertices[1] = LocalPosition(vertex1);

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(vertices);

            CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
            collider.height = .9f * Vector3.Magnitude(LocalPosition(vertex0) - LocalPosition(vertex1));
            collider.radius = Mathf.Min(.025f, collider.height);

			this.transform.rotation = Quaternion.FromToRotation(Vector3.right, LocalPosition(vertex1) - LocalPosition(vertex0));
        }
    }
}