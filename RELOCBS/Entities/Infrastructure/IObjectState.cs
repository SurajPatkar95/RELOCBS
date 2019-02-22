using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities.Infrastructure
{
    public interface IObjectState
    {
        
        ObjectState ObjectState { get; set; }
    }
}