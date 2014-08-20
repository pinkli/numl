﻿// file:	Supervised\Generator.cs
//
// summary:	Implements the generator class
using System;
using numl.Model;
using System.Linq;
using System.Collections.Generic;
using numl.Math;
using System.Net;
using numl.Math.LinearAlgebra;


namespace numl.Supervised
{
    /// <summary>A generator.</summary>
    public abstract class Generator : IGenerator
    {
        /// <summary>Gets or sets the descriptor.</summary>
        /// <value>The descriptor.</value>
        public Descriptor Descriptor { get; set; }
        /// <summary>Generate model based on a set of examples.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="examples">Example set.</param>
        /// <returns>Model.</returns>
        public IModel Generate(IEnumerable<object> examples)
        {
            if (examples.Count() == 0) throw new InvalidOperationException("Empty example set.");

            if (Descriptor == null) // try to generate the descriptor
                Descriptor = Descriptor.Create(examples.First().GetType());

            return Generate(Descriptor, examples);
        }
        /// <summary>Generate model based on a set of examples.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="description">The description.</param>
        /// <param name="examples">Example set.</param>
        /// <returns>Model.</returns>
        public IModel Generate(Descriptor description, IEnumerable<object> examples)
        {
            if (examples.Count() == 0) throw new InvalidOperationException("Empty example set.");

            Descriptor = description;
            if (Descriptor.Features == null || Descriptor.Features.Length == 0)
                throw new InvalidOperationException("Invalid descriptor: Empty feature set!");
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Invalid descriptor: Empty label!");

            var doubles = Descriptor.Convert(examples);
            var tuple = doubles.ToExamples();

            return Generate(tuple.Item1, tuple.Item2);
        }
        /// <summary>Generates the given examples.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="examples">Example set.</param>
        /// <returns>An IModel.</returns>
        public IModel Generate<T>(IEnumerable<T> examples)
             where T : class
        {
            var descriptor = Descriptor.Create<T>();
            return Generate(descriptor, examples);
        }
        /// <summary>Generate model based on a set of examples.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public abstract IModel Generate(Matrix x, Vector y);
    }
}
