using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBrowserBe : MonoBehaviour {

    private float _mouseClickTime;
    private int _mouseClickCount;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
	    {
	        _mouseClickCount++;
	        if (_mouseClickCount == 1)
	        {
	            _mouseClickTime = Time.time;
	        }

	        if (Time.time - _mouseClickTime <= 0.5f)
	        {
	            if (_mouseClickCount >= 4)
	            {
	                SceneManager.LoadScene("SceneBrowser", LoadSceneMode.Additive);
	                Input.ResetInputAxes();
	            }
	        }
	        else
	        {
	            _mouseClickCount = 0;
	        }
	    }
    }
}
