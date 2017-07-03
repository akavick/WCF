﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyDiskInfoLibrary
{
    [ServiceContract]
    public interface IMyDiskInfoServer
    {
        [OperationContract]
        string GetFreeSpace(string diskName);

        [OperationContract]
        string GetTotalSpace(string diskName);
    }
}
