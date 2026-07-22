using UnityEngine;


    public interface IMovementStrategy
    {
        public float TargetSpeed {get; }
    }
    public class WalkStrategy : IMovementStrategy
    {
        public float TargetSpeed => 2f;
    }
    public class RunStrategy : IMovementStrategy
    {
        public float TargetSpeed => 5f;
    }
    public class DuckStrategy : IMovementStrategy
    {
        public float TargetSpeed => 1f;
    }
    
