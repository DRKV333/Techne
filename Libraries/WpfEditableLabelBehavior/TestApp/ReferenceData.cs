using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    public class ReferenceData
    {
        public static List<Gender> GetGenders()
        {
            List<Gender> list = new List<Gender>();
            list.Add(new Gender() { Description = "Male", Identifier = 'M' });
            list.Add(new Gender() { Description = "Female", Identifier = 'F' });
            return list;
        }
    }

    public class Gender
    {
        public string Description { get; set; }
        public char Identifier { get; set; }
    }
}
