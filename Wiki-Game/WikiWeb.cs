﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Wiki_Game
{
    public class Tester
    {
        public static async Task<string> idk(string s)
        {
            await waiter();
            //Thread.Sleep(2000);
            return s;
        }
        public static async Task waiter()
        {
            await Task.Run(() =>
            {
                Task.Delay(3000).Wait();
            });
        }
    }

}