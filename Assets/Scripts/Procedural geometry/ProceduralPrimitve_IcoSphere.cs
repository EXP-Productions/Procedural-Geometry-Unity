using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralPrimitve_IcoSphere : ProceduralPrimitive
{
	private struct TriangleIndices
	{
		public int v1;
		public int v2;
		public int v3;
		
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}
	}

	public int m_RecursionLevel = 3;
	public float RecursionLevel {	set {	m_RecursionLevel = (int)value; IsDirty = true;	}	}

	public float m_Radius = 1f;
	public float Radius {	set {	m_Radius = value; IsDirty = true;	}	}

	[ContextMenu("Generate mesh")]
	public override void GenerateMesh()
	{		
		base.GenerateMesh ();

		List<Vector3> vertList = new List<Vector3>();
		Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();
		int index = 0;

		// create 12 vertices of a icosahedron
		float t = (1f + Mathf.Sqrt(5f)) / 2f;
		
		vertList.Add(new Vector3(-1f,  t,  0f).normalized * m_Radius);
		vertList.Add(new Vector3( 1f,  t,  0f).normalized * m_Radius);
		vertList.Add(new Vector3(-1f, -t,  0f).normalized * m_Radius);
		vertList.Add(new Vector3( 1f, -t,  0f).normalized * m_Radius);
		
		vertList.Add(new Vector3( 0f, -1f,  t).normalized * m_Radius);
		vertList.Add(new Vector3( 0f,  1f,  t).normalized * m_Radius);
		vertList.Add(new Vector3( 0f, -1f, -t).normalized * m_Radius);
		vertList.Add(new Vector3( 0f,  1f, -t).normalized * m_Radius);
		
		vertList.Add(new Vector3( t,  0f, -1f).normalized * m_Radius);
		vertList.Add(new Vector3( t,  0f,  1f).normalized * m_Radius);
		vertList.Add(new Vector3(-t,  0f, -1f).normalized * m_Radius);
		vertList.Add(new Vector3(-t,  0f,  1f).normalized * m_Radius);
		
		
		// create 20 triangles of the icosahedron
		List<TriangleIndices> faces = new List<TriangleIndices>();
		
		// 5 faces around point 0
		faces.Add(new TriangleIndices(0, 11, 5));
		faces.Add(new TriangleIndices(0, 5, 1));
		faces.Add(new TriangleIndices(0, 1, 7));
		faces.Add(new TriangleIndices(0, 7, 10));
		faces.Add(new TriangleIndices(0, 10, 11));
		
		// 5 adjacent faces 
		faces.Add(new TriangleIndices(1, 5, 9));
		faces.Add(new TriangleIndices(5, 11, 4));
		faces.Add(new TriangleIndices(11, 10, 2));
		faces.Add(new TriangleIndices(10, 7, 6));
		faces.Add(new TriangleIndices(7, 1, 8));
		
		// 5 faces around point 3
		faces.Add(new TriangleIndices(3, 9, 4));
		faces.Add(new TriangleIndices(3, 4, 2));
		faces.Add(new TriangleIndices(3, 2, 6));
		faces.Add(new TriangleIndices(3, 6, 8));
		faces.Add(new TriangleIndices(3, 8, 9));
		
		// 5 adjacent faces 
		faces.Add(new TriangleIndices(4, 9, 5));
		faces.Add(new TriangleIndices(2, 4, 11));
		faces.Add(new TriangleIndices(6, 2, 10));
		faces.Add(new TriangleIndices(8, 6, 7));
		faces.Add(new TriangleIndices(9, 8, 1));
		
		
		// refine triangles
		for (int i = 0; i < m_RecursionLevel; i++)
		{
			List<TriangleIndices> faces2 = new List<TriangleIndices>();
			foreach (var tri in faces)
			{
				// replace triangle by 4 triangles
				int a = getMiddlePoint(tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, m_Radius);
				int b = getMiddlePoint(tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, m_Radius);
				int c = getMiddlePoint(tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, m_Radius);
				
				faces2.Add(new TriangleIndices(tri.v1, a, c));
				faces2.Add(new TriangleIndices(tri.v2, b, a));
				faces2.Add(new TriangleIndices(tri.v3, c, b));
				faces2.Add(new TriangleIndices(a, b, c));
			}
			faces = faces2;
		}
		
		m_Verts = vertList.ToArray();
		
		List< int > triList = new List<int>();
		for( int i = 0; i < faces.Count; i++ )
		{
			triList.Add( faces[i].v1 );
			triList.Add( faces[i].v2 );
			triList.Add( faces[i].v3 );
		}		
		m_Tris = triList.ToArray();
		m_UVs = new Vector2[ m_Verts.Length ];
		
		m_Normals = new Vector3[ vertList.Count];
		for( int i = 0; i < m_Normals.Length; i++ )
			m_Normals[i] = vertList[i].normalized;

		UpdateMeshVariables ();
	}

	// return index of point in the middle of p1 and p2
	private static int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
	{
		// first check if we have it already
		bool firstIsSmaller = p1 < p2;
		long smallerIndex = firstIsSmaller ? p1 : p2;
		long greaterIndex = firstIsSmaller ? p2 : p1;
		long key = (smallerIndex << 32) + greaterIndex;
		
		int ret;
		if (cache.TryGetValue(key, out ret))
		{
			return ret;
		}
		
		// not in cache, calculate it
		Vector3 point1 = vertices[p1];
		Vector3 point2 = vertices[p2];
		Vector3 middle = new Vector3
			(
				(point1.x + point2.x) / 2f, 
				(point1.y + point2.y) / 2f, 
				(point1.z + point2.z) / 2f
				);
		
		// add vertex makes sure point is on unit sphere
		int i = vertices.Count;
		vertices.Add( middle.normalized * radius ); 
		
		// store it, return index
		cache.Add(key, i);
		
		return i;
	}
}
