﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace IMRE.HandWaver.HigherDimensions
{
/// <summary>
/// Net of hypercube for scale and dimension study
/// Projected to 3D
/// </summary>
	public class HypercubeNet : AbstractHigherDimSolid
    {

        private static Vector4 up = new Vector4(0, 1, 0, 0);
        private static Vector4 down = new Vector4(0, -1, 0, 0);
        private static Vector4 right = new Vector4(1, 0, 0, 0);
        private static Vector4 left = new Vector4(-1, 0, 0, 0);
        private static Vector4 forward = new Vector4(0, 0, 1, 0);
        private static Vector4 back = new Vector4(0, 0, -1, 0);
        private static Vector4 wForward = new Vector4(0, 0, 0, 1);
        private static Vector4 wBack = new Vector4(0, 0, 0, -1);

        private void Awake()
        {
            Fold = 0f;
        }

        private void FixedUpdate()
        {
            Fold++;
        }

        private float _fold;
        public float Fold
        {
            get
            {
                return _fold;
            }
            set
            {
                _fold = value;
                originalVerts = vertices(value).ToList();
            }
        }


        private static Vector4[] vertices(float t)
        {
            Vector4[] result = new Vector4[4 * 9];

            //core cube.  does not fold.

            result[0] = (up + right + forward)/2f;
            result[1] = (up + left + forward)/2f;
            result[2] = (up + left + back)/2f;
            result[3] = (up + right + back)/2f;

            result[4] = (down + right + forward) / 2f;
            result[5] = (down + left + forward) / 2f;
            result[6] = (down + left + back) / 2f;
            result[7] = (down + right + back) / 2f;

            //above up face.
            result[8] = result[0] + up.rotate(up, wForward, t);
            result[9] = result[1] + up.rotate(up, wForward, t);
            result[10] = result[2] + up.rotate(up, wForward, t);
            result[11] = result[3] + up.rotate(up, wForward, t);

            //below down face
            result[12] = result[4] + down.rotate(down, wForward, t);
            result[13] = result[5] + down.rotate(down, wForward, t);
            result[14] = result[6] + down.rotate(down, wForward, t);
            result[15] = result[7] + down.rotate(down, wForward, t);

            //right of right face;
            result[16] = result[0] + right.rotate(right, wForward, t);
            result[17] = result[3] + right.rotate(right, wForward, t);
            result[18] = result[7] + right.rotate(right, wForward, t);
            result[19] = result[4] + right.rotate(right, wForward, t);

            //left of left face
            result[20] = result[1] + left.rotate(left, wForward, t);
            result[21] = result[2] + left.rotate(left, wForward, t);
            result[22] = result[6] + left.rotate(left, wForward, t);
            result[23] = result[5] + left.rotate(left, wForward, t);

            //forward of forward face.
            result[24] = result[0] + forward.rotate(forward, wForward, t);
            result[25] = result[1] + forward.rotate(forward, wForward, t);
            result[26] = result[5] + forward.rotate(forward, wForward, t);
            result[27] = result[4] + forward.rotate(forward, wForward, t);

            //back of back face.
            result[28] = result[2] + back.rotate(back, wForward, t);
            result[29] = result[3] + back.rotate(back, wForward, t);
            result[30] = result[7] + back.rotate(back, wForward, t);
            result[31] = result[6] + back.rotate(back, wForward, t);

            //down of double down.
            result[32] = result[12] + down.rotate(down, wForward, t);
            result[33] = result[13] + down.rotate(down, wForward, t);
            result[34] = result[14] + down.rotate(down, wForward, t);
            result[35] = result[15] + down.rotate(down, wForward, t);

            return result;
        }

        private int[] _faces;

        private int[] faces { get
            {
                if (_faces == null) {
                    int[] result = new int[24+20*8];
                    cubeFaces(0, 1, 2, 3, 4, 5, 6, 7).CopyTo(result, 0);  //core
                    cubeFacesNoTop(0, 1, 2, 3,8, 9, 10, 11).CopyTo(result, 24 + 20 * 1); //up
                    cubeFacesNoTop(8, 9, 10, 11, 32, 33, 34, 35).CopyTo(result, 24 + 20 * 2); //down
                    cubeFacesNoTop(4, 5, 6, 7, 12, 13, 14, 15).CopyTo(result, 24 + 20 * 3); //down of down
                    cubeFacesNoTop(0, 3, 7, 4, 16, 17, 18, 19).CopyTo(result, 24 + 20 * 4);//right
                    cubeFacesNoTop(1, 2, 6, 5, 20, 21, 22, 23).CopyTo(result, 24 + 20 * 5); //left
                    cubeFacesNoTop(0, 1, 5, 4, 24, 25, 26, 27).CopyTo(result, 24 + 20 * 6); //forward
                    cubeFacesNoTop(2, 3, 7, 6, 28, 29, 30, 31).CopyTo(result, 24 + 20 * 7); //back
                    _faces = result;
                }
                return _faces;
            }
        }

        private int[] cubeFaces(int a0, int a1, int a2,int a3, int a4, int a5, int a6, int a7) {
            return new int[]
            {
            //core cube
            a0,a1,a2,a3,  //top
            a4,a5,a6,a7,  //bottom
            a0,a1,a5,a4,  //forward
            a2,a3,a7,a6,  //back
            a0,a3,a4,a7,  //right
            a1,a2,a6,a5,  //left
            };
        }
        private int[] cubeFacesNoTop(int a0, int a1, int a2, int a3, int a4, int a5, int a6, int a7)
        {
            return new int[]
            {
            //core cube
            //a0,a1,a2,a3,  //neglect top
            a4,a5,a6,a7,  //bottom
            a0,a1,a5,a4,  //forward
            a2,a3,a7,a6,  //back
            a0,a3,a4,a7,  //right
            a1,a2,a6,a5,  //left
            };
        }

        internal override void drawFigure()
        {
            mesh.Clear();
            verts = new List<Vector3>();
            tris = new List<int>();
            uvs = new List<Vector2>();

            int numFaces = faces.Length/4;

            for (int i = 0; i < numFaces; i++)
            {
                CreatePlane(rotatedVerts[faces[i * 4]], rotatedVerts[faces[i * 4 + 1]], rotatedVerts[faces[i * 4 + 2]], rotatedVerts[faces[i * 4 + 3]]);
            }
        }
    }
}