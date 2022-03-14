using System;
using System.Collections.Generic;
using System.Text;

using CGALDotNetGeometry.Numerics;
using CGALDotNetGeometry.Shapes;

namespace CGALDotNetGeometry.Extensions
{

    public static class ArrayExtension
    {

        public static bool IsFinite(this IList<Point2d> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (!array[i].IsFinite)
                    return false;
            }

            return true;
        }

        public static void MakeFinite(this IList<Point2d> array)
        {
            for (int i = 0; i < array.Count; i++)
                array[i] = array[i].Finite;
        }

        public static bool IsFinite(this IList<Point3d> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (!array[i].IsFinite)
                    return false;
            }

            return true;
        }

        public static void MakeFinite(this IList<Point3d> array)
        {
            for (int i = 0; i < array.Count; i++)
                array[i] = array[i].Finite;
        }

        public static List<Point2d> RemoveNonFinite(this IList<Point2d> array)
        {
            var a = new List<Point2d>(array.Count);

            for (int i = 0; i < array.Count; i++)
                if (array[i].IsFinite)
                    a.Add(array[i]);

            return a;
        }

        public static List<Point3d> RemoveNonFinite(this IList<Point3d> array)
        {
            var a = new List<Point3d>(array.Count);

            for (int i = 0; i < array.Count; i++)
                if (array[i].IsFinite)
                    a.Add(array[i]);

            return a;
        }

        public static void Round(this IList<Point2d> array, int digits)
        {
            if (digits < 0) return;
            for (int i = 0; i < array.Count; i++)
                array[i] = array[i].Rounded(digits);
        }

        public static void Round(this IList<Point3d> array, int digits)
        {
            if (digits < 0) return;
            for (int i = 0; i < array.Count; i++)
                array[i] = array[i].Rounded(digits);
        }

        public static double[] ToDouble(this IList<Point2d> array)
        {
            var a = new double[array.Count * 2];

            for (int i = 0; i < array.Count; i++)
            {
                a[i * 2 + 0] = array[i].x;
                a[i * 2 + 1] = array[i].y;
            }
                    
            return a;
        }

        public static double[] ToDouble(this IList<Point3d> array)
        {
            var a = new double[array.Count * 3];

            for (int i = 0; i < array.Count; i++)
            {
                a[i * 3 + 0] = array[i].x;
                a[i * 3 + 1] = array[i].y;
                a[i * 3 + 2] = array[i].z;
            }

            return a;
        }

        public static bool HasNullIndex(this IList<int> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].IsNullIndex())
                    return true;
            }

            return false;
        }

        public static List<SegmentIndex> RemoveDuplicateSegments(this IList<int> array)
        {
            var set = new HashSet<SegmentIndex>();

            for (int i = 0; i < array.Count / 2; i++)
            {
                int i0 = array[i * 2 + 0];
                int i1 = array[i * 2 + 1];

                if (i0.IsNullIndex() ||
                    i1.IsNullIndex())
                    continue;

                var seg = new SegmentIndex(i0, i1);

                if (set.Contains(seg) || set.Contains(seg.Reversed))
                    continue;

                set.Add(seg);
            }

            var list = new List<SegmentIndex>(set.Count);
            list.AddRange(set);

            return list;
        }


        public static List<TriangleIndex> RemoveDuplicateTriangles(this IList<int> array)
        {
            var triangles = new TriangleIndex[6];
            var set = new HashSet<TriangleIndex>();

            for (int i = 0; i < array.Count / 3; i++)
            {
                int i0 = array[i * 3 + 0];
                int i1 = array[i * 3 + 1];
                int i2 = array[i * 3 + 2];

                if (i0.IsNullIndex() ||
                    i1.IsNullIndex() ||
                    i2.IsNullIndex())
                    continue;

                triangles[0] = new TriangleIndex(i0, i1, i2);
                triangles[1] = new TriangleIndex(i2, i0, i1);
                triangles[2] = new TriangleIndex(i1, i2, i0);
                triangles[3] = new TriangleIndex(i2, i1, i0);
                triangles[4] = new TriangleIndex(i0, i2, i1);
                triangles[5] = new TriangleIndex(i1, i0, i2);

                for (int j = 0; j < 6; j++)
                    if (set.Contains(triangles[j]))
                        continue;

                set.Add(triangles[0]);
            }

            var list = new List<TriangleIndex>(set.Count);
            list.AddRange(set);

            return list;
        }

        public static int[] RemoveNullSegments(this IList<int> array)
        {
            int count = 0;
            for (int i = 0; i < array.Count / 2; i++)
            {
                int i0 = array[i * 2 + 0];
                int i1 = array[i * 2 + 1];

                if (i0.IsNullIndex() ||
                    i1.IsNullIndex())
                    continue;

                count += 3;
            }

            int j = 0;
            var new_array = new int[count];
            for (int i = 0; i < array.Count / 2; i++)
            {
                int i0 = array[i * 2 + 0];
                int i1 = array[i * 2 + 1];
    
                if (i0.IsNullIndex() ||
                    i1.IsNullIndex())
                    continue;

                new_array[j * 2 + 0] = i0;
                new_array[j * 2 + 1] = i1;
 
                j++;
            }

            return new_array;
        }

        public static int[] RemoveNullTriangles(this IList<int> array)
        {
            int count = 0;
            for (int i = 0; i < array.Count / 3; i++)
            {
                int i0 = array[i * 3 + 0];
                int i1 = array[i * 3 + 1];
                int i2 = array[i * 3 + 2];

                if (i0.IsNullIndex() ||
                    i1.IsNullIndex() ||
                    i2.IsNullIndex())
                    continue;

                count += 3;
            }

            int j = 0;
            var new_array = new int[count];
            for (int i = 0; i < array.Count / 3; i++)
            {
                int i0 = array[i * 3 + 0];
                int i1 = array[i * 3 + 1];
                int i2 = array[i * 3 + 2];

                if (i0.IsNullIndex() ||
                    i1.IsNullIndex() ||
                    i2.IsNullIndex())
                    continue;

                new_array[j * 3 + 0] = i0;
                new_array[j * 3 + 1] = i1;
                new_array[j * 3 + 2] = i2;

                j++;
            }

            return new_array;
        }

        public static int[] RemoveNullQuads(this IList<int> array)
        {
            int count = 0;
            for (int i = 0; i < array.Count / 4; i++)
            {
                int i0 = array[i * 4 + 0];
                int i1 = array[i * 4 + 1];
                int i2 = array[i * 4 + 2];
                int i3 = array[i * 4 + 3];

                if (i0.IsNullIndex() ||
                    i1.IsNullIndex() ||
                    i2.IsNullIndex() ||
                    i3.IsNullIndex())
                    continue;

                count += 4;
            }

            int j = 0;
            var new_array = new int[count];
            for (int i = 0; i < array.Count / 4; i++)
            {
                int i0 = array[i * 4 + 0];
                int i1 = array[i * 4 + 1];
                int i2 = array[i * 4 + 2];
                int i3 = array[i * 4 + 3];

                if (i0.IsNullIndex() ||
                   i1.IsNullIndex() ||
                   i2.IsNullIndex() ||
                   i3.IsNullIndex())
                    continue;

                new_array[j * 4 + 0] = i0;
                new_array[j * 4 + 1] = i1;
                new_array[j * 4 + 2] = i2;
                new_array[j * 4 + 3] = i3;

                j++;
            }

            return new_array;
        }
    }

}
