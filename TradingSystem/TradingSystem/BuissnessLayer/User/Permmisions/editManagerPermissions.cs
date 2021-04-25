﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.BuissnessLayer.User.Permmisions
{
    public class editManagerPermissions : aPermission
    {
        public editManagerPermissions(string storeName, string sponser) : base(storeName, sponser) { }
        public override object todo(PersmissionsTypes func, object[] args)
        {// string storeName, string username, string userSponser, aPermission Permissions
            if (func == PersmissionsTypes.EditManagerPermissions && this.store.Equals((string)args[0]))
            {
                return ((Member)UserServices.gerUser((string)args[1])).editPermission((string)args[0], (string)args[2],(aPermission)args[3]);
            }
            return base.todo(func, args);
        }
    }
}
