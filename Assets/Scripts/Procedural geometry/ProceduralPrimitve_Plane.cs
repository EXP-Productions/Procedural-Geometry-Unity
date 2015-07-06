using UnityEngine;
using System.Collections;

public class ProceduralPrimitve_Plane : ProceduralPrimitive 
{
	public float 	m_Length = 1f;
	public float 	Length {	set {	m_Length = value; IsDirty = true;	}	}

	public float 	m_Width = 1f;
	public float 	Width {	set {	m_Width = value; IsDirty = true;	}	}

	public int 		m_ResX = 2; // 2 minimum
	public int 	ResX {	set {	m_ResX = value; IsDirty = true;	}	}

	public int 		m_ResZ = 2;
	public int 	ResZ {	set {	m_ResZ = value; IsDirty = true;	}	}

	public override void GenerateMesh()
	{
		base.GenerateMesh ();

		print ( name + " Generating mesh");

		m_ResX = Mathf.Max (m_ResX, 2);
		m_ResZ = Mathf.Max (m_ResZ, 2);

		#region Vertices		
		m_Verts = new Vector3[ m_ResX * m_ResZ ];
		for(int z = 0; z < m_ResZ; z++)
		{
			// [ -length / 2, length / 2 ]
			float zPos = ((float)z / (m_ResZ - 1) - .5f) * m_Length;
			for(int x = 0; x < m_ResX; x++)
			{
				// [ -width / 2, width / 2 ]
				float xPos = ((float)x / (m_ResX - 1) - .5f) * m_Width;
				m_Verts[ x + z * m_ResX ] = new Vector3( xPos, 0f, zPos );
			}
		}
		#endregion
		
		#region Normales
		m_Normals = new Vector3[ m_Verts.Length ];
		for( int n = 0; n < m_Normals.Length; n++ )
			m_Normals[n] = Vector3.up;
		#endregion
		
		#region UVs		
		Vector2[] uvs = new Vector2[ m_Verts.Length ];
		for(int v = 0; v < m_ResZ; v++)
		{
			for(int u = 0; u < m_ResX; u++)
			{
				uvs[ u + v * m_ResX ] = new Vector2( (float)u / (m_ResX - 1), (float)v / (m_ResZ - 1) );
			}
		}
		#endregion
		
		#region Triangles
		int nbFaces = (m_ResX - 1) * (m_ResZ - 1);
		m_Tris = new int[ nbFaces * 6 ];
		int t = 0;
		for(int face = 0; face < nbFaces; face++ )
		{
			// Retrieve lower left corner from face ind
			int i = face % (m_ResX - 1) + (face / (m_ResZ - 1) * m_ResX);
			
			m_Tris[t++] = i + m_ResX;
			m_Tris[t++] = i + 1;
			m_Tris[t++] = i;
			
			m_Tris[t++] = i + m_ResX;	
			m_Tris[t++] = i + m_ResX + 1;
			m_Tris[t++] = i + 1; 
		}
		#endregion

		UpdateMeshVariables ();
	}
}
