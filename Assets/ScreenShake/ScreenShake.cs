using Unity.VisualScripting;
using UnityEngine;

/*
 * ScreenShake.cs
 * 
 * Author: Joel Harman
 * Version: 1.0
 * Date: 26 August 2024
 * 
 * Description:
 * 
 * This script provides a simple screen shake effect that can be easily added to any camera in Unity. The shake effect 
 * is adjustable in terms of magnitude and falloff, allowing for customizable intensity and duration. 
 * The script ensures that the camera's original position is restored after the shake effect is applied.
 * 
 * Key Features:
 * - Apply screen shake to any camera in the scene.
 * - Adjust the magnitude (intensity) of the shake.
 * - Control the falloff (decay rate) of the shake over time.
 * - Restore the camera's position after rendering.
 * 
 * How to Use:
 * 
 * 1. Attach this script to the camera you want to apply the shake effect to, or call it programmatically.
 * 
 * 2. To add a shake effect, use the following static method:
 *    `ScreenShake.AddShake(float magnitude = 1f, Camera cameraToShake = null);`
 *    Example: 
 *      ScreenShake.AddShake(2.0f); // This will add a shake with magnitude 2 to the main camera.
 * 
 * 3. To set the shake with a specific magnitude and falloff:
 *    `ScreenShake.SetShake(float magnitude = 1f, float falloff = 1.0f, Camera cameraToShake = null);`
 *    Example:
 *      ScreenShake.SetShake(3.0f, 0.5f); // Sets a shake with magnitude 3 and a falloff rate of 0.5 on the main camera.
 * 
 * 4. To stop the shake effect immediately:
 *    `ScreenShake.StopShake(Camera camera);`
 *    Example:
 *      ScreenShake.StopShake(Camera.main); // Stops the shake on the main camera.
 * 
 * 5. You can also adjust the falloff independently:
 *    `ScreenShake.SetFalloff(float falloff = 1f, Camera camera = null);`
 *    Example:
 *      ScreenShake.SetFalloff(0.2f); // Sets the falloff rate to 0.2 on the main camera.
 * 
 * This script is versatile and can be used in any scenario where you need to apply a shake effect, such as during 
 * explosions, heavy impacts, or other intense moments in your game.
 * 
 * Note:
 * Ensure that there is a main camera in the scene or specify the camera to shake manually. The script will warn you 
 * if no camera is found.
 * 
 */
namespace DeveloperToolbox
{
    public class ScreenShake : MonoBehaviour
    {
        public static ScreenShake Instance { get; private set; }

        private Vector3 originalPosition;
        public float magnitude;
        private float falloff = 5.0f;

        private const float MagnitudeMod = 0.1f;
        private const float FalloffMod = 2.0f;
        private const float NoiseOffsetSpeed = 30.0f;
        private Vector2 noiseOffset;
        private static bool useAdvancedShake = true;

        public static void AddShake(float magnitude = 1f, Camera cameraToShake = null)
        {
            var screenShake = GetScreenShaker(cameraToShake);
            screenShake?.AddShake(magnitude);
        }

        public static void SetShake(float magnitude = 1f, float falloff = 1.0f, Camera cameraToShake = null)
        {
            var screenShake = GetScreenShaker(cameraToShake);
            screenShake?.SetShake(magnitude, falloff);
        }

        public static void StopShake(Camera camera = null)
        {
            var screenShake = camera.GetOrAddComponent<ScreenShake>();
            screenShake?.SetShake(0);
        }

        public static void SetFalloff(float falloff = 1f, Camera camera = null)
        {
            var screenShake = GetScreenShaker(camera);
            screenShake?.SetFalloff(falloff);
        }

        private static ScreenShake GetScreenShaker(Camera camera)
        {
            camera ??= Camera.main;

            if (camera == null)
            {
                Debug.LogWarning("No main camera has been included in the scene. Place one or manually specify the camera to shake.");
                return null;
            }

            return camera.GetOrAddComponent<ScreenShake>();
        }

        public void SetShake(float magnitude = 1, float falloff = 1.0f)
        {
            this.magnitude = magnitude * MagnitudeMod;
            this.falloff = falloff;
        }

        public void AddShake(float magnitude)
        {
            this.magnitude += magnitude * MagnitudeMod;
        }

        public void SetFalloff(float falloff)
        {
            this.falloff = falloff;
        }

        private void LateUpdate()
        {
            originalPosition = transform.localPosition;
            magnitude = Mathf.Lerp(magnitude, 0, FalloffMod * falloff * Time.deltaTime);

            if (useAdvancedShake)
            {
                noiseOffset += new Vector2(NoiseOffsetSpeed, NoiseOffsetSpeed) * Time.deltaTime;
                float offsetX = (Mathf.PerlinNoise(noiseOffset.x, 0) - 0.5f) * 2f * magnitude;
                float offsetY = (Mathf.PerlinNoise(0, noiseOffset.y) - 0.5f) * 2f * magnitude;
                transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            }
            else
            {
                transform.localPosition = originalPosition +
                                      transform.right * Random.Range(-magnitude, magnitude) +
                                      transform.up * Random.Range(-magnitude, magnitude);
            }
        }

        private void OnPostRender()
        {
            transform.localPosition = originalPosition;
        }
    }
}