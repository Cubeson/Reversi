using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTasker
{
    public delegate bool Func();
    interface ITasker
    {
        public void AddTask(Func func);
        public void Update();
        public bool IsEmpty();
    }
}
