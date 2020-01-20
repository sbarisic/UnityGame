using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {
	public Transform[] backgrounds;  //BGs to be parallaxed
	private float[] parallaxScales;  //the proportion of the camera's movement ot move the background by
	public float smoothing = 1f;     //how smooth the parallax would be, must be > 0

	private Transform cam;           //main camera's transform
	private Vector3 previousCamPos;  //the position of the camera in the previous frame

	//great for references; called before Start()
	void Awake() {
		//main camera reference
		cam = Camera.main.transform;
	}

	// Start is called before the first frame update
	void Start() {
		previousCamPos = cam.position;

		//asigning corresponding parallaxScales
		parallaxScales = new float[backgrounds.Length];

		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}

	}

	// Update is called once per frame
	void Update() {
		//for each background
		for (int i = 0; i < backgrounds.Length; i++) {
			//the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			//set a target x position which is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			//create a target position which is the background current position with it's target X position
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			//fade between current position and the target position using lerp
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

		}
		//set the previousCamPos to the camera's position at the end of the frame
		previousCamPos = cam.position;
	}
}
