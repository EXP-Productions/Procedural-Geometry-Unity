using UnityEngine;
using System.Collections;

/// <summary>
/// Procedural primitives.
/// An implimentation of the code found on the Unify community wiki
/// http://wiki.unity3d.com/index.php/ProceduralPrimitives
/// 
/// </summary>

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class ProceduralPrimitive : MonoBehaviour 
{
	MeshFilter 		m_Filter;
	Mesh 			m_Mesh;

	protected Vector3[] m_Verts;
	protected Vector3[] m_Normals;
	protected Vector2[] m_UVs;
	protected int[] 	m_Tris;

	protected bool 		IsDirty = false;
	protected bool		m_Initialized = false;

	void Awake()
	{
		Init ();
	}

	// Use this for initialization
	void Init ()
	{
		m_Filter = gameObject.GetComponent< MeshFilter >();
		m_Mesh = m_Filter.mesh;
		m_Mesh.Clear();

		IsDirty = true;
		m_Initialized = true;

		print (name + " initialized");
	}

	void Update()
	{
		if (IsDirty) 
		{
			GenerateMesh ();
		}
	}


	public virtual void GenerateMesh()
	{
		if( !m_Initialized )
			Init();
	}
	
	// Update is called once per frame
	protected void UpdateMeshVariables() 
	{
		print ( name + " updating mesh");

		m_Mesh.vertices = m_Verts;
		m_Mesh.normals = m_Normals;
		m_Mesh.uv = m_UVs;
		m_Mesh.triangles = m_Tris;
		
		m_Mesh.RecalculateBounds();
		m_Mesh.Optimize();

		m_Filter.mesh = m_Mesh;

		IsDirty = false;
	}
}
