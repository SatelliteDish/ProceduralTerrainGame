using Godot;
using System;

public static class Common {
    public const int CHUNKSIZE = 16;
    public const int RENDERDISTANCE = 5;
    public static Godot.Vector3 WorldToChunkCoords(Godot.Vector3 vec) {
        vec.X = (int)vec.X/Common.CHUNKSIZE;
        vec.Y = 0;
        vec.Z = (int)vec.Z/Common.CHUNKSIZE;
        return vec;
    }
    public static float RoundToNth(float val, int n) {
        val = (int)val * n;
        return val/n;
    } 
}
