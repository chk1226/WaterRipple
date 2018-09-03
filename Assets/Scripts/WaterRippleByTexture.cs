using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRippleByTexture : MonoBehaviour {

	public Texture2D    WaveTexture;
	public GameObject   PlaneRoot;
    public float        Amplitude       = 1.5f;
    public Vector2      TitleWidth      = new Vector2(20, 20);

	private readonly int FPS 			= 30;
    private readonly float WaitTime     = 1 / 30;
    private readonly int TotalClip 		= 36;
	private readonly int ClipWidth		= 128;
	private readonly int ClipHeight		= 128;
	private readonly int MaxWidthClip 	= 8;
	private readonly int MaxHeightClip 	= 8;
    private readonly float OffsetColorValue = 0.623529f;



    private Mesh mesh;
	private int planeX;
	private int planeY;
    private Vector2 playWaveCenter;
    //private List<Vector2> playWaveCenter;
    private int step = 0;

	private bool doPlay = false;

	// Use this for initialization
	void Start () {

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

		switch (step) {
		case 0:
			mesh = PlaneRoot.GetComponentsInChildren<MeshFilter>()[0].mesh;
			var createPlane = PlaneRoot.GetComponent<CreatePlane> ();
			planeX = createPlane.lengthX;
			planeY = createPlane.lengthY;

			step++;
			break;
		}
	}



	IEnumerator PlayWave(Vector2 center)
	{
		var verAry = mesh.vertices;
        //var texData = WaveTexture.GetRawTextureData ();
        //Debug.Log ("texData size : " + texData.Length);
        //float rateX = WaveTexture.width / ((float)planeX * MaxWidthClip);
        //float rateY = WaveTexture.height/ ((float)planeY * MaxHeightClip);
        //float rateX = ClipWidth / (float)planeX ;
        //float rateY = ClipHeight / (float)planeY ;
        float rateX = ClipWidth / TitleWidth.x;
        float rateY = ClipHeight / TitleWidth.y;

        for (int clip = 0; clip < TotalClip; clip++) {
			Vector2 sPos;
			sPos.x = (clip % MaxWidthClip) * ClipWidth;
			sPos.y = (clip / MaxWidthClip) * ClipHeight;

            var texData = WaveTexture.GetPixels((int)sPos.x, (int)sPos.y, ClipWidth, ClipHeight);

            Vector2 pivot = new Vector2(center.x - (TitleWidth.x / 2), center.y - (TitleWidth.y / 2));
            for (int y = 0; y < TitleWidth.y; y++) {
				for (int x = 0; x < TitleWidth.x; x++) {

                    int nowX = (int)pivot.x + x;
                    if(nowX < 0 || nowX > planeX)
                    {
                        continue;
                    }

                    int nowY = (int)pivot.y + y;
                    if(nowY < 0 || nowY > planeY)
                    {
                        continue;
                    }


                    //Vector2 sPos;
                    //sPos.x = (clip % TotalClip) * ClipWidth;
                    //sPos.y = (clip / TotalClip) * ClipHeight;

                    //var basePos = (int)(sPos.x + x * rateX) + (int)(sPos.y + y * rateY) * WaveTexture.width;
                    var basePos = (int)(x * rateX) + (int)(y * rateY) * ClipWidth;


                    //var value = WaveTexture.getp;
                    //verAry [x + y * planeX].y = value;
                    var value = (texData[basePos].r) - OffsetColorValue;
                    verAry[nowX + nowY * planeX].y = value * Amplitude;

                    //Debug.Log (value);

                }
            }


			mesh.vertices = verAry;
			mesh.RecalculateNormals ();

            yield return new WaitForSeconds(WaitTime);
		}

		doPlay = false;
    }
		


	void mouseClick()
	{
		if (doPlay == false) {
			doPlay = true;
            playWaveCenter.x = planeY / 2;
            playWaveCenter.y = 0;

            StartCoroutine(PlayWave(playWaveCenter));
		}
		//playWaveCenter.Add (Vector2 (planeX / 2, planeY / 2));
	}
}
