using SFML.System;

namespace Objects;
public interface ISimulationObject<T>
{
    public ObjectClient<T> Client();
    public void UpdateClient();

}

public struct ObjectClient<T>
{
    public Vector2f Pos { get; set; }
    public float Radius { get; init; }
    public int? HashKey { get; set; }
    public T Object { get; init; }

    public ObjectClient(Vector2f pos, float radius, T obj)
    {
        Pos = pos;
        Radius = radius;
        Object = obj;
    }
}

