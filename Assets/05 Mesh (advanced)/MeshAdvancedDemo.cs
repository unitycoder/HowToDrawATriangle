﻿/*
	Copyright © Carl Emil Carlsen 2020-2022
	http://cec.dk

	Using the new Mash API introduced in 2020.
*/

using UnityEngine;
using UnityEngine.Rendering;

public class MeshAdvancedDemo : MonoBehaviour
{
	Vector3[] _vertices;
	Mesh _mesh;
	Material _material;

	const MeshUpdateFlags meshFlags = MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices;


	void Awake()
	{
		// Create vertices.
		_vertices = new Vector3[]{
			Quaternion.AngleAxis( 0/3f * 360, Vector3.forward ) *  Vector3.up * 0.5f,
			Quaternion.AngleAxis( 2/3f * 360, Vector3.forward ) *  Vector3.up * 0.5f,
			Quaternion.AngleAxis( 1/3f * 360, Vector3.forward ) *  Vector3.up * 0.5f,
		};

		// Create mesh and markt it dynamic, to tell Unity that we will update the verticies continously.
		_mesh = new Mesh();
		_mesh.MarkDynamic();

		// Set the index parameters. UInt16 will allow 65535 indices. If you need more, use UInt32.
		_mesh.SetIndexBufferParams( 3, IndexFormat.UInt16 );

		// Set the vertex parameters (the data layout for a one vertex). For the yellow triangle, we just need positions.
		VertexAttributeDescriptor[] vertexDataLayout = new VertexAttributeDescriptor[] {
			new VertexAttributeDescriptor( VertexAttribute.Position, VertexAttributeFormat.Float32, 3 )
		};
		_mesh.SetVertexBufferParams( 3, vertexDataLayout );

		// Set the sub mesh descriptor.
		SubMeshDescriptor meshDescriptor = new SubMeshDescriptor( 0, 3, MeshTopology.Triangles );
		_mesh.SetSubMesh( 0, meshDescriptor, meshFlags );

		// Set bounds. If you vertices move a lot, you may want to change this in Update.
		_mesh.bounds = new Bounds( Vector3.zero, Vector3.one );

		// Set indices.
		_mesh.SetIndexBufferData( new ushort[]{ 0, 1, 2 }, 0, 0, 3, meshFlags );

		// Create material (from shader located in Resources folder).
		_material = new Material( Shader.Find( "Hidden/" + GetType().Name ) );
	}


	void OnDestroy()
	{
		Destroy( _material );
	}


	void Update()
	{
		// Update vertices.
		Quaternion rotation = Quaternion.Euler( 0, 0, 360/8f * Time.deltaTime );
		for( int v = 0; v < _vertices.Length; v++ ) _vertices[v] = rotation * _vertices[v];

		// Apply to mesh.
		_mesh.SetVertexBufferData( _vertices, 0, 0, _vertices.Length, 0, meshFlags );

		// Draw.
		Graphics.DrawMesh( _mesh, Matrix4x4.identity, _material, gameObject.layer );
	}
}