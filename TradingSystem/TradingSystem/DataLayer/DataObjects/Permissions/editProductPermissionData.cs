﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.DataLayer.Permissions
{
    public class editProductPermissionData : aPermissionData
    {
        public editProductPermissionData(MemberData myOwner, string store, string sponser) : this(store, sponser)
        {
            this.myOwner = myOwner;
            this.myOwnerName = myOwner.userName;
        }

        public editProductPermissionData(string store, string sponser) : base(store, sponser)
        {

        }
    }
}
