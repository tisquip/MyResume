namespace Resume.Domain.BaseClasses
{
    public abstract class ValueObjectBase<T>
        where T : ValueObjectBase<T>
    {
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;

            if (valueObject is null)
                return false;

            if (!GetType().Equals(obj.GetType()))
                return false;

            return EqualsCore(valueObject);
        }

        protected abstract bool EqualsCore(T other);

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        protected abstract int GetHashCodeCore();

        public static bool operator ==(ValueObjectBase<T> left, ValueObjectBase<T> right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObjectBase<T> a, ValueObjectBase<T> b)
        {
            return !(a == b);
        }
    }
}
