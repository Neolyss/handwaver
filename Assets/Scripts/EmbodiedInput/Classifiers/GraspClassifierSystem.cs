﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Jobs;

namespace IMRE.EmbodiedUserInput
{
    public class GraspClassifierSystem : JobComponentSystem
    {
        public struct GraspClassifier : IEmbodiedClassifier<BodyInput>
        {
            public Chirality chirality;
            public bool shouldActivate(BodyInput data)
            {
                Hand hand = data.GetHand(chirality);
                return (hand.IsPinching);
            }

            public bool shouldCancel(BodyInput data)
            {
                return !shouldActivate(data);
            }

            public bool shouldFinish { get; set; }
            public bool isEligible { get; set; }
            public bool wasCancelled { get; set; }
            public bool wasFinished { get; set; }
            public bool wasActivated { get; set; }
        }

        /// <summary>
        /// A thin layer of general abstraction for one-handed and two-handed gestures.
        /// Inspired by LeapPaint https://github.com/leapmotion/Paint
        /// </summary>
        [BurstCompile]
        public struct EmbodiedUserInputClassifierJob : IJobForEach<BodyInput, GraspClassifier>
        {

            public void Execute([ReadOnly] ref BodyInput cBodyInput, ref GraspClassifier classifier)
            {
                if (!classifier.isEligible)
                {
                    classifier.isEligible = classifier.shouldActivate(cBodyInput);
                    classifier.wasActivated = classifier.isEligible;
                    classifier.wasCancelled = false;
                    classifier.wasFinished = false;
                }
                else
                {
                    classifier.wasCancelled = classifier.shouldCancel(cBodyInput);
                    classifier.wasFinished = classifier.shouldFinish && !classifier.wasCancelled;
                    classifier.isEligible = !(classifier.wasCancelled || classifier.wasFinished);
                    classifier.wasActivated = false;
                    classifier.shouldFinish = false;
                }
            }
        }

        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new EmbodiedUserInputClassifierJob
            {
                //cBodyInput = 
                //clasifier struct = static
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}
