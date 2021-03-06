﻿using UnityEngine;
using System.Collections;

public class Hands : MonoBehaviour {

	// Use this for initialization
	[HideInInspector]
	public Hand left, right;
	public float maxAngle = 90;
	public float radiusA = 2;//This is the horizontal axis and should be larger
	public float radiusB = 1;
	
	void Start () {
		left = transform.GetChild(0).GetComponent<Hand>();
		left.side = "left";
		right = transform.GetChild(1).GetComponent<Hand>();
		right.side = "right";
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//If hands has no weapons, show default hands
		left.checkEmpty();
		right.checkEmpty();
		updateHands(mousePos);
	}


	public void updateHands(Vector3 mousePos) {
		Vector3 relativePos = mousePos - transform.position;
		float angle = Vector2.Angle(Vector2.right, relativePos);
		if (relativePos.y < 0) {
			angle = 360 - angle;
		}
		updateDrawOrder(angle);
		Vector2[] pos = HandsAt(angle);
		left.transform.localPosition = pos[0];
		//left.GetComponent<Weapon>().updateSprite(angle);
		right.transform.localPosition = pos[1];
		left.updateSprite(angle);
		right.updateSprite(angle);
	}
	//Returns a point on the ellipse given an angle. Zero starts from right and goes anticlock
	public Vector2 PointAt(float deg) {
		//Convert deg to rad
		float rad = deg * Mathf.Deg2Rad;
		float radialLength = radiusA*radiusB / (Mathf.Sqrt(Mathf.Pow(radiusB*Mathf.Cos(rad), 2) + Mathf.Pow(radiusA*Mathf.Sin(rad), 2)));
		return new Vector2(radialLength*Mathf.Cos(rad) , radialLength*Mathf.Sin(rad));
	}

	public Vector2[] HandsAt(float deg) {
		float rad = deg * Mathf.Deg2Rad;
		Vector2[] output = new Vector2[2]; //[Left Hand, Right Hand]
		for (int i = 0; i < 2; i++) {
			//Find angle away from center point
			float halfAngle = Mathf.Abs(Mathf.Sin(rad)) * maxAngle/ 2;
			//output[i] = PointAt(rad + 0.1f*(-1 + i*2));
			output[i] = PointAt(deg + halfAngle*(1+i*-2));
		}
		return output;

	}

	public void updateDrawOrder(float angle) {
		if (angle > 45 && angle <= 135) {//UP
			//Draw below body, below weapons
			left.GetComponent<SpriteRenderer>().sortingOrder = 3;
			right.GetComponent<SpriteRenderer>().sortingOrder = 3;
		} else if (angle > 135 && angle <= 225) {//LEFT
			//Left above right, left gun is sandwiched
			left.GetComponent<SpriteRenderer>().sortingOrder = 4;
			right.GetComponent<SpriteRenderer>().sortingOrder = 2;
		} else if (angle > 225 && angle <= 315) {//DOWN
			//Hands above body
			left.GetComponent<SpriteRenderer>().sortingOrder = 6;
			right.GetComponent<SpriteRenderer>().sortingOrder = 6;			
		} else {//RIGHT
			left.GetComponent<SpriteRenderer>().sortingOrder = 2;
			right.GetComponent<SpriteRenderer>().sortingOrder = 4;
		}
	}
}
