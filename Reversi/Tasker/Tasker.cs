using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTasker
{

    /* Return value is treated as a condition for function to be removed 
    * true  -> function will not be removed after execution
    * false -> function will be removed after execution
    * Use responsibly!
    */
    public class Tasker : ITasker
    {
        private readonly List<Func<bool>> funcList;
        private readonly List<Func<bool>> funcsToRemoveList;

        public Tasker()
        {
            funcList = new List<Func<bool>>(1);
            funcsToRemoveList = new List<Func<bool>>();
        }
        public Tasker(int initialCapacity)
        {
            funcList = new List<Func<bool>>(initialCapacity);
            funcsToRemoveList = new List<Func<bool>>();
        }
        public void AddTask(Func<bool> func)
        {
            funcList.Add(func);
        }
        public void Update()
        {
            foreach(var run in funcList)
            {
                if(run() == false)
                    funcsToRemoveList.Add(run);
            }
            foreach(var run in funcsToRemoveList)
            {
                funcList.Remove(run);
            }
            funcsToRemoveList.Clear();
        }
        public bool IsEmpty()
        {
            return funcList.Count == 0;
        }
    }
}
