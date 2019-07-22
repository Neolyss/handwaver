/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IMRE.Chess3D {
    /// <summary>
    /// A static script for determining all of the valid moves for a given piece.
    /// </summary>
	public static class allValidMoves {

        /// <summary>
        /// A static function for determining the valid moves for a given piece.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static float3[] validMoves(AbstractPiece piece)
        {
            Vector3 orientation = Vector3.forward;
            if (piece.Team == currentTeam.black)
            {
                orientation = Vector3.back;
            }

            List<Vector3> friendly = new List<Vector3>();
            piece.myTeam().ForEach(p => friendly.Add(p.Location));
            
            List<Vector3> enemy = new List<Vector3>(); 
            piece.otherTeam().ForEach(p => enemy.Add(p.Location));
            
            return validMoves(piece.PieceType, orientation, piece.Location, friendly,enemy,orientation);
        }

       #region CalculateMoves
        private static float3[] validMoves(chessBoard.PieceType piece, Vector3 currentPosition, List<Vector3> friendlyPieces, List<Vector3> enemyPieces, Vector3 orientation)
        {
            //list all possible positions
            List<float3> caniditePositions = new List<float3>();
            switch (piece)
            {
                case chessBoard.PieceType.king:
                    //kings can move one unit through a face or an edge
                    
                    //moving through faces
                    caniditePositions.Add(currentPosition.moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveBack(orientation));
                    caniditePositions.Add(currentPosition.moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveUp(orientation));
                    caniditePositions.Add(currentPosition.moveDown(orientation));

                    //movign through edges 
                    //TODO remove redundancies (e.g. back + forward  == forward + back)
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveBack(orientation).moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveBack(orientation).moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveBack(orientation).moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveBack(orientation).moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveBack(orientation));
                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveRight(orientation).moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveRight(orientation).moveBack(orientation));
                    caniditePositions.Add(currentPosition.moveRight(orientation).moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveRight(orientation).moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveUp(orientation).moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveUp(orientation).moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveUp(orientation).moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveUp(orientation).moveBack(orientation));

                    caniditePositions.Add(currentPosition.moveDown(orientation).moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveDown(orientation).moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveDown(orientation).moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveDown(orientation).moveBack(orientation));

                    break;
                case chessBoard.PieceType.queen:
                    
                    //queens can move n units through a face or an edge

                    for (int i = 1; i < 8; i++)
                    {
                        //movign through faces
                        caniditePositions.Add(currentPosition.moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i));

                        //moving through edges 
                        //TODO remove redundancies (e.g. back + forward  == forward + back)
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveBack(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveBack(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveBack(orientation, i));

                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveBack(orientation, i));
                    }

                    break;
                case chessBoard.PieceType.rook:
                    //rooks can move n units through a face
                    
                    for (int i = 1; i < 8; i++)
                    {
                        //striaght in each direction
                        caniditePositions.Add(currentPosition.moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i));
                    }

                    break;
                case chessBoard.PieceType.bishop:
                    //diagonal in any direction
                    //TODO remove redundancies (e.g. back + forward  == forward + back)
                    for (int i = 1; i < 8; i++)
                    {
                        //moving through edges 
                        //TODO remove redundancies (e.g. back + forward  == forward + back)
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveForward(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveBack(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveBack(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveLeft(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveBack(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveDown(orientation, i));
                        caniditePositions.Add(currentPosition.moveRight(orientation, i).moveUp(orientation, i));

                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveUp(orientation, i).moveBack(orientation, i));

                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveLeft(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveRight(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveForward(orientation, i));
                        caniditePositions.Add(currentPosition.moveDown(orientation, i).moveBack(orientation, i));
                    }

                    break;
                case chessBoard.PieceType.knight:
                    //face then edge
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveForward(orientation)
                        .moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveForward(orientation)
                        .moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveForward(orientation)
                        .moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveForward(orientation).moveForward(orientation)
                        .moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveBack(orientation).moveBack(orientation)
                        .moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveBack(orientation).moveBack(orientation)
                        .moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveBack(orientation).moveBack(orientation)
                        .moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveBack(orientation).moveBack(orientation)
                        .moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveLeft(orientation)
                        .moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveLeft(orientation)
                        .moveBack(orientation));
                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveLeft(orientation)
                        .moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveLeft(orientation).moveLeft(orientation)
                        .moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveRight(orientation).moveRight(orientation)
                        .moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveRight(orientation).moveRight(orientation)
                        .moveBack(orientation));
                    caniditePositions.Add(currentPosition.moveRight(orientation).moveRight(orientation)
                        .moveDown(orientation));
                    caniditePositions.Add(currentPosition.moveRight(orientation).moveRight(orientation)
                        .moveUp(orientation));

                    caniditePositions.Add(currentPosition.moveUp(orientation).moveUp(orientation)
                        .moveLeft(orientation));
                    caniditePositions.Add(
                        currentPosition.moveUp(orientation).moveUp(orientation).moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveUp(orientation).moveUp(orientation)
                        .moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveUp(orientation).moveUp(orientation)
                        .moveBack(orientation));

                    caniditePositions.Add(currentPosition.moveDown(orientation).moveDown(orientation)
                        .moveLeft(orientation));
                    caniditePositions.Add(currentPosition.moveDown(orientation).moveDown(orientation)
                        .moveRight(orientation));
                    caniditePositions.Add(currentPosition.moveDown(orientation).moveDown(orientation)
                        .moveForward(orientation));
                    caniditePositions.Add(currentPosition.moveDown(orientation).moveDown(orientation)
                        .moveBack(orientation));

                    break;
                case chessBoard.PieceType.pawn:
                    //move through forward face
                    caniditePositions.Add(currentPosition.moveForward(orientation));
                    
                    //double move if in starting location.
                    //distance from starting position in forward direction
                    int dist = (int) Vector3.Project(currentPosition, orientation).magnitue;
                    if (dist == 2 || dist == 1)
                    {
                        caniditePositions.Add(currentPosition.moveForward(orientation, 2));
                    }

                    //attack on foward edges.  the pawn can attack on the forward diagionals when they are not blocked by another piece directly in fron of them.
                    if (!enemyPieces.contains(currentPosition.moveForward(orientation)))
                    {
                        List<Vector3> cPos = new List<Vector3>();
                        cPos.Add(currentPosition.moveForward(orientation).moveLeft(orientation));
                        cPos.Add(currentPosition.moveForward(orientation).moveRight(orientation));
                        cPos.Add(currentPosition.moveForward(orientation).moveDown(orientation));
                        cPos.Add(currentPosition.moveForward(orientation).moveUp(orientation));

                        //the canidate position must have an enemy piece to be used for an attack
                        //add those positions to the canidate positions.
                        cPos.Where(p => enemyPieces.contains(p)).ForEach(p => caniditePositions.Add(p));
                    }

                    break;
            }

            //check if off board
            caniditePositions = caniditePositions.Where(p => p.onBoard())
            
            //check if occupied by friendly
            caniditePositions = caniditePositions.Where(p => !friendlyPieces.Contains(p));
	       
	      //check if places self in check
	       caniditePositions = canidatePositions.Where(p => !placeSelfInCheck(piece, p));

            //queen, rook, bishop - check if passing through any piece
            if (piece == chessBoard.PieceType.queen || piece == chessBoard.PieceType.rook ||
                piece == chessBoard.PieceType.queen)
            {
                caniditePositions =
                    caniditePositions.Where(p => !piecesInPath(currentPosition, p, friendlyPieces, enemyPieces));
            }
	       return canidatePositions.ToArray();
        }

        private static float3 moveDirection(float3 position, float3 orientation, float3 direction)
        {
            math.normalize(orientation);
            return position + Quaternion.FromTo(Vector3.forward, direction)*orientation;
        }
        //single unit movement.

        private static float3 moveForward(this float3 position, float3 orientation)
        {
            return moveDirection(position, orientation, Vector3.forward);
        }
        private static float3 moveBack(this float3 position, float3 orientation)
        {
            return moveDirection(position, orientation, Vector3.back);
        }
        private static float3 moveLeft(this float3 position, float3 orientation)
        {
            return moveDirection(position, orientation, Vector3.left);
        }
        private static float3 moveRight(this float3 position, float3 orientation)
        {
            return moveDirection(position, orientation, Vector3.right);
        }
        private static float3 MoveUp(this float3 position, float3 orientation)
        {
            return moveDirection(position, orientation, Vector3.up);
        }
        private static float3 MoveDown(this float3 position, float3 orientation)
        {
            return moveDirection(position, orientation, Vector3.down);
        }
        
        //multiple unit movement
        private static float3 moveForward(this float3 position, float3 orientation, int i)
        {
            return moveDirection(position, orientation, Vector3.forward);
        }
        private static float3 moveBack(this float3 position, float3 orientation, int i)
        {
            return moveDirection(position, orientation, Vector3.back);
        }
        private static float3 moveLeft(this float3 position, float3 orientation, int i)
        {
            return moveDirection(position, orientation, Vector3.left);
        }
        private static float3 moveRight(this float3 position, float3 orientation, int i)
        {
            return moveDirection(position, orientation, Vector3.right);
        }
        private static float3 MoveUp(this float3 position, float3 orientation, int i)
        {
            return moveDirection(position, orientation, Vector3.up);
        }
        private static float3 MoveDown(this float3 position, float3 orientation, int i)
        {
            return moveDirection(position, orientation, Vector3.down);
        }

        private static bool onBoard(this float3 position)
        {
            return p.x >= 0 && p.x <= 8 && p.y >= 0 && p.y <= 8 && p.z >= 0 && p.z <= 8;
        }

        private static bool PiecesInPath(float3 position, float3 destination, List<float3> friendly, List<float3> enemy)
        {
            bool testResult = false;
            direction = destination - position;
            length = (int) direction.magnitude;
            if (length == 1)
            {
                //assume that friendlies have already been ruled out.
                return false;
            }
            
            for (i = 1; i < length; i++)
            {
                checkAtPos = position + direction.normalized * i;
                testResult = testResult && friendly.Contains(checkAtPos) && enemy.Contains(checkAtPos);
            }

            return testResult;
        }
		
	public bool placeSelfInCheck(AbstractPiece piece, int3 attemptedMove)
        {
            List<AbstractPiece> listToCheck;
            if (piece.Team == currentTeam.black)
                listToCheck = whiteTeam;
            else
                listToCheck = blackTeam;
            foreach (AbstractPiece piece in listToCheck)
            {
                if (piece.IsValid(attemptedMove))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
