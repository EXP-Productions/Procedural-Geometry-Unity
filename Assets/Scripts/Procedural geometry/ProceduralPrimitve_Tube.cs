using UnityEngine;
using System.Collections;

public class ProceduralPrimitve_Tube : ProceduralPrimitive
{
	public float 	m_Height = 1f;
	public float 	Height {	set {	m_Height = value; IsDirty = true;	}	}

	public int 		m_Sides = 24;
	public int 		Sides {		set {	m_Sides = value; IsDirty = true;	}	}

	[ContextMenu("Generate mesh")]
	public override void GenerateMesh()
	{
		base.GenerateMesh ();

		// Outter shell is at radius1 + radius2 / 2, inner shell at radius1 - radius2 / 2
		float bottomRadius1 = .5f;
		float bottomRadius2 = .15f; 
		float topRadius1 = .5f;
		float topRadius2 = .15f;
		
		int nbVerticesCap = m_Sides * 2 + 2;
		int nbVerticesSides = m_Sides * 2 + 2;
		#region Vertices
		
		// bottom + top + sides
		m_Verts = new Vector3[nbVerticesCap * 2 + nbVerticesSides * 2];
		int vert = 0;
		float _2pi = Mathf.PI * 2f;
		
		// Bottom cap
		int sideCounter = 0;
		while( vert < nbVerticesCap )
		{
			sideCounter = sideCounter == m_Sides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / m_Sides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			m_Verts[vert] = new Vector3( cos * (bottomRadius1 - bottomRadius2 * .5f), 0f, sin * (bottomRadius1 - bottomRadius2 * .5f));
			m_Verts[vert+1] = new Vector3( cos * (bottomRadius1 + bottomRadius2 * .5f), 0f, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert += 2;
		}
		
		// Top cap
		sideCounter = 0;
		while( vert < nbVerticesCap * 2 )
		{
			sideCounter = sideCounter == m_Sides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / m_Sides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			m_Verts[vert] = new Vector3( cos * (topRadius1 - topRadius2 * .5f), m_Height, sin * (topRadius1 - topRadius2 * .5f));
			m_Verts[vert+1] = new Vector3( cos * (topRadius1 + topRadius2 * .5f), m_Height, sin * (topRadius1 + topRadius2 * .5f));
			vert += 2;
		}
		
		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides )
		{
			sideCounter = sideCounter == m_Sides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / m_Sides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			
			m_Verts[vert] = new Vector3(cos * (topRadius1 + topRadius2 * .5f), m_Height, sin * (topRadius1 + topRadius2 * .5f));
			m_Verts[vert + 1] = new Vector3(cos * (bottomRadius1 + bottomRadius2 * .5f), 0, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert+=2;
		}
		
		// Sides (in)
		sideCounter = 0;
		while (vert < m_Verts.Length )
		{
			sideCounter = sideCounter == m_Sides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / m_Sides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			
			m_Verts[vert] = new Vector3(cos * (topRadius1 - topRadius2 * .5f), m_Height, sin * (topRadius1 - topRadius2 * .5f));
			m_Verts[vert + 1] = new Vector3(cos * (bottomRadius1 - bottomRadius2 * .5f), 0, sin * (bottomRadius1 - bottomRadius2 * .5f));
			vert += 2;
		}
		#endregion
		
		#region normals
		
		// bottom + top + sides
		m_Normals = new Vector3[m_Verts.Length];
		vert = 0;
		
		// Bottom cap
		while( vert < nbVerticesCap )
		{
			m_Normals[vert++] = Vector3.down;
		}
		
		// Top cap
		while( vert < nbVerticesCap * 2 )
		{
			m_Normals[vert++] = Vector3.up;
		}
		
		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides )
		{
			sideCounter = sideCounter == m_Sides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / m_Sides * _2pi;
			
			m_Normals[vert] = new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1));
			m_Normals[vert+1] = m_Normals[vert];
			vert+=2;
		}
		
		// Sides (in)
		sideCounter = 0;
		while (vert < m_Verts.Length )
		{
			sideCounter = sideCounter == m_Sides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / m_Sides * _2pi;
			
			m_Normals[vert] = -(new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1)));
			m_Normals[vert+1] = m_Normals[vert];
			vert+=2;
		}
		#endregion
		
		#region UVs
		m_UVs = new Vector2[m_Verts.Length];
		
		vert = 0;
		// Bottom cap
		sideCounter = 0;
		while( vert < nbVerticesCap )
		{
			float t = (float)(sideCounter++) / m_Sides;
			m_UVs[ vert++ ] = new Vector2( 0f, t );
			m_UVs[ vert++ ] = new Vector2( 1f, t );
		}
		
		// Top cap
		sideCounter = 0;
		while( vert < nbVerticesCap * 2 )
		{
			float t = (float)(sideCounter++) / m_Sides;
			m_UVs[ vert++ ] = new Vector2( 0f, t );
			m_UVs[ vert++ ] = new Vector2( 1f, t );
		}
		
		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides )
		{
			float t = (float)(sideCounter++) / m_Sides;
			m_UVs[ vert++ ] = new Vector2( t, 0f );
			m_UVs[ vert++ ] = new Vector2( t, 1f );
		}
		
		// Sides (in)
		sideCounter = 0;
		while (vert < m_Verts.Length )
		{
			float t = (float)(sideCounter++) / m_Sides;
			m_UVs[ vert++ ] = new Vector2( t, 0f );
			m_UVs[ vert++ ] = new Vector2( t, 1f );
		}
		#endregion
		
		#region Triangles
		int nbFace = m_Sides * 4;
		int nbTriangles = nbFace * 2;
		int nbIndexes = nbTriangles * 3;
		m_Tris = new int[nbIndexes];
		
		// Bottom cap
		int i = 0;
		sideCounter = 0;
		while (sideCounter < m_Sides)
		{
			int current = sideCounter * 2;
			int next = sideCounter * 2 + 2;
			
			m_Tris[ i++ ] = next + 1;
			m_Tris[ i++ ] = next;
			m_Tris[ i++ ] = current;
			
			m_Tris[ i++ ] = current + 1;
			m_Tris[ i++ ] = next + 1;
			m_Tris[ i++ ] = current;
			
			sideCounter++;
		}
		
		// Top cap
		while (sideCounter < m_Sides * 2)
		{
			int current = sideCounter * 2 + 2;
			int next = sideCounter * 2 + 4;
			
			m_Tris[ i++ ] = current;
			m_Tris[ i++ ] = next;
			m_Tris[ i++ ] = next + 1;
			
			m_Tris[ i++ ] = current;
			m_Tris[ i++ ] = next + 1;
			m_Tris[ i++ ] = current + 1;
			
			sideCounter++;
		}
		
		// Sides (out)
		while( sideCounter < m_Sides * 3 )
		{
			int current = sideCounter * 2 + 4;
			int next = sideCounter * 2 + 6;
			
			m_Tris[ i++ ] = current;
			m_Tris[ i++ ] = next;
			m_Tris[ i++ ] = next + 1;
			
			m_Tris[ i++ ] = current;
			m_Tris[ i++ ] = next + 1;
			m_Tris[ i++ ] = current + 1;
			
			sideCounter++;
		}
		
		
		// Sides (in)
		while( sideCounter < m_Sides * 4 )
		{
			int current = sideCounter * 2 + 6;
			int next = sideCounter * 2 + 8;
			
			m_Tris[ i++ ] = next + 1;
			m_Tris[ i++ ] = next;
			m_Tris[ i++ ] = current;
			
			m_Tris[ i++ ] = current + 1;
			m_Tris[ i++ ] = next + 1;
			m_Tris[ i++ ] = current;
			
			sideCounter++;
		}
		#endregion
		UpdateMeshVariables ();
	}
}
