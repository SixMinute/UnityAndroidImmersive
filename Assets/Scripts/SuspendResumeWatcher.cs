using UnityEngine;
using System.Collections;
using System;

public class SuspendResumeWatcher : MonoBehaviour
{

	void OnApplicationPause(bool isPaused)
	{
		Debug.Log("APP: SUSPEND/RESUME, isPaused: " + isPaused);
		
		if( !isPaused )
		{
			ensureAndroidFullscreen();
		}
	}
	
	private void ensureAndroidFullscreen()
	{
		#if UNITY_ANDROID
		
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}

		try
		{
			//        import android.os.Build$VERSION;
			//        import android.view.View;
			//        import android.R$id;
			
			int sdkInt = new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
			
			if (sdkInt >= 19)
			{ //KITKAT
				AndroidJavaClass cView = new AndroidJavaClass("android.view.View");
				AndroidJavaObject oAct = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

				oAct.Call<AndroidJavaObject>
					(
						"findViewById",
						new AndroidJavaClass("android.R$id").GetStatic<int>("content")
					)
					.Call
					(
						"setSystemUiVisibility",
						cView.GetStatic<int>("SYSTEM_UI_FLAG_LAYOUT_STABLE") |
						cView.GetStatic<int>("SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION") |
						cView.GetStatic<int>("SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN") |
						cView.GetStatic<int>("SYSTEM_UI_FLAG_HIDE_NAVIGATION") |
						cView.GetStatic<int>("SYSTEM_UI_FLAG_FULLSCREEN") |
						cView.GetStatic<int>("SYSTEM_UI_FLAG_IMMERSIVE_STICKY")
					);
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
		
		#endif
	}
}
