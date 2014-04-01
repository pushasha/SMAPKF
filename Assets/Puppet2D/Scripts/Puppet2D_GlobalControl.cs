using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Puppet2D_GlobalControl : MonoBehaviour {

    public List<Puppet2D_IKHandle> _Ikhandles = new List<Puppet2D_IKHandle>();
    public List<Puppet2D_ParentControl> _ParentControls = new List<Puppet2D_ParentControl>();
    private List<SpriteRenderer> _Controls = new List<SpriteRenderer>();
    private List<SpriteRenderer> _Bones = new List<SpriteRenderer>();

    public bool ControlsVisiblity = true;
    public bool BonesVisiblity = true;
	public bool CombineMeshes = false;

	public bool flip = false;
	// Use this for initialization
	void OnEnable () 
    {
        _Ikhandles.Clear();
        _ParentControls.Clear();
        _Controls.Clear();
        TraverseHierarchy(transform);
	}
	void Awake () 
	{

		if(Application.isPlaying)
		{
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));

			if(CombineMeshes)			
				CombineAllMeshes();

			if(flip)
				transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);

		}
		
	}
	// Update is called once per frame
    public void Init()
    {
        _Ikhandles.Clear();
        _ParentControls.Clear();
        _Controls.Clear();
        TraverseHierarchy(transform);
    }
	void OnValidate ()
	{
		foreach(SpriteRenderer ctrl in _Controls)
		{
			if(ctrl)
				ctrl.enabled = ControlsVisiblity;
		}
		foreach(SpriteRenderer bone in _Bones)
		{
			if(bone)
				bone.enabled = BonesVisiblity;
		}

	}
	void Update () 
    {

		transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));

        
        foreach(Puppet2D_ParentControl parentControl in _ParentControls)
        {
			if(parentControl)
            	parentControl.ParentControlRun();
        }
        foreach(Puppet2D_IKHandle ik in _Ikhandles)
        {
			if(ik)
            	ik.CalculateIK();
        }
		if(flip)
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
	
	}
    void TraverseHierarchy(Transform root) 
    {

        foreach (Transform child in root) 
        {
            GameObject Go = child.gameObject;
            SpriteRenderer spriteRenderer = Go.transform.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
				if(spriteRenderer.sprite.name.Contains("Control"))
                    _Controls.Add(spriteRenderer);
				else if(spriteRenderer.sprite.name.Contains("Bone"))
                    _Bones.Add(spriteRenderer);
            }
            Puppet2D_ParentControl newParentCtrl = Go.transform.GetComponent<Puppet2D_ParentControl>();

            if (newParentCtrl)
            {
                _ParentControls.Add(newParentCtrl);

            }
            Puppet2D_IKHandle newIKCtrl = Go.transform.GetComponent<Puppet2D_IKHandle>();
            if (newIKCtrl)
                _Ikhandles.Add(newIKCtrl);

            TraverseHierarchy(child);

        }

    }
	void CombineAllMeshes() 
	{        
		SkinnedMeshRenderer[] smRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
		List<Transform> bones = new List<Transform>();        
		List<BoneWeight> boneWeights = new List<BoneWeight>();        
		List<CombineInstance> combineInstances = new List<CombineInstance>();
		List<Texture2D> textures = new List<Texture2D>();
		int numSubs = 0;
		var smRenderersDict = new Dictionary<SkinnedMeshRenderer, float>(smRenderers.Length);
		foreach(SkinnedMeshRenderer smr in smRenderers)
		{
			smRenderersDict.Add(smr, smr.transform.position.z);
		}
		var items = from pair in smRenderersDict
			orderby pair.Value descending
				select pair;
		foreach (KeyValuePair<SkinnedMeshRenderer, float> pair in items)
		{
			//Debug.Log(pair.Key.name + " " + pair.Value);
			numSubs += pair.Key.sharedMesh.subMeshCount;
		}

		int[] meshIndex = new int[numSubs];
		int boneOffset = 0;

		int s = 0;
		foreach (KeyValuePair<SkinnedMeshRenderer, float> pair in items)
		{
			SkinnedMeshRenderer smr = pair.Key;    
			
			BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;
			
			foreach( BoneWeight bw in meshBoneweight ) {
				BoneWeight bWeight = bw;
				
				bWeight.boneIndex0 += boneOffset;
				bWeight.boneIndex1 += boneOffset;
				bWeight.boneIndex2 += boneOffset;
				bWeight.boneIndex3 += boneOffset;                
				
				boneWeights.Add( bWeight );
			}
			boneOffset += smr.bones.Length;
			
			Transform[] meshBones = smr.bones;
			foreach( Transform bone in meshBones )
				bones.Add( bone );
			
			if( smr.material.mainTexture != null )
				textures.Add( smr.renderer.material.mainTexture as Texture2D );
			
			CombineInstance ci = new CombineInstance();
			ci.mesh = smr.sharedMesh;
			meshIndex[s] = ci.mesh.vertexCount;
			ci.transform = smr.transform.localToWorldMatrix;
			combineInstances.Add( ci );
			
			Object.Destroy( smr.gameObject );
			s++;
		}
		
		List<Matrix4x4> bindposes = new List<Matrix4x4>();
		
		for( int b = 0; b < bones.Count; b++ ) {
			bindposes.Add( bones[b].worldToLocalMatrix * transform.worldToLocalMatrix );
		}
		
		SkinnedMeshRenderer r = gameObject.AddComponent<SkinnedMeshRenderer>();
		r.sharedMesh = new Mesh();
		r.sharedMesh.CombineMeshes( combineInstances.ToArray(), true, true );
		
		Material combinedMat = new Material( Shader.Find( "Unlit/Transparent" ) );
		combinedMat.mainTexture = textures[0];
		r.sharedMesh.uv = r.sharedMesh.uv;
		r.sharedMaterial = combinedMat;
		
		r.bones = bones.ToArray();
		r.sharedMesh.boneWeights = boneWeights.ToArray();
		r.sharedMesh.bindposes = bindposes.ToArray();
		r.sharedMesh.RecalculateBounds();
	}
}
