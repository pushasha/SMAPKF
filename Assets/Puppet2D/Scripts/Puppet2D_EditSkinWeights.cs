using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Puppet2D_EditSkinWeights : MonoBehaviour 
{   
    public GameObject Bone0,Bone1,Bone2,Bone3;
    public int boneIndex0, boneIndex1,boneIndex2,boneIndex3;
    public float Weight0,Weight1,Weight2,Weight3;
    public Mesh mesh;
    public SkinnedMeshRenderer meshRenderer;
    public int vertNumber;
    GameObject[] handles;
    public Vector3[] verts;
    static private Mesh skinnedMesh;


	/*class Bone
	{
		internal Transform bone;

		internal float weight;

		internal Vector3 delta;
	}

	List<Bone> bones = new List<Bone>();

	void OnEnable()
	{


		Vector3 position = mesh.vertices[vertNumber];

		position = transform.TransformPoint(position);

		BoneWeight weights = mesh.boneWeights[vertNumber];

		int[] boneIndices = new int[] { weights.boneIndex0, weights.boneIndex1, weights.boneIndex2, weights.boneIndex3 };

		float[] boneWeights = new float[] { weights.weight0, weights.weight1, weights.weight2, weights.weight3 };


		for (int j = 0; j < 4; j++)
		{
			if (boneWeights[j] > 0)
			{
				Bone bone = new Bone();

				bones.Add(bone);

				bone.bone = meshRenderer.bones[boneIndices[j]];

				bone.weight = boneWeights[j];

				bone.delta = bone.bone.InverseTransformPoint(position);

			}
		}



	}
	void Update()
	{


		Vector3 position = Vector3.zero;

		foreach (Bone bone in bones)
			position += bone.bone.TransformPoint(bone.delta) * bone.weight;


		transform.position = position;



	}*/


    /*
    void LateUpdate()
    {

        Transform bone0 = meshRenderer.bones[mesh.boneWeights[vertNumber].boneIndex0];
        Transform bone1 = meshRenderer.bones[mesh.boneWeights[vertNumber].boneIndex1];
        Transform bone2 = meshRenderer.bones[mesh.boneWeights[vertNumber].boneIndex2];
        Transform bone3 = meshRenderer.bones[mesh.boneWeights[vertNumber].boneIndex3];

        Vector3 newPos = Vector3.zero;
        newPos = bone0.TransformPoint((mesh.vertices[vertNumber])) * mesh.boneWeights[vertNumber].weight0;
       
        transform.localPosition = newPos;

       

    }*/
    public void Refresh()
    {
    
        BoneWeight[] boneWeights = mesh.boneWeights;

        if(Bone0)
            boneWeights[vertNumber].boneIndex0 = boneIndex0;
        if(Bone1)
            boneWeights[vertNumber].boneIndex1 = boneIndex1;
        if(Bone2)
            boneWeights[vertNumber].boneIndex2 = boneIndex2;
        if(Bone3)
            boneWeights[vertNumber].boneIndex3 = boneIndex3;

        boneWeights[vertNumber].weight0 = Weight0;
        boneWeights[vertNumber].weight1 = Weight1;
        boneWeights[vertNumber].weight2 = Weight2;
        boneWeights[vertNumber].weight3 = Weight3;

        mesh.boneWeights = boneWeights;
        meshRenderer.sharedMesh = mesh;
    }
}