using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using HomeWork9;
using Unity.Jobs;
using Unity.Collections;
using System;

public class AsteroidField : MonoBehaviour
{
    public struct AsteroidPart
    {
        public Vector3 WorldPosition;
        public Quaternion WorldRotation;
    }

    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _radius;
    [SerializeField] private int _asteroidsCount;
    [SerializeField] private float _deltaOffset;
    [SerializeField] private float _scale;


    private const float _positionOffset = 1.5f;
    private const float _scaleBias = .5f;
    private const int _childCount = 5;

    private NativeArray<Matrix4x4> _matrices;
    private ComputeBuffer _matricesBuffer;

    private static readonly int _matricesId = Shader.PropertyToID("_Matrices");
    private static MaterialPropertyBlock _propertyBlock;


    private void OnEnable()
    {
        _matrices = new NativeArray<Matrix4x4>(_asteroidsCount, Allocator.Persistent);

        var stride = 16 * 4;
        _matricesBuffer = new ComputeBuffer(_asteroidsCount, stride);

        _propertyBlock ??= new MaterialPropertyBlock();


        float singleAngle = 2 * Mathf.PI / _asteroidsCount;
        float angle = 0;

        for (int i = 0; i < _asteroidsCount; i++)
        {
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            var position = new Vector3()
            {
                x = x * _radius + GetRandomOffset(),
                y = GetRandomOffset(),
                z = y * _radius + GetRandomOffset()
            };
            _matrices[i] = Matrix4x4.TRS(position, UnityEngine.Random.rotation, _scale * Vector3.one);
            angle += singleAngle;
        }

    }

    private float GetRandomOffset()
    {
        return UnityEngine.Random.value * _deltaOffset;
    }

    private void OnDisable()
    {
        _matricesBuffer.Release();
        _matrices.Dispose();
        _matricesBuffer = null;
    }

    private void Update()
    {
        var bounds = new Bounds(_startPosition.position, 3f * Vector3.one);

        _matricesBuffer.SetData(_matrices);
        _propertyBlock.SetBuffer(_matricesId, _matricesBuffer);
        _material.SetBuffer(_matricesId, _matricesBuffer);
        Graphics.DrawMeshInstancedProcedural(_mesh, 0, _material, bounds, _matricesBuffer.count, _propertyBlock);

    }
}