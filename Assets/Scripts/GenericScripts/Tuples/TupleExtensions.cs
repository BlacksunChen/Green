﻿/// <summary>
/// TupleExtensions v1.0.0 by Christian Chomiak, christianchomiak@gmail.com
/// 
/// Some functions to help find the Min and Max values of a Tuple.
/// 
/// Only works for:
///     * Tuples where all elements have the same type.
///     * The elements of the tuples are comparable.
/// </summary>

using System;

namespace Generic.Tuples
{
    public static class TupleExtensions
    {
        public static T1 Max<T1>(this Tuple<T1, T1> t) where T1 : IComparable<T1>
        {
            if (t.Item1.CompareTo(t.Item2) > 0)
                return t.Item1;
            else
                return t.Item2;
        }

        public static T1 Min<T1>(this Tuple<T1, T1> t) where T1 : IComparable<T1>
        {
            if (t.Item1.CompareTo(t.Item2) < 0)
                return t.Item1;
            else
                return t.Item2;
        }

        public static T1 Max<T1>(this Tuple3<T1, T1, T1> t) where T1 : IComparable<T1>
        {
            if (t.Item1.CompareTo(t.Item2) > 0)
                if (t.Item1.CompareTo(t.Item3) > 0)
                    return t.Item1;
                else
                    return t.Item3;
            else if (t.Item2.CompareTo(t.Item3) > 0)
                return t.Item2;
            else
                return t.Item3;
        }

        public static T1 Min<T1>(this Tuple3<T1, T1, T1> t) where T1 : IComparable<T1>
        {
            if (t.Item1.CompareTo(t.Item2) < 0)
                if (t.Item1.CompareTo(t.Item3) < 0)
                    return t.Item1;
                else
                    return t.Item3;
            else if (t.Item2.CompareTo(t.Item3) < 0)
                return t.Item2;
            else
                return t.Item3;
        }

        public static T1 Max<T1>(this Tuple4<T1, T1, T1, T1> t) where T1 : IComparable<T1>
        {
            if (t.first.CompareTo(t.second) > 0)
                if (t.first.CompareTo(t.third) > 0)
                    if (t.first.CompareTo(t.fourth) > 0)
                        return t.first;
                    else
                        return t.fourth;
                else
                    if (t.third.CompareTo(t.fourth) > 0)
                        return t.third;
                    else
                        return t.fourth;
            else if (t.second.CompareTo(t.third) > 0)
                if (t.second.CompareTo(t.fourth) > 0)
                    return t.second;
                else
                    return t.fourth;
            else if (t.third.CompareTo(t.fourth) > 0)
                return t.third;
            else
                return t.fourth;
        }

        public static T1 Min<T1>(this Tuple4<T1, T1, T1, T1> t) where T1 : IComparable<T1>
        {
            if (t.first.CompareTo(t.second) < 0)
                if (t.first.CompareTo(t.third) < 0)
                    if (t.first.CompareTo(t.fourth) < 0)
                        return t.first;
                    else
                        return t.fourth;
                else
                    if (t.third.CompareTo(t.fourth) < 0)
                        return t.third;
                    else
                        return t.fourth;
            else if (t.second.CompareTo(t.third) < 0)
                if (t.second.CompareTo(t.fourth) < 0)
                    return t.second;
                else
                    return t.fourth;
            else if (t.third.CompareTo(t.fourth) < 0)
                return t.third;
            else
                return t.fourth;
        }
    }

}