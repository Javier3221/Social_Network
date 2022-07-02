using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Custom_Data_Annotations
{
    public class MaxImgRange : ValidationAttribute
    {
        private readonly int _maxElements;

        public MaxImgRange(int maxElements)
        {
            _maxElements = maxElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list == null)
            {
                return true;
            }
            else if (list != null)
            {
                return list.Count <= _maxElements && list.Count > 0;
            }
            return false;
        }
    }
}
