using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace IMRE.Gestures
{
  public class LatticeLandEraseGesture : SwipeGesture {
    public bool DeactivationConditionsActionComplete(){
      return false;
    }

    public bool WhileGestureActive(Hand hand,InputDevice osvrController){
      return false;
    }
  }
}