﻿using IMRE.EmbodiedUserInput;

namespace IMRE.HandWaver.Space
{
public class PinTerminatorToggle : EmbodiedAction
    {
    		public override float desiredAngle = 90f;
    		
    		public override void pinFunction(RSDESPin pin){
			if(pin != null)
			{
				pin.toggleTerminator();
			}
		}
		
    		public override Unity.Mathematics.float3 pinDirection(RSDESPin pin){
    			return (Unity.Mathematics.float3) pin.contactPoint;
    		}
    }
}