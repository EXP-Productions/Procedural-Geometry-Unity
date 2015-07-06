using UnityEngine;
using System.Collections;

public class ProceduralPrimitve_Cone : ProceduralPrimitive 
{
	public float m_Height = 1f;
	public float Height {	set {	m_Height = value; IsDirty = true;	}	}

	public float m_BtmRadius = .25f;
	public float BtmRadius {	set {	m_BtmRadius = value; IsDirty = true;	}	}

	public float m_TopRadius = .05f;
	public float TopRadius {	set {	m_TopRadius = value; IsDirty = true;	}	}

	public int m_Sides = 18;
	public int Sides {	set {	m_Sides = value; IsDirty = true;	}	}

	public int m_HeightSegments = 1; // Not implemented yet
	public int HeightSegments {	set {	m_HeightSegments = value; IsDirty = true;	}	}

	[ContextMenu("Generate mesh")]
	public override void GenerateMesh()
	{
		base.GenerateMesh ();

		int nbVerticesCap = m_Sides + 1;
		#region Vertices
		
		// bottom + top + sides
		m_Verts = new Vector3[nbVerticesCap + nbVerticesCap + m_Sides * m_HeightSegments * 2 + 2];
		int vert = 0;
		float _2pi = Mathf.PI * 2f;
		
		// Bottom cap
		m_Verts[vert++] = new Vector3(0f, 0f, 0f);
		while( vert <= m_Sides )
		{
			float rad = (float)vert / m_Sides * _2pi;
			m_Verts[vert] = new Vector3(Mathf.Cos(rad) * m_BtmRadius, 0f, Mathf.Sin(rad) * m_BtmRadius);
			vert++;
		}
		
		// Top cap
		m_Verts[vert++] = new Vector3(0f, m_Height, 0f);
		while (vert <= m_Sides * 2 + 1)
		{
			float rad = (float)(vert - m_Sides - 1)  / m_Sides * _2pi;
			m_Verts[vert] = new Vector3(Mathf.Cos(rad) * m_TopRadius, m_Height, Mathf.Sin(rad) * m_TopRadius);
			vert++;
		}
		
		// Sides
		int v = 0;
		while (vert <= m_Verts.Length - 4 )
		{
			float rad = (float)v / m_Sides * _2pi;
			m_Verts[vert] = new Vector3(Mathf.Cos(rad) * m_TopRadius, m_Height, Mathf.Sin(rad) * m_TopRadius);
			m_Verts[vert + 1] = new Vector3(Mathf.Cos(rad) * m_BtmRadius, 0, Mathf.Sin(rad) * m_BtmRadius);
			vert+=2;
			v++;
		}
		m_Verts[vert] = m_Verts[ m_Sides * 2 + 2 ];
		m_Verts[vert + 1] = m_Verts[m_Sides * 2 + 3 ];
		#endregion
		
		#region normals
		
		// bottom + top + sides
		m_Normals = new Vector3[m_Verts.Length];
		vert = 0;
		
		// Bottom cap
		while( vert  <= m_Sides )
		{
			m_Normals[vert++] = Vector3.down;
		}
		
		// Top cap
		while( vert <= m_Sides * 2 + 1 )
		{
			m_Normals[vert++] = Vector3.up;
		}
		
		// Sides
		v = 0;
		while (vert <= m_Verts.Length - 4 )
		{			
			float rad = (float)v / m_Sides * _2pi;
			float cos = Mathf.Cos(rad);
			float sin = Mathf.Sin(rad);
			
			m_Normals[vert] = new Vector3(cos, 0f, sin);
			m_Normals[vert+1] = m_Normals[vert];
			
			vert+=2;
			v++;
		}
		m_Normals[vert] = m_Normals[ m_Sides * 2 + 2 ];
		m_Normals[vert + 1] = m_Normals[m_Sides * 2 + 3 ];
		#endregion
		
		#region UVs
		m_UVs = new Vector2[m_Verts.Length];
		
		// Bottom cap
		int u = 0;
		m_UVs[u++] = new Vector2(0.5f, 0.5f);
		while (u <= m_Sides)
		{
			float rad = (float)u / m_Sides * _2pi;
			m_UVs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			u++;
		}
		
		// Top cap
		m_UVs[u++] = new Vector2(0.5f, 0.5f);
		while (u <= m_Sides * 2 + 1)
		{
			float rad = (float)u / m_Sides * _2pi;
			m_UVs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			u++;
		}
		
		// Sides
		int u_sides = 0;
		while (u <= m_UVs.Length - 4 )
		{
			float t = (float)u_sides / m_Sides;
			m_UVs[u] = new Vector3(t, 1f);
			m_UVs[u + 1] = new Vector3(t, 0f);
			u += 2;
			u_sides++;
		}
		m_UVs[u] = new Vector2(1f, 1f);
		m_UVs[u + 1] = new Vector2(1f, 0f);
		#endregion 
		
		#region Triangles
		int nbTriangles = m_Sides + m_Sides + m_Sides*2;
		m_Tris = new int[nbTriangles * 3 + 3];
		
		// Bottom cap
		int tri = 0;
		int i = 0;
		while (tri < m_Sides - 1)
		{
			m_Tris[ i ] = 0;
			m_Tris[ i+1 ] = tri + 1;
			m_Tris[ i+2 ] = tri + 2;
			tri++;
			i += 3;
		}
		m_Tris[i] = 0;
		m_Tris[i + 1] = tri + 1;
		m_Tris[i + 2] = 1;
		tri++;
		i += 3;
		
		// Top cap
		//tri++;
		while (tri < m_Sides*2)
		{
			m_Tris[ i ] = tri + 2;
			m_Tris[i + 1] = tri + 1;
			m_Tris[i + 2] = nbVerticesCap;
			tri++;
			i += 3;
		}
		
		m_Tris[i] = nbVerticesCap + 1;
		m_Tris[i + 1] = tri + 1;
		m_Tris[i + 2] = nbVerticesCap;		
		tri++;
		i += 3;
		tri++;
		
		// Sides
		while( tri <= nbTriangles )
		{
			m_Tris[ i ] = tri + 2;
			m_Tris[ i+1 ] = tri + 1;
			m_Tris[ i+2 ] = tri + 0;
			tri++;
			i += 3;
			
			m_Tris[ i ] = tri + 1;
			m_Tris[ i+1 ] = tri + 2;
			m_Tris[ i+2 ] = tri + 0;
			tri++;
			i += 3;
		}
		#endregion
		UpdateMeshVariables ();
	}
}
