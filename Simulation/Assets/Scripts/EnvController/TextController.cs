using UnityEngine;
using System.Collections;

public class TextController : MonoBehaviour {

    private TextMesh tm;
    private double t;

	// Use this for initialization
	void Start () {
        tm = GetComponent<TextMesh>();
        Debug.Log(tm.text);
        Debug.Log("Text Mesh says its " + tm.text + "!");
	}

	// Update is called once per frame
	void Update () {
        // t += 0.1;
        // tm.text = t.ToString() + "˚C";
	}

    // Assumes the temperature is a formated string with ˚C appended to it
    void SetText (string  temperature ) {
        tm.text = temperature;
    }


}
