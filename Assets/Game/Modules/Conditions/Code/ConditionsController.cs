using System.Collections.Generic;
using FlatLands.Architecture;

namespace FlatLands.Conditions
{
    public sealed class ConditionsController : SharedObject
    {
        public void ApplyConditions(IEnumerable<BaseCondition> conditions)
        {
            if(conditions == null)
                return;
            
            foreach (var condition in conditions)
            {
                ApplyCondition(condition);
            }
        }
		
        public void ApplyCondition(BaseCondition condition)
        {
            if(condition == null)
                return;

            _container.InjectAt(condition);
            
            if (!condition.CanApply) 
                return;
			
            condition.ApplyCondition();
        }
    }
}