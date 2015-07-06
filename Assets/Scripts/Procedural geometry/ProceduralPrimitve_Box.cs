using UnityEngine;
using System.Collections;

/// <summary>
/// Procedural primitives.
/// An implimentation of the code found on the Unify community wiki
/// http://wiki.unity3d.com/index.php/ProceduralPrimitives
/// 
/// </summary>
/// 
public class ProceduralPrimitve_Box : ProceduralPrimitive
{
	public float m_Length = 1f;
	public float Length {	set {	m_Length = value; IsDirty = true;	}	}

	public float m_Width = 1f;
	public float Width {	set {	m_Width = value; IsDirty = true;	}	}

	public float m_Height = 1f;
	public float Height {	set {	m_Height = value; IsDirty = true;	}	}

	[ContextMenu("Generate mesh")]
	public override void GenerateMesh()
	{		
		base.GenerateMesh ();

		#region Vertices
		Vector3 p0 = new Vector3( -m_Length * .5f,	-m_Width * .5f, m_Height * .5f );
		Vector3 p1 = new Vector3( m_Length * .5f, 	-m_Width * .5f, m_Height * .5f );
		Vector3 p2 = new Vector3( m_Length * .5f, 	-m_Width * .5f, -m_Height * .5f );
		Vector3 p3 = new Vector3( -m_Length * .5f,	-m_Width * .5f, -m_Height * .5f );	
		
		Vector3 p4 = new Vector3( -m_Length * .5f,	m_Width * .5f,  m_Height * .5f );
		Vector3 p5 = new Vector3( m_Length * .5f, 	m_Width * .5f,  m_Height * .5f );
		Vector3 p6 = new Vector3( m_Length * .5f, 	m_Width * .5f,  -m_Height * .5f );
		Vector3 p7 = new Vector3( -m_Length * .5f,	m_Width * .5f,  -m_Height * .5f );
		
		m_Verts = new Vector3[]
		{
			// Bottom
			p0, p1, p2, p3,
			
			// Left
			p7, p4, p0, p3,
			
			// Front
			p4, p5, p1, p0,
			
			// Back
			p6, p7, p3, p2,
			
			// Right
			p5, p6, p2, p1,
			
			// Top
			p7, p6, p5, p4
		};
		#endregion
		
		#region normals
		Vector3 up 	= Vector3.up;
		Vector3 down 	= Vector3.down;
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;
		
		m_Normals = new Vector3[]
		{
			// Bottom
			down, down, down, down,
			
			// Left
			left, left, left, left,
			
			// Front
			front, front, front, front,
			
			// Back
			back, back, back, back,
			
			// Right
			right, right, right, right,
			
			// Top
			up, up, up, up
		};
		#endregion	
		
		#region UVs
		Vector2 _00 = new Vector2( 0f, 0f );
		Vector2 _10 = new Vector2( 1f, 0f );
		Vector2 _01 = new Vector2( 0f, 1f );
		Vector2 _11 = new Vector2( 1f, 1f );
		
		m_UVs = new Vector2[]
		{
			// Bottom
			_11, _01, _00, _10,
			
			// Left
			_11, _01, _00, _10,
			
			// Front
			_11, _01, _00, _10,
			
			// Back
			_11, _01, _00, _10,
			
			// Right
			_11, _01, _00, _10,
			
			// Top
			_11, _01, _00, _10,
		};
		#endregion
		
		#region Triangles
		m_Tris = new int[]
		{
			// Bottom
			3, 1, 0,
			3, 2, 1,			
			
			// Left
			3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
			3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
			
			// Front
			3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
			3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
			
			// Back
			3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
			3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
			
			// Right
			3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
			3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
			
			// Top
			3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
			3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
			
		};
		#endregion

		UpdateMeshVariables ();
	}
}
