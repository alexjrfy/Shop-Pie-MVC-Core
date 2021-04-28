﻿using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Order Order);
    }
}
