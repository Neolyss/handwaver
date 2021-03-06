/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;



namespace IMRE.HandWaver
{
	/// <summary>
	/// This script does ___.
	/// The main contributor(s) to this script is __
	/// Status: ???
	/// </summary>
	public class debugkeyboardinput : MonoBehaviour//sorry
    {
        private bool unloadBool;
		public bool interalBuild = true;

        public bool loadBackground = true;
        public string backgroundName = "darkPrototype";
		public static bool autoLoadPlaintains = false;
		public Transform toolsObject;

		/// <summary>
		/// Set this in the editor to load a set of scenes on start.
		/// this will disable keyboard input.
		/// </summary>
		public List<string> loadScenesOnStart = new List<string>();

		public static bool PointToSelectEnabled = false;


		void Start()
		{

#if StandaloneWindows64
			commandLineArgumentParse.logOverride |= interalBuild;   //or equal the internal build bool so that if its internal it automatically starts logging
			#endif

			if (loadBackground)
				loadSceneAsyncByName(backgroundName, false);
//#if !UNITY_EDITOR
//			if (interalBuild)
//			{
//				Display.displays[0].Activate();
//				Display.displays[1].Activate();
//				FindObjectOfType<HWMixcastIO>().currMode = mixCastTargetMode.primaryAlt;
//			}
//#endif
			autoLoadPlaintains = (loadScenesOnStart.Count == 0 && !(SceneManager.GetSceneAt(0).name == "ThreeTorus"));
			PointToSelectEnabled = !loadScenesOnStart.Contains("LatticeLand");
			Debug.Log("TOOLS: " + autoLoadPlaintains);
			Debug.Log("PointSELECT: " + PointToSelectEnabled);
			foreach (string name in loadScenesOnStart)
			{
				loadSceneAsyncByName(name, unloadBool);
			}
			if (autoLoadPlaintains)
			{
				StartCoroutine(enablePlaintains());
			}
		}

		private void loadSceneByName(string scene)
		{
			SceneManager.LoadScene(scene);
		}

		private void demoToggle()
		{
			playMode.demo = !playMode.demo;
            resetCurrentScenes();
		}


        public static void loadNewBaseScene(string SceneName)
        {
            removeAllLayers();
            loadSceneAsyncByName(SceneName, false);
        }

        public static void loadSceneAsyncByName(string SceneName)
        {
            loadSceneAsyncByName(SceneName, false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SceneName">Name of Scene to Load</param>
        /// <param name="unloadOther">Whether or not to unload other current Scenes</param>
        public static void loadSceneAsyncByName(string SceneName, bool unloadOther)
        {
            //Debug.Log(SceneName +" attempted to load.");
            if (unloadOther)
            {
                List<Scene> layers = new List<Scene>();
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {

					if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
					{
						layers.Add(SceneManager.GetSceneAt(i));
					}
				}
                foreach (Scene s in layers)
                {
                    SceneManager.UnloadSceneAsync(s);
                }
            }
			SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

		}

		public static void removeAllLayers()
		{

			List<Scene> layers = new List<Scene>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{

				if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
					layers.Add(SceneManager.GetSceneAt(i));
			}
			foreach (Scene s in layers)
			{
				SceneManager.UnloadSceneAsync(s);
			}
		}


        /// <summary>
        /// Reloads base scene then all scenes that do not contain the keyword base.
        /// </summary>
        public static void resetCurrentScenes() {
            List<Scene> layers = new List<Scene>();
            Scene baseLayer = new Scene();
#region Get Lists of Current Scenes
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
                    layers.Add(SceneManager.GetSceneAt(i));
                else
                    baseLayer = SceneManager.GetSceneAt(i);
            }
#endregion
#region Reload Base & Layers
			
            SceneManager.LoadScene(baseLayer.name, LoadSceneMode.Single);
			foreach (Scene s in layers)
            {
                SceneManager.LoadSceneAsync(s.name, LoadSceneMode.Additive);
            }
#endregion

        }

        public static void reloadCurrentScenes()
        {
            List<Scene> layers = new List<Scene>();
            Scene baseLayer = new Scene();
#region Get Lists of Current Scenes
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (!(SceneManager.GetSceneAt(i).name.Contains("Base")))
                    layers.Add(SceneManager.GetSceneAt(i));
                else
                    baseLayer = SceneManager.GetSceneAt(i);
            }
#endregion
#region Reload Base & Layers
            SceneManager.LoadScene(baseLayer.name, LoadSceneMode.Single);
            if (layers.Count == 0)
                return;
            foreach (Scene s in layers)
            {
                SceneManager.LoadSceneAsync(s.name, LoadSceneMode.Additive);
            }
#endregion

        }

        private void toggleMixCastCamera()
        {
			#if StandaloneWindows64
            mixCastCameraToggle.toggleCameraz();
			#endif
        }

		IEnumerator enablePlaintains()
		{
			yield return new WaitForSeconds(2f);
			toolsObject.gameObject.SetActive(true);
		}
	}
}
