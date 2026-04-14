// Decompiled with JetBrains decompiler
// Type: UMedical.Models.Medical.Values.SVector3
// Assembly: UMedical, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B63ED5D2-FC9A-498B-AE7F-48DB80E0EB02
// Assembly location: C:\Users\Administrator\Downloads\UMedical.dll

using UnityEngine;

namespace UMedical.Models.Medical.Values
{
  public sealed class SVector3
  {
    public float x { get; set; }

    public float y { get; set; }

    public float z { get; set; }

    public SVector3()
    {
    }

    public SVector3(float x, float y, float z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public SVector3(Vector3 Vector)
    {
      this.x = Vector.x;
      this.y = Vector.y;
      this.z = Vector.z;
    }

    public Vector3 ToVector3() => new Vector3(this.x, this.y, this.z);
  }
}
