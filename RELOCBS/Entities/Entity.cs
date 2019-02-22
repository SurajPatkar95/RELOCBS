using RELOCBS.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Entity: IObjectState
    {
        
        public ObjectState ObjectState { get; set; }
    }
}