using System;

namespace MoistBot.Models
{
    public class LifetimeAttribute : Attribute
    {
        public LifetimeAttribute(ServiceLifeTime lifeTime = ServiceLifeTime.Transient)
        {
            LifeTime = lifeTime;
        }

        public ServiceLifeTime LifeTime { get; }
    }

    public enum ServiceLifeTime
    {
        Transient = 0,
        Scoped = 1,
        Singleton = 2,
    }
}