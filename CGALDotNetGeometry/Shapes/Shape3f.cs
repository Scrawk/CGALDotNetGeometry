using System;
using System.Collections.Generic;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Shapes
{

    public interface IShape3f
    { 
        Box3f Bounds { get; }

        bool Contains(Point3f p, bool includeBorder);

        bool Intersects(Box3f box, bool includeBorder);

        Point3f Closest(Point3f p);

        float SignedDistance(Point3f p);

    }

    public abstract class Shape3f : IShape3f
    {
        public abstract Box3f Bounds { get; }

        public virtual Point3f Closest(Point3f p)
        {
            throw new NotImplementedException();
        }

        public virtual bool Contains(Point3f p, bool includeBorder)
        {
            throw new NotImplementedException();
        }

        public virtual bool Intersects(Box3f box, bool includeBorder)
        {
            throw new NotImplementedException();
        }

        public virtual float SignedDistance(Point3f p)
        {
            throw new NotImplementedException();
        }
    }
}
