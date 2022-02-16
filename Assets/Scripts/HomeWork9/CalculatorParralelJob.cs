using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static Fractal3;

namespace HomeWork9
{
    public struct CalculatorParrallelJob : IJobParallelFor
    {
        public NativeArray<FractalPart> LevelParts;
        [ReadOnly]
        public NativeArray<FractalPart> ParrentParts;
        
        public NativeArray<Matrix4x4> LevelMatrices;
        public Quaternion DeltaRotation;
        public int ChildCount;
        public float SpinAngleDelta;
        public float Scale;
        public float PositionOffset;

        public void Execute(int index)
        {
            var parent = ParrentParts[index / ChildCount];
            var part = LevelParts[index];
            part.SpinAngle += SpinAngleDelta;
            DeltaRotation = Quaternion.Euler(.0f, part.SpinAngle, .0f);
            part.WorldRotation = parent.WorldRotation * part.Rotation * DeltaRotation;
            part.WorldPosition = parent.WorldPosition + parent.WorldRotation * (PositionOffset * Scale * part.Direction);
            LevelParts[index] = part;
            LevelMatrices[index] = Matrix4x4.TRS(part.WorldPosition, part.WorldRotation, Scale * Vector3.one);
        }
    }
}