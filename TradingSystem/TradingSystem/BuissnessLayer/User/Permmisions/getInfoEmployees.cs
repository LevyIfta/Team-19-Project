﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.BuissnessLayer.User.Permmisions
{
    public class getInfoEmployees : aPermission
    {
        public override object todo(PersmissionsTypes func, object[] args)
        {
            if (func == PersmissionsTypes.getInfoEmployees)
                return null; //todo
            return base.todo(func, args);
        }
    }
}
