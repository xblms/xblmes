﻿namespace XBLMS.Dto
{
    public class ObjectResult<T> where T : class
    {
        public T Value { get; set; }
    }
}