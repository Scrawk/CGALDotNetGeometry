using System;
using System.Collections.Generic;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Shapes
{

    public interface IShape2d
    {
        /// <summary>
        /// The bounding box that contains the shape.
        /// </summary>
        Box2d Bounds { get; }

        /// <summary>
        /// Does the shape contain the point.
        /// Points on the shapes surface count as 
        /// being contained in the shape.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="includeBorder"></param>
        /// <returns></returns>
        bool Contains(Point2d p, bool includeBorder = true);

        /// <summary>
        /// Does the shape intersect the box.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="includeBorder"></param>
        /// <returns></returns>
        bool Intersects(Box2d box, bool includeBorder = true);

        /// <summary>
        /// The closest point to the shape.
        /// If point inside shape return the same point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Point2d Closest(Point2d p);

        /// <summary>
        /// The signed distance between the shapes surface and the point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Positive if outside shape, negative if inside and 0 on boundary</returns>
        double SignedDistance(Point2d p);
    }

    public abstract class Shape2d : IShape2d
    {
        /// <summary>
        /// The bounding box that contains the shape.
        /// </summary>
        public abstract Box2d Bounds { get; }


        /// <summary>
        /// Does the shape contain the point.
        /// Points on the shapes surface count as 
        /// being contained in the shape.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="includeBorder"></param>
        /// <returns></returns>
        public virtual bool Contains(Point2d p, bool includeBorder = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does the shape intersect the box.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="includeBorder"></param>
        /// <returns></returns>
        public virtual bool Intersects(Box2d box, bool includeBorder = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The closest point to the shape.
        /// If point inside shape return the same point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual Point2d Closest(Point2d p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The signed distance between the shapes surface and the point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Positive if outside shape, negative if inside and 0 on boundary</returns>
        public virtual double SignedDistance(Point2d p)
        {
            throw new NotImplementedException();
        }
    }
}
