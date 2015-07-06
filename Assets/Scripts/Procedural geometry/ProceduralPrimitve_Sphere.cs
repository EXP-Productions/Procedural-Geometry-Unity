using UnityEngine;
using System.Collections;

public class ProceduralPrimitve_Sphere : ProceduralPrimitive
{
	public float 	m_Radius = 1f;
	public float 	Radius {	set {	m_Radius = value; IsDirty = true;	}	}

	public int 	m_VerticalSections = 24;
	public int 	VerticalSections {	set {	m_VerticalSections = value; IsDirty = true;	}	}

	public int 		m_HorizontalSections = 16;
	public int 	HorizontalSections {	set {	m_HorizontalSections = value; IsDirty = true;	}	}

	[ContextMenu("Generate mesh")]
	public override void GenerateMesh()
	{		
		base.GenerateMesh ();

		m_VerticalSections = Mathf.Max (m_VerticalSections, 3);
		m_HorizontalSections = Mathf.Max (m_HorizontalSections, 3);

		#region Vertices
		m_Verts = new Vector3[(m_VerticalSections+1) * m_HorizontalSections + 2];
		float _pi = Mathf.PI;
		float _2pi = _pi * 2f;
		
		m_Verts[0] = Vector3.up * m_Radius;
		for( int lat = 0; lat < m_HorizontalSections; lat++ )
		{
			float a1 = _pi * (float)(lat+1) / (m_HorizontalSections+1);
			float sin1 = Mathf.Sin(a1);
			float cos1 = Mathf.Cos(a1);
			
			for( int lon = 0; lon <= m_VerticalSections; lon++ )
			{
				float a2 = _2pi * (float)(lon == m_VerticalSections ? 0 : lon) / m_VerticalSections;
				float sin2 = Mathf.Sin(a2);
				float cos2 = Mathf.Cos(a2);
				
				m_Verts[ lon + lat * (m_VerticalSections + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * m_Radius;
			}
		}
		m_Verts[m_Verts.Length-1] = Vector3.up * -m_Radius;
		#endregion
		
		#region Normales		
		m_Normals = new Vector3[m_Verts.Length];
		for( int n = 0; n < m_Verts.Length; n++ )
			m_Normals[n] = m_Verts[n].normalized;
		#endregion
		
		#region UVs
		m_UVs = new Vector2[m_Verts.Length];
		m_UVs[0] = Vector2.up;
		m_UVs[m_UVs.Length-1] = Vector2.zero;
		for( int lat = 0; lat < m_HorizontalSections; lat++ )
			for( int lon = 0; lon <= m_VerticalSections; lon++ )
				m_UVs[lon + lat * (m_VerticalSections + 1) + 1] = new Vector2( (float)lon / m_VerticalSections, 1f - (float)(lat+1) / (m_HorizontalSections+1) );
		#endregion
		
		#region Triangles
		int nbFaces = m_Verts.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		m_Tris = new int[ nbIndexes ];
		
		//Top Cap
		int i = 0;
		for( int lon = 0; lon < m_VerticalSections; lon++ )
		{
			m_Tris[i++] = lon+2;
			m_Tris[i++] = lon+1;
			m_Tris[i++] = 0;
		}
		
		//Middle
		for( int lat = 0; lat < m_HorizontalSections - 1; lat++ )
		{
			for( int lon = 0; lon < m_VerticalSections; lon++ )
			{
				int current = lon + lat * (m_VerticalSections + 1) + 1;
				int next = current + m_VerticalSections + 1;
				
				m_Tris[i++] = current;
				m_Tris[i++] = current + 1;
				m_Tris[i++] = next + 1;
				
				m_Tris[i++] = current;
				m_Tris[i++] = next + 1;
				m_Tris[i++] = next;
			}
		}
		
		//Bottom Cap
		for( int lon = 0; lon < m_VerticalSections; lon++ )
		{
			m_Tris[i++] = m_Verts.Length - 1;
			m_Tris[i++] = m_Verts.Length - (lon+2) - 1;
			m_Tris[i++] = m_Verts.Length - (lon+1) - 1;
		}
		#endregion	

		UpdateMeshVariables ();
	}
}
