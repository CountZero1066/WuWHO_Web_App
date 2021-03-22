using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WuWHO_Web_App
{
    //used in the WuWHO_dataControllers Chart_example controller action rather than suffer the difficulty of extracting
    //the elements of an anonymous data object
    public class MyClass
    {
        public int name { get; set; }
        public int count { get; set; }
    }
}
