using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    delegate void VoidOp();
    internal class FixedUpdate
    {
        
        VoidOp updates = delegate { };
        public void Add(VoidOp operation)
        {// Make sure to clean after yourself with Remove method
            updates += operation;
        }
        public void Remove(VoidOp operation)
        {
            try
            {
                updates -= operation;
            }catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        public void Update()
        {
            updates();
        }
    }
}
