using System;
using System.Collections.Generic;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Shapes
{

    public interface IShape2f
    {
        /// <summary>
        /// The bounding box that contains the shape.
        /// </summary>
        Box2f Bounds { get; }

        /// <summary>
        /// Does the shape contain the point.
        /// Points on the shapes surface count as 
        /// being contained in the shape.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <param name="includeBorder"></param>
        /// <returns>Does the shape contain the point.</returns>
        bool Contains(Point2f p, bool includeBorder);

        /// <summary>
        /// Does the shape intersect the box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="includeBorder"></param>
        /// <returns>Does the shape intersect the box.</returns>
        bool Intersects(Box2f box, bool includeBorder);

        /// <summary>
        /// The closest point to the shape.
        /// If point inside shape return the same point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The closest point to the shape.</returns>
        Point2f Closest(Point2f p);

        /// <summary>
        /// The signed distance between the shapes surface and the point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>Positive if outside shape, negative if inside and 0 on boundary</returns>
        float SignedDistance(Point2f p);
    }

    public abstract class Shape2f : IShape2f
    {
        /// <summary>
        /// The bounding box that contains the shape.
        /// </summary>
        public abstract Box2f Bounds { get; }


        /// <summary>
        /// Does the shape contain the point.
        /// Points on the shapes surface count as 
        /// being contained in the shape.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <param name="includeBorder"></param>
        /// <returns>Does the shape contain the point.</returns>
        public virtual bool Contains(Point2f p, bool includeBorder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does the shape intersect the box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="includeBorder"></param>
        /// <returns>Does the shape intersect the box.</returns>
        public virtual bool Intersects(Box2f box, bool includeBorder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The closest point to the shape.
        /// If point inside shape return the same point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The closest point to the shape.</returns>
        public virtual Point2f Closest(Point2f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The signed distance between the shapes surface and the point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>Positive if outside shape, negative if inside and 0 on boundary</returns>
        public virtual float SignedDistance(Point2f p)
        {
            throw new NotImplementedException();
        }
    }
}
