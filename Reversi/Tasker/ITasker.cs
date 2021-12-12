using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTasker
{
    interface ITasker
    {
        public void AddTask(Func<bool> func);
        public void Update();
        public bool IsEmpty();
    }
}
