/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueprintReality.MixCast;
using System.Linq;
using UnityEngine.Events;

namespace IMRE.HandWaver
{
	public enum mixCastTargetMode { primaryMonitor, secondaryMonitor, primaryAlt};

	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public static class commandLineArgumentParse 
	{
		public static string[] commandLineArguments = System.Environment.GetCommandLineArgs();
		private static bool _logOverride = false;
		public static UnityEvent logStateChange = new UnityEvent();

		public static bool logOverride
		{
			get
			{
				return _logOverride;
			}

			set
			{
				_logOverride = value;
				if(logStateChange.GetPersistentEventCount() != 0)
					logStateChange.Invoke();
			}
		}

		public static int monitorCountArgument()
		{
			int result = 1;

			foreach (string argument in commandLineArguments)
			{
				if (argument.ToLower().Contains("-dualmonitor"))
				{
					result = 2;
				}else if (argument.ToLower().Contains("-triplemonitor"))
				{
					result = 3;
				}
			}
			//this call of sceneStart() is placed here just so that its called on start of application; meaning no impact on the function.
			sceneStart();
			return result;
		}

		internal static void sceneStart()
		{
			foreach (string argument in commandLineArguments)
			{
                if (argument.ToLower().Contains("-handwaver"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadNewBaseScene("HandWaverBase");
                }

                if (argument.ToLower().Contains("-geoplanet"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadNewBaseScene("GeometersPlanetariumBase");
                }

                if (argument.ToLower().Contains("-chess3d"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadSceneAsyncByName("Chess3DLayer", false);
                }

                if (argument.ToLower().Contains("-tutorial"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadSceneAsyncByName("tutorialLayer", false);
                }

                if (argument.ToLower().Contains("-LittleBeartha"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadSceneAsyncByName("LittleBeartha", true);
                }

                if (argument.ToLower().Contains("-HorizonAnalysis"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadSceneAsyncByName("HorizonAnalysis", true);
                }

                if (argument.ToLower().Contains("-HigherDimensionsLayer"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadSceneAsyncByName("HigherDimensionsLayer", false);
                }

                if (argument.ToLower().Contains("-LatticeLand"))
                {
                    playMode.demo = true;
                    debugkeyboardinput.loadSceneAsyncByName("LatticeLand", false);
                }
            }
		}

		public static bool logCheck()
		{
			return (commandLineArguments.Any(c => c.ToLower().Contains("-logging")) || logOverride);
		}

		public static mixCastTargetMode mixCastTarget()
		{
			mixCastTargetMode result = mixCastTargetMode.primaryMonitor;

			foreach (string argument in commandLineArguments)
			{
				if (argument.ToLower().Contains("-mixcast2"))
				{
					result = mixCastTargetMode.secondaryMonitor;
				}else if (argument.ToLower().Contains("-mixcast1"))
				{
					result = mixCastTargetMode.primaryAlt;
				}
			}

			return result;
		}

	}
}