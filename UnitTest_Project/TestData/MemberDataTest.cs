using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_Project.TestData
{
    public class MemberDataTest
    {
        public static List<object[]> GetData()
        {
            var list = new List<Object[]>()
            {
                new Object[] {44,56,100},
                new Object[] {700,800,1500},
                new Object[] {88,22,110}
            };

            return list;
        }
    }
}
