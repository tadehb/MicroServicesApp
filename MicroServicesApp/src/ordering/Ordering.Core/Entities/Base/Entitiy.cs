namespace Ordering.Core.Entities.Base
{
    public abstract class Entitiy : EntityBase<int>
    {
         public static bool operator !=(EntityBase<TId> left,EntityBase<TId> right)
        {
            return !(left == right);
        }
    }
}
