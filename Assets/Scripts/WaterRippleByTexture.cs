using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WaveData
{
    public Vector2 Center  = Vector2.zero;
    public int NowClip     = 0;
    public bool DoPlay     = false;
}

public class WaterRippleByTexture : MonoBehaviour {

	public Texture2D    WaveTexture;
	public GameObject   PlaneRoot;
    public float        Amplitude           = 1.5f;
    public Vector2      TitleWidth          = new Vector2(20, 20);

	static private readonly int FPS 	    = 30;
    private readonly float WaitTime         = 1 / FPS;
    private readonly int TotalClip 		    = 36;
	private readonly int ClipWidth		    = 128;
	private readonly int ClipHeight		    = 128;
	private readonly int MaxWidthClip 	    = 8;
	private readonly int MaxHeightClip 	    = 8;
    private readonly float OffsetColorValue = 0.623529f;

    private Mesh topMesh;
    private Mesh sideMesh;
	private int planeX;
	private int planeY;
    //private Vector2 playWaveCenter;
    private List<WaveData> playWaveCenter = new List<WaveData>();
    private int step = 0;

	//private bool doPlay = false;

	// Use this for initialization
	void Start () {

		//vertexAry = new Vector3[mesh.vertices.Length];
		//material = GetComponent<Renderer> ().material;
		//		heightMapPre = new float[planeX * planeY];
		//		heightMapCur = new float[planeX * planeY];



	}

    void initPlayWaveList()
    {
        for (int i = 0; i < 5; i++ )
        {
            WaveData wave = new WaveData();
            wave.DoPlay = false;
            wave.NowClip = 0;
            playWaveCenter.Add(wave);
        }
    }

    WaveData getNotPlayWave()
    {
        for(int i = 0; i < playWaveCenter.Count; i++ )
        {
            if(playWaveCenter[i].DoPlay == false)
            {
                return playWaveCenter[i];
            }
        }

        return null;
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			mouseClick ();
		}

		switch (step) {
		case 0:
            initPlayWaveList();
            step++;
            break;
        case 1:
            var allMesh = PlaneRoot.GetComponentsInChildren<MeshFilter>();
            topMesh = allMesh[0].mesh;
            sideMesh = allMesh[1].mesh;
			var createPlane = PlaneRoot.GetComponent<CreatePlane> ();
			planeX = createPlane.lengthX;
			planeY = createPlane.lengthY;

			step++;
			break;
            
        case 2:
            StartCoroutine(playWaveUpdate());
            step++;
            break;
		}
	}

    IEnumerator playWaveUpdate()
    {
        while(true)
        {
            playWavePerFrame();
            yield return new WaitForSeconds(WaitTime);

        }
    }


	void playWavePerFrame()
	{
		var topVerAry = topMesh.vertices;
        var sideVerAry = sideMesh.vertices;
        //var texData = WaveTexture.GetRawTextureData ();
        //Debug.Log ("texData size : " + texData.Length);
        //float rateX = WaveTexture.width / ((float)planeX * MaxWidthClip);
        //float rateY = WaveTexture.height/ ((float)planeY * MaxHeightClip);
        //float rateX = ClipWidth / (float)planeX ;
        //float rateY = ClipHeight / (float)planeY ;
        float rateX = ClipWidth / TitleWidth.x;
        float rateY = ClipHeight / TitleWidth.y;

        //reset plane height
        for (int y = 0; y < planeY; y++)
        {
            for (int x = 0; x < planeX; x++)
            {
                topVerAry[x + y * planeX].y = 0;

                if(y == 0)
                {
                    sideVerAry[x].z = 0;
                }
            }
        }
        //sideVerAry[planeX].z = 0;

        for (int i = 0; i < playWaveCenter.Count; i++ )
        {
            WaveData wave = playWaveCenter[i];
            if(wave.DoPlay == false)
            {
                continue;
            }

            int clip = wave.NowClip;
            Vector2 center = wave.Center;

            Vector2 sPos;
            sPos.x = (clip % MaxWidthClip) * ClipWidth;
            sPos.y = (clip / MaxWidthClip) * ClipHeight;

            var texData = WaveTexture.GetPixels((int)sPos.x, (int)sPos.y, ClipWidth, ClipHeight);

            Vector2 pivot = new Vector2(center.x - (TitleWidth.x / 2), center.y - (TitleWidth.y / 2));
            for (int y = 0; y < TitleWidth.y; y++)
            {
                for (int x = 0; x < TitleWidth.x; x++)
                {

                    int nowX = (int)pivot.x + x;
                    if (nowX < 0 || nowX > planeX)
                    {
                        continue;
                    }

                    int nowY = (int)pivot.y + y;
                    if (nowY < 0 || nowY > planeY)
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
                    topVerAry[nowX + nowY * planeX].y += value * Amplitude;

                    //Debug.Log (value);

                    // side plane
                    if (nowY == 0)
                    {
                        sideVerAry[planeX - nowX].z += value * Amplitude * -1;
                    }



                }
            }


            wave.NowClip++;
            if(wave.NowClip == TotalClip)
            {
                wave.DoPlay = false;
            }
               
        }

        topMesh.vertices = topVerAry;
        topMesh.RecalculateNormals();

        sideMesh.vertices = sideVerAry;
        sideMesh.RecalculateNormals();

    }

	void mouseClick()
	{
        var wave = getNotPlayWave();
        if (wave != null)
        {
            wave.Center.x = Random.Range(0, planeY);
            wave.Center.y = 0;
            wave.DoPlay = true;
            wave.NowClip = 0;

		}
	}
}
