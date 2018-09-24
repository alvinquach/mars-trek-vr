﻿using UnityEngine;
using BitMiracle.LibTiff.Classic;

public class SphericalTerrainMesh : TerrainMesh {

    //[SerializeField] private string _filePath;

    public override TerrainGeometryType SurfaceGeometryType {
        get { return TerrainGeometryType.Spherical; }
    }

    public override void InitMesh() {
        base.InitMesh();

        // Add a sphere collider to the mesh, so that it can be manipulated using the controller.
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = _scale;
    }

    protected override Mesh GenerateMesh(TiffTerrainMeshGenerator meshGenerator, int downsample) {
        return meshGenerator.GenerateSphericalMesh(_scale, _heightScale, downsample);
    }

}
