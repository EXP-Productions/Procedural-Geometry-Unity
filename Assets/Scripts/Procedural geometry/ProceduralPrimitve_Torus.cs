using UnityEngine;
using System.Collections;

public class ProceduralPrimitve_Torus : ProceduralPrimitive
{
	public float m_RadiusOuter = 1f;
	public float RadiusOuter {	set {	m_RadiusOuter = value; IsDirty = true;	}	}

	public float m_RadiusInner = .3f;
	public float RadiusInner {	set {	m_RadiusInner = value; IsDirty = true;	}	}

	public int m_RadialSegments = 24;
	public int RadialSegments {	set {	m_RadialSegments = value; IsDirty = true;	}	}

	public int m_Sides = 18;
	public int Sides {	set {	m_Sides = value; IsDirty = true;	}	}

	[ContextMenu("Generate mesh")]
	public override void GenerateMesh()
	{
		base.GenerateMesh ();

		#region Vertices		
		m_Verts = new Vector3[(m_RadialSegments+1) * (m_Sides+1)];
		float _2pi = Mathf.PI * 2f;
		for( int seg = 0; seg <= m_RadialSegments; seg++ )
		{
			int currSeg = seg  == m_RadialSegments ? 0 : seg;
			
			float t1 = (float)currSeg / m_RadialSegments * _2pi;
			Vector3 r1 = new Vector3( Mathf.Cos(t1) * m_RadiusOuter, 0f, Mathf.Sin(t1) * m_RadiusOuter );
			
			for( int side = 0; side <= m_Sides; side++ )
			{
				int currSide = side == m_Sides ? 0 : side;
				
				Vector3 normale = Vector3.Cross( r1, Vector3.up );
				float t2 = (float)currSide / m_Sides * _2pi;
				Vector3 r2 = Quaternion.AngleAxis( -t1 * Mathf.Rad2Deg, Vector3.up ) *new Vector3( Mathf.Sin(t2) * m_RadiusInner, Mathf.Cos(t2) * m_RadiusInner );
				
				m_Verts[side + seg * (m_Sides+1)] = r1 + r2;
			}
		}
		#endregion
		
		#region normals		
		m_Normals = new Vector3[m_Verts.Length];
		for( int seg = 0; seg <= m_RadialSegments; seg++ )
		{
			int currSeg = seg  == m_RadialSegments ? 0 : seg;
			
			float t1 = (float)currSeg / m_RadialSegments * _2pi;
			Vector3 r1 = new Vector3( Mathf.Cos(t1) * m_RadiusOuter, 0f, Mathf.Sin(t1) * m_RadiusOuter );
			
			for( int side = 0; side <= m_Sides; side++ )
			{
				m_Normals[side + seg * (m_Sides+1)] = (m_Verts[side + seg * (m_Sides+1)] - r1).normalized;
			}
		}
		#endregion
		
		#region UVs
		m_UVs = new Vector2[m_Verts.Length];
		for( int seg = 0; seg <= m_RadialSegments; seg++ )
			for( int side = 0; side <= m_Sides; side++ )
				m_UVs[side + seg * (m_Sides+1)] = new Vector2( (float)seg / m_RadialSegments, (float)side / m_Sides );
		#endregion
		
		#region Triangles
		int nbFaces = m_Verts.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		m_Tris = new int[ nbIndexes ];
		
		int i = 0;
		for( int seg = 0; seg <= m_RadialSegments; seg++ )
		{			
			for( int side = 0; side <= m_Sides - 1; side++ )
			{
				int current = side + seg * (m_Sides+1);
				int next = side + (seg < (m_RadialSegments) ?(seg+1) * (m_Sides+1) : 0);
				
				if( i < m_Tris.Length - 6 )
				{
					m_Tris[i++] = current;
					m_Tris[i++] = next;
					m_Tris[i++] = next+1;
					
					m_Tris[i++] = current;
					m_Tris[i++] = next+1;
					m_Tris[i++] = current+1;
				}
			}
		}
		#endregion

		UpdateMeshVariables ();
	}
}
