using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

public class Puppet2D_Editor : EditorWindow 
{

    static bool BoneCreation = false;

    int boneNumber = 0;

    GameObject currentBone;
    GameObject previousBone;

    public bool ReverseNormals ;

	static string _boneSortingLayer,_controlSortingLayer;
	static int _boneSortingIndex,_controlSortingIndex;

    List<Transform> bonesToParent = new List<Transform>(); 

    static List<GameObject> SkinnedMeshesBeingEditted = new List<GameObject>(); 

	static float BoneSize = 22.360679775f;

	static float ControlSize = 22.360679775f;

	static float VertexHandleSize = 22.360679775f;

	[MenuItem ("GameObject/Puppet2D/Window/Puppet2D")]
	[MenuItem ("Window/Puppet2D")]
    static void Init () 
    {
        //Puppet2D_Editor window = (Puppet2D_Editor)EditorWindow.GetWindow (typeof (Puppet2D_Editor));
		EditorWindow.GetWindow (typeof (Puppet2D_Editor));
    }

    void OnGUI () 
    {
        GUILayout.Label ("Bone Creation", EditorStyles.boldLabel);
		Texture aTexture = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/GUI_Bones.png",typeof(Texture))as Texture;
		Texture puppetManTexture = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/GUI_puppetman.png",typeof(Texture))as Texture;
		Texture rigTexture = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/GUI_Rig.png",typeof(Texture))as Texture;

		
		//Texture CreateBoneButton = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI_CreateBoneButton.png",typeof(Texture))as Texture;


		GUILayout.Space(15);
		GUI.DrawTexture(new Rect(0, 20, 64, 128), aTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUILayout.Space(15);
		//if (GUILayout.Button("Create Bone Tool"))	
		if (GUI.Button(new Rect(80, 30, 150, 30),"Create Bone Tool" ))
        {
            BoneCreation = true;
            /*
            foreach(string word in GetSortingLayerNames())
            Debug.Log(word);*/

        }
        
		if (GUI.Button(new Rect(80, 60, 150, 30),"Finish Bone" ))
        {

            BoneFinishCreation();
        }


		//BoneSize = GUI.HorizontalSlider(new Rect(80, 100, 110, 30), BoneSize, 31.6227766017F, 1F);
		BoneSize = EditorGUI.Slider(new Rect(80, 100, 150, 20), BoneSize, 0.01F, 100F);
		//string BoneNoJointTextureName = ("Assets/Puppet2D/Textures/GUI/BoneNoJoint.psd");
		//Sprite BoneNoJointTexture =AssetDatabase.LoadAssetAtPath(BoneNoJointTextureName, typeof(Sprite)) as Sprite;
//		if (GUI.Button(new Rect(200, 95, 30, 30),BoneNoJointTexture.texture ))
//		{
//			ChangeBoneSize();
//		}

		string[] sortingLayers = GetSortingLayerNames();

		_boneSortingIndex  = EditorGUI.Popup(new Rect(80, 130, 150, 30),_boneSortingIndex, sortingLayers);

		_boneSortingLayer = sortingLayers[_boneSortingIndex];


		GUILayout.Space(100);
          
        GUILayout.Label ("Rigging Setup", EditorStyles.boldLabel);
		GUI.DrawTexture(new Rect(0, 180, 64, 128), rigTexture, ScaleMode.StretchToFill, true, 10.0F);
		if (GUI.Button(new Rect(80, 180, 150, 30),"Create IK Control" ))
		{
			IKCreateTool();

        }
		if (GUI.Button(new Rect(80, 210, 150, 30),"Create Parent Control" ))
		{
            CreateParentControl();

        }
		if (GUI.Button(new Rect(80, 240, 150, 30),"Create Orient Control" ))
		{        
            CreateOrientControl();

        }

		ControlSize = EditorGUI.Slider(new Rect(80, 280, 150, 20), ControlSize, 0.01F, 100F);
		
		//Sprite IKTexture =AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/IKControl.psd", typeof(Sprite)) as Sprite;
		
		//if (GUI.Button(new Rect(200, 275, 30, 30),IKTexture.texture ))
		//{
		//	ChangeControlSize();
		//}

		_controlSortingIndex  = EditorGUI.Popup(new Rect(80, 310, 150, 30),_controlSortingIndex, sortingLayers);
		
		_controlSortingLayer = sortingLayers[_controlSortingIndex];


		GUILayout.Space(160);

        GUILayout.Label ("Skinning", EditorStyles.boldLabel);
		GUI.DrawTexture(new Rect(0, 360, 64, 128), puppetManTexture, ScaleMode.StretchToFill, true, 10.0F);


        if (GUI.Button(new Rect(80, 360, 150, 30),"Convert Sprite To Mesh" ))
        {
            ConvertSpriteToMesh();
        }
        if (GUI.Button(new Rect(80, 390, 150, 30),"Parent Object To Bones" ))
        {
            BindRigidSkin();

        }
        if (GUI.Button(new Rect(80, 420, 150, 30),"Bind Smooth Skin" ))
        {
            BindSmoothSkin();

        }
        
		if (GUI.Button(new Rect(80, 450, 150, 30),"Edit Skin Weights" ))
        {
           EditWeights();
                
        }
       
		if (GUI.Button(new Rect(80, 480, 150, 30),"Finish Edit Skin Weights" ))
        {   

            FinishEditingWeights(); 
                        
        }
		//VertexHandleSize = GUI.HorizontalSlider(new Rect(80, 520, 110, 30), VertexHandleSize, 50F, 10F);
		VertexHandleSize = EditorGUI.Slider(new Rect(80, 520, 150, 20), VertexHandleSize, 0.01F, 100F);
		
		//Sprite vertexHandleTexture =AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/VertexHandle.psd", typeof(Sprite)) as Sprite;
		
//		if (GUI.Button(new Rect(200, 515, 30, 30),vertexHandleTexture.texture ))
//		{
//			ChangeVertexHandleSize();
//		}
		if (GUI.changed)
		{
			ChangeControlSize();
			ChangeBoneSize();
			ChangeVertexHandleSize();
		}

    }
/*	void OnInspectorUpdate() 
	{
		if (GUI.changed)
		{
			ChangeControlSize();
			ChangeBoneSize();
			ChangeVertexHandleSize();
		}
	}*/
    void BoneFinishCreation()
    {
        for (int i = bonesToParent.Count-1; i >0; i--)
        {
			if(bonesToParent[i]!=null)
            	bonesToParent[i].parent = bonesToParent[i-1];
        }
        bonesToParent.Clear();
        BoneCreation = false;
        boneNumber = 0;
        currentBone = null;
    }
	[MenuItem ("GameObject/Puppet2D/Skeleton/Create Bone Tool")]
    static void CreateBoneTool()
    {
        BoneCreation = true;
    }

    void BoneCreationMode(Vector3 mousePos)
    {

		Transform itHasAParent = null;
		if (Selection.activeObject != null)
        {
			GameObject selection = Selection.activeObject as GameObject;
			if(selection.GetComponent<SpriteRenderer>())
			{
				if((selection.GetComponent<SpriteRenderer>().sprite.name == "Bone")||(selection.GetComponent<SpriteRenderer>().sprite.name == "BoneNoJoint"))
				{
		            currentBone = Selection.activeObject as GameObject;
					if(currentBone.transform.parent)
					{
						itHasAParent = currentBone.transform.parent;
						currentBone.transform.parent = null;
					}
					bonesToParent.Add(currentBone.transform); 
				}
				else
				{
					//bonesToParent.Add(selection.transform); 
				}
			}

        }
        

		GameObject newBone = new GameObject(GetUniqueBoneName());
		Undo.RegisterCreatedObjectUndo (newBone, "Created newBone");
        newBone.transform.position = mousePos;
        newBone.transform.position = new Vector3(newBone.transform.position.x, newBone.transform.position.y, 0);

        SpriteRenderer spriteRenderer = newBone.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingLayerName = _boneSortingLayer;
        string path = ("Assets/Puppet2D/Textures/GUI/BoneNoJoint.psd");
        string path2 = ("Assets/Puppet2D/Textures/GUI/Bone.psd");

        if (currentBone != null)
        {

            Sprite sprite2 =AssetDatabase.LoadAssetAtPath(path2, typeof(Sprite)) as Sprite;
            currentBone.GetComponent<SpriteRenderer>().sprite = sprite2;

            if (currentBone.transform.childCount == 0)
            {
                currentBone.transform.rotation = Quaternion.LookRotation(newBone.transform.position - currentBone.transform.position, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right);
                float length = (currentBone.transform.position - newBone.transform.position).magnitude;

                currentBone.transform.localScale = new Vector3(length, length , length );


            }




        }
        bonesToParent.Add(newBone.transform); 

        Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
        previousBone = currentBone;
		if(itHasAParent)
			currentBone.transform.parent = itHasAParent;
        currentBone = newBone;
        boneNumber++;
		itHasAParent = null;

    }

	string GetUniqueBoneName ()
	{
		string nameToAdd = "bone";
		int index =0;
		foreach(GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if(go.name.StartsWith(nameToAdd))
			{
				string endOfName = go.name.Substring(5,go.name.Length-5);
								
				int indexTest = 0;
				if(int.TryParse(endOfName,out indexTest))
				{
					if(int.Parse(endOfName)>index)
					{
						index =int.Parse(endOfName);
					}
				}


			}
		}
		index++;
		return ("bone_"+index);

	}

    void BoneMoveMode(Vector3 mousePos)
    {
		if(currentBone)
		{ 
	        currentBone.transform.position = mousePos;
	        currentBone.transform.position = new Vector3(currentBone.transform.position.x, currentBone.transform.position.y, 0);
	        //currentBone.transform.localScale = new Vector3(1, 1 , 1 );

	        if (previousBone != null)
	        {
	            //previousBone.transform.localScale = new Vector3(1, 1 , 1 );
	            previousBone.transform.eulerAngles = new Vector3(0, 0 , 0 );
	            previousBone.transform.rotation = Quaternion.LookRotation(currentBone.transform.position - previousBone.transform.position, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right);
	            float length = (previousBone.transform.position - currentBone.transform.position).magnitude;

	            previousBone.transform.localScale = new Vector3(length, length , length );


	        }
		}

    }
	[MenuItem ("GameObject/Puppet2D/Rig/Create IK Control")]
    static void IKCreateTool()
    {

        GameObject bone = Selection.activeObject as GameObject;
		if(bone)
		{
			if(bone.GetComponent<SpriteRenderer>())
			{
				if(!bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
				{
					Debug.LogWarning("This is not a Puppet2D Bone");
					return;
				}
			}
			else
			{
				Debug.LogWarning("This is not a Puppet2D Bone");
				return;
			}
		}
		else
		{
			Debug.LogWarning("This is not a Puppet2D Bone");
			return;
		}
		GameObject globalCtrl = CreateGlobalControl();
		foreach(Puppet2D_ParentControl parentControl in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls)
		{
			if((parentControl.bone.transform == bone.transform)||(parentControl.bone.transform == bone.transform.parent.transform))
			{
				Debug.LogWarning("Can't create a IK Control on Bone; it alreay has an Parent Control");
				return;
			}
		}
		foreach(Puppet2D_IKHandle ikhandle in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._Ikhandles)
		{
			if((ikhandle.bottomJointTransform == bone.transform)||(ikhandle.middleJointTransform == bone.transform)||(ikhandle.topJointTransform == bone.transform))
			{
				Debug.LogWarning("Can't create a IK Control on Bone; it alreay has an IK handle");
				return;
			}
		}

		GameObject IKRoot = null;
		if(bone.transform.parent)
			if(bone.transform.parent.transform.parent)
        		IKRoot = bone.transform.parent.transform.parent.gameObject;
		if(IKRoot==null)
		{
			Debug.LogWarning("You need to select the end of a chain of three bones");
			return;
		}
        GameObject control = new GameObject();
		Undo.RegisterCreatedObjectUndo (control, "Created control");
        control.name = (bone.name+"_CTRL");
        GameObject controlGroup = new GameObject();
        controlGroup.name = (bone.name+"_CTRL_GRP");
		Undo.RegisterCreatedObjectUndo (controlGroup, "Created controlgrp");

		control.transform.parent = controlGroup.transform;
        controlGroup.transform.position = bone.transform.position;
        controlGroup.transform.rotation = bone.transform.rotation;

        GameObject poleVector = new GameObject();
		Undo.RegisterCreatedObjectUndo (poleVector, "Created polevector");
        poleVector.name = (bone.name+"_POLE");

        SpriteRenderer spriteRenderer = control.AddComponent<SpriteRenderer>();
        string path = ("Assets/Puppet2D/Textures/GUI/IKControl.psd");
        Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingLayerName = _controlSortingLayer;
        Puppet2D_IKHandle ikHandle = control.AddComponent<Puppet2D_IKHandle>();
        ikHandle.topJointTransform = IKRoot.transform;
        ikHandle.middleJointTransform = IKRoot.transform.GetChild(0);
        ikHandle.bottomJointTransform = IKRoot.transform.GetChild(0).GetChild(0);
        ikHandle.poleVector = poleVector.transform;
        ikHandle.scaleStart[0] = IKRoot.transform.localScale;
        ikHandle.scaleStart[1] = IKRoot.transform.GetChild(0).localScale;
		ikHandle.OffsetScale = bone.transform.localScale;

        if(bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
        {
            ikHandle.AimDirection = Vector3.forward;
            ikHandle.UpDirection = Vector3.right;
        }
        else
        {
			Debug.LogWarning("This is not a Puppet2D Bone");
            ikHandle.AimDirection = Vector3.right;
            ikHandle.UpDirection = Vector3.up;
        }


        if (bone.transform.parent.transform.position.x < IKRoot.transform.position.x)
            ikHandle.Flip = true;
        Selection.activeObject = ikHandle;

        controlGroup.transform.parent = globalCtrl.transform;
        poleVector.transform.parent = globalCtrl.transform;
        globalCtrl.GetComponent<Puppet2D_GlobalControl>().Init();


    }
	[MenuItem ("GameObject/Puppet2D/Rig/Create Parent Control")]
    static void CreateParentControl()
    {
        GameObject bone = Selection.activeObject as GameObject;
		if(bone)
		{
			if(bone.GetComponent<SpriteRenderer>())
			{
				if(!bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
				{
					Debug.LogWarning("This is not a Puppet2D Bone");
					return;
				}
			}
			else
			{
				Debug.LogWarning("This is not a Puppet2D Bone");
				return;
			}
		}
		else
		{
			Debug.LogWarning("This is not a Puppet2D Bone");
			return;
		}
		GameObject globalCtrl = CreateGlobalControl();
		foreach(Puppet2D_IKHandle ikhandle in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._Ikhandles)
		{
			if((ikhandle.bottomJointTransform == bone.transform)||(ikhandle.middleJointTransform == bone.transform))
			{
				Debug.LogWarning("Can't create a parent Control on Bone; it alreay has an IK handle");
				return;
			}
		}
		foreach(Puppet2D_ParentControl parentControl in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls)
		{
			if((parentControl.bone.transform == bone.transform))
			{
				Debug.LogWarning("Can't create a Parent Control on Bone; it alreay has an Parent Control");
				return;
			}
		}
        GameObject control = new GameObject();
		Undo.RegisterCreatedObjectUndo (control, "Created control");
        control.name = (bone.name+"_CTRL");
        GameObject controlGroup = new GameObject();
		Undo.RegisterCreatedObjectUndo (controlGroup, "Created controlgrp");
        controlGroup.name = (bone.name+"_CTRL_GRP");
       control.transform.parent = controlGroup.transform;
        controlGroup.transform.position = bone.transform.position;
        controlGroup.transform.rotation = bone.transform.rotation;

        SpriteRenderer spriteRenderer = control.AddComponent<SpriteRenderer>();
        string path = ("Assets/Puppet2D/Textures/GUI/ParentControl.psd");
        Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
		spriteRenderer.sortingLayerName = _controlSortingLayer;
        Puppet2D_ParentControl parentConstraint = control.AddComponent<Puppet2D_ParentControl>();
        parentConstraint.IsEnabled = true;
        parentConstraint.Orient = true;
        parentConstraint.Point = true;
        parentConstraint.bone = bone;
		parentConstraint.OffsetScale = bone.transform.localScale;
        Selection.activeObject = control;

        
        controlGroup.transform.parent = globalCtrl.transform;
        globalCtrl.GetComponent<Puppet2D_GlobalControl>().Init();


    }
    static GameObject CreateGlobalControl()
    {
        GameObject globalCtrl = GameObject.Find("Global_CTRL");
        if (globalCtrl)
        {
            return globalCtrl;
        }
        else
        {
            globalCtrl = new GameObject("Global_CTRL");
			Undo.RegisterCreatedObjectUndo (globalCtrl, "Created globalCTRL");

			globalCtrl.AddComponent<Puppet2D_GlobalControl>();
            return globalCtrl ;
        }

    }
	[MenuItem ("GameObject/Puppet2D/Rig/Create Orient Control")]
    static void CreateOrientControl()
    {
        GameObject bone = Selection.activeObject as GameObject;
		if(bone)
		{
			if(bone.GetComponent<SpriteRenderer>())
			{
				if(!bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
				{
					Debug.LogWarning("This is not a Puppet2D Bone");
					return;
				}
			}
			else
			{
				Debug.LogWarning("This is not a Puppet2D Bone");
				return;
			}
		}
		else
		{
			Debug.LogWarning("This is not a Puppet2D Bone");
			return;
		}
		GameObject globalCtrl = CreateGlobalControl();
		foreach(Puppet2D_IKHandle ikhandle in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._Ikhandles)
		{
			if((ikhandle.bottomJointTransform == bone.transform)||(ikhandle.middleJointTransform == bone.transform))
			{
				Debug.LogWarning("Can't create a orient Control on Bone; it alreay has an IK handle");
				return;
			}
		}
		foreach(Puppet2D_ParentControl parentControl in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls)
		{
			if((parentControl.bone.transform == bone.transform))
			{
				Debug.LogWarning("Can't create a Parent Control on Bone; it alreay has an Parent Control");
				return;
			}
		}


        GameObject control = new GameObject();
		Undo.RegisterCreatedObjectUndo (control, "Created control");
        control.name = (bone.name+"_CTRL");
        GameObject controlGroup = new GameObject();
		Undo.RegisterCreatedObjectUndo (controlGroup, "Created controlGroup");
        controlGroup.name = (bone.name+"_CTRL_GRP");
        control.transform.parent = controlGroup.transform;
        controlGroup.transform.position = bone.transform.position;
        controlGroup.transform.rotation = bone.transform.rotation;
        SpriteRenderer spriteRenderer = control.AddComponent<SpriteRenderer>();
        string path = ("Assets/Puppet2D/Textures/GUI/OrientControl.psd");
        Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
		spriteRenderer.sortingLayerName = _controlSortingLayer;
        Puppet2D_ParentControl parentConstraint = control.AddComponent<Puppet2D_ParentControl>();
        parentConstraint.IsEnabled = true;
        parentConstraint.Orient = true;
        parentConstraint.Point = false;
        //parentConstraint.ConstrianedPosition = true;
        parentConstraint.bone = bone;
        Selection.activeObject = control;
		parentConstraint.OffsetScale = bone.transform.localScale;

        controlGroup.transform.parent = globalCtrl.transform;
        globalCtrl.GetComponent<Puppet2D_GlobalControl>().Init();
    }

    /*void OnEnable()
    {
        SceneView.onSceneGUIDelegate += SceneGUI;
    }

    void SceneGUI(SceneView sceneView)
    {


        Event e = Event.current;


        switch (e.type)
        {
            case EventType.keyDown:
            {
                if (Event.current.keyCode == (KeyCode.Return))
                {
                    BoneFinishCreation();
                }
                break;
            }
                case EventType.MouseDown:
            {

                if (Event.current.button == 0)
                {
                    if (BoneCreation)
                    {
                        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
						
					    BoneCreationMode(worldRay.GetPoint(0));

                    }  
                }
                else if (Event.current.button == 2)
                {
                    if (BoneCreation)
                    {
                        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                        BoneMoveMode(worldRay.GetPoint(0));

                    } 
                }
                else if (Event.current.button == 1)
                {
                    if (BoneCreation)
                    {                       
                        BoneFinishCreation();
                        Selection.activeObject = null;
                        BoneCreation = true;
                    } 
                }
                break;

            }
        }


    }*/

    void OnFocus() {


        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        
        // Add (or re-add) the delegate.
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }
    
    void OnDestroy() {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }
    
    void OnSceneGUI(SceneView sceneView) {
		Event e = Event.current;
		
		
		switch (e.type)
		{
		case EventType.keyDown:
		{
			if (Event.current.keyCode == (KeyCode.Return))
			{
				BoneFinishCreation();
			}
			break;
		}
		case EventType.MouseDown:
		{
			
			if (Event.current.button == 0)
			{
				if (BoneCreation)
				{
					Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
					
					BoneCreationMode(worldRay.GetPoint(0));
					
				}  
			}
			else if (Event.current.button == 2)
			{
				if (BoneCreation)
				{
					Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
					
					BoneMoveMode(worldRay.GetPoint(0));
					
				} 
			}
			else if (Event.current.button == 1)
			{
				if (BoneCreation)
				{                       
					BoneFinishCreation();
					Selection.activeObject = null;
					BoneCreation = true;
				} 
			}
			break;
			
		}
		}
		
		// Do your drawing here using Handles.
		
		GameObject[] selection = Selection.gameObjects;
		
		Handles.BeginGUI();
		if(BoneCreation)
		{
			if(selection.Length>0)
			{
				Handles.color = Color.blue;
				Handles.Label(selection[0].transform.position + Vector3.up*2,
				              "Press Enter To Finish.\nRight Click To start new Bone\nMiddle Click To Move Bone");
			}
			else
			{
				Handles.color = Color.blue;
				Handles.Label(SceneView.lastActiveSceneView.camera.transform.position+Vector3.forward*2,
                              "Bone Create Mode.\nLeft Click to Draw Bones.\nOr click on a bone to be a parent");
            }

        }
        // Do your drawing here using GUI.
        Handles.EndGUI();    
    }



	[MenuItem ("GameObject/Puppet2D/Skin/ConvertSpriteToMesh")]
    static void ConvertSpriteToMesh()
    {
        GameObject[] selection = Selection.gameObjects;
        foreach(GameObject spriteGO in selection)
        {
			if(spriteGO.GetComponent<SpriteRenderer>())
			{
	            PolygonCollider2D polyCol;
	            GameObject MeshedSprite;
	            Quaternion rot = spriteGO.transform.rotation;
	            spriteGO.transform.eulerAngles = Vector3.zero;
				int layer = spriteGO.layer;
				string sortingLayer = spriteGO.renderer.sortingLayerName;
				int sortingOrder = spriteGO.renderer.sortingOrder;

	            if(spriteGO.GetComponent<PolygonCollider2D>()==null)
	            {
	                polyCol = Undo.AddComponent<PolygonCollider2D> (spriteGO);
					//Puppet2D_CreatePolygonFromSprite polyFromSprite = Undo.AddComponent<Puppet2D_CreatePolygonFromSprite> (spriteGO);
					Puppet2D_CreatePolygonFromSprite polyFromSprite = ScriptableObject.CreateInstance("Puppet2D_CreatePolygonFromSprite") as Puppet2D_CreatePolygonFromSprite;
					MeshedSprite = polyFromSprite.Run(spriteGO.transform, true);

					//polyFromSprite.ReverseNormals = true;
	                //MeshedSprite =polyFromSprite.Run();
	                MeshedSprite.name = (spriteGO.name+"_GEO");
	                //DestroyImmediate(polyFromSprite);
	                Undo.DestroyObjectImmediate(polyCol);


	                
	            }
	            else
	            {
	                polyCol = spriteGO.GetComponent<PolygonCollider2D>();

					//Puppet2D_CreatePolygonFromSprite polyFromSprite = Undo.AddComponent<Puppet2D_CreatePolygonFromSprite> (spriteGO);
					Puppet2D_CreatePolygonFromSprite polyFromSprite = ScriptableObject.CreateInstance("Puppet2D_CreatePolygonFromSprite") as Puppet2D_CreatePolygonFromSprite;
					MeshedSprite = polyFromSprite.Run(spriteGO.transform, true);


					//polyFromSprite.ReverseNormals = true;
	                //MeshedSprite = polyFromSprite.Run();
					MeshedSprite.name = (spriteGO.name+"_GEO");

	                //DestroyImmediate(polyFromSprite); 
					Undo.DestroyObjectImmediate(polyCol);

				}
				MeshedSprite.layer = layer;
				MeshedSprite.renderer.sortingLayerName = sortingLayer;
				MeshedSprite.renderer.sortingOrder = sortingOrder;
				
				MeshedSprite.transform.position = spriteGO.transform.position;
	            MeshedSprite.transform.rotation = rot;
	            
	            Sprite spriteInfo = spriteGO.GetComponent<SpriteRenderer>().sprite;
	            
	            TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(spriteInfo)) as TextureImporter;
	            
	            //textureImporter.textureType = TextureImporterType.Image;
	            
	            MeshedSprite.renderer.sharedMaterial.shader = Shader.Find("Unlit/Transparent");
	            
	            MeshedSprite.renderer.sharedMaterial.SetTexture("_MainTex", spriteInfo.texture);
	            
	            textureImporter.textureType = TextureImporterType.Sprite;
	            
	            DestroyImmediate(spriteGO);
	            
	            Selection.activeGameObject = MeshedSprite;
	            
			}
			else
			{
				Debug.LogWarning("Object is not a sprite");
				return;
			}
        }
    }
	[MenuItem ("GameObject/Puppet2D/Skin/Parent Mesh To Bones")]
    static void BindRigidSkin()
    {
        GameObject[] selection = Selection.gameObjects;
        List<GameObject> selectedBones = new List<GameObject>();
        List<GameObject> selectedMeshes= new List<GameObject>();
        
        //GameObject bone;
        //GameObject control;
        foreach (GameObject Obj in selection)
        {
			if(Obj.GetComponent<SpriteRenderer>())
			{
				if (Obj.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
	            {
	                selectedBones.Add(Obj);
				}
				else
				{
					selectedMeshes.Add(Obj);
				}
			}
            else
            {
                selectedMeshes.Add(Obj);
            }
        }

		if((selectedBones.Count == 0)||(selectedMeshes.Count==0))
		{
			Debug.LogWarning("You need to select at least one bone and one other object");
			return;
		}
        foreach (GameObject mesh in selectedMeshes)
        {
            float testdist = 1000000;
            GameObject closestBone =  null;
            foreach (GameObject bone in selectedBones)
            {
                float dist = Vector2.Distance(new Vector2(bone.renderer.bounds.center.x,bone.renderer.bounds.center.y), new Vector2(mesh.transform.position.x,mesh.transform.position.y));
                if (dist < testdist)
                {
                    testdist = dist;
                    //Debug.Log("closest bone to " + mesh.name + " is " + bone.name + " distance " + dist);
                    closestBone = bone;
                }
                
            }
            //mesh.transform.parent = closestBone.transform;
			Undo.SetTransformParent (mesh.transform, closestBone.transform, "parent bone");

            /*
            ParentControl parentConstraint = closestBone.AddComponent<ParentControl>();
            parentConstraint.IsEnabled = true;
            parentConstraint.Orient = true;
            parentConstraint.Point = true;
            parentConstraint.MaintainOffset = true;
   
            parentConstraint.OffsetPos = closestBone.transform.InverseTransformPoint(mesh.transform.position);
            parentConstraint.OffsetOrient = closestBone.transform.rotation * mesh.transform.rotation ;
            parentConstraint.bone = mesh;
            */
        }
        
    }
	[MenuItem ("GameObject/Puppet2D/Skin/Bind Smooth Skin")]
    static void BindSmoothSkin()
    {
        GameObject[] selection = Selection.gameObjects;
        List<Transform> selectedBones = new List<Transform>();
        List<GameObject> selectedMeshes= new List<GameObject>();
        foreach (GameObject Obj in selection)
        {
            if (Obj.GetComponent<SpriteRenderer>()== null)
            {
                selectedMeshes.Add(Obj);
            }
            else
            {
                selectedBones.Add(Obj.transform);
            }
        }

        foreach (GameObject mesh in selectedMeshes)
        {
            Material mat = null;
            if(mesh.GetComponent<MeshRenderer>()!=null)
            {
                mat = mesh.GetComponent<MeshRenderer>().sharedMaterial;
                Undo.DestroyObjectImmediate(mesh.GetComponent<MeshRenderer>());
            }

            SkinnedMeshRenderer renderer = mesh.GetComponent<SkinnedMeshRenderer>();
            if(renderer == null)
                renderer = Undo.AddComponent<SkinnedMeshRenderer>(mesh);


            Mesh sharedMesh = mesh.transform.GetComponent<MeshFilter>().sharedMesh;
            Vector3[] verts = sharedMesh.vertices;




            Matrix4x4[] bindPoses = new Matrix4x4[selectedBones.Count];


            List<Transform> closestBones =  new List<Transform>();
            closestBones.Clear();
            //Debug.Log(verts.Length);
            BoneWeight[] weights = new BoneWeight[verts.Length];
            int index = 0;

            for (int j = 0; j < weights.Length; j++)
            {
                float testdist = 1000000;

                //closestBones.Add(selectedBones[0].transform);
                for (int i = 0; i < selectedBones.Count; i++)
                {

                    Vector3 worldPt = mesh.transform.TransformPoint(verts[j]);
                    float dist = Vector2.Distance(new Vector2(selectedBones[i].renderer.bounds.center.x,selectedBones[i].renderer.bounds.center.y), new Vector2(worldPt.x,worldPt.y));
                    if (dist < testdist)
                    {
                        testdist = dist;
                        index = selectedBones.IndexOf(selectedBones[i]);

                    }
                    
                    
                    Transform bone = selectedBones[i];
                    bindPoses[i] = bone.worldToLocalMatrix * mesh.transform.localToWorldMatrix;
                }

                weights[j].boneIndex0 = index;
                weights[j].weight0 = 1;


                //Debug.Log("Skinning " + j + " closest bone is " + selectedBones[index].name + " index is " + index);
            }

            sharedMesh.boneWeights = weights;

            sharedMesh.bindposes = bindPoses;

            renderer.bones = selectedBones.ToArray();

            //sharedMesh = SmoothSkinWeights(sharedMesh);


            renderer.sharedMesh = sharedMesh;
            if(mat)
            renderer.sharedMaterial = mat;


           
        }

    }
	[MenuItem ("GameObject/Puppet2D/Skin/Edit Skin Weights")]
    static void EditWeights()
    {
        GameObject[] selection = Selection.gameObjects;

		/*foreach(GameObject sel in selection)
		{
			if(sel.GetComponent<Puppet2D_Bakedmesh>())
			{
				Debug.LogWarning("Already in edit mode");
				return;
			}
		}*/
		if(SkinnedMeshesBeingEditted.Count>0)
		{
			Debug.LogWarning("Already in edit mode, first click finish Edit SKin Weights to start again");
			return;

		}
        //SkinnedMeshesBeingEditted.Clear();
        foreach(GameObject sel in selection)
        {
			if(sel.GetComponent<SkinnedMeshRenderer>())
			{
				SkinnedMeshRenderer renderer = sel.GetComponent<SkinnedMeshRenderer>();
				Undo.RecordObject (sel, "add mesh to meshes being editted");
				SkinnedMeshesBeingEditted.Add(sel);
	            Undo.AddComponent<Puppet2D_Bakedmesh>(sel);
	            Mesh mesh = sel.GetComponent<MeshFilter>().sharedMesh;
	            

	            Vector3[] verts = mesh.vertices;
	            BoneWeight[] boneWeights = mesh.boneWeights;

	            for (int i = 0; i < verts.Length; i++)
	            {
	                Vector3 vert = verts[i];
	                Vector3 vertPos = sel.transform.TransformPoint(vert);
	                GameObject handle = new GameObject("vertex"+ i);
	                handle.transform.position = vertPos;
	                //handle.transform.parent = sel.transform;
					Undo.SetTransformParent (handle.transform, sel.transform, "parent handle");
	                //handle.tag = "handle";

	                SpriteRenderer spriteRenderer = Undo.AddComponent<SpriteRenderer>(handle);
	                string path = ("Assets/Puppet2D/Textures/GUI/VertexHandle.psd");
	                Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
	                spriteRenderer.sprite = sprite;
					Puppet2D_EditSkinWeights editSkinWeights = Undo.AddComponent<Puppet2D_EditSkinWeights>(handle);

	                editSkinWeights.verts = mesh.vertices;

	                editSkinWeights.Weight0 = boneWeights[i].weight0;
	                editSkinWeights.Weight1 = boneWeights[i].weight1;
	                editSkinWeights.Weight2 = boneWeights[i].weight2;
	                editSkinWeights.Weight3 = boneWeights[i].weight3;

	                if(boneWeights[i].weight0>0)
	                {
	                    editSkinWeights.Bone0 = renderer.bones[boneWeights[i].boneIndex0].gameObject;
	                    editSkinWeights.boneIndex0 = boneWeights[i].boneIndex0;
	                }
	                else
	                    editSkinWeights.Bone0 = null;

	                if(boneWeights[i].weight1>0)
	                {
	                    editSkinWeights.Bone1 = renderer.bones[boneWeights[i].boneIndex1].gameObject;
	                    editSkinWeights.boneIndex1 = boneWeights[i].boneIndex1;
	                }
	                else
	                {
	                    editSkinWeights.Bone1 = null;
	                    editSkinWeights.boneIndex1 =renderer.bones.Length ;
	                }

	                if (boneWeights[i].weight2 > 0)
	                {
	                    editSkinWeights.Bone2 = renderer.bones[boneWeights[i].boneIndex2].gameObject;
	                    editSkinWeights.boneIndex2 = boneWeights[i].boneIndex2;
	                }
	                else
	                {
	                    editSkinWeights.Bone2 = null;
	                    editSkinWeights.boneIndex2 =renderer.bones.Length ;
	                }

	                if (boneWeights[i].weight3 > 0)
	                {
	                    editSkinWeights.Bone3 = renderer.bones[boneWeights[i].boneIndex3].gameObject;
	                    editSkinWeights.boneIndex3 = boneWeights[i].boneIndex3;
	                }
	                else
	                {
	                    editSkinWeights.Bone3 = null;
	                    editSkinWeights.boneIndex3 =renderer.bones.Length ;
	                }

	                editSkinWeights.mesh = mesh;
	                editSkinWeights.meshRenderer = renderer;
	                editSkinWeights.vertNumber = i;
	            }
			}
			else
			{
				Debug.LogWarning("Selection does not have a meshRenderer");

			}

        }
    }
	[MenuItem ("GameObject/Puppet2D/Skin/Finish Editting Skin Weights")]
    static void FinishEditingWeights()
    {
        foreach(GameObject sel in SkinnedMeshesBeingEditted)
        {
			if(sel.GetComponent<Puppet2D_Bakedmesh>())
			{
				Puppet2D_Bakedmesh bakedMeshComp = sel.GetComponent<Puppet2D_Bakedmesh>();
				DestroyImmediate(bakedMeshComp);
			}
            int numberChildren = sel.transform.childCount;
            List<GameObject> vertsToDestroy = new List<GameObject>();
            for(int i = 0;i< numberChildren;i++)
            {
                vertsToDestroy.Add(sel.transform.GetChild(i).gameObject);


            }
            foreach(GameObject vert in vertsToDestroy)
                DestroyImmediate(vert);
        }
        SkinnedMeshesBeingEditted.Clear();
    }

    static Mesh SmoothSkinWeights(Mesh sharedMesh)
    {
        Debug.Log("smoothing weights");
        int[] triangles = sharedMesh.GetTriangles(0);
        //Vector3[] verts = sharedMesh.vertices;
        BoneWeight[] boneWeights = sharedMesh.boneWeights;

        for(int i =0;i<triangles.Length;i+=3)
        {
            BoneWeight v1 = boneWeights[triangles[i]];
            BoneWeight v2 = boneWeights[triangles[i+1]];
            BoneWeight v3 = boneWeights[triangles[i+2]];

            List<int> v1Bones = new List<int>(new int[] {v1.boneIndex0,v1.boneIndex1,v1.boneIndex2,v1.boneIndex3 });
            List<int> v2Bones = new List<int>(new int[]  {v2.boneIndex0,v2.boneIndex1,v2.boneIndex2,v2.boneIndex3 });
            List<int> v3Bones = new List<int>(new int[]  {v3.boneIndex0,v3.boneIndex1,v3.boneIndex2,v3.boneIndex3 });

            List<float> v1Weights = new List<float>(new float[] {v1.weight0,v1.weight1,v1.weight2,v1.weight3 });
            List<float> v2Weights = new List<float>(new float[]  {v2.weight0,v2.weight1,v2.weight2,v2.weight3 });
            List<float> v3Weights = new List<float>(new float[]  {v3.weight0,v3.weight1,v3.weight2,v3.weight3 });

            //List<int> v1v2Bones = v1Bones.Intersect(v2Bones).ToList();
            /*for (int j = 0; j < 4; j++)
            {
                if (!v2Bones.Contains(v1Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v2Bones[k] == null)
                       // {
                            v2Bones[1] =v1Bones[j];
                            v2Weights[1] =0;
                        //    break;
                       // }
                    }

                }
                if (!v3Bones.Contains(v1Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v3Bones[k] == null)
                        //{
                            v3Bones[1] =v1Bones[j];
                            v3Weights[1] =0;
                        //    break;
                       // }
                    }
                    
                }
                if (!v3Bones.Contains(v2Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v3Bones[k] == null)
                        //{
                            v3Bones[1] =v2Bones[j];
                            v3Weights[1] =0;
                         //   break;
                       // }
                    }                    
                }
                                    
                if (!v1Bones.Contains(v2Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v1Bones[k] == null)
                        //{
                            v1Bones[1] =v2Bones[j];
                            v1Weights[1] =0;
                        //    break;
                        //}
                    }
                    
                }
                if (!v1Bones.Contains(v3Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v1Bones[k] == null)
                        //{
                            v1Bones[1] =v3Bones[j];
                            v1Weights[1] =0;
                        //    break;
                       // }
                    }
                    
                }
                if (!v2Bones.Contains(v3Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v2Bones[k] == null)
                        //{
                            v2Bones[1] =v3Bones[j];
                            v2Weights[1] =0;
                         //   break;
                        //}
                    }                    
                }
                
            }*/
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    if (v1Bones[j] == v2Bones[k])
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            if (v1Bones[j] == v3Bones[l])
                            {

                                v1Weights[j] =(v1Weights[j]+v2Weights[k]+v3Weights[l])/3;
                                v2Weights[k] = (v1Weights[j]+v2Weights[k]+v3Weights[l])/3;
                                v3Weights[l] = (v1Weights[j]+v2Weights[k]+v3Weights[l])/3;


                            }
                        }
                    }
                }/*
                for (int k = 0; k < 2; k++)
                {
                    if (v1Bones[j] == v3Bones[k])
                    {
                        v1Weights[j] =(v1Weights[j]+v3Weights[k])/2;
                        v3Weights[k] = (v1Weights[j]+v3Weights[k])/2;
                    }

                }
                for (int k = 0; k < 2; k++)
                {
                    if (v2Bones[j] == v3Bones[k])
                    {                       
                        v2Weights[j] =(v2Weights[j]+v3Weights[k])/2;
                        v3Weights[k] = (v2Weights[j]+v3Weights[k])/2;
                    }
                    
                }*/

            }
            boneWeights[triangles[i]].weight0 = v1Weights[0];
            boneWeights[triangles[i]].weight1 = v1Weights[1];
            //boneWeights[triangles[i]].weight2 = v1Weights[2];
            //boneWeights[triangles[i]].weight3 = v1Weights[3];

            boneWeights[triangles[i+1]].weight0 = v2Weights[0];
            boneWeights[triangles[i+1]].weight1 = v2Weights[1];
            //boneWeights[triangles[i+1]].weight2 = v2Weights[2];
            //boneWeights[triangles[i+1]].weight3 = v2Weights[3];

            boneWeights[triangles[i+2]].weight0 = v3Weights[0];
            boneWeights[triangles[i+2]].weight1 = v3Weights[1];
            //boneWeights[triangles[i+2]].weight2 = v3Weights[2];
            //boneWeights[triangles[i+2]].weight3 = v3Weights[3];

        }
        sharedMesh.boneWeights = boneWeights;
        return sharedMesh;
    }

	void ChangeBoneSize ()
	{
		string path = ("Assets/Puppet2D/Textures/GUI/BoneNoJoint.psd");
		Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = BoneSize;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

	}

	void ChangeControlSize ()
	{
		string path = ("Assets/Puppet2D/Textures/GUI/IKControl.psd");
		Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = ControlSize;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		path = ("Assets/Puppet2D/Textures/GUI/orientControl.psd");
		sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = ControlSize;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		path = ("Assets/Puppet2D/Textures/GUI/parentControl.psd");
		sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = ControlSize;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

	}

	void ChangeVertexHandleSize ()
	{
		string path = ("Assets/Puppet2D/Textures/GUI/VertexHandle.psd");
		Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = VertexHandleSize;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
	}
    
    static public void AddNewSortingName() 
    {
        object newName= new object();

        var internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        //string[] stuff = (string[])sortingLayersProperty.GetValue(null, new object[0]);
        string[] stuff = (string[])sortingLayersProperty.GetValue(null, new object[0]);

        sortingLayersProperty.SetValue(null, newName,new object[stuff.Length]);
    }

    public string[] GetSortingLayerNames() 
    {
        var internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        //string[] stuff = (string[])sortingLayersProperty.GetValue(null, new object[0]);

        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
}
