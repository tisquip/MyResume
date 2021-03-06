﻿using Resume.Domain.Interfaces;
using System;

namespace Resume.Domain.BaseClasses
{
    public abstract class EntityBase : IEntity, IEquatable<EntityBase>
    {
        public int Id { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityBase);
        }

        public bool Equals(EntityBase other)
        {
            return other is object
                    && GetType().Equals(other.GetType())
                    && Id != default
                    && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !(left == right);
        }
    }
}
