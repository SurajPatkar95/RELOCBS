using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Extensions
{
    public class SelectListComparer : IEqualityComparer<SelectListItem>
    {

        public bool Equals(SelectListItem x, SelectListItem y)
        {
            //First check if both object reference are equal then return true
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            //If either one of the object refernce is null, return false
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return false;
            }
            //Comparing all the properties one by one
            return x.Text == y.Text && x.Value == y.Value;
        }
        public int GetHashCode(SelectListItem obj)
        {
            //If obj is null then return 0
            if (obj == null)
            {
                return 0;
            }
            //Get the ID hash code value
            int IDHashCode = obj.Value.GetHashCode();
            //Get the Name HashCode Value
            int NameHashCode = obj.Text == null ? 0 : obj.Text.GetHashCode();
            return IDHashCode ^ NameHashCode;
        }

    }
}