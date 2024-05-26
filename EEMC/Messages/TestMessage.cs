using EEMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Messages
{
    class TestMessage : IMessage
    {
        public Test? Test { get; set; }

        public TestMessage(Test? test)
        {
            Test = test;
        }
    }
}
