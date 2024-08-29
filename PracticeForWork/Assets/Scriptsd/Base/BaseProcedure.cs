using Koakuma.Game;

using System.Threading.Tasks;
using UnityEngine;

namespace TGame.Procedure
{
    public class BaseProcedure : MonoBehaviour
    {
        public async Task ChangeProcedure<T>(object value = null) where T : BaseProcedure
        {
            await GameManagers.Procedure.ChangeProcedure<T>(value);
        }

        public virtual async Task OnEnterProcedure(object value)
        {
            await Task.Yield();
        }

        public virtual async Task OnLeaveProcedure()
        {
            await Task.Yield();
        }
    }
}

