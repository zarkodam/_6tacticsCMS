using AutoMapper;
using System.Collections.Generic;

namespace _6tactics.Utilities.ObjectMapping
{
    //Requires http://automapper.org/
    public class ObjectMapper
    {
        public static TOut Map<TIn, TOut>(object objectForMapping)
            where TIn : class
            where TOut : class
        {
            Mapper.CreateMap<TIn, TOut>();
            return Mapper.Map<TOut>(objectForMapping);
        }

        public static List<TOut> MapToList<TIn, TOut>(object objectForMapping)
            where TIn : class
            where TOut : class
        {
            Mapper.CreateMap<TIn, TOut>();
            return Mapper.Map<List<TOut>>(objectForMapping);
        }
    }
}
