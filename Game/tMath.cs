using OpenTK;

namespace infiniteTerrain.Game
{
    public class tMath
    {
        public static Vector3 calculateNormal(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
        {
            Vector3 tangentA = Vector3.Subtract(vertexB, vertexA);
            Vector3 tangentB = Vector3.Subtract(vertexC, vertexA);

            Vector3 normal = Vector3.Cross(tangentA, tangentB);
            normal.Normalize();
            return normal;
        }
    }
}
