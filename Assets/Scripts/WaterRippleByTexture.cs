using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRippleByTexture : MonoBehaviour {

	public Texture2D WaveTexture;
	public GameObject PlaneRoot;

	private readonly int FPS 		= 30;
	private readonly int TotalClip 	= 36;
	private readonly int ClipWidth	= 128;
	private readonly int ClipHeight	= 128;
	private readonly int MaxWidthClip = 8;
	private Mesh mesh;
	private int planeX;
	private int planeY;
	//private List<Vector2> playWaveCenter;


	private bool doPlay = false;

	// Use this for initialization
	void Start () {
		
		mesh = GetComponent<MeshFilter>().mesh;

		var createPlane = transform.parent.GetComponent<CreatePlane> ();
		planeX = createPlane.lengthX;
		planeY = createPlane.lengthY;


		//vertexAry = new Vector3[mesh.vertices.Length];
		//material = GetComponent<Renderer> ().material;
		//		heightMapPre = new float[planeX * planeY];
		//		heightMapCur = new float[planeX * planeY];



	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			mouseClick ();
		}
	}



	IEnumerator PlayWave()
	{
		var verAry = mesh.vertices;
		var texData = WaveTexture.GetRawTextureData ();
		float rateX = WaveTexture.width / (float)planeX;
		float rateY = WaveTexture.height/ (float)planeY;

		for (int clip = 0; clip < 1; clip++) {
			for (int y = 0; y < planeY; y++) {
				for (int x = 0; x < planeX; x++) {

					Vector2 sPos;
					sPos.x = (TotalClip % MaxWidthClip) * ClipWidth;
					sPos.y = (TotalClip / MaxWidthClip) * ClipHeight;

					var basePos = (int)(sPos.x + x * rateX) + (int)(sPos.y + y * rateY) * WaveTexture.width;

					verAry [x + y * planeX].y = texData [basePos * 4];

				}
			}


			mesh.vertices = verAry;
			mesh.RecalculateNormals ();
		}

		doPlay = false;
		return null;
		//yield return new WaitForSeconds(5);
	}
		


	void mouseClick()
	{
		if (doPlay == false) {
			doPlay = true;

			PlayWave ();
		}
		//playWaveCenter.Add (Vector2 (planeX / 2, planeY / 2));
	}
}
