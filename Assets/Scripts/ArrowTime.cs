using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTime : MonoBehaviour
{
    public Color DefaultColor = Color.green;
    public Color HighlightColor = Color.red;
    public float LerpSpeed = 0.03f;

    private Material _mat;

    private Bounce bouncy;

    // Start is called before the first frame update
    void Start()
    {
        bouncy = this.GetComponent<Bounce>();

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }

        _mat = renderer.material;

        DefaultColor = _mat.GetColor("_Color");

        StartCoroutine(UpdateColor());
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateColor());
    }

    private IEnumerator UpdateColor()
    {
        Color lerpedColor = Color.white;
        float currentTime = 0;

        while (this.enabled)
        {
            lerpedColor = Color.Lerp(DefaultColor, HighlightColor, currentTime += (Time.deltaTime * LerpSpeed / 1));
            if (bouncy != null)
            {
                if (bouncy.frequency < 6f)
                {
                    bouncy.frequency = ((currentTime += (Time.deltaTime * LerpSpeed / 1)) + 1) * 3;
                }
                else
                {
                    bouncy.frequency = 10f;
                }
            }

            if (_mat != null )
            {
                _mat.SetColor("_Color", lerpedColor);
            }
            yield return new WaitForEndOfFrame();
        }

        _mat.SetColor("_Color", DefaultColor);
    }

}
