﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DataStore
{
    [Serializable]
    public class ActionSequence<T>:  ISerializable where T : ISkeleton
    {
        private List<T> states;
        private int cur;

        public ActionSequence()
        {
            this.cur = -1;
            this.states = new List<T>();
        }

        public ActionSequence(List<T> states)
        {
            this.cur = -1;
            this.states = states;
        }

        public void append(ActionSequence<T> other)
        {
            other.reset();
            while (other.hasNext())
            {
                this.states.Add(other.next());
            }
        }

        public int size()
        {
            return this.states.Count;
        }

        public T get(int i)
        {
            return this.states[i];
        }

        public void append(T other)
        {
            this.states.Add(other);
        }

        public bool isEmpty()
        {
            return this.states.Count == 0;
        }

        public bool hasNext()
        {
            return this.cur != states.Count - 1;
        }

        public T current()
        {
            return this.states[this.cur];
        }

        public T next()
        {
            this.cur++;
            return this.states[this.cur];
        }

        public void reset()
        {
            this.cur = -1;
        }

        public ActionSequence(SerializationInfo info, StreamingContext ctxt)
        {
            this.states = (List<T>)info.GetValue("states", typeof(List<T>));
            this.cur = -1;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("states", states);
        }

        /// <summary>
        /// Returns a TxD matrix of values where value[t][d] is the value of dimension d at timestep t
        /// </summary>
        /// <param name="jointVals">Whether the underlying skeleton should return joints or positions</param>
        /// <returns></returns>
        public double[][] toArray(bool jointVals = false)
        {

            double[][] arr = new double[states.Count][];

            double[] start = states[0].toArray(jointVals);

            // Make all motions relative to the starting point
            for (int i = 0; i < states.Count; i++)
            {
                arr[i] = states[i].toArray(jointVals);
                for (int j = 0; j < arr[0].Length; j++)
                {
                    arr[i][j] -= start[j];
                }
            }
            return arr;
        }
    }
}
