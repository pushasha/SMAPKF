using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using Poly2Tri;
using Poly2Tri.Triangulation;
using Poly2Tri.Triangulation.Delaunay;
using Poly2Tri.Triangulation.Delaunay.Sweep;
using Poly2Tri.Triangulation.Polygon;
using System.Linq;


public class Puppet2D_CreatePolygonFromSprite : Editor {

    private GameObject MeshedSprite;
    private MeshFilter mf;
    private MeshRenderer mr;
    private Mesh mesh;
    public Sprite mysprite;
    //public bool ReverseNormals;
	public GameObject Run (Transform transform,bool ReverseNormals )
    {
        
        PolygonCollider2D polygonCollider = transform.GetComponent<PolygonCollider2D>();
				
		Sprite spr = transform.GetComponent<SpriteRenderer>().sprite;
		Rect rec = spr.rect;

		//for(int path =0;path<polygonCollider.pathCount;path++)
		//{
		int path =0;
		bool overwrite = false;
		MeshedSprite = new GameObject();
		Undo.RegisterCreatedObjectUndo (MeshedSprite, "Created Mesh");
		mf = MeshedSprite.AddComponent<MeshFilter>();
		mr = MeshedSprite.AddComponent<MeshRenderer>();
		mesh = new Mesh();
		
		if(AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Models/"+transform.name+"_MESH.asset",typeof(Mesh)))
		{
			if(EditorUtility.DisplayDialog("Overwrite Asset?","Do you want to overwrite the current Mesh & Material?","Yes, Overwrite","No, Create New Mesh & Material"))
			{
				mesh = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Models/"+transform.name+"_MESH.asset",typeof(Mesh))as Mesh;
				overwrite = true;
			}
			else
			{
				string meshPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Puppet2D/Models/"+transform.name+"_MESH.asset");
				AssetDatabase.CreateAsset(mesh,meshPath);
			}
		}
		else
		{
			string meshPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Puppet2D/Models/"+transform.name+"_MESH.asset");
			AssetDatabase.CreateAsset(mesh,meshPath);
		}

		Vector3 bound = transform.renderer.bounds.max- transform.renderer.bounds.min ;

		List<Vector3> results = new List<Vector3>();
		List<int> resultsTriIndexes = new List<int>();
		List<int> resultsTriIndexesReversed = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		List<Vector3> normals = new List<Vector3>();


		Vector2[] vertsToCopy = polygonCollider.GetPath(path);
			       

		int i =  0;

        
        
		Polygon poly =  new Polygon();

		for (i=0; i <vertsToCopy.Length;i++) 
        {
			poly.Points.Add(new TriangulationPoint(vertsToCopy[i].x, vertsToCopy[i].y));                       

        }

		DTSweepContext tcx = new DTSweepContext();
		tcx.PrepareTriangulation(poly);
		DTSweep.Triangulate(tcx);


		int indexNumber = 0;

		foreach (DelaunayTriangle triangle in poly.Triangles)
		{
			Vector3 v = new Vector3();
			foreach (TriangulationPoint p in triangle.Points)
			{
				v = new Vector3((float)p.X, (float)p.Y,0);
				if(!results.Contains(v))
				{
					results.Add(v);
					resultsTriIndexes.Add(indexNumber);
					Vector2 newUv = new Vector2((v.x /bound.x) + 0.5f,  (v.y /bound.y)  + 0.5f);

					newUv.x *= rec.width/ spr.texture.width;
					newUv.y *= rec.height/ spr.texture.height;
					//Debug.Log(spr.textureRectOffset);			
					newUv.x += (rec.x)/ spr.texture.width;
					newUv.y += (rec.y) / spr.texture.height;

					uvs.Add(newUv);
					normals.Add(new Vector3(0,0,-1));


					indexNumber++;

				}
				else
				{
					resultsTriIndexes.Add(results.LastIndexOf(v));
				}
			}

		}


		for (int j = resultsTriIndexes.Count-1; j >=0; j--) 
		{
			resultsTriIndexesReversed.Add(resultsTriIndexes[j]);
		}


		mf.mesh = mesh;
		mesh.vertices = results.ToArray();
		
		mesh.uv = uvs.ToArray();
		mesh.normals = normals.ToArray();
		
		mesh.triangles = resultsTriIndexesReversed.ToArray();
		mesh.RecalculateBounds();

		results.Clear();
		resultsTriIndexesReversed.Clear();
		//}
		    




		if(overwrite)
		{
			mr.material = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Models/Materials/"+transform.name+"_MAT.mat",typeof(Material)) as Material;
		}
		else
		{

			Material newMat = new Material(Shader.Find("Unlit/Transparent"));
			string materialPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Puppet2D/Models/Materials/"+transform.name+"_MAT.mat");
			AssetDatabase.CreateAsset(newMat, materialPath);
			mr.material = newMat;
		}
		

		

        return MeshedSprite;

    }


	
}
