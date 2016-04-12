using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// help calculate the average value of a history
    /// of values. This can only be used with types that have a 'zero'
    /// value and that have the += and / operators overloaded.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Smoother<T>
    {
        protected List<T> _history;
        protected int _nextUpdateSlot;
        protected T _zeroValue;
        int _sampleSize = 0;

        public Smoother(int sampleSize, T zeroValue)
        {
            _sampleSize = sampleSize;
            _history = new List<T>(_sampleSize);
            _zeroValue = zeroValue;
            _nextUpdateSlot = 0;
        }

        public T Update(T mostRecentValue)
        {
            if(_history.Count == _sampleSize)
            {
                _history[_nextUpdateSlot++] = mostRecentValue;
                if (_nextUpdateSlot == _history.Count)
                {
                    _nextUpdateSlot = 0;
                }
            }
            else
            {
                _history.Add(mostRecentValue);
            }

            T sum = _zeroValue;

            foreach (var it in _history)
            {
                //sum += it;
                Add(ref sum, it);
            }
            return Divide(sum, (float)_history.Count);
            //return sum / ;
        }
        protected abstract void Add(ref T origin, T toAdd);
        protected abstract T Divide<toDiv>(T origin, toDiv div);
    }

    public class SmootherVector : Smoother<Vector2>
    {
        public SmootherVector(int sampleSize, Vector2 zeroValue) : base(sampleSize, zeroValue)
        {

        }

        protected override void Add(ref Vector2 origin, Vector2 toAdd)
        {
            origin += toAdd;
        }

        protected override Vector2 Divide<toDiv>(Vector2 origin, toDiv div)
        {
            return origin / (float)_history.Count;
        }
    }
}
