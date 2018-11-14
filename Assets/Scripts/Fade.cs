using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    public Color fade;
    public float elapsedTime = 0f;
    public float fadeSpeed;

    void Start () {
		fade=this.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = new Color(fade.r, fade.g, fade.b, 1);
    }
	
    //Scene change fade
    public void FadeIn()
    {
        elapsedTime += Time.deltaTime;
        fade.a = Mathf.Clamp01(elapsedTime / fadeSpeed);
        this.GetComponent<SpriteRenderer>().color = new Color(fade.r, fade.g, fade.b, fade.a);
    }
    public void FadeOut()
    {
        elapsedTime += Time.deltaTime;
        fade.a = 1.0f- Mathf.Clamp01(elapsedTime / fadeSpeed);
        this.GetComponent<SpriteRenderer>().color = new Color(fade.r, fade.g, fade.b, fade.a);
    }
}
