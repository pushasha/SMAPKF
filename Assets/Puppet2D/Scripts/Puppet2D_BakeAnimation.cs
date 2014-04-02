
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 
using System.IO;

using UnityEditor;

public class Puppet2D_BakeAnimation : MonoBehaviour {
	private bool ready = false;
	private AnimationClip clip;
	private AnimationClip newClip;
	private float end;
	private List<AnimationCurve> curvePosX = new List<AnimationCurve>();
	private List<AnimationCurve> curvePosY = new List<AnimationCurve>();
	private List<AnimationCurve> curvePosZ = new List<AnimationCurve>();
	private List<AnimationCurve> curveRotX = new List<AnimationCurve>();
	private List<AnimationCurve> curveRotY = new List<AnimationCurve>();
	private List<AnimationCurve> curveRotZ = new List<AnimationCurve>();
	private List<AnimationCurve> curveRotW = new List<AnimationCurve>();
	private List<AnimationCurve> curveScaleX = new List<AnimationCurve>();
	private List<AnimationCurve> curveScaleY = new List<AnimationCurve>();
	private List<AnimationCurve> curveScaleZ = new List<AnimationCurve>();

	private List<Transform> bones = new List<Transform>();
	private List<GameObject>childsOfGameobject = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{

		List<AnimationClip> animClips = GetAnimationLengths();
		bones = GetListBones () ;

		//clip = Selection.objects[0] as AnimationClip;
		newClip = new AnimationClip();
		//AnimatorStateInfo state =  transform.GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0);
		//float leng = state.length;

		clip = animClips[0];
		float leng = clip.length;
		Debug.Log(leng);
		transform.GetComponent<Animator> ().StartRecording (0);


		Invoke ("stopRecording", leng);

	}


	// Update is called once per frame
	List<Transform> GetListBones () 
	{
		List<Transform> returnList = new List<Transform>();
		GetAllChilds(transform.gameObject);
		foreach (GameObject child in childsOfGameobject)
		{
			if(child.transform.GetComponent<SpriteRenderer>())
				if(child.transform.GetComponent<SpriteRenderer>().sprite.name.Contains ("Bone"))
					returnList.Add(child.transform);
		}
		return returnList;
	}

	private List<GameObject> GetAllChilds(GameObject transformForSearch)
	{
		List<GameObject> getedChilds = new List<GameObject>();

		foreach (Transform trans in transformForSearch.transform)
		{

			GetAllChilds ( trans.gameObject );
			childsOfGameobject.Add ( trans.gameObject );       
		}   
		return getedChilds;
	}
	void LateUpdate () 
	{

		if(ready)
		{


			transform.GetComponent<Animator>().playbackTime +=Time.deltaTime;

			foreach(Transform bone in bones)
				bakeAnimation(bone, transform.GetComponent<Animator>().playbackTime);
			if(transform.GetComponent<Animator>().playbackTime>end)
			{
				Debug.Log("here");
				ready = false;
				newClip.name = clip.name + "_Baked";
				newClip.wrapMode = clip.wrapMode;

				SaveAnimationClip( newClip );
				AssetDatabase.SaveAssets();
			}

			//}
		}
	}
	void stopRecording()
	{
		transform.GetComponent<Animator> ().StopRecording();
		transform.GetComponent<Animator> ().StartPlayback();
		ready = true;
		//float start = transform.GetComponent<Animator>().recorderStartTime;
		end =  transform.GetComponent<Animator>().recorderStopTime;
		for (int i = 0; i < bones.Count; i++) 
		{
			curvePosX.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curvePosY.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curvePosZ.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curveRotX.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curveRotY.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curveRotZ.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curveRotW.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curveScaleX.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curveScaleY.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
			curveScaleZ.Add (AnimationCurve.Linear (0, 0, 2.2f, 0));
		}

	}
	void bakeAnimation (Transform bone, float time)
	{


		int index = bones.LastIndexOf(bone);	
		string boneNameFul = GetFullName(bone, bone.name);
		Debug.Log ("setting curve for " + boneNameFul);

		curvePosX[index].AddKey( new Keyframe(time, bone.localPosition.x ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localPosition.x", curvePosX[index]);

		curvePosY[index].AddKey( new Keyframe(time, bone.localPosition.y  ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localPosition.y", curvePosY[index]);

		curvePosZ[index].AddKey( new Keyframe(time, bone.localPosition.z  ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localPosition.z", curvePosZ[index]);

		curveRotX[index].AddKey( new Keyframe(time, bone.localRotation.x ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localRotation.x", curveRotX[index]);

		curveRotY[index].AddKey( new Keyframe(time, bone.localRotation.y  ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localRotation.y", curveRotY[index]);

		curveRotZ[index].AddKey( new Keyframe(time, bone.localRotation.z  ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localRotation.z", curveRotZ[index]);

		curveRotW[index].AddKey( new Keyframe(time, bone.localRotation.w  ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localRotation.w", curveRotW[index]);

		curveScaleX[index].AddKey( new Keyframe(time, bone.localScale.x ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localScale.x", curveScaleX[index]);

		curveScaleY[index].AddKey( new Keyframe(time, bone.localScale.y  ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localScale.y", curveScaleY[index]);

		curveScaleZ[index].AddKey( new Keyframe(time, bone.localScale.z  ) );
		newClip.SetCurve(boneNameFul, typeof(Transform), "localScale.z", curveScaleZ[index]);



	}
	string GetFullName(Transform obj, string name)
	{

		if(obj.parent)
		{
			if(obj.parent.name == transform.name)
			{

				return name;
			}
			else
			{
				name = (obj.parent.name + "/" + name);
				name = GetFullName(obj.parent, name);
				return name;
			}
		}
		else
			return name;

	}
	void SaveAnimationClip(AnimationClip a) 
	{
		if(!Directory.Exists("Assets/Animations Generated")) {
			AssetDatabase.CreateFolder("Assets", "Animations Generated");
			AssetDatabase.Refresh();
		}
		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Animations Generated/"+a.name+".asset");
		AssetDatabase.CreateAsset(a, path);

	}
	List<AnimationClip> GetAnimationLengths()
	{
		List<AnimationClip> animationClips = new List<AnimationClip>();
		RuntimeAnimatorController controller = Selection.gameObjects[0].GetComponent<Animator>().runtimeAnimatorController;

		if (controller is UnityEditorInternal.AnimatorController)
		{

			UnityEditorInternal.StateMachine m = ((UnityEditorInternal.AnimatorController)controller).GetLayer(0).stateMachine;

			for (int i = 0; i < m.stateCount; i++)

			{
				AnimationClip clip = new AnimationClip();
				// Obviously loading it depends on where/how clip is stored, best case its a resource, worse case you have to search asset database.
				if (m.GetState(i).GetMotion())
				{
					clip = (AnimationClip)Resources.LoadAssetAtPath("Assets/Puppet2D/Animation/" + m.GetState(i).GetMotion().name + ".anim", typeof(AnimationClip));
					animationClips.Add(clip);
				}
				if (clip)

				{

					Debug.Log(clip.name + ": " + clip.length);

				}

			}

		}
		return animationClips;
	}


}
#endif