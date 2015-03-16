using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    public class Resource
    {
        public ResourceType Type { get; private set; }
        public int Id_ { get; internal set; }

        public Resource(ResourceType t)
            : this(t, -1)
        {
        }

        public Resource(ResourceType t, int i)
        {
            Type = t;
            Id_ = i;
        }

        public override string ToString()
        {
            if (Id_ == -1)
            {
                return Type.ToString();
            }
            else
            {
                return Type.ToString() + "(" + Id_ + ")";
            }
        }

    }
}
