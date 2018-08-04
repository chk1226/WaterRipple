using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlane : MonoBehaviour {

	public int lengthY = 50; //網格的長
	public int lengthX = 50; //寬
	public Material mat; //貼圖材質
	public Vector3[] matrix; //把網格中各點的座標存下來
	public Mesh mesh; //把建立的Mesh留下參照
	public float levelYPosition = 0; //網格的Y軸位置

	void Awake() {
		Application.targetFrameRate = 60; //60FPSに設定
	}

	// Use this for initialization
	void Start () {
		create ();
	}
	
	void create()
	{
		//建立網格點座標陣列
		matrix = new Vector3[lengthX * lengthY];
		for (int y = 0; y < lengthY; ++y)
		{
			for(int x = 0; x < lengthX; ++x)
			{
				matrix[y * lengthX + x] = new Vector3(((float)x)/10, levelYPosition, ((float)y)/10);
			}
		}

		//建立[vert][Normals][UVs]
		Vector3[] vertices = new Vector3[lengthX * lengthY];
		Vector3[] norms = new Vector3[lengthX * lengthY];
		Vector2[] UVs = new Vector2[lengthX * lengthY];

		for(int y = 0; y < lengthY; ++y)
		{
			for(int x = 0; x < lengthX; ++x)
			{
				vertices[y * lengthX + x] = matrix[y * lengthX + x];
				norms[y * lengthX + x] = Vector3.up;
				UVs[y * lengthX + x] = new Vector2((1/(float)(lengthX-1))*x , (1/(float)(lengthY-1))*y);
			}
		}

		//建立[Triangle]
		int[] triangles = new int[(lengthX-1) * (lengthY-1) * 6];
		int ind = 0;
		for(int y = 0; y < lengthY-1; ++y)
		{
			for(int x = 0; x < lengthX-1; ++x)
			{
				triangles[ind++] = y * lengthX + x;
				triangles[ind++] = (y + 1) * lengthX + (x + 1);
				triangles[ind++] = y * lengthX + (x + 1);
				triangles[ind++] = y * lengthX + x;
				triangles[ind++] = (y + 1) * lengthX + x;
				triangles[ind++] = (y + 1) * lengthX + (x + 1);
			}
		}

		//建立新的MeshRenderer並設定好材質
		GameObject newMesh = new GameObject ();
		MeshRenderer mr = newMesh.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		mr.material = mat;

		MeshFilter mf = newMesh.AddComponent (typeof(MeshFilter)) as MeshFilter;
		mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.normals = norms;
		mesh.triangles = triangles;
		mesh.uv = UVs;
		mf.mesh = mesh;

		//把這個Mesh掛在當前物件底下
		newMesh.transform.parent = this.transform;


		// add cusom component
		newMesh.AddComponent(typeof(WaterWave));
	}
}
