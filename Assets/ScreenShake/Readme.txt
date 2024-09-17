The ScreenShake class provides some basic functionality for shaking the screen. The ScreenFade class provides some basic 
functionality for fading the screen. The script logic is in the 'DeveloperToolbox' namespace. Either add a 'using DeveloperToolbox;' 
include in your script, or reference the namespace explicitly (like below).


No setup is required for this to work. Note that for the below methods, the parameters are *optional*.

If you do not specify a camera, the shake will default to the Main Camera.
If you do not specify a magnitude or falloff, it will default to a 'reasonable' value.

To Add Shake: Call "DeveloperToolbox.ScreenShake.AddShake(magnitude, camera);"
To Set Shake (and override the existing shake): Call "DeveloperToolbox.ScreenShake.SetShake(magnitude, camera);"
To Stop Shake: Call "DeveloperToolbox.ScreenShake.StopShake(camera);"
To Set Falloff: Call "DeveloperToolbox.ScreenShake.SetFalloff(falloff, camera);"