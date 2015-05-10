/************************************************************************************

Filename    :   TimeWarp30HzSample.cs
Content     :   An example of how to set the Time Warp vsync rate
Created     :   June 27, 2014
Authors     :   Andrew Welch

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.


************************************************************************************/

using UnityEngine;
using System.Collections;					// required for Coroutines
using System.Runtime.InteropServices;		// required for DllImport

public class TimeWarp30HzSample : MonoBehaviour
{
	public OVRTimeWarpUtils.VsyncMode			targetFrameRate = OVRTimeWarpUtils.VsyncMode.VSYNC_30FPS;

	public Material								fps60Material = null;
	public Material								fps30Material = null;
	public Material								fps20Material = null;

	private Renderer							targetFpsRenderer = null;

#if (UNITY_ANDROID && !UNITY_EDITOR)
	[DllImport("OculusPlugin")]
	// Support to fix 60/30/20 FPS frame rate for consistency or power savings
	private static extern void OVR_TW_SetMinimumVsyncs( OVRTimeWarpUtils.VsyncMode mode );
#endif

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		targetFpsRenderer = GetComponentInChildren<Renderer>();
#if (UNITY_ANDROID && !UNITY_EDITOR)
		// delay one frame because OVRCameraController initializes TimeWarp in Start()
		Invoke("UpdateVSyncMode", 0.01666f);
#endif
	}
	
	/// <summary>
	/// Set the Time Warp vsync rate
	/// </summary>
	void UpdateVSyncMode()
	{
		// This is initialized in OVRCameraController.Start() to VSYNC_60FPS, however, you can override the setting
		// as desired to support Time Warp VSync at 60/30/20 FPS frame rates for consistency or power savings.
		// Time Warp at 30 FPS will also help app that can't consistently perform at 60 FPS
#if (UNITY_ANDROID && !UNITY_EDITOR)
		OVR_TW_SetMinimumVsyncs(targetFrameRate);
#endif

		// swap the material on the fps quad renderer
		switch(targetFrameRate)
		{
		case OVRTimeWarpUtils.VsyncMode.VSYNC_20FPS:
			targetFpsRenderer.sharedMaterial = fps20Material;
			//Application.targetFrameRate = 20;
			break;
		case OVRTimeWarpUtils.VsyncMode.VSYNC_30FPS:
			targetFpsRenderer.sharedMaterial = fps30Material;
			//Application.targetFrameRate = 30;
			break;
		case OVRTimeWarpUtils.VsyncMode.VSYNC_60FPS:
			targetFpsRenderer.sharedMaterial = fps60Material;
			//Application.targetFrameRate = 60;
			break;
		}
	}

	/// <summary>
	/// Check input and switch between target frame rates
	/// These samples use the default Unity Input Mappings with the addition of "Right Shoulder" and "Left Shoulder"
	/// </summary>
	void Update()
	{
		if (Input.GetButtonDown("Right Shoulder"))
		{
			//*************************
			// switch to the next mode
			//*************************
			switch(targetFrameRate)
			{
			case OVRTimeWarpUtils.VsyncMode.VSYNC_20FPS:
				targetFrameRate = OVRTimeWarpUtils.VsyncMode.VSYNC_30FPS;
				break;
			case OVRTimeWarpUtils.VsyncMode.VSYNC_30FPS:
				targetFrameRate = OVRTimeWarpUtils.VsyncMode.VSYNC_60FPS;
				break;
			case OVRTimeWarpUtils.VsyncMode.VSYNC_60FPS:
				targetFrameRate = OVRTimeWarpUtils.VsyncMode.VSYNC_20FPS;
				break;
			}
			UpdateVSyncMode();
		}
		else if (Input.GetButtonDown("Left Shoulder"))
		{
			//*************************
			// switch to the previous mode
			//*************************
			switch(targetFrameRate)
			{
			case OVRTimeWarpUtils.VsyncMode.VSYNC_20FPS:
				targetFrameRate = OVRTimeWarpUtils.VsyncMode.VSYNC_60FPS;
				break;
			case OVRTimeWarpUtils.VsyncMode.VSYNC_30FPS:
				targetFrameRate = OVRTimeWarpUtils.VsyncMode.VSYNC_20FPS;
				break;
			case OVRTimeWarpUtils.VsyncMode.VSYNC_60FPS:
				targetFrameRate = OVRTimeWarpUtils.VsyncMode.VSYNC_30FPS;
				break;
			}
			UpdateVSyncMode();
		}
	}
}
