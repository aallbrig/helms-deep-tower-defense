using CleverCrow.Fluid.BTs.Trees;

namespace Model.AI
{
    public interface IBehaviorTreeBuilder<in T>
    {
        public BehaviorTree BuildBehaviorTree(T context);
    }
}