using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : MonoBehaviour {

//	public float scale 			= 0.11f;	// 10.0f;
//	public float speed 			= 1.0f;		//1.0f;
//	public float noiseStrength 	= 0.36f;	//4.0f;
//	public float noiseWalk 		= 4.55f;	//1f;


//	public float Wavelength = 3;
//	public float Amplitude = 0.1f;
//	public float Speed = 1.82f;

    private Mesh mesh;
	private Material material;
    private Vector3[] vertexAry;
	private int planeX;
	private int planeY;


//	private float[] heightMapPre;
//	private float[] heightMapCur;
//	public float DRAG = 0.15f;
//	private float start = 10.0f;


	private Vector2 startPos;

	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        vertexAry = new Vector3[mesh.vertices.Length];

		material = GetComponent<Renderer> ().material;


		var createPlane = transform.parent.GetComponent<CreatePlane> ();
		planeX = createPlane.lengthX;
		planeY = createPlane.lengthY;

//		heightMapPre = new float[planeX * planeY];
//		heightMapCur = new float[planeX * planeY];

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown (0)) {
			mouseClick ();
		}


		vertexAry = mesh.vertices;

		//doRipple ();
		//randMove ();
		//swapRippleAry ();

        mesh.vertices = vertexAry;
        mesh.RecalculateNormals();
	}

	void randMove()
	{
		for (int y = 1; y < planeY - 1; y++) {
			for (int x = 1; x < planeX - 1; x++) {
				var v = vertexAry[y * planeX + x];

				//v.y = Mathf.Sin(Time.time * speed + v.x + v.y + v.z) * scale;
				//v.y += Mathf.PerlinNoise(v.x + noiseWalk, v.y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

				// plus ripple wave
//				v.y = heightMapCur[y * planeX + x];

				// sin ripple
				//v.y = rippleSin(new Vector2(x,y), startPos);
//				v.y = rippleSin(new Vector2(x,y), startPos);
//

				vertexAry[y * planeX + x] = v;
			}
		}




	}

	void doRipple()
	{
		/*
		for(int i = 1; i < planeY-1; i++)
		{
			for (int j = 1; j < planeX-1; j++) 
			{
				
//				float above 	= heightMapPre [(i + 1) * planeX + j];
//				float bottom 	= heightMapPre [(i - 1) * planeX + j];
//				float left 		= heightMapPre [i * planeX + j + 1];
//				float right 	= heightMapPre [i * planeX + j - 1];
				var value = (
					heightMapPre [(i + 1) * planeX + j] + 
					heightMapPre [(i - 1) * planeX + j]+ 
					heightMapPre [i * planeX + j + 1]+
					heightMapPre [i * planeX + j - 1]

				) / 2.0f;

				heightMapCur [i * planeX + j] = value - heightMapCur [i * planeX + j];

				//Drag (otherwise water never stop moving)
				heightMapCur[i * planeX + j] -= heightMapCur[i * planeX + j] * DRAG;	


			}
		}
*/
	}


	void swapRippleAry()
	{
		/*
		// copy current to pre
		for(int i = 0; i < planeY; i++)
		{
			for (int j = 0; j < planeX; j++) 
			{
				float temp = heightMapPre [i * planeX + j];
				heightMapPre [i * planeX + j] = heightMapCur [i * planeX + j];
				heightMapCur [i * planeX + j] = temp;
			}
		}
		*/
	}

	void mouseClick()
	{
//		heightMapCur [1*planeX + 1] = start;

//		heightMapCur [1 + planeX * (planeY/2)] = rippleSin(new Vector2(1, planeY/2), new Vector2(1, planeY/2));
//		heightMapCur [1 + planeX * (planeY/2 + 1)] = rippleSin(new Vector2(1, planeY/2 + 1), new Vector2(1, planeY/2));
//		heightMapCur [1 + planeX * (planeY/2 - 1)] = rippleSin(new Vector2(1, planeY/2 - 1), new Vector2(1, planeY/2));
//		heightMapCur [1 + 1 + planeX * (planeY/2)] = rippleSin(new Vector2(1 + 1, planeY/2), new Vector2(1, planeY/2));
//

//		startPos = new Vector2 (1, planeY/2);
//		for (int y = 20; y < 30; y++) {
//			for (int x = 1; x < 10; x++) {
//				// plus ripple wave
//				heightMapCur[y * planeX + x] = rippleSin(new Vector2(x,y), startPos);
//			}
//		}

		material.SetVector ("_StartPos", new Vector4(0.0f, 25.0f));

//		startPos = new Vector2 (0.0f, 25.0f);
	}

	float rippleSin(Vector2 offPos, Vector2 org)
	{
		/*
		float d = Vector2.Distance (offPos, org);
		float w = 2 / Wavelength;
		float miu = Speed * w;

		float t = Time.time;
		//float r = d * w - 4.21726f * miu;
		//float r = d * w - 8.365f * miu;
		float r = d * w -t * miu;
		float h = Amplitude * Mathf.Sin (r);
		return h;
		*/
		return 0;
	}

}
